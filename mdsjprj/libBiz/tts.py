from gtts import gTTS
import os


def text_to_speech(text, lang='zh'):
    try:
        # Read the text from the file


        if not text:
            raise ValueError("The file is empty or couldn't be read.")

        # Convert text to speech
        tts = gTTS(text=text, lang=lang)

        # Save the speech to a file
        audio_file = "output.mp3"
        tts.save(audio_file)

        # Play the audio file
        os.system(f"start {audio_file}" if os.name == "nt" else f"afplay {audio_file}")

        print(f"Text-to-speech conversion completed. Audio saved as {audio_file}.")
        return  audio_file
    except Exception as e:
        print(f"An error occurred: {e}")


# Example usage
#text_to_speech("我是谁")
