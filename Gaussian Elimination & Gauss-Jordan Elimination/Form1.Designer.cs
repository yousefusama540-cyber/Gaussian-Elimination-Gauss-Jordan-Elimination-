namespace WinFormsApp1
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            lblRows = new Label();
            lblCols = new Label();
            numRows = new NumericUpDown();
            numCols = new NumericUpDown();
            btnGenerateMatrix = new Button();
            panelMatrix = new Panel();
            btnSolve = new Button();
            txtOutput = new RichTextBox();
            ((System.ComponentModel.ISupportInitialize)numRows).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numCols).BeginInit();
            SuspendLayout();
            // 
            // lblRows
            // 
            lblRows.AutoSize = true;
            lblRows.Location = new Point(4, 18);
            lblRows.Name = "lblRows";
            lblRows.Size = new Size(74, 20);
            lblRows.TabIndex = 0;
            lblRows.Text = "Rows (m):";
            // 
            // lblCols
            // 
            lblCols.AutoSize = true;
            lblCols.Location = new Point(140, 20);
            lblCols.Name = "lblCols";
            lblCols.Size = new Size(91, 20);
            lblCols.TabIndex = 2;
            lblCols.Text = "Columns (n):";
            // 
            // numRows
            // 
            numRows.Location = new Point(84, 13);
            numRows.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numRows.Name = "numRows";
            numRows.Size = new Size(50, 27);
            numRows.TabIndex = 1;
            numRows.Value = new decimal(new int[] { 3, 0, 0, 0 });
            // 
            // numCols
            // 
            numCols.Location = new Point(237, 14);
            numCols.Minimum = new decimal(new int[] { 2, 0, 0, 0 });
            numCols.Name = "numCols";
            numCols.Size = new Size(50, 27);
            numCols.TabIndex = 3;
            numCols.Value = new decimal(new int[] { 4, 0, 0, 0 });
            // 
            // btnGenerateMatrix
            // 
            btnGenerateMatrix.Location = new Point(317, 16);
            btnGenerateMatrix.Name = "btnGenerateMatrix";
            btnGenerateMatrix.Size = new Size(120, 25);
            btnGenerateMatrix.TabIndex = 4;
            btnGenerateMatrix.Text = "Generate Matrix";
            btnGenerateMatrix.Click += btnGenerateMatrix_Click;
            // 
            // panelMatrix
            // 
            panelMatrix.AutoScroll = true;
            panelMatrix.BorderStyle = BorderStyle.FixedSingle;
            panelMatrix.Location = new Point(12, 46);
            panelMatrix.Name = "panelMatrix";
            panelMatrix.Size = new Size(425, 180);
            panelMatrix.TabIndex = 5;
            // 
            // btnSolve
            // 
            btnSolve.Location = new Point(23, 249);
            btnSolve.Name = "btnSolve";
            btnSolve.Size = new Size(400, 30);
            btnSolve.TabIndex = 6;
            btnSolve.Text = "Solve (Gaussian & Gauss-Jordan)";
            btnSolve.Click += btnSolve_Click;
            // 
            // txtOutput
            // 
            txtOutput.Font = new Font("Consolas", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtOutput.Location = new Point(443, 12);
            txtOutput.Name = "txtOutput";
            txtOutput.ReadOnly = true;
            txtOutput.Size = new Size(470, 521);
            txtOutput.TabIndex = 7;
            txtOutput.Text = "";
            // 
            // Form1
            // 
            ClientSize = new Size(915, 522);
            Controls.Add(lblRows);
            Controls.Add(numRows);
            Controls.Add(lblCols);
            Controls.Add(numCols);
            Controls.Add(btnGenerateMatrix);
            Controls.Add(panelMatrix);
            Controls.Add(btnSolve);
            Controls.Add(txtOutput);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Gauss-Jordan Elimination Solver";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)numRows).EndInit();
            ((System.ComponentModel.ISupportInitialize)numCols).EndInit();
            ResumeLayout(false);
            PerformLayout();

        }

        private System.Windows.Forms.Label lblRows;
        private System.Windows.Forms.Label lblCols;
        private System.Windows.Forms.NumericUpDown numRows;
        private System.Windows.Forms.NumericUpDown numCols;
        private System.Windows.Forms.Button btnGenerateMatrix;
        private System.Windows.Forms.Panel panelMatrix;
        private System.Windows.Forms.Button btnSolve;
        private System.Windows.Forms.RichTextBox txtOutput;
    }
}