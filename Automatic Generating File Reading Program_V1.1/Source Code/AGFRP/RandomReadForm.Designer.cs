namespace AGFRP
{
    partial class RandomReadForm
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.ElementTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.IndexBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.RandomlyReadRadioButton = new System.Windows.Forms.RadioButton();
            this.FullReadRadioButton = new System.Windows.Forms.RadioButton();
            this.Cancelbutton = new System.Windows.Forms.Button();
            this.SubmitButton = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.ElementTableLayoutPanel);
            this.panel1.Location = new System.Drawing.Point(23, 35);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(566, 76);
            this.panel1.TabIndex = 0;
            // 
            // ElementTableLayoutPanel
            // 
            this.ElementTableLayoutPanel.ColumnCount = 2;
            this.ElementTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.ElementTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.ElementTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.ElementTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ElementTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.ElementTableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.ElementTableLayoutPanel.Name = "ElementTableLayoutPanel";
            this.ElementTableLayoutPanel.RowCount = 1;
            this.ElementTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.ElementTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.ElementTableLayoutPanel.Size = new System.Drawing.Size(564, 74);
            this.ElementTableLayoutPanel.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(19, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 23);
            this.label1.TabIndex = 4;
            this.label1.Text = "Data item";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(19, 128);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 23);
            this.label2.TabIndex = 7;
            this.label2.Text = "Mode";
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.IndexBox);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.RandomlyReadRadioButton);
            this.panel2.Controls.Add(this.FullReadRadioButton);
            this.panel2.Location = new System.Drawing.Point(24, 154);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(302, 84);
            this.panel2.TabIndex = 9;
            // 
            // IndexBox
            // 
            this.IndexBox.Enabled = false;
            this.IndexBox.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IndexBox.Location = new System.Drawing.Point(214, 50);
            this.IndexBox.Name = "IndexBox";
            this.IndexBox.Size = new System.Drawing.Size(61, 25);
            this.IndexBox.TabIndex = 6;
            this.IndexBox.TextChanged += new System.EventHandler(this.IndexBox_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Times New Roman", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(165, 53);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 16);
            this.label3.TabIndex = 12;
            this.label3.Text = "Index: ";
            // 
            // RandomlyReadRadioButton
            // 
            this.RandomlyReadRadioButton.AutoSize = true;
            this.RandomlyReadRadioButton.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RandomlyReadRadioButton.Location = new System.Drawing.Point(165, 23);
            this.RandomlyReadRadioButton.Name = "RandomlyReadRadioButton";
            this.RandomlyReadRadioButton.Size = new System.Drawing.Size(118, 21);
            this.RandomlyReadRadioButton.TabIndex = 10;
            this.RandomlyReadRadioButton.TabStop = true;
            this.RandomlyReadRadioButton.Text = "Randomly read";
            this.RandomlyReadRadioButton.UseVisualStyleBackColor = true;
            this.RandomlyReadRadioButton.CheckedChanged += new System.EventHandler(this.RandomlyReadRadioButton_CheckedChanged);
            // 
            // FullReadRadioButton
            // 
            this.FullReadRadioButton.AutoSize = true;
            this.FullReadRadioButton.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FullReadRadioButton.Location = new System.Drawing.Point(16, 23);
            this.FullReadRadioButton.Name = "FullReadRadioButton";
            this.FullReadRadioButton.Size = new System.Drawing.Size(129, 21);
            this.FullReadRadioButton.TabIndex = 9;
            this.FullReadRadioButton.TabStop = true;
            this.FullReadRadioButton.Text = "Sequentially read";
            this.FullReadRadioButton.UseVisualStyleBackColor = true;
            this.FullReadRadioButton.CheckedChanged += new System.EventHandler(this.FullReadRadioButton_CheckedChanged);
            // 
            // Cancelbutton
            // 
            this.Cancelbutton.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Cancelbutton.Location = new System.Drawing.Point(482, 179);
            this.Cancelbutton.Name = "Cancelbutton";
            this.Cancelbutton.Size = new System.Drawing.Size(107, 34);
            this.Cancelbutton.TabIndex = 11;
            this.Cancelbutton.Text = "Cancel";
            this.Cancelbutton.UseVisualStyleBackColor = true;
            this.Cancelbutton.Click += new System.EventHandler(this.Cancelbutton_Click);
            // 
            // SubmitButton
            // 
            this.SubmitButton.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SubmitButton.Location = new System.Drawing.Point(352, 179);
            this.SubmitButton.Name = "SubmitButton";
            this.SubmitButton.Size = new System.Drawing.Size(107, 34);
            this.SubmitButton.TabIndex = 10;
            this.SubmitButton.Text = "Submit";
            this.SubmitButton.UseVisualStyleBackColor = true;
            this.SubmitButton.Click += new System.EventHandler(this.SubmitButton_Click);
            // 
            // RandomReadForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(615, 266);
            this.ControlBox = false;
            this.Controls.Add(this.Cancelbutton);
            this.Controls.Add(this.SubmitButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Name = "RandomReadForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Randomly reading data";
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.TableLayoutPanel ElementTableLayoutPanel;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button Cancelbutton;
        private System.Windows.Forms.Button SubmitButton;
        private System.Windows.Forms.RadioButton RandomlyReadRadioButton;
        private System.Windows.Forms.RadioButton FullReadRadioButton;
        public System.Windows.Forms.TextBox IndexBox;
        private System.Windows.Forms.Label label3;
    }
}