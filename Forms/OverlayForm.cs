using System;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Linq;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;
using System.Timers;
using Bodycam.Utilities;
using Bodycam.Handlers;

namespace Bodycam.Forms
{
    public partial class OverlayForm : Form
    {
        private Label statusLabel;
        private PictureBox cameraFeed;
        private Label idLabel;
        private Button startRecordingButton;
        private Button stopRecordingButton;
        private PictureBox axonLogo;
        private Timer screenCaptureTimer;
        private System.Timers.Timer twoMinuteTimer;
        private System.Timers.Timer recordingTimer;
        private Label timerLabel;
        private DateTime recordingStartTime;
        private SoundManager soundManager;
        private bool _isRecording = false;

        private static readonly string logoDirectory = @"C:\Bodycam\Assets";
        private static readonly string logoFilePath = Path.Combine(logoDirectory, "axon_logo.png");

        private static readonly string SoundDirectory = @"C:\Bodycam\Sounds";
        private static readonly string startRecordingSoundFilePath = Path.Combine(SoundDirectory, "startRecording.mp3");
        private static readonly string twoMinuteBeepSoundFilePath = Path.Combine(SoundDirectory, "bodycam_recording_reminder.mp3");
        private static readonly string stopRecordingSoundFilePath = Path.Combine(SoundDirectory, "stopRecording.mp3");

        public OverlayForm()
        {
            if (!Directory.Exists(logoDirectory))
            {
                Directory.CreateDirectory(logoDirectory);
            }

            if (!Directory.Exists(SoundDirectory))
            {
                Directory.CreateDirectory(SoundDirectory);
            }

            InitializeComponent();

            this.ClientSize = new Size(500, 250);
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.Manual;
            this.TopMost = true;
            this.BackColor = Color.Black;

            InitUI();
            InitSoundManager();
            DisplayBodycamID();
            StartScreenCapture();
        }

        private void InitUI()
        {
            this.BackColor = Color.FromArgb(25, 25, 25);
            this.Size = new Size(500, 250);

            cameraFeed = new PictureBox
            {
                Size = new Size(120, 120),
                Location = new Point(10, 10),
                BackColor = Color.Black,
                BorderStyle = BorderStyle.FixedSingle,
                SizeMode = PictureBoxSizeMode.StretchImage
            };
            cameraFeed.Paint += (sender, e) =>
            {
                e.Graphics.DrawRectangle(new Pen(Color.DarkGray, 5), 0, 0, cameraFeed.Width - 1, cameraFeed.Height - 1);
                e.Graphics.DrawRectangle(new Pen(Color.Black, 2), 2, 2, cameraFeed.Width - 5, cameraFeed.Height - 5);
            };
            this.Controls.Add(cameraFeed);

            statusLabel = new Label
            {
                Size = new Size(180, 30),
                Location = new Point(10, 130),
                ForeColor = Color.Red,
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Arial", 12, FontStyle.Bold),
                Text = "Not Recording"
            };
            this.Controls.Add(statusLabel);

            startRecordingButton = new Button
            {
                Size = new Size(180, 40),
                Location = new Point(200, 50),
                Text = "Start Recording",
                BackColor = Color.FromArgb(0, 122, 204),
                ForeColor = Color.White,
                Font = new Font("Arial", 12, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                FlatAppearance = { BorderSize = 0 }
            };
            startRecordingButton.Click += StartRecordingButton_Click;
            this.Controls.Add(startRecordingButton);

            stopRecordingButton = new Button
            {
                Size = new Size(180, 40),
                Location = new Point(200, 100),
                Text = "Stop Recording",
                BackColor = Color.FromArgb(255, 59, 48),
                ForeColor = Color.White,
                Font = new Font("Arial", 12, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                FlatAppearance = { BorderSize = 0 }
            };
            stopRecordingButton.Click += StopRecordingButton_Click;
            this.Controls.Add(stopRecordingButton);

            axonLogo = new PictureBox
            {
                Size = new Size(100, 50),
                Location = new Point(this.ClientSize.Width - 110, 10),
                Image = Image.FromFile(logoFilePath),
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.Transparent
            };
            this.Controls.Add(axonLogo);

            timerLabel = new Label
            {
                Size = new Size(100, 30),
                Location = new Point(70, 170),
                ForeColor = Color.Lime,
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.MiddleLeft,
                Text = "00:00:00",
                Visible = false
            };
            this.Controls.Add(timerLabel);
        }

        private void InitSoundManager()
        {
            soundManager = new SoundManager();
            soundManager.LoadSound("startRecording", startRecordingSoundFilePath);
            soundManager.LoadSound("twoMinuteBeep", twoMinuteBeepSoundFilePath);
            soundManager.LoadSound("stopRecording", stopRecordingSoundFilePath);
        }

        private void DisplayBodycamID()
        {
            string bodycamID = BodycamIDManager.GetOrCreateBodycamID();

            idLabel = new Label
            {
                Size = new Size(250, 20),
                Location = new Point(170, 10),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Arial", 12, FontStyle.Regular),
                Text = $"ID: {bodycamID}"
            };
            this.Controls.Add(idLabel);
        }

        private void StartScreenCapture()
        {
            screenCaptureTimer = new Timer
            {
                Interval = 1000
            };
            screenCaptureTimer.Tick += CaptureScreen;
            screenCaptureTimer.Start();
        }

        private void CaptureScreen(object sender, EventArgs e)
        {
            var screenBounds = Screen.PrimaryScreen.Bounds;
            using (var bitmap = new Bitmap(screenBounds.Width, screenBounds.Height))
            using (var graphics = Graphics.FromImage(bitmap))
            {
                graphics.CopyFromScreen(screenBounds.Location, Point.Empty, screenBounds.Size);
                cameraFeed.Image?.Dispose();
                cameraFeed.Image = new Bitmap(bitmap);
            }
        }

        private void StartRecordingButton_Click(object sender, EventArgs e)
        {
            _isRecording = true;
            soundManager.PlaySound("startRecording");
            UpdateStatus("Recording...");
            Logger.Log("Start Button", "Recording Started");
            recordingStartTime = DateTime.Now;
            timerLabel.Visible = true;
            timerLabel.BringToFront();

            twoMinuteTimer = new System.Timers.Timer(120000);
            twoMinuteTimer.Elapsed += OnTwoMinuteInterval;
            twoMinuteTimer.Start();

            recordingTimer = new System.Timers.Timer(1000);
            recordingTimer.Elapsed += OnRecordingTimerTick;
            recordingTimer.Start();
        }

        private void OnTwoMinuteInterval(object sender, ElapsedEventArgs e)
        {
            soundManager.PlaySound("twoMinuteBeep");
        }

        private void OnRecordingTimerTick(object sender, ElapsedEventArgs e)
        {
            var elapsed = DateTime.Now - recordingStartTime;
            Invoke(new Action(() => timerLabel.Text = elapsed.ToString(@"hh\:mm\:ss")));
        }

        private void StopRecordingButton_Click(object sender, EventArgs e)
        {
            _isRecording = false;
            soundManager.PlaySound("stopRecording");
            UpdateStatus("Not Recording");
            Logger.Log("Start Button", "Recording Stoped");

            if (twoMinuteTimer != null)
            {
                twoMinuteTimer.Stop();
                twoMinuteTimer.Dispose();
                twoMinuteTimer = null;
            }

            recordingTimer?.Stop();
            recordingTimer?.Dispose();
            recordingTimer = null;

            timerLabel.Visible = false;
        }

        public void UpdateStatus(string status)
        {
            statusLabel.Text = status;
            statusLabel.ForeColor = status == "Recording..." ? Color.Green : Color.Red;
        }
    }
}