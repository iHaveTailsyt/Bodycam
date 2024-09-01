using System;
using System.Windows.Forms;
using Bodycam.Handlers;
using Bodycam.Utilities;
using System.Speech.Recognition;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using System.Net.Http;
using System.IO;

namespace Bodycam.Forms
{
    public partial class Test : Form
    {
        private OverlayForm overlayForm;
        private SpeechRecognitionHandler speechHandler;

        public Test()
        {
            InitializeComponent();
            Console.WriteLine("");
            CheckMicrophoneAccess();
            Console.WriteLine("");
        }

        private void CheckMicrophoneAccess()
        {
            try
            {
                using (var testRecognizer = new SpeechRecognitionEngine())
                {
                    testRecognizer.SetInputToDefaultAudioDevice();
                }

                InitializeOverlay();
                InitializeSpeechRecognition();
            }
            catch (Exception)
            {
                Showin();
            }
        }

        private void Showin()
        {
            MicrophoneForm microphoneForm = new MicrophoneForm();
            microphoneForm.ShowDialog();
            Application.Exit();
        }

        private void InitializeOverlay()
        {
            overlayForm = new OverlayForm();
            overlayForm.Show();
        }

        private void InitializeSpeechRecognition()
        {
            speechHandler = new SpeechRecognitionHandler();
            speechHandler.OnStartRecording += () => StartRecording(speechHandler.LastCommand);
            speechHandler.OnStopRecording += () => StopRecording(speechHandler.LastCommand);
            speechHandler.StartListening();
        }

        private void StartRecording(string command)
        {
            overlayForm.UpdateStatus("Recording...");
            Logger.Log(command, "Recording Started");
        }

        private void StopRecording(string command)
        {
            overlayForm.UpdateStatus("Not Recording");
            Logger.Log(command, "Recording Stoped");
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Name = "test";
        }

        private void label1_Click(object sender, EventArgs e)
        {
            return;
        }
    }
}