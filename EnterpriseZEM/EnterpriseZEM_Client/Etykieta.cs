using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EnterpriseZEM_Client
{
    public partial class Etykieta : Form
    {
        PrintDocument printdoc;
        Bitmap MemoryImage;

        public Etykieta(string Rodzina, string KodLiterowy, string KodCyfrowy, int NaWiazke, int NrKompletu, string Wiazka, string BIN, DateTime DataDostawy, int WDostawie)
        {
            InitializeComponent();

            RodzinaLabel.Text = Rodzina;
            kodLiterowyLabel.Text = KodLiterowy;
            kodCyfrowyLabel.Text = KodCyfrowy;
            naWiazkiLabel.Text = NaWiazke.ToString();
            kompletLabel.Text = NrKompletu.ToString();
            wiazkaLabel.Text = Wiazka;
            binLabel.Text = BIN;
            dataDostawyLabel.Text = DataDostawy.ToShortDateString();
            wDostawieLabel.Text = WDostawie.ToString();

            printdoc = new PrintDocument();
            printdoc.PrintPage += new PrintPageEventHandler(printPage);
        }

        public void Print(string printerName, int paperSize)
        {
            GetPrintArea(panel1);
            printdoc.PrinterSettings.PrinterName = printerName;
            printdoc.DefaultPageSettings.PaperSize = printdoc.PrinterSettings.PaperSizes[paperSize];
            printdoc.DefaultPageSettings.Color = false;
            printdoc.DefaultPageSettings.Landscape = true;
            printdoc.PrintController = new StandardPrintController();
            printdoc.Print();
            this.Close();
        }
        private void GetPrintArea(Panel pnl)
        {
            MemoryImage = new Bitmap(pnl.Width, pnl.Height);
            pnl.DrawToBitmap(MemoryImage, new Rectangle(0, 0, pnl.Width, pnl.Height));
        }

        private void printPage(object sender, PrintPageEventArgs e)
        {
            e.Graphics.DrawImage(MemoryImage, 0, 0);
        }
    }
}
