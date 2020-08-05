using EnterpriseZEM_Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EnterpriseZEM_Client
{
    public partial class AcceptChanges : Form
    {
        List<MissingBackwards> _missingCodes;

        public AcceptChanges(List<MissingBackwards> missingCodes)
        {
            InitializeComponent();
            _missingCodes = missingCodes;
            int i = 0;
            foreach(var missing in missingCodes)
            {
                CheckBox cb = new CheckBox();
                cb.Text = $"{missing.Data.ToShortDateString()}, {missing.Missing}, {missing.Zeskanowane}/{missing.Ilosc}, Komplet: {missing.NrKompletu}";
                cb.Location = new Point(10, i);
                i += 30;
                panel1.Controls.Add(cb);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int i = 0;
            foreach(var control in panel1.Controls)
            {
                if (((CheckBox)control).Checked)
                {
                    _missingCodes[i].Update = true;
                    i++;
                }
            }

            this.Close();
        }
    }
}
