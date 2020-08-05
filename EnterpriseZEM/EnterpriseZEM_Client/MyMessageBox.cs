using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EnterpriseZEM_Client
{
    class MyMessageBox
    {
        private static Button CreateButton(int locationX, string text, DialogResult dr)
        {
            Button b = new Button();
            b.Size = new Size(200, 50);
            b.Text = text;
            b.DialogResult = dr;
            b.Location = new Point(locationX, 320);

            return b;
        }

        public static Form CreateDialog(string message, MessageBoxButtons buttons)
        {
            Form form = new Form();
            form.Size = new System.Drawing.Size(700, 400);
            //form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.AutoSize = true;
            form.AutoSizeMode = AutoSizeMode.GrowAndShrink;

            Label label = new Label();
            label.Text = message;
            label.Size = new System.Drawing.Size(650, 200);
            label.Font = new System.Drawing.Font(FontFamily.GenericSansSerif, 22);
            label.Location = new Point(10, 10);
            form.Controls.Add(label);

            switch (buttons)
            {
                case MessageBoxButtons.OK:
                    form.Controls.Add(CreateButton(200, "OK", DialogResult.OK));
                    break;
                case MessageBoxButtons.YesNo:
                    form.Controls.Add(CreateButton(150, "Tak", DialogResult.Yes));
                    form.Controls.Add(CreateButton(400, "Nie", DialogResult.No));
                    break;
                case MessageBoxButtons.OKCancel:
                    form.Controls.Add(CreateButton(150, "OK", DialogResult.OK));
                    form.Controls.Add(CreateButton(400, "Anuluj", DialogResult.Cancel));
                    break;
                case MessageBoxButtons.YesNoCancel:
                    form.Controls.Add(CreateButton(10, "Tak", DialogResult.Yes));
                    form.Controls.Add(CreateButton(250, "Nie", DialogResult.No));
                    form.Controls.Add(CreateButton(450, "Anuluj", DialogResult.Cancel));
                    break;
            }

            return form;
        }
    }
}
