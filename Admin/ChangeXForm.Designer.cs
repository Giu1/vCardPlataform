using System.Windows.Forms;

namespace Admin
{
    partial class ChangeXForm
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
            this.Xlabel = new System.Windows.Forms.Label();
            this.x2Label = new System.Windows.Forms.Label();
            this.InsertLabel = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.labelx2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Xlabel
            // 
            this.Xlabel.AutoSize = true;
            this.Xlabel.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Xlabel.Location = new System.Drawing.Point(115, 15);
            this.Xlabel.Name = "Xlabel";
            this.Xlabel.Size = new System.Drawing.Size(26, 30);
            this.Xlabel.TabIndex = 10;
            this.Xlabel.Text = "X";
            this.Xlabel.Click += new System.EventHandler(this.label2_Click);
            // 
            // x2Label
            // 
            this.x2Label.AutoSize = true;
            this.x2Label.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.x2Label.Location = new System.Drawing.Point(71, 45);
            this.x2Label.Name = "x2Label";
            this.x2Label.Size = new System.Drawing.Size(149, 30);
            this.x2Label.TabIndex = 11;
            this.x2Label.Text = "Change Menu";
            // 
            // InsertLabel
            // 
            this.InsertLabel.AutoSize = true;
            this.InsertLabel.Location = new System.Drawing.Point(71, 158);
            this.InsertLabel.Name = "InsertLabel";
            this.InsertLabel.Size = new System.Drawing.Size(64, 15);
            this.InsertLabel.TabIndex = 12;
            this.InsertLabel.Text = "InsertLabel";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(71, 176);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(156, 23);
            this.textBox1.TabIndex = 13;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(52, 237);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(208, 15);
            this.label2.TabIndex = 14;
            this.label2.Text = "Insert Secret Password of this Account";
            this.label2.Click += new System.EventHandler(this.label2_Click_1);
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(71, 270);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(156, 23);
            this.textBox2.TabIndex = 15;
            this.textBox2.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(104, 252);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(97, 15);
            this.label3.TabIndex = 16;
            this.label3.Text = " to Verify Change";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(104, 334);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(97, 23);
            this.button1.TabIndex = 17;
            this.button1.Text = "Change";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(104, 119);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 15);
            this.label4.TabIndex = 18;
            this.label4.Text = "Current :";
            this.label4.Click += new System.EventHandler(this.label4_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(173, 119);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(14, 15);
            this.label5.TabIndex = 19;
            this.label5.Text = "X";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(71, 93);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(96, 15);
            this.label6.TabIndex = 20;
            this.label6.Text = "Account Owner :";
            this.label6.Click += new System.EventHandler(this.label6_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(104, 379);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(97, 23);
            this.button2.TabIndex = 21;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(117, 306);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(70, 15);
            this.label7.TabIndex = 22;
            this.label7.Text = "Status Label";
            this.label7.Visible = false;
            // 
            // labelx2
            // 
            this.labelx2.AutoSize = true;
            this.labelx2.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.labelx2.Location = new System.Drawing.Point(91, 15);
            this.labelx2.Name = "labelx2";
            this.labelx2.Size = new System.Drawing.Size(26, 30);
            this.labelx2.TabIndex = 23;
            this.labelx2.Text = "X";
            this.labelx2.Visible = false;
            // 
            // ChangeXForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(310, 450);
            this.Controls.Add(this.labelx2);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.InsertLabel);
            this.Controls.Add(this.x2Label);
            this.Controls.Add(this.Xlabel);
            this.Name = "ChangeXForm";
            this.Text = "ChangeXForm";
            this.Load += new System.EventHandler(this.ChangeXForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label Xlabel;
        private Label x2Label;
        private Label InsertLabel;
        private TextBox textBox1;
        private Label label2;
        private TextBox textBox2;
        private Label label3;
        private Button button1;
        private Label label4;
        private Label label5;
        private Label label6;
        private Button button2;
        private Label label7;
        private Label labelx2;
    }
}