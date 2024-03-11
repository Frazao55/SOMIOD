namespace AppEvaluation
{
    partial class AppEvaluation
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
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.listBoxApplications = new System.Windows.Forms.ListBox();
            this.listBoxSubscriptions = new System.Windows.Forms.ListBox();
            this.listBoxContainers = new System.Windows.Forms.ListBox();
            this.listBoxData = new System.Windows.Forms.ListBox();
            this.btnAdd01 = new System.Windows.Forms.Button();
            this.btnEdit01 = new System.Windows.Forms.Button();
            this.btnRemove01 = new System.Windows.Forms.Button();
            this.btnDelete02 = new System.Windows.Forms.Button();
            this.btnAdd02 = new System.Windows.Forms.Button();
            this.btnDelete04 = new System.Windows.Forms.Button();
            this.btnAdd04 = new System.Windows.Forms.Button();
            this.btnRemove02 = new System.Windows.Forms.Button();
            this.btnEdit03 = new System.Windows.Forms.Button();
            this.btnAdd03 = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxInformation = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(48, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(141, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "Applications";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(350, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(155, 25);
            this.label2.TabIndex = 1;
            this.label2.Text = "Subscriptions";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(48, 294);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(126, 25);
            this.label3.TabIndex = 2;
            this.label3.Text = "Containers";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(350, 294);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(61, 25);
            this.label4.TabIndex = 3;
            this.label4.Text = "Data";
            // 
            // listBoxApplications
            // 
            this.listBoxApplications.FormattingEnabled = true;
            this.listBoxApplications.Location = new System.Drawing.Point(33, 54);
            this.listBoxApplications.Name = "listBoxApplications";
            this.listBoxApplications.Size = new System.Drawing.Size(264, 173);
            this.listBoxApplications.TabIndex = 4;
            this.listBoxApplications.SelectedIndexChanged += new System.EventHandler(this.listBoxApplications_SelectedIndexChanged);
            // 
            // listBoxSubscriptions
            // 
            this.listBoxSubscriptions.FormattingEnabled = true;
            this.listBoxSubscriptions.Location = new System.Drawing.Point(333, 54);
            this.listBoxSubscriptions.Name = "listBoxSubscriptions";
            this.listBoxSubscriptions.Size = new System.Drawing.Size(264, 173);
            this.listBoxSubscriptions.TabIndex = 5;
            this.listBoxSubscriptions.SelectedIndexChanged += new System.EventHandler(this.listBoxSubscriptions_SelectedIndexChanged);
            // 
            // listBoxContainers
            // 
            this.listBoxContainers.FormattingEnabled = true;
            this.listBoxContainers.Location = new System.Drawing.Point(33, 322);
            this.listBoxContainers.Name = "listBoxContainers";
            this.listBoxContainers.Size = new System.Drawing.Size(264, 173);
            this.listBoxContainers.TabIndex = 6;
            this.listBoxContainers.SelectedIndexChanged += new System.EventHandler(this.listBoxContainers_SelectedIndexChanged);
            // 
            // listBoxData
            // 
            this.listBoxData.FormattingEnabled = true;
            this.listBoxData.Location = new System.Drawing.Point(333, 322);
            this.listBoxData.Name = "listBoxData";
            this.listBoxData.Size = new System.Drawing.Size(264, 173);
            this.listBoxData.TabIndex = 7;
            this.listBoxData.SelectedIndexChanged += new System.EventHandler(this.listBoxData_SelectedIndexChanged);
            // 
            // btnAdd01
            // 
            this.btnAdd01.Location = new System.Drawing.Point(42, 244);
            this.btnAdd01.Name = "btnAdd01";
            this.btnAdd01.Size = new System.Drawing.Size(76, 31);
            this.btnAdd01.TabIndex = 8;
            this.btnAdd01.Text = "Add";
            this.btnAdd01.UseVisualStyleBackColor = true;
            this.btnAdd01.Click += new System.EventHandler(this.btnAdd01_Click);
            // 
            // btnEdit01
            // 
            this.btnEdit01.Location = new System.Drawing.Point(124, 244);
            this.btnEdit01.Name = "btnEdit01";
            this.btnEdit01.Size = new System.Drawing.Size(76, 31);
            this.btnEdit01.TabIndex = 9;
            this.btnEdit01.Text = "Edit";
            this.btnEdit01.UseVisualStyleBackColor = true;
            this.btnEdit01.Click += new System.EventHandler(this.btnEdit01_Click);
            // 
            // btnRemove01
            // 
            this.btnRemove01.Location = new System.Drawing.Point(206, 244);
            this.btnRemove01.Name = "btnRemove01";
            this.btnRemove01.Size = new System.Drawing.Size(76, 31);
            this.btnRemove01.TabIndex = 10;
            this.btnRemove01.Text = "Remove";
            this.btnRemove01.UseVisualStyleBackColor = true;
            this.btnRemove01.Click += new System.EventHandler(this.btnRemove01_Click);
            // 
            // btnDelete02
            // 
            this.btnDelete02.Location = new System.Drawing.Point(459, 244);
            this.btnDelete02.Name = "btnDelete02";
            this.btnDelete02.Size = new System.Drawing.Size(76, 31);
            this.btnDelete02.TabIndex = 12;
            this.btnDelete02.Text = "Delete";
            this.btnDelete02.UseVisualStyleBackColor = true;
            this.btnDelete02.Click += new System.EventHandler(this.btnDelete03_Click);
            // 
            // btnAdd02
            // 
            this.btnAdd02.Location = new System.Drawing.Point(377, 244);
            this.btnAdd02.Name = "btnAdd02";
            this.btnAdd02.Size = new System.Drawing.Size(76, 31);
            this.btnAdd02.TabIndex = 11;
            this.btnAdd02.Text = "Add";
            this.btnAdd02.UseVisualStyleBackColor = true;
            this.btnAdd02.Click += new System.EventHandler(this.btnAdd02_Click);
            // 
            // btnDelete04
            // 
            this.btnDelete04.Location = new System.Drawing.Point(459, 501);
            this.btnDelete04.Name = "btnDelete04";
            this.btnDelete04.Size = new System.Drawing.Size(76, 31);
            this.btnDelete04.TabIndex = 17;
            this.btnDelete04.Text = "Delete";
            this.btnDelete04.UseVisualStyleBackColor = true;
            this.btnDelete04.Click += new System.EventHandler(this.btnDelete04_Click);
            // 
            // btnAdd04
            // 
            this.btnAdd04.Location = new System.Drawing.Point(377, 501);
            this.btnAdd04.Name = "btnAdd04";
            this.btnAdd04.Size = new System.Drawing.Size(76, 31);
            this.btnAdd04.TabIndex = 16;
            this.btnAdd04.Text = "Add";
            this.btnAdd04.UseVisualStyleBackColor = true;
            this.btnAdd04.Click += new System.EventHandler(this.btnAdd04_Click);
            // 
            // btnRemove02
            // 
            this.btnRemove02.Location = new System.Drawing.Point(206, 501);
            this.btnRemove02.Name = "btnRemove02";
            this.btnRemove02.Size = new System.Drawing.Size(76, 31);
            this.btnRemove02.TabIndex = 15;
            this.btnRemove02.Text = "Remove";
            this.btnRemove02.UseVisualStyleBackColor = true;
            this.btnRemove02.Click += new System.EventHandler(this.btnRemove02_Click);
            // 
            // btnEdit03
            // 
            this.btnEdit03.Location = new System.Drawing.Point(124, 501);
            this.btnEdit03.Name = "btnEdit03";
            this.btnEdit03.Size = new System.Drawing.Size(76, 31);
            this.btnEdit03.TabIndex = 14;
            this.btnEdit03.Text = "Edit";
            this.btnEdit03.UseVisualStyleBackColor = true;
            this.btnEdit03.Click += new System.EventHandler(this.btnEdit03_Click);
            // 
            // btnAdd03
            // 
            this.btnAdd03.Location = new System.Drawing.Point(42, 501);
            this.btnAdd03.Name = "btnAdd03";
            this.btnAdd03.Size = new System.Drawing.Size(76, 31);
            this.btnAdd03.TabIndex = 13;
            this.btnAdd03.Text = "Add";
            this.btnAdd03.UseVisualStyleBackColor = true;
            this.btnAdd03.Click += new System.EventHandler(this.btnAdd03_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(48, 565);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(129, 25);
            this.label5.TabIndex = 18;
            this.label5.Text = "Information";
            // 
            // textBoxInformation
            // 
            this.textBoxInformation.Location = new System.Drawing.Point(53, 593);
            this.textBoxInformation.Multiline = true;
            this.textBoxInformation.Name = "textBoxInformation";
            this.textBoxInformation.ReadOnly = true;
            this.textBoxInformation.Size = new System.Drawing.Size(544, 226);
            this.textBoxInformation.TabIndex = 19;
            // 
            // AppEvaluation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(630, 857);
            this.Controls.Add(this.textBoxInformation);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.btnDelete04);
            this.Controls.Add(this.btnAdd04);
            this.Controls.Add(this.btnRemove02);
            this.Controls.Add(this.btnEdit03);
            this.Controls.Add(this.btnAdd03);
            this.Controls.Add(this.btnDelete02);
            this.Controls.Add(this.btnAdd02);
            this.Controls.Add(this.btnRemove01);
            this.Controls.Add(this.btnEdit01);
            this.Controls.Add(this.btnAdd01);
            this.Controls.Add(this.listBoxData);
            this.Controls.Add(this.listBoxContainers);
            this.Controls.Add(this.listBoxSubscriptions);
            this.Controls.Add(this.listBoxApplications);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "AppEvaluation";
            this.Text = "AppEvaluation";
            this.Load += new System.EventHandler(this.AppEvaluation_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ListBox listBoxApplications;
        private System.Windows.Forms.ListBox listBoxSubscriptions;
        private System.Windows.Forms.ListBox listBoxContainers;
        private System.Windows.Forms.ListBox listBoxData;
        private System.Windows.Forms.Button btnAdd01;
        private System.Windows.Forms.Button btnEdit01;
        private System.Windows.Forms.Button btnRemove01;
        private System.Windows.Forms.Button btnDelete02;
        private System.Windows.Forms.Button btnAdd02;
        private System.Windows.Forms.Button btnDelete04;
        private System.Windows.Forms.Button btnAdd04;
        private System.Windows.Forms.Button btnRemove02;
        private System.Windows.Forms.Button btnEdit03;
        private System.Windows.Forms.Button btnAdd03;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxInformation;
    }
}

