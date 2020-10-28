using EnterpriseZEM.db.tables;
using EnterpriseZEM_Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Serilog;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Protocols.WSTrust;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EnterpriseZEM
{
    class Server : ServerBase
    {
        //ZEMDBContext _db;
        Serilog.Core.Logger _log;

        public Server(int port, string ip, Serilog.Core.Logger log) : base(port, ip)
        {
            //_db = new ZEMDBContext();
            _log = log;
        }

        public override void onClientConnect(string clientAdress)
        {
            base.onClientConnect(clientAdress);
            _log.Information($"Connection from: {clientAdress}");
        }

        public override void onConnectionClose(string clientAdress)
        {
            base.onConnectionClose(clientAdress);
            _log.Information($"Cliend {clientAdress} disconnected.");
        }

        public override void onConnectionLost(string clientAdress, Exception exception)
        {
            base.onConnectionLost(clientAdress, exception);
            _log.Warning($"Client: {clientAdress}, lost connection because {exception.Message}, {exception.InnerException}");
        }

        public override void onServerListen(string serverIP, int serverPort)
        {
            base.onServerListen(serverIP, serverPort);
            _log.Information($"Server is listening on: {serverIP}:{serverPort}");

        }

        protected CustomPacket Scan(CustomPacket packet)
        {
            using (var _db = new ZEMDBContext())
            {
                ScannedCode sc = (ScannedCode)packet.Payload;
                var scan = _db.ScanCache.Find(sc.SessionGUID);
                if (scan == null)
                {
                    _db.ScanCache.Add(new ScanCache { ScanCacheId = sc.SessionGUID });
                    _db.SaveChanges();
                    scan = _db.ScanCache.Find(sc.SessionGUID);
                }
                VTInsertFunctions VTFuncs = new VTInsertFunctions(_db, _log, scan);
                ScannedResponse sr = new ScannedResponse();
                CustomPacket response = new CustomPacket(FlagType.basic, HeaderTypes.basic, "scan", new List<string>(), sr);
                // 23102720A

                sc.Declared = false;
                sc.complete = false;
                sc.isFullSet = false;
                sc.addedBefore = false;
                sc.dataDostawyOld = DateTime.MinValue;
                sc.dataDoskanowania = DateTime.Now;
                sc.dataUtworzenia = DateTime.Now;

                sr.missingEntries = new List<MissingBackwards>();
                var techEntry = _db.Technical.IgnoreQueryFilters().FirstOrDefault(c => c.PrzewodCiety == sc.kodCiety);
                if (techEntry == null)
                {
                    if (_db.MissingFromTech.Find(sc.kodCiety) == null)
                    {
                        _db.MissingFromTech.Add(new MissingFromTech { DataDodania = sc.dataDoskanowania, Kod = sc.kodCiety, User = sc.User });
                        _db.SaveChanges();
                    }
                    response.Header = HeaderTypes.error;
                    response.Flag = FlagType.notInTech;
                    return response;
                }
                else if (techEntry.Deleted == true)
                {
                    response.Header = HeaderTypes.error;
                    response.Flag = FlagType.isDeleted;
                    return response;

                }
                else if (techEntry.KanBan == true)
                {
                    if (_db.ScannedKanbans.FirstOrDefault(c => c.Kod == sc.kodCiety && c.Wiazka == techEntry.Wiazka) == null)
                    {
                        _db.ScannedKanbans.Add(new ScannedKanban { DataDodania = sc.dataDoskanowania, Kod = sc.kodCiety, Wiazka = techEntry.Wiazka, User = sc.User });
                        _db.SaveChanges();
                    }
                    response.Header = HeaderTypes.error;
                    response.Flag = FlagType.isKanban;
                    return response;
                }

                sc.Wiazka = techEntry.Wiazka;
                sc.Rodzina = techEntry.Rodzina;
                sc.BIN = techEntry.BIN;

                sr.PrzewodCiety = techEntry.PrzewodCiety;
                sr.BIN = techEntry.BIN;
                sr.KodWiazki = techEntry.KodWiazki;
                sr.LiteraRodziny = techEntry.LiterRodziny;
                sr.IlePrzewodow = techEntry.IlePrzewodow;


                var dostawaEntry = _db.Dostawa.FirstOrDefault(c => c.Data.Date == sc.dataDostawy.Date && c.Kod == "PLC" + sc.kodCietyFull);
                if (dostawaEntry != null)
                {
                    sc.dataDostawy = dostawaEntry.Data;
                    sc.dataDostawyOld = dostawaEntry.Data;
                    sc.sztukiDeklarowane = dostawaEntry.Ilosc;
                    sc.Declared = true;

                    if (sc.sztukiSkanowane == sc.sztukiDeklarowane)
                    {
                        var sets = _db.VTMagazyn.Where(c => c.Wiazka == sc.Wiazka && c.DataDostawy.Date == sc.dataDostawyOld.Date).ToList();
                        var deliveries = _db.Dostawa.Include(c => c.Technical).Where(c => c.Technical.Wiazka == sc.Wiazka && c.Data.Date == sc.dataDostawyOld.Date).ToList();
                        int declared = VTFuncs.GetPossibleDeclaredValue(sc, sets, deliveries, sc.NumerKompletu);

                        if (sc.sztukiSkanowane != declared && !sc.isForcedOverDeclared)
                        {
                            response.Header = HeaderTypes.error;
                            response.Flag = FlagType.quantityOverDeclated;
                            response.Args.Add(sc.sztukiDeklarowane.ToString());
                            response.Args.Add(VTFuncs.GetScannedForDay(sc, sets).ToString());
                            response.Args.Add(declared.ToString());
                            response.Args.Add($"{declared - sc.sztukiSkanowane}");

                            return response;
                        }
                        else
                        {
                            if (!VTFuncs.CheckBackOrAdd(response, techEntry, sc, dostawaEntry))
                                return response;
                        }
                    }
                    else if (sc.sztukiSkanowane != sc.sztukiDeklarowane)
                    {
                        if (!sc.isForcedQuantity)
                        {
                            response.Header = HeaderTypes.error;
                            response.Flag = FlagType.quantityIncorrect;
                            response.Args.Add(sc.sztukiDeklarowane.ToString());
                            var sets = _db.VTMagazyn.Where(c => c.Wiazka == sc.Wiazka && c.DataDostawy.Date == sc.dataDostawyOld.Date).ToList();
                            var deliveries = _db.Dostawa.Include(c => c.Technical).Where(c => c.Technical.Wiazka == sc.Wiazka && c.Data.Date == sc.dataDostawyOld.Date).ToList();
                            int declared = VTFuncs.GetPossibleDeclaredValue(sc, sets, deliveries, sc.NumerKompletu);
                            response.Args.Add(VTFuncs.GetScannedForDay(sc, sets).ToString());

                            response.Args.Add(declared.ToString());
                            response.Args.Add($"{declared - sc.sztukiSkanowane}");

                            return response;
                        }
                        else
                        {
                            if (!VTFuncs.CheckBackOrAddQuantityIncorrect(response, techEntry, sc, dostawaEntry))
                                return response;
                        }
                    }
                }
                else
                {
                    sc.dataDostawy = sc.dataDostawy.Date;
                    sc.dataDostawyOld = sc.dataDostawy.Date;

                    if (!sc.isForcedUndeclared)
                    {
                        response.Header = HeaderTypes.error;
                        response.Flag = FlagType.notInDeclared;
                        return response;
                    }
                    else
                    {
                        if (!sc.isForcedQuantity)
                        {
                            response.Header = HeaderTypes.error;
                            response.Flag = FlagType.quantityIncorrect;
                            response.Args.Add("0");
                            var sets = _db.VTMagazyn.Where(c => c.Wiazka == sc.Wiazka && c.DataDostawy.Date == sc.dataDostawyOld.Date).ToList();
                            var deliveries = _db.Dostawa.Include(c => c.Technical).Where(c => c.Technical.Wiazka == sc.Wiazka && c.Data.Date == sc.dataDostawyOld.Date).ToList();
                            int declared = VTFuncs.GetPossibleDeclaredValue(sc, sets, deliveries, sc.NumerKompletu);
                            response.Args.Add(VTFuncs.GetScannedForDay(sc, sets).ToString());

                            response.Args.Add(declared.ToString());
                            response.Args.Add($"{declared - sc.sztukiSkanowane}");

                            return response;
                        }
                        else
                        {
                            if (!VTFuncs.CheckBackOrAddQuantityIncorrect(response, techEntry, sc, dostawaEntry))
                                return response;
                        }
                    }
                }
                _db.SaveChanges();

                bool isComplete = VTFuncs.checkComplete(sc, out int numToComplete, out int numScanned, out int numScannedToComplete);

                sr.DataDostawy = sc.dataDostawy;
                sr.DataDostawyOld = sc.dataDostawyOld;
                sr.numToComplete = numToComplete;
                sr.numScanned = numScanned;
                sr.numScannedToComplete = numScannedToComplete;
                sr.isComplete = isComplete;
                sr.sztukiDeklatowane = sc.sztukiDeklarowane;
                sr.numerKompletu = sc.NumerKompletu;
                sr.Wiazka = sc.Wiazka;
                sr.Rodzina = sc.Rodzina;
                sr.sztukiSkanowane = sc.sztukiSkanowane;

                if (numScanned == 1)
                {
                    sr.Print = true;
                    sr.isSpecialColor = false;
                }
                if (VTFuncs.shouldPrintSpecial(sc))
                {
                    sr.Print = true;
                    sr.isSpecialColor = true;
                }

                _log.Information("Scan details: {@sc}", sc);
                _log.Information("Response details: {@sr}", sr);

                return response;
            }
        }

        protected CustomPacket getBin(CustomPacket packet)
        {
            using (var _db = new ZEMDBContext())
            {
                CustomPacket response = new CustomPacket(FlagType.basic, HeaderTypes.basic, null, null, null);
                var bundle = _db.Technical.AsNoTracking().FirstOrDefault(c => c.PrzewodCiety == (string)packet.Payload).Wiazka;
                var BIN = _db.Technical.AsNoTracking().Where(c => c.Wiazka == bundle).Select(c => c.BIN).Distinct().ToList();
                if (BIN == null)
                {
                    response.Header = HeaderTypes.error;
                    response.Flag = FlagType.binNotFound;
                }
                else
                {
                    response.Args = BIN;
                }
                return response;
            }
        }
        protected CustomPacket showMissing(CustomPacket packet)
        {
            using (var _db = new ZEMDBContext())
            {
                CustomPacket response = new CustomPacket(FlagType.basic, HeaderTypes.basic, null, new List<string>(), null);
                VTInsertFunctions vTInsert = new VTInsertFunctions(_db, _log, null);
                string cutcode = packet.Args[0];
                DateTime date = DateTime.Parse(packet.Args[1]);

                var techEntry = _db.Technical.FirstOrDefault(c => c.PrzewodCiety == cutcode);
                if (techEntry == null)
                {
                    response.Flag = FlagType.notInTech;
                    response.Header = HeaderTypes.error;
                    return response;
                }
                else if (techEntry.KanBan)
                {
                    response.Flag = FlagType.isKanban;
                    response.Header = HeaderTypes.error;
                    return response;
                }
                var wiazka = techEntry.Wiazka;
                var SetIDs = vTInsert.GetSetIDsForBundle(new ScannedCode { Wiazka = wiazka, dataDostawyOld = date });
                if (SetIDs.Count() == 0)
                    SetIDs.Add(0);

                List<string> missingCodes = new List<string>();
                var deliveries = _db.Dostawa.AsNoTracking().Include(c => c.Technical).Where(c => c.Technical.Wiazka == wiazka &&
                    c.Data.Date == date.Date).ToList();
                var scans = _db.VTMagazyn.AsNoTracking().Where(c => c.Wiazka == wiazka && c.DataDostawy.Date == date.Date).ToList();

                foreach (int setNumber in SetIDs)
                {
                    var codesForWiazka = _db.Technical.Where(c => c.Wiazka == wiazka && c.KanBan == false).Select(c => c.PrzewodCiety).ToList();
                    var scannedCodes = scans.Where(c => c.NumerKompletu == setNumber).Select(c => c.KodCiety).ToList();
                    if (scannedCodes.Count() == 0)
                    {
                        response.Flag = FlagType.nonScanned;
                        response.Header = HeaderTypes.error;
                        return response;
                    }


                    missingCodes.Add($"Brakujące kody dla wiązki {wiazka} komplet nr. {setNumber} po {vTInsert.GetPossibleDeclaredValue(new ScannedCode { kodCiety = cutcode, Wiazka = wiazka, dataDostawyOld = date }, scans, deliveries, setNumber)}");
                    missingCodes.AddRange(codesForWiazka.Except(scannedCodes).ToList());
                }

                response.Args = missingCodes;

                return response;
            }
        }

        public override CustomPacket Commands(CustomPacket packet, TcpClient client)
        {
            CustomPacket responsePacket = null;

            var watch = Stopwatch.StartNew();

            switch(packet.Command)
            {
                case "scan":
                    _log.Information($"Incoming scan from: {client.Client.RemoteEndPoint}");
                    responsePacket = Scan(packet);
                    break;
                case "getBIN":
                    responsePacket = getBin(packet);
                    break;
                case "showMissing":
                    responsePacket = showMissing(packet);
                    break;
                case "disposeCache":
                    using (var _db = new ZEMDBContext())
                    {
                        var stray = _db.ScanCache.Find((Guid)packet.Payload);
                        if (stray != null)
                            _db.Remove(stray);
                        _db.SaveChanges();
                        responsePacket = new CustomPacket(FlagType.basic, HeaderTypes.basic, "disposeCache", null, null);   
                    }
                    break;
            }

            watch.Stop();
            _log.Information($"Command {packet.Command} execution time [{watch.ElapsedMilliseconds}]");

            return responsePacket;
        }
    }
}
