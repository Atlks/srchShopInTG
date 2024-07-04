import sys
ptharr=['d:\\aaa','D:\\0prj\\mdsj\\mdsjprj\\libBiz', 'D:\\0prj\\mdsj\\mdsjprj', 'D:\\Program Files\\JetBrains\\PyCharm 2024.1.4\\plugins\\python\\helpers\\pycharm_display', 'C:\\Users\\Administrator\\AppData\\Local\\Programs\\Python\\Python312\\python312.zip', 'C:\\Users\\Administrator\\AppData\\Local\\Programs\\Python\\Python312\\DLLs', 'C:\\Users\\Administrator\\AppData\\Local\\Programs\\Python\\Python312\\Lib', 'C:\\Users\\Administrator\\AppData\\Local\\Programs\\Python\\Python312', 'C:\\Users\\Administrator\\AppData\\Local\\Programs\\Python\\Python312\\Lib\\site-packages', 'D:\\Program Files\\JetBrains\\PyCharm 2024.1.4\\plugins\\python\\helpers\\pycharm_matplotlib_backend', 'path_to_your_module_directory']
sys.path=sys.path+(ptharr)
print(sys.path)


import urllib.parse
import json
import os
from gtts import gTTS
import os
#from libBiz.tts import text_to_speech


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
       # os.system(f"start {audio_file}" if os.name == "nt" else f"afplay {audio_file}")

        print(f"Text-to-speech conversion completed. Audio saved as {audio_file}.")
        return  audio_file
    except Exception as e:
        print(f"An error occurred: {e}")



# Function to mimic file_get_contents in Python
def file_get_contents(filename):
    with open(filename, 'r', encoding='utf-8') as file:
        return file.read()

# Function to mimic urldecode in Python
def urldecode(encoded_url):
    return urllib.parse.unquote(encoded_url)


sys.path.append('path_to_your_module_directory')
print(sys.path)
ptharr=['D:\\0prj\\mdsj\\mdsjprj\\libBiz', 'D:\\0prj\\mdsj\\mdsjprj', 'D:\\Program Files\\JetBrains\\PyCharm 2024.1.4\\plugins\\python\\helpers\\pycharm_display', 'C:\\Users\\Administrator\\AppData\\Local\\Programs\\Python\\Python312\\python312.zip', 'C:\\Users\\Administrator\\AppData\\Local\\Programs\\Python\\Python312\\DLLs', 'C:\\Users\\Administrator\\AppData\\Local\\Programs\\Python\\Python312\\Lib', 'C:\\Users\\Administrator\\AppData\\Local\\Programs\\Python\\Python312', 'C:\\Users\\Administrator\\AppData\\Local\\Programs\\Python\\Python312\\Lib\\site-packages', 'D:\\Program Files\\JetBrains\\PyCharm 2024.1.4\\plugins\\python\\helpers\\pycharm_matplotlib_backend', 'path_to_your_module_directory']
sys.path.append(ptharr)

# Get command line arguments excluding script name
args = sys.argv[1:]


#test
#args = [1, 2, 3, 4, 5]
#args[0]=r'D:\0prj\mdsj\mdsjprj\bin\Debug\net8.0\prmDir\prm20240704_115009_018.txt'

dbF = urldecode(args[0])
sdaveObjstr = file_get_contents(dbF)
prmobj = json.loads(sdaveObjstr)


ret =text_to_speech(prmobj['txt']) # Replace "some_value" with your actual variable or expression

print(sys.path)
print("Return value:", ret)
marker = "----------marker----------"

print(marker)
print(ret)



