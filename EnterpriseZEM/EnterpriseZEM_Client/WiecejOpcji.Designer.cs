namespace EnterpriseZEM_Client
{
    partial class WiecejOpcji
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.label4 = new System.Windows.Forms.Label();
            this.binLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.binCutCodeTextBox = new System.Windows.Forms.TextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.label3 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.missingDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.missingCutCodeTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(755, 459);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.AutoScroll = true;
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.binLabel);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.binCutCodeTextBox);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(747, 430);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Sprawdź BIN";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(699, 3);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(40, 17);
            this.label4.TabIndex = 3;
            this.label4.Text = "1.1.4";
            // 
            // binLabel
            // 
            this.binLabel.AutoSize = true;
            this.binLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.binLabel.Location = new System.Drawing.Point(151, 246);
            this.binLabel.Name = "binLabel";
            this.binLabel.Size = new System.Drawing.Size(147, 69);
            this.binLabel.TabIndex = 2;
            this.binLabel.Text = "BIN:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 22.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(250, 78);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(195, 44);
            this.label1.TabIndex = 1;
            this.label1.Text = "Kod Cięty:";
            // 
            // binCutCodeTextBox
            // 
            this.binCutCodeTextBox.Location = new System.Drawing.Point(172, 139);
            this.binCutCodeTextBox.Name = "binCutCodeTextBox";
            this.binCutCodeTextBox.Size = new System.Drawing.Size(345, 22);
            this.binCutCodeTextBox.TabIndex = 0;
            this.binCutCodeTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.binCutCodeTextBox_KeyDown);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Controls.Add(this.panel2);
            this.tabPage2.Controls.Add(this.panel1);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(747, 430);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Sprawdź brakujące";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(3, 74);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(183, 29);
            this.label3.TabIndex = 0;
            this.label3.Text = "Brakujące kody:";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.missingDateTimePicker);
            this.panel2.Controls.Add(this.missingCutCodeTextBox);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Location = new System.Drawing.Point(7, 6);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(710, 51);
            this.panel2.TabIndex = 1;
            // 
            // missingDateTimePicker
            // 
            this.missingDateTimePicker.CustomFormat = "";
            this.missingDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.missingDateTimePicker.Location = new System.Drawing.Point(425, 16);
            this.missingDateTimePicker.Name = "missingDateTimePicker";
            this.missingDateTimePicker.Size = new System.Drawing.Size(144, 22);
            this.missingDateTimePicker.TabIndex = 2;
            // 
            // missingCutCodeTextBox
            // 
            this.missingCutCodeTextBox.Location = new System.Drawing.Point(81, 16);
            this.missingCutCodeTextBox.Name = "missingCutCodeTextBox";
            this.missingCutCodeTextBox.Size = new System.Drawing.Size(327, 22);
            this.missingCutCodeTextBox.TabIndex = 1;
            this.missingCutCodeTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.missingCutCodeTextBox_KeyDown);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(3, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 17);
            this.label2.TabIndex = 0;
            this.label2.Text = "Kod Cięty:";
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.AutoScroll = true;
            this.panel1.Location = new System.Drawing.Point(13, 106);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(711, 294);
            this.panel1.TabIndex = 0;
            // 
            // WiecejOpcji
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(755, 459);
            this.Controls.Add(this.tabControl1);
            this.Name = "WiecejOpcji";
            this.Text = "WiecejOpcji";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label binLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox binCutCodeTextBox;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TextBox missingCutCodeTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DateTimePicker missingDateTimePicker;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
    }
}