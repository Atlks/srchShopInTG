using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoutubeExplode;
using YoutubeExplode.Videos.Streams;
using Xabe.FFmpeg;
using Xabe.FFmpeg.Downloader;
using YoutubeExplode.Videos.Streams;
using Xabe.FFmpeg;
using Xabe.FFmpeg.Downloader;
using static mdsj.lib.music;
using YoutubeExplode.Common;
using DocumentFormat.OpenXml.Wordprocessing;
using prj202405.lib;
using System.Reflection;
using System.Collections;
using EchoPrintSharp;
//using ChromaWrapper;
//using ChromaWrapper.Fingerprint;
using Newtonsoft.Json;
using AcoustID;
namespace mdsj.lib
{
    internal class music
    {
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
            dbg_setDbgFunEnter(__METHOD__, func_get_args(mp3FilePath));

            // 读取音频文件的二进制数据
            byte[] audioData = File.ReadAllBytes(mp3FilePath);
            MusicMetadata musicMetadataFft = new MusicMetadata
            {
                Title = "unk" + filenameBydtme(),
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
                        dbg_setDbgValRtval(__METHOD__, firstResult);
                        return new MusicMetadata
                        {
                            Title = firstResult.Recordings[0].Title,
                            Artist = firstResult.Recordings[0].Artists[0].Name
                        };
                    }
                    else
                    {
                        dbg_setDbgValRtval(__METHOD__, musicMetadataFft);
                        return musicMetadataFft;
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
             
            }
          
            dbg_setDbgValRtval(__METHOD__, musicMetadataFft);
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
        public static async Task DownloadSongAsMp3(string songName,string dir)
        {
            var __METHOD__ = "DownloadSongAsMp3";
            dbgCls.dbg_setDbgFunEnter(__METHOD__, dbgCls.func_get_args(  songName, dir));

            try
            {
                var youtube = new YoutubeClient();

                // 搜索视频
                var searchResults = await youtube.Search.GetVideosAsync(songName);
                var video = searchResults.FirstOrDefault();

                if (video == null)
                {
                    Console.WriteLine("未找到歌曲！");
                    return;
                }

                var streamManifest = await youtube.Videos.Streams.GetManifestAsync(video.Id);
                var audioStreams = streamManifest.GetAudioOnlyStreams();
                var streamInfo = audioStreams.OrderByDescending(s => s.Bitrate).FirstOrDefault();

                if (streamInfo == null)
                {
                    Console.WriteLine("未找到音频流！");
                    return;
                }
                Directory.CreateDirectory(dir);
                // 下载音频流
                var tempFile = Path.GetTempFileName();
              // string filePathTmp = dir + "/" + tempFile;
                Console.WriteLine($"down tmpfile=>{tempFile}");
                await youtube.Videos.Streams.DownloadAsync(streamInfo, tempFile);

                // 转换为 MP3
                // 转换为 MP3
                // 转换为 MP3
                string fname = filex.ConvertToValidFileName2024(songName);
                var outputFilePath = $"{dir}/{fname}.mp3";
                Console.WriteLine($"outputFilePath =>{outputFilePath}");
                await ConvertToMp3(tempFile, outputFilePath);

                // 删除临时文件
              //  File.Delete(tempFile);

                Console.WriteLine($"歌曲已下载并转换为 MP3：{outputFilePath}");
            }catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            dbgCls.dbg_setDbgValRtval(__METHOD__, 0);
        }

        static async Task ConvertToMp3(string inputFilePath, string outputFilePath)
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
