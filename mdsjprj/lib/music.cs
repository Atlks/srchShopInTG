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
namespace mdsj.lib
{
    internal class music
    {
       public static async Task DownloadSongAsMp3(string songName,string dir)
        {
            var __METHOD__ = "DownloadSongAsMp3";
            dbgCls.setDbgFunEnter(__METHOD__, dbgCls.func_get_args(  songName, dir));

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
                string filePathTmp = dir + "/" + tempFile;
                Console.WriteLine($"down tmpfile=>{filePathTmp}");
                await youtube.Videos.Streams.DownloadAsync(streamInfo, filePathTmp);

                // 转换为 MP3
                // 转换为 MP3
                // 转换为 MP3
               
                var outputFilePath = $"{dir}/{songName}.mp3";
                Console.WriteLine($"outputFilePath =>{outputFilePath}");
                await ConvertToMp3(tempFile, outputFilePath);

                // 删除临时文件
              //  File.Delete(tempFile);

                Console.WriteLine($"歌曲已下载并转换为 MP3：{outputFilePath}");
            }catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            dbgCls.setDbgValRtval(__METHOD__, 0);
        }

        static async Task ConvertToMp3(string inputFilePath, string outputFilePath)
        {
            var ffmpeg = new NReco.VideoConverter.FFMpegConverter();
            ffmpeg.ConvertMedia(inputFilePath, outputFilePath, "mp3");
        }

    }
}
