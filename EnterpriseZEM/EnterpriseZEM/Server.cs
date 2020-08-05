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
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EnterpriseZEM
{
    class Server : ServerBase
    {
        ZEMDBContext _db;
        Serilog.Core.Logger _log;

        public Server(int port, string ip, Serilog.Core.Logger log) : base(port, ip)
        {
            _db = new ZEMDBContext();
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
            _db = new ZEMDBContext();
            VTInsertFunctions VTFuncs = new VTInsertFunctions(_db, _log);
            ScannedResponse sr = new ScannedResponse();
            CustomPacket response = new CustomPacket(FlagType.basic, HeaderTypes.basic, "scan", new List<string>(), sr);
            //PLCGTJ229015
            ScannedCode sc = (ScannedCode)packet.Payload;
            sc.Declared = false;
            sc.complete = false;
            sc.isFullSet = false;
            sc.addedBefore = false;
            sc.dataDostawyOld = DateTime.MinValue;
            sc.dataDoskanowania = DateTime.Now;
            sc.dataUtworzenia = DateTime.Now;

            sr.missingEntries = new List<MissingBackwards>();

            var watch = Stopwatch.StartNew();
            var techEntry = _db.Technical.IgnoreQueryFilters().FirstOrDefault(c => c.PrzewodCiety == sc.kodCiety);
            watch.Stop();
            _log.Debug($"Technical DB search time [{watch.ElapsedMilliseconds}]");
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
            else if(techEntry.Deleted == true)
            {
                response.Header = HeaderTypes.error;
                response.Flag = FlagType.isDeleted;
                return response;

            }
            else if(techEntry.KanBan == true)
            {
                response.Header = HeaderTypes.error;
                response.Flag = FlagType.isKanban;
                return response;
            }

            sc.Wiazka = techEntry.Wiazka;
            sc.Rodzina = techEntry.Rodzina;

            sr.PrzewodCiety = techEntry.PrzewodCiety;
            sr.BIN = techEntry.BIN;
            sr.KodWiazki = techEntry.KodWiazki;
            sr.LiteraRodziny = techEntry.LiterRodziny;
            sr.IlePrzewodow = techEntry.IlePrzewodow;

            watch.Restart();
            var dostawaEntry = _db.Dostawa.FirstOrDefault(c => c.Data.Date == sc.dataDostawy.Date && c.Kod == "PLC" + sc.kodCiety);
            watch.Stop();
            _log.Debug($"Delivery DB search time [{watch.ElapsedMilliseconds}]");
            watch.Restart();
            if (dostawaEntry != null)
            {
                sc.dataDostawy = dostawaEntry.Data;
                sc.dataDostawyOld = dostawaEntry.Data;
                sc.isFullSet = VTFuncs.CheckIfFullSetOfSupply(sc);
                sc.sztukiDeklarowane = dostawaEntry.Ilosc;
                sc.Declared = true;
                // Dostawa nie zawiera dosłanych
                if (sc.isFullSet)
                {
                    // if codes to complete set are missing check back
                    if (sc.sztukiSkanowane == sc.sztukiDeklarowane)
                    {
                        if (!VTFuncs.CheckBackOrAdd(response, techEntry, sc, dostawaEntry))
                            return response;
                    }
                    else if (sc.sztukiSkanowane != sc.sztukiDeklarowane)
                    {
                        if (!VTFuncs.CheckBackOrAddQuantityIncorrect(response, techEntry, sc, dostawaEntry))
                            return response;
                    }
                }
                else // Dostawa zawiera dosłane
                {
                    if (sc.sztukiSkanowane == sc.sztukiDeklarowane)
                    {
                        if (!VTFuncs.CheckBackOrAdd(response, techEntry, sc, dostawaEntry))
                            return response;
                    }
                    else if (sc.sztukiSkanowane != sc.sztukiDeklarowane)
                    {
                        if (!VTFuncs.CheckBackOrAddQuantityIncorrect(response, techEntry, sc, dostawaEntry))
                            return response;
                    }
                }
            }
            else
            {
                sc.dataDostawyOld = sc.dataDostawy;
                if (!sc.isForcedUndeclared)
                {
                    response.Header = HeaderTypes.error;
                    response.Flag = FlagType.notInDeclared;
                    return response;
                }
                else
                {
                    if(!sc.isForcedQuantity)
                    {
                        response.Header = HeaderTypes.error;
                        response.Flag = FlagType.quantityIncorrect;
                        response.Args.Add("0");
                        var vt = VTFuncs.ExistsInVT(sc);
                        if (vt != null)
                            response.Args.Add(vt.SztukiZeskanowane.ToString());
                        else
                            response.Args.Add("0");
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
            
            watch.Stop();
            _log.Debug($"Scan processing time [{watch.ElapsedMilliseconds}]");

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

            _log.Information("Scan details: {@sc}", sc);
            _log.Information("Response details: {@sr}", sr);

            return response;
        }

        protected CustomPacket getBin(CustomPacket packet)
        {
            CustomPacket response = new CustomPacket(FlagType.basic, HeaderTypes.basic, null, null, null);

            var BIN = _db.Technical.AsNoTracking().FirstOrDefault(c => c.PrzewodCiety == (string)packet.Payload).BIN;
            if(BIN == null)
            {
                response.Header = HeaderTypes.error;
                response.Flag = FlagType.binNotFound;
            }
            else
            {
                response.Payload = BIN;
            }
            return response;
        }
        protected CustomPacket showMissing(CustomPacket packet)
        {
            CustomPacket response = new CustomPacket(FlagType.basic, HeaderTypes.basic, null, new List<string>(), null);
            VTInsertFunctions vTInsert = new VTInsertFunctions(_db, _log);
            string cutcode = packet.Args[0];
            DateTime date = DateTime.Parse(packet.Args[1]);

            var techEntry = _db.Technical.FirstOrDefault(c => c.PrzewodCiety == cutcode);
            if(techEntry == null)
            {
                response.Flag = FlagType.notInTech;
                response.Header = HeaderTypes.error;
                return response;
            }
            else if(techEntry.KanBan)
            {
                response.Flag = FlagType.isKanban;
                response.Header = HeaderTypes.error;
                return response;
            }
            var wiazka = techEntry.Wiazka;
            var SetIDs = vTInsert.GetCompleteID(new ScannedCode { kodCiety = cutcode, dataDostawyOld = date });
            if (SetIDs.Count() == 0)
                SetIDs.Add(0);
            List<string> missingCodes = new List<string>();

            foreach (int setNumber in SetIDs)
            {
                var codesForWiazka = _db.Technical.Where(c => c.Wiazka == wiazka && c.KanBan == false).Select(c => c.PrzewodCiety).ToList();
                var scannedCodes = _db.VTMagazyn.Where(c => c.DataDostawy.Date == date.Date && codesForWiazka.Contains(c.KodCiety) && c.NumerKompletu == setNumber).Select(c => c.KodCiety).ToList();
                if (scannedCodes.Count() == 0)
                {
                    response.Flag = FlagType.nonScanned;
                    response.Header = HeaderTypes.error;
                    return response;
                }

                
                missingCodes.Add($"Brakujące kody dla wiązki {wiazka} komplet nr. {setNumber} po {vTInsert.GetPossibleDeclaredValue(new ScannedCode { kodCiety = cutcode, Wiazka = wiazka, dataDostawyOld = date}, setNumber)}");
                missingCodes.AddRange(codesForWiazka.Except(scannedCodes).ToList());
            }

            response.Args = missingCodes;

            return response;
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
            }

            watch.Stop();
            _log.Information($"Command {packet.Command} execution time [{watch.ElapsedMilliseconds}]");

            return responsePacket;
        }
    }
}
