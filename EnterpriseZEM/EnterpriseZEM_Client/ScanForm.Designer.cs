namespace EnterpriseZEM_Client
{
    partial class ScanForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.kodWiazkiTextbox = new System.Windows.Forms.TextBox();
            this.recznieCheckbox = new System.Windows.Forms.CheckBox();
            this.dataSkanowaniaDateTime = new System.Windows.Forms.DateTimePicker();
            this.dokumentDostawyTextbox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label15 = new System.Windows.Forms.Label();
            this.nrKompletuLabel = new System.Windows.Forms.Label();
            this.zalegle_label = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.ilosc_label = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.datadostawynowa_label = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.datadostawystara_label = new System.Windows.Forms.Label();
            this.komplet_label = new System.Windows.Forms.Label();
            this.zeskanowane_label = new System.Windows.Forms.Label();
            this.kodwiazki_label = new System.Windows.Forms.Label();
            this.litera_label = new System.Windows.Forms.Label();
            this.bin_label = new System.Windows.Forms.Label();
            this.ciety_label = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lookBackCheckbox = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label14 = new System.Windows.Forms.Label();
            this.paperSizeComboBox = new System.Windows.Forms.ComboBox();
            this.label13 = new System.Windows.Forms.Label();
            this.printersComboBox = new System.Windows.Forms.ComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.userIdTextBox = new System.Windows.Forms.TextBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // kodWiazkiTextbox
            // 
            this.kodWiazkiTextbox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.kodWiazkiTextbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.kodWiazkiTextbox.Location = new System.Drawing.Point(760, 29);
            this.kodWiazkiTextbox.Name = "kodWiazkiTextbox";
            this.kodWiazkiTextbox.Size = new System.Drawing.Size(410, 45);
            this.kodWiazkiTextbox.TabIndex = 0;
            this.kodWiazkiTextbox.TextChanged += new System.EventHandler(this.kodWiazkiTextbox_TextChanged);
            this.kodWiazkiTextbox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.kodWiazkiTextbox_KeyDown);
            // 
            // recznieCheckbox
            // 
            this.recznieCheckbox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.recznieCheckbox.AutoSize = true;
            this.recznieCheckbox.Location = new System.Drawing.Point(786, 76);
            this.recznieCheckbox.Name = "recznieCheckbox";
            this.recznieCheckbox.Size = new System.Drawing.Size(114, 21);
            this.recznieCheckbox.TabIndex = 1;
            this.recznieCheckbox.Text = "wpisz ręcznie";
            this.recznieCheckbox.UseVisualStyleBackColor = true;
            // 
            // dataSkanowaniaDateTime
            // 
            this.dataSkanowaniaDateTime.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dataSkanowaniaDateTime.Location = new System.Drawing.Point(14, 20);
            this.dataSkanowaniaDateTime.Name = "dataSkanowaniaDateTime";
            this.dataSkanowaniaDateTime.Size = new System.Drawing.Size(157, 22);
            this.dataSkanowaniaDateTime.TabIndex = 2;
            this.dataSkanowaniaDateTime.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dataSkanowaniaDateTime_KeyDown);
            // 
            // dokumentDostawyTextbox
            // 
            this.dokumentDostawyTextbox.Location = new System.Drawing.Point(14, 65);
            this.dokumentDostawyTextbox.Name = "dokumentDostawyTextbox";
            this.dokumentDostawyTextbox.Size = new System.Drawing.Size(157, 22);
            this.dokumentDostawyTextbox.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(127, 17);
            this.label1.TabIndex = 4;
            this.label1.Text = "Dokument dostawy";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(116, 17);
            this.label2.TabIndex = 5;
            this.label2.Text = "Data skanowania";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(783, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(74, 17);
            this.label3.TabIndex = 6;
            this.label3.Text = "Kod wiązki";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.BackColor = System.Drawing.SystemColors.Control;
            this.panel1.Controls.Add(this.label15);
            this.panel1.Controls.Add(this.nrKompletuLabel);
            this.panel1.Controls.Add(this.zalegle_label);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.ilosc_label);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.datadostawynowa_label);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.label10);
            this.panel1.Controls.Add(this.label11);
            this.panel1.Controls.Add(this.datadostawystara_label);
            this.panel1.Controls.Add(this.komplet_label);
            this.panel1.Controls.Add(this.zeskanowane_label);
            this.panel1.Controls.Add(this.kodwiazki_label);
            this.panel1.Controls.Add(this.litera_label);
            this.panel1.Controls.Add(this.bin_label);
            this.panel1.Controls.Add(this.ciety_label);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1182, 769);
            this.panel1.TabIndex = 7;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.Location = new System.Drawing.Point(1095, 243);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(210, 39);
            this.label15.TabIndex = 37;
            this.label15.Text = "Nr. kompletu";
            // 
            // nrKompletuLabel
            // 
            this.nrKompletuLabel.AutoSize = true;
            this.nrKompletuLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 28F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nrKompletuLabel.Location = new System.Drawing.Point(1201, 281);
            this.nrKompletuLabel.Name = "nrKompletuLabel";
            this.nrKompletuLabel.Size = new System.Drawing.Size(360, 68);
            this.nrKompletuLabel.TabIndex = 36;
            this.nrKompletuLabel.Text = "Nr. kompletu";
            // 
            // zalegle_label
            // 
            this.zalegle_label.AutoSize = true;
            this.zalegle_label.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.zalegle_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.zalegle_label.ForeColor = System.Drawing.Color.Red;
            this.zalegle_label.Location = new System.Drawing.Point(963, 638);
            this.zalegle_label.Name = "zalegle_label";
            this.zalegle_label.Size = new System.Drawing.Size(364, 116);
            this.zalegle_label.TabIndex = 35;
            this.zalegle_label.Text = "zalegle";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(679, 243);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(99, 39);
            this.label8.TabIndex = 34;
            this.label8.Text = "Ilość:";
            // 
            // ilosc_label
            // 
            this.ilosc_label.AutoSize = true;
            this.ilosc_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 28F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ilosc_label.Location = new System.Drawing.Point(785, 281);
            this.ilosc_label.Name = "ilosc_label";
            this.ilosc_label.Size = new System.Drawing.Size(146, 68);
            this.ilosc_label.TabIndex = 33;
            this.ilosc_label.Text = "ilosc";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(10, 636);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(394, 39);
            this.label7.TabIndex = 32;
            this.label7.Text = "Zeskanowanych/komplet";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(679, 480);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(335, 39);
            this.label6.TabIndex = 31;
            this.label6.Text = "Data dostawy, nowa:";
            // 
            // datadostawynowa_label
            // 
            this.datadostawynowa_label.AutoSize = true;
            this.datadostawynowa_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 28F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.datadostawynowa_label.Location = new System.Drawing.Point(785, 519);
            this.datadostawynowa_label.Name = "datadostawynowa_label";
            this.datadostawynowa_label.Size = new System.Drawing.Size(529, 68);
            this.datadostawynowa_label.TabIndex = 30;
            this.datadostawynowa_label.Text = "data dostawy nowa";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(13, 480);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(330, 39);
            this.label5.TabIndex = 29;
            this.label5.Text = "Data dostawy, stara:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(13, 358);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(190, 39);
            this.label4.TabIndex = 28;
            this.label4.Text = "Kod wiązki:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(679, 141);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(231, 39);
            this.label9.TabIndex = 27;
            this.label9.Text = "Litera rodziny:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(13, 246);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(85, 39);
            this.label10.TabIndex = 26;
            this.label10.Text = "BIN:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(13, 141);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(168, 39);
            this.label11.TabIndex = 25;
            this.label11.Text = "Kod cięty:";
            // 
            // datadostawystara_label
            // 
            this.datadostawystara_label.AutoSize = true;
            this.datadostawystara_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 28F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.datadostawystara_label.Location = new System.Drawing.Point(119, 519);
            this.datadostawystara_label.Name = "datadostawystara_label";
            this.datadostawystara_label.Size = new System.Drawing.Size(520, 68);
            this.datadostawystara_label.TabIndex = 24;
            this.datadostawystara_label.Text = "data dostawy stara";
            // 
            // komplet_label
            // 
            this.komplet_label.AutoSize = true;
            this.komplet_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 28F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.komplet_label.Location = new System.Drawing.Point(119, 881);
            this.komplet_label.Name = "komplet_label";
            this.komplet_label.Size = new System.Drawing.Size(243, 68);
            this.komplet_label.TabIndex = 23;
            this.komplet_label.Text = "Komplet";
            // 
            // zeskanowane_label
            // 
            this.zeskanowane_label.AutoSize = true;
            this.zeskanowane_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 28F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.zeskanowane_label.Location = new System.Drawing.Point(116, 675);
            this.zeskanowane_label.Name = "zeskanowane_label";
            this.zeskanowane_label.Size = new System.Drawing.Size(523, 68);
            this.zeskanowane_label.TabIndex = 22;
            this.zeskanowane_label.Text = "zeskanowanych/ile";
            // 
            // kodwiazki_label
            // 
            this.kodwiazki_label.AutoSize = true;
            this.kodwiazki_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 28F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.kodwiazki_label.Location = new System.Drawing.Point(119, 396);
            this.kodwiazki_label.Name = "kodwiazki_label";
            this.kodwiazki_label.Size = new System.Drawing.Size(299, 68);
            this.kodwiazki_label.TabIndex = 21;
            this.kodwiazki_label.Text = "kod wiazki";
            // 
            // litera_label
            // 
            this.litera_label.AutoSize = true;
            this.litera_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 28F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.litera_label.Location = new System.Drawing.Point(785, 180);
            this.litera_label.Name = "litera_label";
            this.litera_label.Size = new System.Drawing.Size(155, 68);
            this.litera_label.TabIndex = 20;
            this.litera_label.Text = "litera";
            // 
            // bin_label
            // 
            this.bin_label.AutoSize = true;
            this.bin_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 28F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bin_label.Location = new System.Drawing.Point(119, 281);
            this.bin_label.Name = "bin_label";
            this.bin_label.Size = new System.Drawing.Size(106, 68);
            this.bin_label.TabIndex = 19;
            this.bin_label.Text = "bin";
            // 
            // ciety_label
            // 
            this.ciety_label.AutoSize = true;
            this.ciety_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 28F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ciety_label.Location = new System.Drawing.Point(119, 180);
            this.ciety_label.Name = "ciety_label";
            this.ciety_label.Size = new System.Drawing.Size(260, 68);
            this.ciety_label.TabIndex = 18;
            this.ciety_label.Text = "kod ciety";
            // 
            // panel2
            // 
            this.panel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panel2.Controls.Add(this.lookBackCheckbox);
            this.panel2.Controls.Add(this.button1);
            this.panel2.Controls.Add(this.label14);
            this.panel2.Controls.Add(this.paperSizeComboBox);
            this.panel2.Controls.Add(this.label13);
            this.panel2.Controls.Add(this.printersComboBox);
            this.panel2.Controls.Add(this.label12);
            this.panel2.Controls.Add(this.userIdTextBox);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.kodWiazkiTextbox);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.recznieCheckbox);
            this.panel2.Controls.Add(this.dokumentDostawyTextbox);
            this.panel2.Controls.Add(this.dataSkanowaniaDateTime);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1182, 100);
            this.panel2.TabIndex = 8;
            // 
            // lookBackCheckbox
            // 
            this.lookBackCheckbox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lookBackCheckbox.AutoSize = true;
            this.lookBackCheckbox.Checked = true;
            this.lookBackCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.lookBackCheckbox.Location = new System.Drawing.Point(1041, 76);
            this.lookBackCheckbox.Name = "lookBackCheckbox";
            this.lookBackCheckbox.Size = new System.Drawing.Size(114, 21);
            this.lookBackCheckbox.TabIndex = 14;
            this.lookBackCheckbox.Text = "wpisuj wstecz";
            this.lookBackCheckbox.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(590, 21);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(138, 42);
            this.button1.TabIndex = 13;
            this.button1.Text = "Więcej opcji";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(348, -2);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(120, 17);
            this.label14.TabIndex = 12;
            this.label14.Text = "Rozmiar wydruku:";
            // 
            // paperSizeComboBox
            // 
            this.paperSizeComboBox.FormattingEnabled = true;
            this.paperSizeComboBox.Location = new System.Drawing.Point(348, 18);
            this.paperSizeComboBox.Name = "paperSizeComboBox";
            this.paperSizeComboBox.Size = new System.Drawing.Size(165, 24);
            this.paperSizeComboBox.TabIndex = 11;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(177, -2);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(70, 17);
            this.label13.TabIndex = 10;
            this.label13.Text = "Drukarka:";
            // 
            // printersComboBox
            // 
            this.printersComboBox.FormattingEnabled = true;
            this.printersComboBox.Location = new System.Drawing.Point(177, 18);
            this.printersComboBox.Name = "printersComboBox";
            this.printersComboBox.Size = new System.Drawing.Size(165, 24);
            this.printersComboBox.TabIndex = 9;
            this.printersComboBox.SelectedIndexChanged += new System.EventHandler(this.printersComboBox_SelectedIndexChanged);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(177, 45);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(165, 17);
            this.label12.TabIndex = 8;
            this.label12.Text = "Identyfikator uzytkownika";
            // 
            // userIdTextBox
            // 
            this.userIdTextBox.Enabled = false;
            this.userIdTextBox.Location = new System.Drawing.Point(177, 65);
            this.userIdTextBox.Name = "userIdTextBox";
            this.userIdTextBox.Size = new System.Drawing.Size(165, 22);
            this.userIdTextBox.TabIndex = 7;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // ScanForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1182, 769);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.KeyPreview = true;
            this.Name = "ScanForm";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox kodWiazkiTextbox;
        private System.Windows.Forms.CheckBox recznieCheckbox;
        private System.Windows.Forms.DateTimePicker dataSkanowaniaDateTime;
        private System.Windows.Forms.TextBox dokumentDostawyTextbox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label zalegle_label;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label ilosc_label;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label datadostawynowa_label;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label datadostawystara_label;
        private System.Windows.Forms.Label komplet_label;
        private System.Windows.Forms.Label zeskanowane_label;
        private System.Windows.Forms.Label kodwiazki_label;
        private System.Windows.Forms.Label litera_label;
        private System.Windows.Forms.Label bin_label;
        private System.Windows.Forms.Label ciety_label;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox userIdTextBox;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.ComboBox printersComboBox;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.ComboBox paperSizeComboBox;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label nrKompletuLabel;
        private System.Windows.Forms.CheckBox lookBackCheckbox;
    }
}

