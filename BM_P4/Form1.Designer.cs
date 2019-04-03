namespace BM_P4
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.mainPBox = new System.Windows.Forms.PictureBox();
            this.sourcePBox = new System.Windows.Forms.PictureBox();
            this.sourceGBox = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.irisErosionButton = new System.Windows.Forms.Button();
            this.irisDilationButton = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.pupilErosionButton = new System.Windows.Forms.Button();
            this.pupilDilationButton = new System.Windows.Forms.Button();
            this.mainGBox = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mainPBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sourcePBox)).BeginInit();
            this.sourceGBox.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel1.Controls.Add(this.mainPBox, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.sourcePBox, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.mainGBox, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.sourceGBox, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(996, 606);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // mainPBox
            // 
            this.mainPBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainPBox.Image = global::BM_P4.Properties.Resources.Img_078_L_1_1;
            this.mainPBox.Location = new System.Drawing.Point(3, 3);
            this.mainPBox.Name = "mainPBox";
            this.mainPBox.Size = new System.Drawing.Size(591, 297);
            this.mainPBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.mainPBox.TabIndex = 0;
            this.mainPBox.TabStop = false;
            // 
            // sourcePBox
            // 
            this.sourcePBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sourcePBox.Image = global::BM_P4.Properties.Resources.Img_078_L_1_1;
            this.sourcePBox.Location = new System.Drawing.Point(3, 306);
            this.sourcePBox.Name = "sourcePBox";
            this.sourcePBox.Size = new System.Drawing.Size(591, 297);
            this.sourcePBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.sourcePBox.TabIndex = 1;
            this.sourcePBox.TabStop = false;
            // 
            // sourceGBox
            // 
            this.sourceGBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sourceGBox.Controls.Add(this.button1);
            this.sourceGBox.Controls.Add(this.groupBox3);
            this.sourceGBox.Controls.Add(this.groupBox4);
            this.sourceGBox.Location = new System.Drawing.Point(600, 306);
            this.sourceGBox.Name = "sourceGBox";
            this.sourceGBox.Size = new System.Drawing.Size(393, 297);
            this.sourceGBox.TabIndex = 3;
            this.sourceGBox.TabStop = false;
            this.sourceGBox.Text = "Edition";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.irisErosionButton);
            this.groupBox3.Controls.Add(this.irisDilationButton);
            this.groupBox3.Location = new System.Drawing.Point(23, 51);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(149, 90);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Iris";
            // 
            // irisErosionButton
            // 
            this.irisErosionButton.Location = new System.Drawing.Point(37, 48);
            this.irisErosionButton.Name = "irisErosionButton";
            this.irisErosionButton.Size = new System.Drawing.Size(75, 23);
            this.irisErosionButton.TabIndex = 3;
            this.irisErosionButton.Text = "Erosion";
            this.irisErosionButton.UseVisualStyleBackColor = true;
            // 
            // irisDilationButton
            // 
            this.irisDilationButton.Location = new System.Drawing.Point(37, 19);
            this.irisDilationButton.Name = "irisDilationButton";
            this.irisDilationButton.Size = new System.Drawing.Size(75, 23);
            this.irisDilationButton.TabIndex = 2;
            this.irisDilationButton.Text = "Dilation";
            this.irisDilationButton.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.pupilErosionButton);
            this.groupBox4.Controls.Add(this.pupilDilationButton);
            this.groupBox4.Location = new System.Drawing.Point(23, 147);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(149, 97);
            this.groupBox4.TabIndex = 2;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Pupil";
            // 
            // pupilErosionButton
            // 
            this.pupilErosionButton.Location = new System.Drawing.Point(37, 48);
            this.pupilErosionButton.Name = "pupilErosionButton";
            this.pupilErosionButton.Size = new System.Drawing.Size(75, 23);
            this.pupilErosionButton.TabIndex = 1;
            this.pupilErosionButton.Text = "Erosion";
            this.pupilErosionButton.UseVisualStyleBackColor = true;
            // 
            // pupilDilationButton
            // 
            this.pupilDilationButton.Location = new System.Drawing.Point(37, 19);
            this.pupilDilationButton.Name = "pupilDilationButton";
            this.pupilDilationButton.Size = new System.Drawing.Size(75, 23);
            this.pupilDilationButton.TabIndex = 0;
            this.pupilDilationButton.Text = "Dilation";
            this.pupilDilationButton.UseVisualStyleBackColor = true;
            // 
            // mainGBox
            // 
            this.mainGBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mainGBox.Location = new System.Drawing.Point(600, 3);
            this.mainGBox.Name = "mainGBox";
            this.mainGBox.Size = new System.Drawing.Size(393, 297);
            this.mainGBox.TabIndex = 2;
            this.mainGBox.TabStop = false;
            this.mainGBox.Text = "Source";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(23, 19);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(149, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "Compute both";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(996, 606);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mainPBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sourcePBox)).EndInit();
            this.sourceGBox.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.PictureBox mainPBox;
        private System.Windows.Forms.PictureBox sourcePBox;
        private System.Windows.Forms.GroupBox mainGBox;
        private System.Windows.Forms.GroupBox sourceGBox;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button irisErosionButton;
        private System.Windows.Forms.Button irisDilationButton;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button pupilErosionButton;
        private System.Windows.Forms.Button pupilDilationButton;
    }
}

