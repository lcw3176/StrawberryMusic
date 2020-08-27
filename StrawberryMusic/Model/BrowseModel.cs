using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using YoutubeExplode;
using YoutubeExplode.Videos.Streams;

namespace StrawberryMusic.Model
{
    class BrowseModel
    {
        public delegate void Alert();
        public event Alert alert;

        private ObservableCollection<YoutubeSearch> youtubeSearch = new ObservableCollection<YoutubeSearch>();
        private string searchText;
        private ICommand downloadCommand;

        public ICommand DownloadCommand
        {
            get { return downloadCommand; }
            set { downloadCommand = value; }
        }

        public ObservableCollection<YoutubeSearch> YoutubeSearch
        {
            get { return youtubeSearch; }
            set { youtubeSearch = value; }
        }
    
        public string SearchText
        {
            get { return searchText; }
            set { searchText = value; }
        }

        /// <summary>
        /// 유튜브에서 노래 검색
        /// </summary>
        public async void Search()
        {
            YoutubeSearch.Clear();

            string apiKey = Environment.GetEnvironmentVariable("YOUTUBE_API_KEY", EnvironmentVariableTarget.User);
            string appName = Environment.GetEnvironmentVariable("YOUTUBE_Application", EnvironmentVariableTarget.User);

            var youtube = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = apiKey,
                ApplicationName = appName
            });

            var request = youtube.Search.List("snippet");

            request.Q = SearchText;
            request.MaxResults = 100;

            var result = await request.ExecuteAsync();

            foreach (var item in result.Items)
            {
                if (item.Id.Kind == "youtube#video")
                {
                    YoutubeSearch.Add(new YoutubeSearch()
                    {
                        id = item.Id.VideoId,
                        title = item.Snippet.Title,
                        downloadCommand = DownloadCommand,
                    });
                }
            }

            youtube.Dispose();
        }

        /// <summary>
        /// 노래 다운로드 (사운드만 추출해서 다운)
        /// </summary>
        /// <param name="videoId">비디오 고유 아이디</param>
        /// <param name="videoName">비디오 제목</param>
        public async void Download(string videoId, string videoName)
        {
            try
            {
                var youtube = new YoutubeClient();
                var streamManifest = await youtube.Videos.Streams.GetManifestAsync(videoId);
                var streamInfo = streamManifest.GetAudioOnly().WithHighestBitrate();
                Regex pattern = new Regex(@"[\/:*?<>|]");
                videoName = pattern.Replace(videoName, string.Empty);
                string path = AppDomain.CurrentDomain.BaseDirectory + @"playlist\";


                if (streamInfo != null)
                {
                    await youtube.Videos.Streams.DownloadAsync(streamInfo, path + videoName + ".webm");
                    await Task.Run(() =>
                     {
                         using (var reader = new NAudio.Wave.AudioFileReader(path + videoName + ".webm"))
                         {
                             using (var writer = new NAudio.Lame.LameMP3FileWriter(path + videoName + ".prepareMp3", reader.WaveFormat, NAudio.Lame.LAMEPreset.STANDARD))
                             {
                                 reader.CopyTo(writer);
                             }
                         }

                         // 변환 도중에 사용자가 음악 재생을 클릭했을때 IOException이 나는것을 방지하기 위해
                         // 확장명을 일부러 다르게 써서 mp3 파일로 변환이 끝나면 확장명을 바꿔준다.
                         File.Move(path + videoName + ".prepareMp3", path + videoName + ".mp3");
                         File.Delete(path + videoName + ".webm");
                         File.Delete(path + videoName + ".prepareMp3");
                     });
                                            
                }

            }

            catch
            {
                alert();
            }

        }
    }
}
