using System;
using System.Drawing;
using System.Windows.Forms;

namespace Bodycam.Forms
{
    public partial class MicrophoneForm : Form
    {
        public MicrophoneForm()
        {
            InitializeComponent();
            MicroPhoneForm();
        }

        private void MicroPhoneForm()
        {
            this.ClientSize = new Size(400, 200);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Microphone Access Required";
            this.BackColor = Color.White;

            // Create and configure the instruction label
            Label instructionLabel = new Label
            {
                Text = "Microphone access is required for the bodycam to function.\n" +
                       "Please enable microphone access in your system settings and restart the app or contact your department HC.\n\n" +
                       "To enable microphone access:\n" +
                       "1. Go to Settings > Privacy > Microphone.\n" +
                       "2. Turn on 'Allow apps to access your microphone.'\n" +
                       "3. Restart the bodycam.",
                AutoSize = true,
                Location = new Point(10, 10),
                ForeColor = Color.Black,
                TextAlign = ContentAlignment.MiddleCenter
            };

            this.Controls.Add(instructionLabel);
        }
    }
}
