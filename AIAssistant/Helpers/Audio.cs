using NAudio.Wave;

namespace AIAssistant.Helpers
{
    public static class Audio
    {
        public static async Task<string> RecordAndTranscribeAudio(string filePath)
        {
            Console.Write("You (mic [Recording]) > ");
            RecordAudio(filePath);

            string transcript = OpenAI.TranscribeAudio();
            Console.WriteLine($"You (mic) > {transcript}");

            return transcript;
        }

        public static void RecordAudio(string filePath)
        {
            using (var waveIn = new WaveInEvent())
            {
                waveIn.WaveFormat = new WaveFormat(44100, 1);

                MemoryStream memoryStream = new MemoryStream();
                using (var writer = new WaveFileWriter(memoryStream, waveIn.WaveFormat))
                {
                    waveIn.DataAvailable += (s, e) =>
                    {
                        writer.Write(e.Buffer, 0, e.BytesRecorded);
                    };

                    waveIn.StartRecording();

                    Console.WriteLine("Recording... Press any key to stop recording.");

                    // Wait for a key press to stop recording
                    Console.ReadKey(intercept: true);

                    waveIn.StopRecording();
                }

                byte[] audioData = memoryStream.ToArray();
                File.WriteAllBytes(filePath, audioData);
            }
        }
    }
}
