using NAudio.Wave;
using OpenAI_API.Completions;
using OpenAI_API;
using RestSharp;
using Python.Runtime;

class Program
{
    static readonly string OPENAI_KEY = "aYcZzgPK7ZaU5f2Jm0i8T3BlbkFJ3sYdXhHjhJsz09NbdDhU";
    static readonly string VOICE = "Nus6MS8HnQ3O1r17i96o";
    static readonly string DOWNLOADSPATH = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
    static readonly string FILENAME = Path.Combine(DOWNLOADSPATH, "recording.wav");

    static async Task Main()
    {
        Console.WriteLine("Welcome back!");

        while (true)
        {
            Console.Write("You (mic) > ");
            string userInput = Console.ReadLine();

            string transcript = "";

            if (!string.IsNullOrEmpty(userInput))
            {
                transcript = userInput;
            }
            else
            {
                transcript = await RecordAndTranscribeAudio();
            }

            string response = await GetOpenAICompletion(transcript);
            Console.WriteLine($"CharacterAI Response > {response}");

            await SpeakElevenLabs(response);

            // Delete recording after use
            File.Delete(FILENAME);

            Console.WriteLine("Press ESC to exit or any other key to continue...");

            // Read a key input from the user
            var keyInfo = Console.ReadKey(intercept: true);

            // Check if the pressed key is the "Escape" key
            if (keyInfo.Key == ConsoleKey.Escape)
            {
                break; // Break the loop and exit the program
            }

            Console.WriteLine();
        }
    }

    static async Task<string> RecordAndTranscribeAudio()
    {
        Console.Write("You (mic [Recording]) > ");
        RecordAudio();

        string transcript = await TranscribeAudio();
        Console.WriteLine($"You (mic) > {transcript}");

        return transcript;
    }

    static void RecordAudio()
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
            File.WriteAllBytes(FILENAME, audioData);
        }
    }

    static async Task<string> TranscribeAudio()
    {
        PythonEngine.PythonHome = @"C:\Python310";
        PythonEngine.Initialize();

        try
        {
            using (Py.GIL())
            {
                dynamic openai = Py.Import("openai");
                dynamic audioFile = openai.audio_file(FILENAME, "rb");
                dynamic transcript = await openai.Audio.transcribe("whisper-1", audioFile);

                string transcriptText = transcript["text"];

                return transcriptText;
            }
        }
        finally
        {
            PythonEngine.Shutdown();
        }
    }

    static async Task<string> GetOpenAICompletion(string prompt)
    {
        var openAi = new OpenAIAPI(OPENAI_KEY);
        var request = new CompletionRequest();
        request.Prompt = prompt;
        request.Model = OpenAI_API.Models.Model.DavinciText;
        
        var response = await openAi.Completions.CreateCompletionAsync(request);

        if (response?.Completions != null && response.Completions.Any())
        {
            return response.Completions[0].Text;
        }
        else
        {
            throw new Exception("Request failed: Open AI Completion");
        }
    }

    static async Task SpeakElevenLabs(string text)
    {
        var client = new RestClient($"https://api.elevenlabs.io/v1/text-to-speech/{VOICE}");
        var request = new RestRequest();
        request.Method= Method.Post;
        request.AddHeader("accept", "audio/mpeg");
        request.AddHeader("xi-api-key", "149e26934618b93e0abc6991e9a3f76d");
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
            File.WriteAllBytes("output.wav", audioBytes);

            // Play the audio
            using (var audioFile = new AudioFileReader("output.wav"))
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
