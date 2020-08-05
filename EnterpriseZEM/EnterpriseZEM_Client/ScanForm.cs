using EnterpriseZEM_Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Windows.Forms;
using static EnterpriseZEM_Common.Settings;

namespace EnterpriseZEM_Client
{
    public partial class ScanForm : Form
    {
        Client _client;
        
        LoginForm _LF;

        public ScanForm(string username, LoginForm LF)
        {
            InitializeComponent();
            _client = new Client(int.Parse(Settings.Properties[FieldTypes.ServerPort.ToString()]), Settings.Properties[FieldTypes.ServerAddress.ToString()]);
            _client.Connect();
            dataSkanowaniaDateTime.Value = DateTime.Now;
            userIdTextBox.Text = username;

            foreach (string s in PrinterSettings.InstalledPrinters)
            {
                printersComboBox.Items.Add(s);
            }

            
            _LF = LF;
        }

        public void UpdateForm()
        {
            ScannedCode sc = new ScannedCode();

            string code = kodWiazkiTextbox.Text.Replace("PLC", "").ToUpper().Trim();
            if (code.Length <= 8)
            {
                MessageBox.Show($"Podany kod jest za krótki ({code.Length} znaków), powinien składać się z przynajmniej 9 znaków wykluczjąc przedrostek PLC", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string cutcode = code.Substring(0, 8);
            int.TryParse(code.Substring(8), out int quantity);
            if (quantity == 0)
            {
                MessageBox.Show("Podana ilość kabli nie jest wartością liczbową.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            sc.isLookingBack = lookBackCheckbox.Checked;
            sc.kodCiety = cutcode;
            sc.sztukiSkanowane = quantity;
            sc.dataDostawy = dataSkanowaniaDateTime.Value;
            sc.DokDostawy = dokumentDostawyTextbox.Text;
            sc.isForcedQuantity = false;
            sc.isForcedOverLimit = false;
            sc.isForcedBackAck = false;
            sc.isForcedBack = false;
            sc.isForcedInsert = false;
            sc.isForcedUndeclared = false;
            sc.User = userIdTextBox.Text;
            sc.missingEntries = new List<MissingBackwards>();

            CustomPacket cp = new CustomPacket(FlagType.basic, HeaderTypes.basic, "scan", null, sc);
            CustomPacket response = _client.SendReceiveMessage(cp);

            bool shouldExit = false;

            while(response.Header == HeaderTypes.error)
            {
                if (response.Flag == FlagType.notInTech)
                {
                    MyMessageBox.CreateDialog("Kod nie znaleziony w bazie technicznych", MessageBoxButtons.OK).ShowDialog();
                    shouldExit = true;
                }
                else if(response.Flag == FlagType.quantityIncorrect)
                {
                    Form form = MyMessageBox.CreateDialog($"Deklarowana ilość ({response.Args[0]}) nie zgadza się ze zeskanowaną {quantity} (aktualnie zeskanowanych: {response.Args[1]}), zatwierdzasz, zmień lub kliknij \"Nie\" by anulować.", MessageBoxButtons.YesNo);
                    TextBox txBox = new TextBox();
                    txBox.Location = new Point(10, 220);
                    txBox.Font = new Font(FontFamily.GenericSansSerif, 16);
                    txBox.Size = new Size(form.Size.Width, 50);
                    txBox.Text = quantity.ToString();
                    txBox.Name = "quantitytextbox";
                    form.Controls.Add(txBox);

                    if (form.ShowDialog() == DialogResult.Yes)
                    {
                        sc.sztukiSkanowane = int.Parse((((TextBox)form.Controls.Find("quantitytextbox", false).First()).Text));
                        sc.isForcedQuantity = true;
                        response = _client.SendReceiveMessage(cp);
                    }
                    else
                    {
                        shouldExit = true;
                    }
                }
                else if (response.Flag == FlagType.quantityOverLimit)
                {

                    if (MyMessageBox.CreateDialog($"Po dodaniu rekordu ilość przewodów przekroczy deklarowaną ilość ({response.Args[1]}) do ({response.Args[0]}), kontynuować?", MessageBoxButtons.YesNo).ShowDialog() == DialogResult.Yes)
                    {
                        sc.isForcedOverLimit = true;
                        response = _client.SendReceiveMessage(cp);
                    }
                    else
                    {
                        shouldExit = true;
                    }
                }
                else if (response.Flag == FlagType.codeFitsBack)
                {
                    ((ScannedCode)cp.Payload).missingEntries = ((ScannedResponse)response.Payload).missingEntries;
                    AcceptChanges AC = new AcceptChanges(((ScannedCode)cp.Payload).missingEntries);
                    AC.ShowDialog();
                    response = _client.SendReceiveMessage(cp);
                }
                else if(response.Flag == FlagType.codeExists)
                {
                    if (MyMessageBox.CreateDialog("Kod został dziś zeskanowany, upewnij się że zeskanowałeś poprawny kod, dodać do bazy?", MessageBoxButtons.YesNo).ShowDialog() == DialogResult.Yes)
                    {
                        sc.isForcedInsert = true;
                        response = _client.SendReceiveMessage(cp);
                    }
                    else
                    {
                        shouldExit = true;
                    }
                }
                else if (response.Flag == FlagType.codeExistsBack)
                {
                    if (MyMessageBox.CreateDialog("Kod został dziś zeskanowany i dodany wstecz, upewnij się że zeskanowałeś poprawny kod, dodać do bazy?", MessageBoxButtons.YesNo).ShowDialog() == DialogResult.Yes)
                    {
                        sc.isForcedInsert = true;
                        response = _client.SendReceiveMessage(cp);
                    }
                    else
                    {
                        shouldExit = true;
                    }
                }
                else if (response.Flag == FlagType.notInDeclared)
                {
                    if (MyMessageBox.CreateDialog("Kod nie znaleziony w dokumencie dostawy, dodać mimo to?", MessageBoxButtons.YesNo).ShowDialog() == DialogResult.Yes)
                    {
                        sc.isForcedUndeclared = true;
                        response = _client.SendReceiveMessage(cp);
                    }
                    else
                    {
                        shouldExit = true;
                    }
                }
                else if(response.Flag == FlagType.isKanban)
                {
                    MyMessageBox.CreateDialog("Zeskanowany kod należy do przewodu KanBan.", MessageBoxButtons.OK).ShowDialog();
                    shouldExit = true;
                }
                else if (response.Flag == FlagType.isDeleted)
                {
                    MyMessageBox.CreateDialog("Zeskanowany kod nie powinien być już skanowany, został on usunięty z bazy technicznych.", MessageBoxButtons.OK).ShowDialog();
                    shouldExit = true;
                }
                if (shouldExit)
                    break;
            }

            kodWiazkiTextbox.SelectAll();
            kodWiazkiTextbox.Text = "";
            kodWiazkiTextbox.Focus();

            

            ScannedResponse sr = (ScannedResponse)response.Payload;

            ciety_label.Text = sr.PrzewodCiety;
            bin_label.Text = sr.BIN;
            kodwiazki_label.Text = sr.KodWiazki;
            litera_label.Text = sr.LiteraRodziny;
            zeskanowane_label.Text = $"{sr.numScannedToComplete}/{sr.numToComplete}";
            ilosc_label.Text = $"{sr.sztukiSkanowane}/{sr.sztukiDeklatowane}";
            nrKompletuLabel.Text = sr.numerKompletu.ToString();

            if (sr.isComplete == true)
            {
                komplet_label.Text = "Komplet: Tak";
                komplet_label.ForeColor = Color.Green;
                panel1.BackColor = Color.FromArgb(192, 255, 192);
            }
            else
            {
                panel1.BackColor = Color.FromArgb(255, 192, 192);
                komplet_label.Text = "Komplet: Nie";
                komplet_label.ForeColor = Color.Red;
            }


            datadostawystara_label.Text = sr.DataDostawy.ToShortDateString();
            datadostawynowa_label.Text = sr.DataDostawyOld.ToShortDateString();
            
            if (sr.DataDostawyOld.Date < sr.DataDostawy.Date)
            {
                zalegle_label.Visible = true;

            }
            else
            {
                zalegle_label.Visible = false;
            }

            if(sr.numScanned == 1)
            {
                Etykieta etykieta = new Etykieta(
                    sr.Rodzina, 
                    sr.LiteraRodziny, 
                    sr.KodWiazki,
                    sr.numToComplete,
                    sr.numerKompletu,
                    sr.Wiazka,
                    sr.BIN,
                    sr.DataDostawyOld,
                    sr.sztukiDeklatowane);
                etykieta.Show();
                etykieta.Print(printersComboBox.Text, paperSizeComboBox.SelectedIndex);
            }
        }
        
        private void kodWiazkiTextbox_TextChanged(object sender, EventArgs e)
        {
            if(!recznieCheckbox.Checked && kodWiazkiTextbox.Text != "")
            {
                UpdateForm();
            }
        }

        private void kodWiazkiTextbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                UpdateForm();
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            _client.Disconnect();
            _LF.Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (dataSkanowaniaDateTime.Value != DateTime.MinValue && 
                dokumentDostawyTextbox.Text != "" && 
                userIdTextBox.Text != "" &&
                printersComboBox.SelectedIndex != -1 &&
                paperSizeComboBox.SelectedIndex != -1)
            {
                panel1.Visible = true;
                kodWiazkiTextbox.Enabled = true;
            }
            else
            {
                panel1.Visible = false;
                kodWiazkiTextbox.Enabled = false;
            }
        }

        private void dataSkanowaniaDateTime_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.OemSemicolon)
                dataSkanowaniaDateTime.Value = DateTime.Now;
        }

        private void printersComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            PrintDocument pd = new PrintDocument();
            pd.PrinterSettings.PrinterName = printersComboBox.Text;
            PaperSize pkSize;
            paperSizeComboBox.Items.Clear();
            for (int i = 0; i < pd.PrinterSettings.PaperSizes.Count; i++)
            {
                pkSize = pd.PrinterSettings.PaperSizes[i];
                paperSizeComboBox.Items.Add(pkSize.Kind);
            }
            paperSizeComboBox.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            WiecejOpcji wo = new WiecejOpcji(_client);
            wo.Show();
        }
    }
}
