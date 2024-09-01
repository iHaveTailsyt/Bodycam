using NAudio.Wave;
using System;
using System.Collections.Generic;

namespace Bodycam.Handlers
{
    public class SoundManager
    {
        private Dictionary<string, string> soundFiles;

        public SoundManager()
        {
            soundFiles = new Dictionary<string, string>();
        }

        public void LoadSound(string soundName, string soundFilePath)
        {
            if (!soundFiles.ContainsKey(soundName))
            {
                soundFiles[soundName] = soundFilePath;
            }
        }

        public void PlaySound(string soundName, float volume = 0.3f)
        {
            if (soundFiles.ContainsKey(soundName))
            {
                try
                {
                    using (var audioFile = new AudioFileReader(soundFiles[soundName]))
                    using (var outputDevice = new WaveOutEvent())
                    {
                        outputDevice.Init(audioFile);
                        outputDevice.Volume = volume;
                        outputDevice.Play();
                        while (outputDevice.PlaybackState == PlaybackState.Playing)
                        {
                            System.Threading.Thread.Sleep(100);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error playing sound '{soundName}': {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine($"Sound '{soundName}' Not found");
            }
        }
    }
}