using System.Windows.Forms;

namespace AdminConsole
{
    partial class Admin_Console
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
            this.button17 = new System.Windows.Forms.Button();
            this.button16 = new System.Windows.Forms.Button();
            this.button15 = new System.Windows.Forms.Button();
            this.button14 = new System.Windows.Forms.Button();
            this.button13 = new System.Windows.Forms.Button();
            this.comboBoxParametros = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.comboBoxFiltro = new System.Windows.Forms.ComboBox();
            this.button12 = new System.Windows.Forms.Button();
            this.button11 = new System.Windows.Forms.Button();
            this.listBox2 = new System.Windows.Forms.ListBox();
            this.label9 = new System.Windows.Forms.Label();
            this.button10 = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.button9 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.button8 = new System.Windows.Forms.Button();
            this.label13 = new System.Windows.Forms.Label();
            this.button18 = new System.Windows.Forms.Button();
            this.button19 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button17
            // 
            this.button17.Location = new System.Drawing.Point(913, 427);
            this.button17.Name = "button17";
            this.button17.Size = new System.Drawing.Size(79, 21);
            this.button17.TabIndex = 98;
            this.button17.Text = "Exit";
            this.button17.UseVisualStyleBackColor = true;
            this.button17.Click += new System.EventHandler(this.button17_Click);
            // 
            // button16
            // 
            this.button16.Location = new System.Drawing.Point(94, 434);
            this.button16.Name = "button16";
            this.button16.Size = new System.Drawing.Size(79, 21);
            this.button16.TabIndex = 97;
            this.button16.Text = "Logout";
            this.button16.UseVisualStyleBackColor = true;
            this.button16.Click += new System.EventHandler(this.button16_Click);
            // 
            // button15
            // 
            this.button15.Location = new System.Drawing.Point(584, 594);
            this.button15.Name = "button15";
            this.button15.Size = new System.Drawing.Size(90, 21);
            this.button15.TabIndex = 96;
            this.button15.Text = "Export to Excel";
            this.button15.UseVisualStyleBackColor = true;
            // 
            // button14
            // 
            this.button14.Location = new System.Drawing.Point(476, 594);
            this.button14.Name = "button14";
            this.button14.Size = new System.Drawing.Size(79, 21);
            this.button14.TabIndex = 95;
            this.button14.Text = "Export to Xml";
            this.button14.UseVisualStyleBackColor = true;
            // 
            // button13
            // 
            this.button13.Location = new System.Drawing.Point(493, 551);
            this.button13.Name = "button13";
            this.button13.Size = new System.Drawing.Size(72, 21);
            this.button13.TabIndex = 94;
            this.button13.Text = "Search";
            this.button13.UseVisualStyleBackColor = true;
            this.button13.Click += new System.EventHandler(this.button13_Click);
            // 
            // comboBoxParametros
            // 
            this.comboBoxParametros.FormattingEnabled = true;
            this.comboBoxParametros.Location = new System.Drawing.Point(583, 519);
            this.comboBoxParametros.Name = "comboBoxParametros";
            this.comboBoxParametros.Size = new System.Drawing.Size(104, 21);
            this.comboBoxParametros.TabIndex = 93;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(486, 522);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(66, 13);
            this.label10.TabIndex = 92;
            this.label10.Text = "Parameters :";
            // 
            // comboBoxFiltro
            // 
            this.comboBoxFiltro.FormattingEnabled = true;
            this.comboBoxFiltro.Location = new System.Drawing.Point(583, 487);
            this.comboBoxFiltro.Name = "comboBoxFiltro";
            this.comboBoxFiltro.Size = new System.Drawing.Size(104, 21);
            this.comboBoxFiltro.TabIndex = 91;
            this.comboBoxFiltro.SelectedIndexChanged += new System.EventHandler(this.comboBoxFiltro_SelectedIndexChanged);
            // 
            // button12
            // 
            this.button12.Location = new System.Drawing.Point(469, 486);
            this.button12.Name = "button12";
            this.button12.Size = new System.Drawing.Size(96, 21);
            this.button12.TabIndex = 90;
            this.button12.Text = "Select Filter Type";
            this.button12.UseVisualStyleBackColor = true;
            this.button12.Click += new System.EventHandler(this.button12_Click);
            // 
            // button11
            // 
            this.button11.Location = new System.Drawing.Point(493, 452);
            this.button11.Name = "button11";
            this.button11.Size = new System.Drawing.Size(72, 21);
            this.button11.TabIndex = 89;
            this.button11.Text = "Show All";
            this.button11.UseVisualStyleBackColor = true;
            this.button11.Click += new System.EventHandler(this.button11_Click);
            // 
            // listBox2
            // 
            this.listBox2.FormattingEnabled = true;
            this.listBox2.Location = new System.Drawing.Point(314, 181);
            this.listBox2.Name = "listBox2";
            this.listBox2.Size = new System.Drawing.Size(519, 238);
            this.listBox2.TabIndex = 88;
            this.listBox2.SelectedIndexChanged += new System.EventHandler(this.listBox2_SelectedIndexChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Segoe UI", 18F);
            this.label9.Location = new System.Drawing.Point(471, 112);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(177, 32);
            this.label9.TabIndex = 87;
            this.label9.Text = "Operation Logs";
            // 
            // button10
            // 
            this.button10.Location = new System.Drawing.Point(83, 380);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(104, 21);
            this.button10.TabIndex = 86;
            this.button10.Text = "Set New Values";
            this.button10.UseVisualStyleBackColor = true;
            this.button10.Click += new System.EventHandler(this.button10_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(91, 305);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(98, 13);
            this.label8.TabIndex = 85;
            this.label8.Text = "Transaction Values";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(146, 352);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(97, 20);
            this.textBox2.TabIndex = 84;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(146, 329);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(97, 20);
            this.textBox1.TabIndex = 83;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(59, 355);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(83, 13);
            this.label7.TabIndex = 82;
            this.label7.Text = "Debito Maximo :";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(56, 331);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(85, 13);
            this.label6.TabIndex = 81;
            this.label6.Text = "Credito Maximo :";
            // 
            // button9
            // 
            this.button9.Location = new System.Drawing.Point(899, 322);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(111, 33);
            this.button9.TabIndex = 80;
            this.button9.Text = "Disable/Enable Account";
            this.button9.UseVisualStyleBackColor = true;
            this.button9.Click += new System.EventHandler(this.button9_Click);
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(899, 294);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(111, 21);
            this.button7.TabIndex = 78;
            this.button7.Text = "Create Account";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(899, 187);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(111, 21);
            this.button6.TabIndex = 77;
            this.button6.Text = "Change Email";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(899, 161);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(111, 21);
            this.button5.TabIndex = 76;
            this.button5.Text = "Change Name";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(899, 216);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(111, 21);
            this.button4.TabIndex = 75;
            this.button4.Text = "Change Password";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 18F);
            this.label5.Location = new System.Drawing.Point(866, 112);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(187, 32);
            this.label5.TabIndex = 74;
            this.label5.Text = "Account Control";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(168, 162);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 13);
            this.label4.TabIndex = 73;
            this.label4.Text = "Bank Status";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(899, 268);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(111, 21);
            this.button3.TabIndex = 72;
            this.button3.Text = "Delete Account";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(899, 242);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(111, 21);
            this.button2.TabIndex = 71;
            this.button2.Text = "Change Account";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(151, 249);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(104, 35);
            this.button1.TabIndex = 70;
            this.button1.Text = "Check Connectivity";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(151, 187);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(104, 56);
            this.listBox1.TabIndex = 69;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(495, 80);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 13);
            this.label3.TabIndex = 68;
            this.label3.Text = "Welcome ******";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 31F);
            this.label2.Location = new System.Drawing.Point(353, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(441, 57);
            this.label2.TabIndex = 67;
            this.label2.Text = "Administrator Console";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 18F);
            this.label1.Location = new System.Drawing.Point(39, 112);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(167, 32);
            this.label1.TabIndex = 66;
            this.label1.Text = "Banks & Entities";
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(22, 181);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(99, 20);
            this.textBox3.TabIndex = 99;
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(22, 243);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(99, 20);
            this.textBox4.TabIndex = 100;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(19, 162);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(110, 13);
            this.label11.TabIndex = 101;
            this.label11.Text = "Add New Bank Name";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(33, 206);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(77, 13);
            this.label12.TabIndex = 102;
            this.label12.Text = "Insert endpoint";
            this.label12.Click += new System.EventHandler(this.label12_Click);
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(22, 269);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(99, 27);
            this.button8.TabIndex = 103;
            this.button8.Text = "Add";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(28, 223);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(93, 13);
            this.label13.TabIndex = 104;
            this.label13.Text = "Ex : bank_2/bank";
            // 
            // button18
            // 
            this.button18.Location = new System.Drawing.Point(571, 551);
            this.button18.Name = "button18";
            this.button18.Size = new System.Drawing.Size(77, 21);
            this.button18.TabIndex = 105;
            this.button18.Text = "Unsubscribe";
            this.button18.UseVisualStyleBackColor = true;
            this.button18.Click += new System.EventHandler(this.button18_Click);
            // 
            // button19
            // 
            this.button19.Location = new System.Drawing.Point(583, 452);
            this.button19.Name = "button19";
            this.button19.Size = new System.Drawing.Size(72, 21);
            this.button19.TabIndex = 106;
            this.button19.Text = "Clean Screen";
            this.button19.UseVisualStyleBackColor = true;
            this.button19.Click += new System.EventHandler(this.button19_Click);
            // 
            // Admin_Console
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1086, 636);
            this.Controls.Add(this.button19);
            this.Controls.Add(this.button18);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.button8);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.textBox4);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.button17);
            this.Controls.Add(this.button16);
            this.Controls.Add(this.button15);
            this.Controls.Add(this.button14);
            this.Controls.Add(this.button13);
            this.Controls.Add(this.comboBoxParametros);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.comboBoxFiltro);
            this.Controls.Add(this.button12);
            this.Controls.Add(this.button11);
            this.Controls.Add(this.listBox2);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.button10);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.button9);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "Admin_Console";
            this.Text = "Form2";
            this.Load += new System.EventHandler(this.AdminConsole_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button button17;
        private Button button16;
        private Button button15;
        private Button button14;
        private Button button13;
        private ComboBox comboBoxParametros;
        private Label label10;
        private ComboBox comboBoxFiltro;
        private Button button12;
        private Button button11;
        private ListBox listBox2;
        private Label label9;
        private Button button10;
        private Label label8;
        private TextBox textBox2;
        private TextBox textBox1;
        private Label label7;
        private Label label6;
        private Button button9;
        private Button button7;
        private Button button6;
        private Button button5;
        private Button button4;
        private Label label5;
        private Label label4;
        private Button button3;
        private Button button2;
        private Button button1;
        private ListBox listBox1;
        private Label label3;
        private Label label2;
        private Label label1;
        private TextBox textBox3;
        private TextBox textBox4;
        private Label label11;
        private Label label12;
        private Button button8;
        private Label label13;
        private Button button18;
        private Button button19;
    }
}