using Newtonsoft.Json;
using System.Diagnostics;
using System.Globalization;
using System.Net.Http.Headers;

namespace AIAssistant.Helpers
{
    public static class OpenAI
    {
        public static string TranscribeAudio()
        {
            string output = string.Empty;

            // Get the root directory of the C# project
            string projectRoot = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory);

            try
            {
                // Create a new process instance
                Process process = new Process();

                // Configure the process start info
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = "py", // Specify the Python executable
                    Arguments = "Transcriber.py", // Specify the Python script file
                    RedirectStandardOutput = true, // Redirect the output of the process
                    RedirectStandardError = true, // Redirect the error output of the process
                    UseShellExecute = false, // Don't use the operating system shell
                    CreateNoWindow = true, // Don't create a window for the process
                    WorkingDirectory = projectRoot // Set the working directory to the project's root
                };

                process.StartInfo = startInfo;

                // Start the process
                process.Start();

                // Read the output
                output = process.StandardOutput.ReadToEnd().Replace("\r\n", "").Replace("\n", ""); ;

                // Read the error output
                string errorOutput = process.StandardError.ReadToEnd();

                // Wait for the process to exit
                process.WaitForExit();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception:");
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }

            return output;
        }

        public static string GetAnswerFromOpenAI(string apiKey, int tokens, string input, string engine,
                  double temperature, int topP, int frequencyPenalty, int presencePenalty)
        {

            var openAiKey = apiKey;

            var apiCall = "https://api.openai.com/v1/engines/" + engine + "/completions";

            try
            {

                using (var httpClient = new HttpClient())
                {
                    using (var request = new HttpRequestMessage(new HttpMethod("POST"), apiCall))
                    {
                        request.Headers.TryAddWithoutValidation("Authorization", "Bearer " + openAiKey);
                        request.Content = new StringContent("{\n  \"prompt\": \"" + input + "\",\n  \"temperature\": " +
                                                            temperature.ToString(CultureInfo.InvariantCulture) + ",\n  \"max_tokens\": " + tokens + ",\n  \"top_p\": " + topP +
                                                            ",\n  \"frequency_penalty\": " + frequencyPenalty + ",\n  \"presence_penalty\": " + presencePenalty + "\n}");

                        request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

                        var response = httpClient.SendAsync(request).Result;
                        var json = response.Content.ReadAsStringAsync().Result;

                        dynamic dynObj = JsonConvert.DeserializeObject(json);

                        if (dynObj != null)
                        {
                            return dynObj.choices[0].text.ToString();
                        }


                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }

            return null;
        }
    }
}
