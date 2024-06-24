const ytdl = require('ytdl-core');
const ffmpeg = require('fluent-ffmpeg');
const ffmpegPath = require('ffmpeg-static');
const fs = require('fs');
const path = require('path');
const readline = require('readline');
const fetch = require('node-fetch');

ffmpeg.setFfmpegPath(ffmpegPath);


downloadSong("sweet like cola")

// 通过 YouTube 搜索获取视频 URL
async function getYouTubeVideoUrl(songName) {
    const searchUrl = `https://www.youtube.com/results?search_query=${encodeURIComponent(songName)}`;
    const res = await fetch(searchUrl);
    const text = await res.text();
    const videoIdMatch = text.match(/"videoId":"(.*?)"/);
    if (videoIdMatch) {
        const videoId = videoIdMatch[1];
        return `https://www.youtube.com/watch?v=${videoId}`;
    }
    throw new Error('视频未找到');
}

// 下载并转换为 MP3
async function downloadSong(songName) {
    try {
        const url = await getYouTubeVideoUrl(songName);
        const output = path.resolve(__dirname, `${songName}.mp3`);

        const stream = ytdl(url, {
            quality: 'highestaudio',
        });

        ffmpeg(stream)
            .audioBitrate(128)
            .save(output)
            .on('end', () => {
                console.log(`歌曲下载并保存为: ${output}`);
            })
            .on('error', (err) => {
                console.error('转换为 MP3 时出错:', err);
            });
    } catch (err) {
        console.error('下载歌曲时出错:', err);
    }
}