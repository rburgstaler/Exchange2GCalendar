namespace Ex2GCal
{
    partial class Ex2GCalForm
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
            this.button1 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btDeleteAll = new System.Windows.Forms.Button();
            this.btListAll = new System.Windows.Forms.Button();
            this.btFindCalendars = new System.Windows.Forms.Button();
            this.btSaveGoogle = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.tbCalendar = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbClientSecret = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tbClientID = new System.Windows.Forms.TextBox();
            this.btSynch = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tbExchangeURL = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tbExchangePassword = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tbExchangeUserName = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(28, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(138, 33);
            this.button1.TabIndex = 0;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Location = new System.Drawing.Point(13, 250);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox1.Size = new System.Drawing.Size(1097, 418);
            this.textBox1.TabIndex = 1;
            this.textBox1.WordWrap = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btDeleteAll);
            this.groupBox1.Controls.Add(this.btListAll);
            this.groupBox1.Controls.Add(this.btFindCalendars);
            this.groupBox1.Controls.Add(this.btSaveGoogle);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.tbCalendar);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.tbClientSecret);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.tbClientID);
            this.groupBox1.Location = new System.Drawing.Point(667, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(429, 169);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Google";
            // 
            // btDeleteAll
            // 
            this.btDeleteAll.Location = new System.Drawing.Point(247, 140);
            this.btDeleteAll.Name = "btDeleteAll";
            this.btDeleteAll.Size = new System.Drawing.Size(75, 23);
            this.btDeleteAll.TabIndex = 9;
            this.btDeleteAll.Text = "Delete All";
            this.btDeleteAll.UseVisualStyleBackColor = true;
            this.btDeleteAll.Click += new System.EventHandler(this.btDeleteAll_Click);
            // 
            // btListAll
            // 
            this.btListAll.Location = new System.Drawing.Point(121, 140);
            this.btListAll.Name = "btListAll";
            this.btListAll.Size = new System.Drawing.Size(75, 23);
            this.btListAll.TabIndex = 8;
            this.btListAll.Text = "List All";
            this.btListAll.UseVisualStyleBackColor = true;
            this.btListAll.Click += new System.EventHandler(this.btListAll_Click);
            // 
            // btFindCalendars
            // 
            this.btFindCalendars.Location = new System.Drawing.Point(395, 107);
            this.btFindCalendars.Name = "btFindCalendars";
            this.btFindCalendars.Size = new System.Drawing.Size(28, 23);
            this.btFindCalendars.TabIndex = 7;
            this.btFindCalendars.Text = "...";
            this.btFindCalendars.UseVisualStyleBackColor = true;
            this.btFindCalendars.Click += new System.EventHandler(this.btFindCalendars_Click);
            // 
            // btSaveGoogle
            // 
            this.btSaveGoogle.Location = new System.Drawing.Point(6, 140);
            this.btSaveGoogle.Name = "btSaveGoogle";
            this.btSaveGoogle.Size = new System.Drawing.Size(75, 23);
            this.btSaveGoogle.TabIndex = 6;
            this.btSaveGoogle.Text = "Save";
            this.btSaveGoogle.UseVisualStyleBackColor = true;
            this.btSaveGoogle.Click += new System.EventHandler(this.btSaveGoogle_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 91);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Calendar";
            // 
            // tbCalendar
            // 
            this.tbCalendar.Location = new System.Drawing.Point(6, 107);
            this.tbCalendar.Name = "tbCalendar";
            this.tbCalendar.Size = new System.Drawing.Size(382, 20);
            this.tbCalendar.TabIndex = 4;
            this.tbCalendar.TextChanged += new System.EventHandler(this.Param_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Client Secret";
            // 
            // tbClientSecret
            // 
            this.tbClientSecret.Location = new System.Drawing.Point(6, 68);
            this.tbClientSecret.Name = "tbClientSecret";
            this.tbClientSecret.Size = new System.Drawing.Size(382, 20);
            this.tbClientSecret.TabIndex = 2;
            this.tbClientSecret.TextChanged += new System.EventHandler(this.Param_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Client ID";
            // 
            // tbClientID
            // 
            this.tbClientID.Location = new System.Drawing.Point(6, 29);
            this.tbClientID.Name = "tbClientID";
            this.tbClientID.Size = new System.Drawing.Size(382, 20);
            this.tbClientID.TabIndex = 0;
            this.tbClientID.TextChanged += new System.EventHandler(this.Param_TextChanged);
            // 
            // btSynch
            // 
            this.btSynch.Location = new System.Drawing.Point(28, 64);
            this.btSynch.Name = "btSynch";
            this.btSynch.Size = new System.Drawing.Size(138, 35);
            this.btSynch.TabIndex = 3;
            this.btSynch.Text = "Synch";
            this.btSynch.UseVisualStyleBackColor = true;
            this.btSynch.Click += new System.EventHandler(this.btSynch_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.tbExchangeURL);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.tbExchangePassword);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.tbExchangeUserName);
            this.groupBox2.Location = new System.Drawing.Point(191, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(429, 169);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Exchange";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 91);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "URL";
            // 
            // tbExchangeURL
            // 
            this.tbExchangeURL.Location = new System.Drawing.Point(6, 107);
            this.tbExchangeURL.Name = "tbExchangeURL";
            this.tbExchangeURL.Size = new System.Drawing.Size(382, 20);
            this.tbExchangeURL.TabIndex = 4;
            this.tbExchangeURL.TextChanged += new System.EventHandler(this.Param_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 52);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 13);
            this.label5.TabIndex = 3;
            this.label5.Text = "Password";
            // 
            // tbExchangePassword
            // 
            this.tbExchangePassword.Location = new System.Drawing.Point(6, 68);
            this.tbExchangePassword.Name = "tbExchangePassword";
            this.tbExchangePassword.PasswordChar = '*';
            this.tbExchangePassword.Size = new System.Drawing.Size(382, 20);
            this.tbExchangePassword.TabIndex = 2;
            this.tbExchangePassword.TextChanged += new System.EventHandler(this.Param_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 13);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(55, 13);
            this.label6.TabIndex = 1;
            this.label6.Text = "Username";
            // 
            // tbExchangeUserName
            // 
            this.tbExchangeUserName.Location = new System.Drawing.Point(6, 29);
            this.tbExchangeUserName.Name = "tbExchangeUserName";
            this.tbExchangeUserName.Size = new System.Drawing.Size(382, 20);
            this.tbExchangeUserName.TabIndex = 0;
            this.tbExchangeUserName.TextChanged += new System.EventHandler(this.Param_TextChanged);
            // 
            // Ex2GCalForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1122, 680);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.btSynch);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button1);
            this.Name = "Ex2GCalForm";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbCalendar;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbClientSecret;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbClientID;
        private System.Windows.Forms.Button btSaveGoogle;
        private System.Windows.Forms.Button btFindCalendars;
        private System.Windows.Forms.Button btDeleteAll;
        private System.Windows.Forms.Button btListAll;
        private System.Windows.Forms.Button btSynch;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbExchangeURL;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbExchangePassword;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tbExchangeUserName;
    }
}

