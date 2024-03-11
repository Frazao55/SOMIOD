namespace App2
{
    partial class Form1
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
            this.label1 = new System.Windows.Forms.Label();
            this.btnFanOn = new System.Windows.Forms.Button();
            this.btnFanOff = new System.Windows.Forms.Button();
            this.buttonLowIntensity = new System.Windows.Forms.Button();
            this.buttonMediumIntensity = new System.Windows.Forms.Button();
            this.buttonHighIntensity = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(139, 9);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(231, 48);
            this.label1.TabIndex = 3;
            this.label1.Text = "Fan Control";
            // 
            // btnFanOn
            // 
            this.btnFanOn.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F);
            this.btnFanOn.Location = new System.Drawing.Point(7, 22);
            this.btnFanOn.Margin = new System.Windows.Forms.Padding(4);
            this.btnFanOn.Name = "btnFanOn";
            this.btnFanOn.Size = new System.Drawing.Size(100, 30);
            this.btnFanOn.TabIndex = 4;
            this.btnFanOn.Text = "Fan ON";
            this.btnFanOn.UseVisualStyleBackColor = true;
            this.btnFanOn.Click += new System.EventHandler(this.btnFanOn_Click);
            // 
            // btnFanOff
            // 
            this.btnFanOff.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F);
            this.btnFanOff.Location = new System.Drawing.Point(115, 22);
            this.btnFanOff.Margin = new System.Windows.Forms.Padding(4);
            this.btnFanOff.Name = "btnFanOff";
            this.btnFanOff.Size = new System.Drawing.Size(100, 30);
            this.btnFanOff.TabIndex = 5;
            this.btnFanOff.Text = "Fan OFF";
            this.btnFanOff.UseVisualStyleBackColor = true;
            this.btnFanOff.Click += new System.EventHandler(this.btnFanOff_Click);
            // 
            // buttonLowIntensity
            // 
            this.buttonLowIntensity.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonLowIntensity.Location = new System.Drawing.Point(7, 22);
            this.buttonLowIntensity.Margin = new System.Windows.Forms.Padding(4);
            this.buttonLowIntensity.Name = "buttonLowIntensity";
            this.buttonLowIntensity.Size = new System.Drawing.Size(100, 30);
            this.buttonLowIntensity.TabIndex = 6;
            this.buttonLowIntensity.Text = "Low";
            this.buttonLowIntensity.UseVisualStyleBackColor = true;
            this.buttonLowIntensity.Click += new System.EventHandler(this.buttonLowIntensity_Click);
            // 
            // buttonMediumIntensity
            // 
            this.buttonMediumIntensity.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonMediumIntensity.Location = new System.Drawing.Point(115, 22);
            this.buttonMediumIntensity.Margin = new System.Windows.Forms.Padding(4);
            this.buttonMediumIntensity.Name = "buttonMediumIntensity";
            this.buttonMediumIntensity.Size = new System.Drawing.Size(100, 30);
            this.buttonMediumIntensity.TabIndex = 7;
            this.buttonMediumIntensity.Text = "Medium";
            this.buttonMediumIntensity.UseVisualStyleBackColor = true;
            this.buttonMediumIntensity.Click += new System.EventHandler(this.buttonMediumIntensity_Click);
            // 
            // buttonHighIntensity
            // 
            this.buttonHighIntensity.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonHighIntensity.Location = new System.Drawing.Point(223, 22);
            this.buttonHighIntensity.Margin = new System.Windows.Forms.Padding(4);
            this.buttonHighIntensity.Name = "buttonHighIntensity";
            this.buttonHighIntensity.Size = new System.Drawing.Size(100, 30);
            this.buttonHighIntensity.TabIndex = 8;
            this.buttonHighIntensity.Text = "High";
            this.buttonHighIntensity.UseVisualStyleBackColor = true;
            this.buttonHighIntensity.Click += new System.EventHandler(this.buttonHighIntensity_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnFanOff);
            this.groupBox1.Controls.Add(this.btnFanOn);
            this.groupBox1.Location = new System.Drawing.Point(139, 92);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(225, 64);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Power";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.buttonHighIntensity);
            this.groupBox2.Controls.Add(this.buttonMediumIntensity);
            this.groupBox2.Controls.Add(this.buttonLowIntensity);
            this.groupBox2.Location = new System.Drawing.Point(99, 188);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(338, 64);
            this.groupBox2.TabIndex = 11;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Intensity";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(536, 282);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form1";
            this.Text = "App B";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnFanOn;
        private System.Windows.Forms.Button btnFanOff;
        private System.Windows.Forms.Button buttonLowIntensity;
        private System.Windows.Forms.Button buttonMediumIntensity;
        private System.Windows.Forms.Button buttonHighIntensity;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
    }
}

