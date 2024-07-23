using System;
using System.Collections.Generic;
using System.Linq;
using NAudio.Wave;
using NAudio.Midi;
 
using System.Text;
using System.Threading.Tasks;
using YoutubeExplode;
using YoutubeExplode.Videos.Streams;
using Xabe.FFmpeg;
using Xabe.FFmpeg.Downloader;
using YoutubeExplode.Videos.Streams;
using Xabe.FFmpeg;
using Xabe.FFmpeg.Downloader;
using static mdsj.lib.avClas;
using YoutubeExplode.Common;
using DocumentFormat.OpenXml.Wordprocessing;
using prjx.lib;
using System.Reflection;
using System.Collections;
using EchoPrintSharp;
//using ChromaWrapper;
//using ChromaWrapper.Fingerprint;
using Newtonsoft.Json;
using AcoustID;
using NReco.VideoConverter;
using Concentus.Oggfile;
using Concentus.Structs;
using NAudio.Lame;
using NAudio.Wave;
using NAudio.Midi;
namespace mdsj.lib
{
    internal class avClas
    {

        public static async Task<string> DownloadFileThruTgApi(string filePath, string fileName)
        {

            //static void ConvertAudioToMidi(string inputFilePath, string outputFilePath)
            //{
            //    // Load the audio file
            //    using var reader = new AudioFileReader(inputFilePath);
            //    float[] samples = new float[reader.Length / sizeof(float)];
            //    reader.Read(samples, 0, samples.Length);

            //    // Create a new MIDI file with one track
            //    var midiFile = new MidiFile(1, 1);
            //    var track = new MidiEventCollection(1, reader.WaveFormat.SampleRate);
            //    midiFile.Events.Add(track);

            //    // Perform simple pitch detection and add notes to the MIDI file
            //    for (int i = 0; i < samples.Length; i += reader.WaveFormat.SampleRate / 10)  // Process in 0.1 second chunks
            //    {
            //        float[] chunk = new float[reader.WaveFormat.SampleRate / 10];
            //        Array.Copy(samples, i, chunk, 0, Math.Min(chunk.Length, samples.Length - i));
            //        float pitch = DetectPitch(chunk, reader.WaveFormat.SampleRate);

            //        if (pitch > 0)
            //        {
            //            int pitchMidi = (int)Math.Round(69 + 12 * Math.Log2(pitch / 440.0));  // Convert frequency to MIDI note number
            //            track.AddNoteOnEvent(0, 0, pitchMidi, i / (reader.WaveFormat.SampleRate / 10), 127);
            //            track.AddNoteOffEvent(0, 0, pitchMidi, (i / (reader.WaveFormat.SampleRate / 10)) + 1, 127);
            //        }
            //    }

            //    // Save the MIDI file
            //    MidiFile.Export(outputFilePath, track);
            //}


            var fileUrl = $"https://api.telegram.org/file/bot{BotTokenQunzhushou}/{filePath}";
            var fileFullPath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), fileName);

            using (var httpClient = new HttpClient())
            {
                // 设置超时时间为30秒
                httpClient.Timeout = TimeSpan.FromSeconds(200);
                var response = await httpClient.GetAsync(fileUrl);
                // 检查响应是否成功
                response.EnsureSuccessStatusCode();
                await using var fileStream = new FileStream(fileFullPath, FileMode.Create, FileAccess.Write, FileShare.None);
                await response.Content.CopyToAsync(fileStream);
            }

            return fileFullPath;
        }
        public static void ConvertOggToMp3(string inputFilePath, string outputFilePath)
        {
            if (string.IsNullOrEmpty(inputFilePath))
                throw new ArgumentException("Input file path cannot be null or empty", nameof(inputFilePath));

            if (string.IsNullOrEmpty(outputFilePath))
                throw new ArgumentException("Output file path cannot be null or empty", nameof(outputFilePath));

            // Ensure the input file exists
            if (!System.IO.File.Exists(inputFilePath))
                throw new FileNotFoundException("Input file not found", inputFilePath);

            try
            {
                // Initialize the Opus decoder
                OpusDecoder decoder = new OpusDecoder(48000, 2);

                // Open the Ogg file
                FileStream oggFile = new FileStream(inputFilePath, FileMode.Open);
                OpusOggReadStream oggStream = new OpusOggReadStream(decoder, oggFile);
                using (WaveFileWriter waveWriter = new WaveFileWriter(System.IO.Path.ChangeExtension(outputFilePath, ".wav"), new WaveFormat(48000, 16, 2)))
                {
                    // Read the Ogg file and write to a WAV file
                    while (oggStream.HasNextPacket)
                    {
                        short[] packet = oggStream.DecodeNextPacket();
                        if (packet != null)
                        {
                            byte[] buffer = new byte[packet.Length * sizeof(short)];
                            Buffer.BlockCopy(packet, 0, buffer, 0, buffer.Length);
                            waveWriter.Write(buffer, 0, buffer.Length);
                        }
                    }
                }

                // Convert WAV file to MP3
                using (var reader = new AudioFileReader(System.IO.Path.ChangeExtension(outputFilePath, ".wav")))
                using (var mp3Writer = new LameMP3FileWriter(outputFilePath, reader.WaveFormat, LAMEPreset.VBR_90))
                {
                    reader.CopyTo(mp3Writer);
                }

                // Delete the intermediate WAV file
                //   File.Delete(System.IO.Path.ChangeExtension(outputFilePath, ".wav"));

               Print($"Successfully converted {inputFilePath} to {outputFilePath}");
            }
            catch (Exception ex)
            {
               Print($"Error during conversion: {ex.Message}");
               Print($"Stack Trace: {ex.StackTrace}");
            }
        }

        public static void ConvertVideoToMp3(string videoFilePath, string mp3FilePath)
        {
            var __METHOD__ = "ConvertVideoToMp3";
            PrintCallFunArgs(__METHOD__, func_get_args(videoFilePath, mp3FilePath));

            // var mp3FilePath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), $"{basename}.mp3");
            var ffMpeg = new FFMpegConverter();
            //   ffMpeg.ConvertMedia(videoFilePath, mp3FilePath, "mp3");
            // Convert video to MP3
            // Convert video to MP3
            ffMpeg.ConvertMedia(videoFilePath, mp3FilePath, "mp3");
            //return mp3FilePath;
           Print($"Conversion completed: {mp3FilePath}");
        }

        public static string AcoustIDApiKey = "AMsnvQgE0s";
        private const string AcoustIDApiUrl = "https://api.acoustid.org/v2/lookup";
        //public static string GenerateAudioFingerprint(string mp3FilePath)
        //{
        //    using (var audioFile = new AudioFile(mp3FilePath))
        //    {
        //        var fingerprinter = new ChromaContext();
        //        var fingerprintAlgorithm = FingerprintAlgorithmFactory.Create(FingerprintAlgorithmFactory.ALGORITHM_DEFAULT);
        //        var audioFingerprint = fingerprinter.CreateFingerprint(audioFile, fingerprintAlgorithm);
        //        return BitConverter.ToString(audioFingerprint).Replace("-", "").ToLowerInvariant();
        //    }
        //}

        public static async Task<MusicMetadata> RecognizeMusic(string mp3FilePath)
        {

            AcoustIDApiKey = "X7CFv1rFKI";
            var __METHOD__ = "RecognizeMusic";
            PrintCallFunArgs(__METHOD__, func_get_args(mp3FilePath));

            // 读取音频文件的二进制数据
            byte[] audioData = File.ReadAllBytes(mp3FilePath);
            MusicMetadata musicMetadataFft = new MusicMetadata
            {
                Title = "unk" + FilenameBydtme(),
                Artist = "unkonwARtist"
            };
            try
            {
               
                // 发送 HTTP 请求到 AcoustID Web API
                using (var httpClient = new HttpClient())
                {
                    var content = new MultipartFormDataContent();
                    content.Add(new StringContent(AcoustIDApiKey), "client");
                    content.Add(new ByteArrayContent(audioData), "file", "audio.mp3");

                    var response = await httpClient.PostAsync(AcoustIDApiUrl, content);
                    response.EnsureSuccessStatusCode();

                    var responseBody = await response.Content.ReadAsStringAsync();
                    // 使用 Newtonsoft.Json 解析 JSON 数据
                    var result = JsonConvert.DeserializeObject<AcoustIDResponse>(responseBody);

                    if (result != null && result.Results.Length > 0)
                    {
                        // 获取第一个匹配结果的元数据信息
                        var firstResult = result.Results[0];
                        PrintRet(__METHOD__, firstResult);
                        return new MusicMetadata
                        {
                            Title = firstResult.Recordings[0].Title,
                            Artist = firstResult.Recordings[0].Artists[0].Name
                        };
                    }
                    else
                    {
                        PrintRet(__METHOD__, musicMetadataFft);
                        return musicMetadataFft;
                    }
                }

            }
            catch (Exception e)
            {
               Print(e);
             
            }
          
            PrintRet(__METHOD__, musicMetadataFft);
            return musicMetadataFft;


        }
       
        //public static async Task<object> RecognizeMusic(string mp3FilePath)
        //{
        //    // 读取音频文件的二进制数据
        //    byte[] audioData = File.ReadAllBytes(mp3FilePath);


        //    // 使用 AcoustID 查询音乐数据库
        //    var client = new AcoustIDClient(AcoustIDApiKey);
        //    var response = await client.LookupAsync(audioData);
        //    //if (results != null && results.Count > 0)
        //    //{
        //    // 获取第一个匹配结果的元数据信息
        //    var firstResult = results[0];
        //        return firstResult;
        //        //Hashtable ht=new Hashtable():
        //        //    ht.Add("Title")
        //        //return new Hashtable
        //        //{
        //        //    "" :firstResult.Recordings[0].Title,
        //        //     "Artist" : = firstResult.Recordings[0].Artists[0].Name
        //        //};
        //    //}
        //    //else
        //    //{
        //    //    throw new Exception("No matching music found.");
        //    //}
        //}
        public static string  DownloadSongAsMp3(string songName,string dir)
        {
            var __METHOD__ = "DownloadSongAsMp3";
            dbgCls.PrintCallFunArgs(__METHOD__, dbgCls.func_get_args(  songName, dir));

            try
            {
                var youtube = new YoutubeClient();

                // 搜索视频
                var searchResults = youtube.Search.GetVideosAsync(songName).GetAwaiter().GetResult(); ;
                //Task.Run(async () =>
                //{
                //    await
                //}).aw
                    
                var video = searchResults.FirstOrDefault();

                if (video == null)
                {
                   Print("未找到歌曲！");
                    return "";
                }

                var streamManifest =   youtube.Videos.Streams.GetManifestAsync(video.Id).Result;
                var audioStreams = streamManifest.GetAudioOnlyStreams();
                var streamInfo = audioStreams.OrderByDescending(s => s.Bitrate).FirstOrDefault();

                if (streamInfo == null)
                {
                   Print("未找到音频流！");
                    return "";
                }
                Directory.CreateDirectory(dir);
                // 下载音频流
                var tempFile = Path.GetTempFileName();
              // string filePathTmp = dir + "/" + tempFile;
               Print($"down tmpfile=>{tempFile}");
                youtube.Videos.Streams.DownloadAsync(streamInfo, tempFile).GetAwaiter().GetResult(); ;

                // 转换为 MP3
                // 转换为 MP3
                // 转换为 MP3
                string fname = ConvertToValidFileName2024(songName);
                var outputFilePath = $"{dir}/{fname}.mp3";
               Print($"outputFilePath =>{outputFilePath}");
                  ConvertToMp3(tempFile, outputFilePath);

                // 删除临时文件
              //  File.Delete(tempFile);

               Print($"歌曲已下载并转换为 MP3：{outputFilePath}");
                return outputFilePath;
            }
            catch (Exception ex)
            {
               Print(ex.ToString());
            }
            dbgCls.PrintRet(__METHOD__, 0);
            return "";
        }

        static void  ConvertToMp3(string inputFilePath, string outputFilePath)
        {
            var ffmpeg = new NReco.VideoConverter.FFMpegConverter();
            ffmpeg.ConvertMedia(inputFilePath, outputFilePath, "mp3");
        }

    }



    public class MusicMetadata
    {
        public string Title { get; set; }
        public string Artist { get; set; }
    }

    public class AcoustIDResponse
    {
        public AcoustIDResult[] Results { get; set; }
    }

    public class AcoustIDResult
    {
        public AcoustIDRecording[] Recordings { get; set; }
    }

    public class AcoustIDRecording
    {
        public string Title { get; set; }
        public AcoustIDArtist[] Artists { get; set; }
    }

    public class AcoustIDArtist
    {
        public string Name { get; set; }
    }
}
