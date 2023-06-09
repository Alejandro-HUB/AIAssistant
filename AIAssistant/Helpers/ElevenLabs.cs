using NAudio.Wave;
using RestSharp;

namespace AIAssistant.Helpers
{
    public static class ElevenLabs
    {
        public static async Task SpeakElevenLabs(string text, string voice, string fileName, string apiKey)
        {
            var client = new RestClient($"https://api.elevenlabs.io/v1/text-to-speech/{voice}");
            var request = new RestRequest();
            request.Method = Method.Post;
            request.AddHeader("accept", "audio/mpeg");
            request.AddHeader("xi-api-key", apiKey);
            request.AddHeader("Content-Type", "application/json");
            request.AddJsonBody(new
            {
                text = text,
                model_id = "eleven_monolingual_v1",
                voice_settings = new
                {
                    stability = 0,
                    similarity_boost = 0
                }
            });

            var response = await client.ExecuteAsync(request);

            if (response.IsSuccessful)
            {
                var audioBytes = response.RawBytes;

                // Save the audio to a file
                File.WriteAllBytes(fileName, audioBytes);

                // Play the audio
                using (var audioFile = new AudioFileReader(fileName))
                using (var outputDevice = new WaveOutEvent())
                {
                    outputDevice.Init(audioFile);
                    outputDevice.Play();

                    while (outputDevice.PlaybackState == PlaybackState.Playing)
                    {
                        await Task.Delay(500);
                    }
                }
            }
            else
            {
                Console.WriteLine("Failed to generate audio.");
            }
        }
    }
}
