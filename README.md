# AI Assistant

AI Assistant is a command-line program built with C# that utilizes artificial intelligence to provide interactive conversations and voice responses. This project leverages the power of OpenAI and ElevenLabs APIs to create a virtual assistant-like experience.

## Features

- Speech-to-text conversion: The program allows users to enter messages via text input or by recording their voice.
- AI response generation: It utilizes the OpenAI API to generate AI-based responses by analyzing the user's input or recorded speech.
- Voice synthesis: The generated response is converted into natural-sounding speech using the ElevenLabs API.
- User-friendly interface: The program provides a simple and intuitive command-line interface for smooth interaction with the assistant.

## Usage

1. Set up the necessary API keys:
   - Obtain an OpenAI API key and replace the value of the `OPENAI_KEY` constant.
   - Obtain an ElevenLabs API key and replace the value of the `ELEVENLABSKEY` constant.
   - Configure the desired voice for the assistant by setting the `VOICE` constant.
   - Inside the AIAssistant folder, locate the Transcriber.py file and place your OpenAI API key in the `openai.api_key` variable.
2. Run the program.
3. Upon launching, you can start interacting with the assistant:
   - Type a message and press Enter to send a text-based query.
   - Press Enter without typing to record your voice and convert it into text.
4. The program will generate an AI response using OpenAI's API and display it.
5. The response will be converted into speech using the selected voice from ElevenLabs API and saved as an audio file.
6. The recording file will be automatically deleted after use.
7. Press ESC to exit the program or any other key to continue interacting.

Feel free to customize the AI model and response parameters in the `GetAnswerFromOpenAI` function to fine-tune the assistant's behavior.

Note: Ensure that you have a stable internet connection and the necessary API keys for seamless functionality.

## Dependencies

- AIAssistant.Helpers: A helper library providing functions for audio recording, transcription, and speech synthesis.

## Contribution

Contributions to the project are welcome. Feel free to open issues for bug reports or feature requests and submit pull requests for improvements.

Let's make the AI Assistant even better together!
