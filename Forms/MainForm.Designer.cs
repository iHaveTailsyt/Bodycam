
namespace Bodycam.Forms
{
    partial class Test
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
            this.Warning = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Warning
            // 
            this.Warning.AutoSize = true;
            this.Warning.Location = new System.Drawing.Point(248, 216);
            this.Warning.Name = "Warning";
            this.Warning.Size = new System.Drawing.Size(433, 13);
            this.Warning.TabIndex = 0;
            this.Warning.Text = "WARNING THIS DOES NOT RECORD IT IS HERE FOR ROLEPLAY PURPOSES ONLY";
            this.Warning.Click += new System.EventHandler(this.label1_Click);
            // 
            // Test
            // 
            this.ClientSize = new System.Drawing.Size(906, 488);
            this.Controls.Add(this.Warning);
            this.Name = "Test";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label Warning;
    }
}

