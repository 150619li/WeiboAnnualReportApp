namespace WeiboAnnualReportApp
{
    partial class FormMain
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btnopenReport = new Button();
            btoCreateReport = new Button();
            textBox1 = new TextBox();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            label6 = new Label();
            label7 = new Label();
            labelProgress = new Label();
            SuspendLayout();
            // 
            // btnopenReport
            // 
            btnopenReport.Location = new Point(385, 28);
            btnopenReport.Name = "btnopenReport";
            btnopenReport.Size = new Size(94, 50);
            btnopenReport.TabIndex = 0;
            btnopenReport.Text = "打开年度报告";
            btnopenReport.UseVisualStyleBackColor = true;
            btnopenReport.Click += btnOpenReport_Click;
            // 
            // btoCreateReport
            // 
            btoCreateReport.Location = new Point(258, 28);
            btoCreateReport.Margin = new Padding(2);
            btoCreateReport.Name = "btoCreateReport";
            btoCreateReport.Size = new Size(94, 50);
            btoCreateReport.TabIndex = 1;
            btoCreateReport.Text = "生成年度报告";
            btoCreateReport.UseVisualStyleBackColor = true;
            btoCreateReport.Click += btnCreateReport_Click;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(123, 42);
            textBox1.Margin = new Padding(2);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(96, 23);
            textBox1.TabIndex = 2;
            textBox1.TextChanged += textBox1_TextChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(16, 44);
            label1.Margin = new Padding(2, 0, 2, 0);
            label1.Name = "label1";
            label1.Size = new Size(105, 17);
            label1.TabIndex = 4;
            label1.Text = "输入用户ID或昵称";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(258, 135);
            label2.Margin = new Padding(2, 0, 2, 0);
            label2.Name = "label2";
            label2.Size = new Size(0, 17);
            label2.TabIndex = 5;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(258, 207);
            label3.Margin = new Padding(2, 0, 2, 0);
            label3.Name = "label3";
            label3.Size = new Size(0, 17);
            label3.TabIndex = 6;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(67, 274);
            label4.Margin = new Padding(2, 0, 2, 0);
            label4.Name = "label4";
            label4.Size = new Size(12, 17);
            label4.TabIndex = 7;
            label4.Text = "\u007f";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(258, 274);
            label5.Margin = new Padding(2, 0, 2, 0);
            label5.Name = "label5";
            label5.Size = new Size(0, 17);
            label5.TabIndex = 8;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(67, 135);
            label6.Margin = new Padding(2, 0, 2, 0);
            label6.Name = "label6";
            label6.Size = new Size(0, 17);
            label6.TabIndex = 9;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(67, 207);
            label7.Margin = new Padding(2, 0, 2, 0);
            label7.Name = "label7";
            label7.Size = new Size(0, 17);
            label7.TabIndex = 10;
            // 
            // labelProgress
            // 
            labelProgress.AutoSize = true;
            labelProgress.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            labelProgress.Location = new Point(17, 350);
            labelProgress.Name = "labelProgress";
            labelProgress.Size = new Size(104, 17);
            labelProgress.TabIndex = 11;
            labelProgress.Text = "请先生成年度报告";
            // 
            // FormMain
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(624, 386);
            Controls.Add(labelProgress);
            Controls.Add(label7);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(textBox1);
            Controls.Add(btoCreateReport);
            Controls.Add(btnopenReport);
            Name = "FormMain";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "欢迎来到微博年度报告";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnopenReport;
        private Button btoCreateReport;
        private TextBox textBox1;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label labelProgress;
    }
}
