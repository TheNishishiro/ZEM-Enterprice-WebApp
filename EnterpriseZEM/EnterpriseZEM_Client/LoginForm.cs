using EnterpriseZEM_Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;
using static EnterpriseZEM_Common.Settings;

namespace EnterpriseZEM_Client
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void Login()
        {
            WebClient client = new WebClient();
            string response = "";
            client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
            StringBuilder urlBuilder = new StringBuilder();
            urlBuilder.Append($"{Settings.Properties[FieldTypes.AuthServerProtocol.ToString()]}://" +
                    $"{Settings.Properties[FieldTypes.AuthServerAddress.ToString()]}");
            if (Settings.Properties[FieldTypes.UsePort.ToString()] == "true")
                urlBuilder.Append($":" +
                    $"{Settings.Properties[FieldTypes.AuthServerPort.ToString()]}");
            urlBuilder.Append($"/api/auth/{usernameTextbox.Text},{passwordTextbox.Text}");

            if (Settings.Properties[FieldTypes.UseAuth.ToString()] == "false")
            {
                ScanForm SF = new ScanForm(usernameTextbox.Text, this);
                SF.Show();
                this.Hide();
            }
            else
            {

                string url = urlBuilder.ToString();
                try
                {
                    response = client.DownloadString(url);
                }
                catch (System.Net.WebException ex)
                {
                    MessageBox.Show("Nie można połączyć z serwerem uwierzytelnienia.");
                    return;
                }
                bool.TryParse(response, out bool result);

                if (result == true)
                {
                    ScanForm SF = new ScanForm(usernameTextbox.Text, this);
                    SF.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Logowanie nie powiodło się.");
                }
            }
        }

        private void loginButton_Click(object sender, EventArgs e)
        {
            Login();
        }

        private void passwordTextbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                Login();
        }

        private void usernameTextbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                Login();
        }
    }
}
