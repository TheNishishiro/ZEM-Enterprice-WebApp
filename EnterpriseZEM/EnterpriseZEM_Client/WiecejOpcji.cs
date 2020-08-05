using EnterpriseZEM_Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Resources;
using System.Text;
using System.Windows.Forms;

namespace EnterpriseZEM_Client
{
    public partial class WiecejOpcji : Form
    {
        Client _client;
        public WiecejOpcji(Client client)
        {
            InitializeComponent();
            _client = client;
        }

        private void binCutCodeTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                string code = binCutCodeTextBox.Text.Trim().Replace("PLC", "");
                if (code != "")
                {
                    CustomPacket cp = new CustomPacket(FlagType.basic, HeaderTypes.basic, "getBIN", null, code);
                    CustomPacket response = _client.SendReceiveMessage(cp);

                    if (response.Header == HeaderTypes.error)
                    {
                        if (response.Flag == FlagType.binNotFound)
                        {
                            MessageBox.Show("Nie znaleziono BINu dla podanego kodu, sprawdź poprawność wpisanego kodu ciętego.", "Nie znaleziono", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                        else
                        {
                            MessageBox.Show("Serwer zwrócił nieznany błąd.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    if (response.Payload != null)
                    {
                        binLabel.Text = $"BIN: {(string)response.Payload}";
                    }
                    else
                    {
                        MessageBox.Show("Otrzymano pustą odpowiedź z serwera", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void missingCutCodeTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            string inputCode = missingCutCodeTextBox.Text.Replace("PLC", "").Trim();

            if (e.KeyCode == Keys.Enter && inputCode != "")
            {
                CustomPacket cp = new CustomPacket(FlagType.basic, HeaderTypes.basic, "showMissing", new List<string>(), null);
                cp.Args.Add(inputCode);
                cp.Args.Add(missingDateTimePicker.Value.ToShortDateString());

                CustomPacket response = _client.SendReceiveMessage(cp);
                if(response.Header == HeaderTypes.error)
                {
                    if(response.Flag == FlagType.notInTech)
                    {
                        MessageBox.Show("Nie znaleziono kodu w wykazie wiązek, sprawdź poprawność wpisanego kodu ciętego.", "Nie znaleziono", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    else if(response.Flag == FlagType.nonScanned)
                    {
                        MessageBox.Show("Żaden kod należący do tej wiązki nie był zeskanowany.", "Nie znaleziono", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }


                panel1.Controls.Clear();
                int y = 0;
                foreach (var code in response.Args)
                {
                    Label codeEntry = new Label();
                    codeEntry.Text = code;

                    if (!code.StartsWith("Brakuj"))
                    {
                        codeEntry.Font = new Font("Microsoft Sans Serif", 16, FontStyle.Regular);
                        codeEntry.Location = new Point(30, y);
                    }
                    else
                    {
                        codeEntry.Font = new Font("Microsoft Sans Serif", 20, FontStyle.Regular);
                        codeEntry.Location = new Point(10, y);
                    }
                    
                    codeEntry.AutoSize = true;
                    y += 30;
                    panel1.Controls.Add(codeEntry);
                }
            }
        }
    }
}
