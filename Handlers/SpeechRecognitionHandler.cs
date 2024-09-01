using System;
using System.Collections.Generic;
using System.Speech.Recognition;

namespace Bodycam.Handlers
{
    public class SpeechRecognitionHandler
    {
        private SpeechRecognitionEngine recognizer;
        private List<string> startCommands = new List<string> { "dispatch show me 10-8", "dispatch show me on a 10-11", "dispatch show me 10-23" };
        private List<string> stopCommands = new List<string> { "dispatch show me code-4" };

        public event Action OnStartRecording;
        public event Action OnStopRecording;

        // Property to store the last recognized command
        public string LastCommand { get; private set; }

        public SpeechRecognitionHandler()
        {
            recognizer = new SpeechRecognitionEngine();
            recognizer.SetInputToDefaultAudioDevice();

            var choices = new Choices();
            choices.Add(startCommands.ToArray());
            choices.Add(stopCommands.ToArray());

            // Create a grammar builder and grammer
            var grammarBuilder = new GrammarBuilder(choices);
            var grammar = new Grammar(grammarBuilder);

            // Load the grammer
            recognizer.LoadGrammar(grammar);

            // Event handler for recognized speech
            recognizer.SpeechRecognized += Recognizer_SpeechRecognized;
        }

        public void StartListening()
        {
            recognizer.RecognizeAsync(RecognizeMode.Multiple);
        }

        private void Recognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            var recognizedText = e.Result.Text.ToLower();

            if (startCommands.Contains(recognizedText)) 
            {
                LastCommand = recognizedText;
                OnStartRecording?.Invoke();
            }
            else if (stopCommands.Contains(recognizedText))
            {
                LastCommand = recognizedText;
                OnStopRecording?.Invoke();
            }
        }
    }
}