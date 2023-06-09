using AIAssistant.Helpers;
public partial class Program
{
    static readonly string OPENAI_KEY = "";
    public static readonly string ELEVENLABSKEY = "";
    static readonly string VOICE = "";
    static readonly string DOWNLOADSPATH = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
    static readonly string RECORDINGFILENAME = Path.Combine(DOWNLOADSPATH, "recording.mp3");
    static readonly string RESPONSEFILENAME = Path.Combine(DOWNLOADSPATH, "output.mp3");

    static async Task Main()
    {
        Console.WriteLine("Welcome back! Type a message or press enter to record!");

        while (true)
        {
            Console.Write("You (input) > ");
            string userInput = Console.ReadLine();

            string transcript = "";

            if (!string.IsNullOrEmpty(userInput))
            {
                transcript = userInput;
            }
            else
            {
                transcript = await Audio.RecordAndTranscribeAudio(RECORDINGFILENAME);
            }

            string response = OpenAI.GetAnswerFromOpenAI(OPENAI_KEY, 250, transcript, "text-davinci-002", 0.7, 1, 0, 0);
            Console.WriteLine($"CharacterAI Response > {response}");

            await ElevenLabs.SpeakElevenLabs(response, VOICE, RESPONSEFILENAME, ELEVENLABSKEY);

            // Delete recording after use
            File.Delete(RECORDINGFILENAME);

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
}




