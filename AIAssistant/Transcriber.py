import openai
import os

#Get file path
DOWNLOADSPATH = os.path.expanduser("~")
RECORDINGFILENAME = os.path.join(DOWNLOADSPATH, "recording.mp3")

#Api key
openai.api_key = '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'

audio_file = open(RECORDINGFILENAME, "rb")
transcript = openai.Audio.transcribe("whisper-1", audio_file)

print(transcript["text"])
