
import os

from pydub import AudioSegment
import librosa
import numpy as np

from midiutil import MIDIFile
#AudioSegment.ffmpeg = os.path.join(r"D:\0prj\mdsj\mdsjprj\bin\Debug\net8.0\ffmpeg.exe")
# 设置 ffmpeg 的路径（例如 "C:/ffmpeg/bin/ffmpeg.exe"）

print(os.path)
# D:\0prj\mdsj\venv\Scripts\pip.exe   install numpy
# D:\0prj\mdsj\venv\Scripts\pip.exe install numpy cython

import subprocess

def mp3_to_wav(input_mp3, output_wav):
    command = [r'D:\prgrm\ffmpeg.exe','-y', '-i', input_mp3, output_wav]
    subprocess.run(command, check=True)


def extract_notes_from_wav(wav_file):
    y, sr = librosa.load(wav_file)
    onset_env = librosa.onset.onset_strength(y=y, sr=sr)
    tempo, beats = librosa.beat.beat_track(onset_envelope=onset_env, sr=sr)
    pitches, magnitudes = librosa.core.piptrack(y=y, sr=sr)

    notes = []
    for t in beats:
        index = magnitudes[:, t].argmax()
        pitch = pitches[index, t]
        if pitch > 0:
            note = librosa.hz_to_midi(pitch)
            notes.append((note, t))

    return notes

def notes_to_midi(notes, midi_file):
    midi = MIDIFile(1)
    track = 0
    time = 0
    midi.addTrackName(track, time, "Track")
    midi.addTempo(track, time, 120)

    channel = 0
    volume = 100

    for note, beat in notes:
        midi.addNote(track, channel, int(note), beat, 1, volume)

    with open(midi_file, "wb") as output_file:
        midi.writeFile(output_file)

def mp3_to_midi(mp3_file, midi_file):
    wav_file = "temp2024.wav"
    mp3_to_wav(mp3_file, wav_file)
    notes = extract_notes_from_wav(wav_file)
    notes_to_midi(notes, midi_file)

# 示例用法
mp3_file = r"D:\zzcdsk\wala man sayo ang lahat.mp3"
midi_file = "wala.mid"
mp3_to_midi(mp3_file, midi_file)
# 示例使用
#convert_mp3_to_midi(r'D:\zzcdsk\wala man sayo ang lahat.mp3', r'wala man sayo ang lahat.mid')
