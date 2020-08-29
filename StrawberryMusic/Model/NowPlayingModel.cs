using NAudio.Wave;
using StrawberryMusic.Model.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace StrawberryMusic.Model
{
    enum State
    {
        Pause,
        Play
    }

    class NowPlayingModel
    {
        public delegate void PlayEndEvent(object Null); // NowPlayViewModel의 NextSongExecuteMethod와 연결됨. 다음곡으로 넘기는 기능
        public event PlayEndEvent playEndEvent; 

        private SongInfo songInfo = new SongInfo();
        private DisplayInfo displayInfo = new DisplayInfo();
        private MediaElement media = new MediaElement();
        private DispatcherTimer timer = new DispatcherTimer();
        private ImageSourceConverter c = new ImageSourceConverter();
        private AudioFileReader audioReader;
        private WaveOut waveOut;

        public AudioFileReader AudioReader 
        {
            get { return audioReader; }
            set { audioReader = value; }
        }

        /// <summary>
        /// 현재 재생중인 노래 정보 클래스
        /// </summary>
        public SongInfo SongInfo
        {
            get { return songInfo; }
            set { songInfo = value; }
        }

        /// <summary>
        /// 디스플레이 되는 부가적인 정보 클래스
        /// </summary>
        public DisplayInfo DisplayInfo
        {
            get { return displayInfo; }
            set { displayInfo = value; }
        }


        public NowPlayingModel()
        {
            timer.Interval = TimeSpan.FromMilliseconds(500);
            displayInfo.ButtonImage = (ImageSource)c.ConvertFrom("./Resource/play.png");
        }

        /// <summary>
        /// 노래 정보 업데이트 후 재생
        /// </summary>
        public void UpdateMedia(string songName)
        {
            App.Current.Dispatcher.Invoke(() =>
            {

                if (waveOut != null) { waveOut.Dispose(); }
                if (audioReader != null) { audioReader.Dispose(); audioReader.Close(); }


                audioReader = new AudioFileReader(AppDomain.CurrentDomain.BaseDirectory + @"playlist\" + songName);

                audioReader.Volume = SongInfo.Volume / 100;

                DisplayInfo.NowSong = songName;
                DisplayInfo.ButtonImage = (ImageSource)c.ConvertFrom("./Resource/stop.png");
                DisplayInfo.PlayModeImage = (ImageSource)c.ConvertFrom("./Resource/shuffle.png");

                DisplayInfo.SliderLength = audioReader.TotalTime.TotalSeconds;

                SongInfo.TotalLength = audioReader.TotalTime;
                SongInfo.IsRun = true;
                waveOut = new WaveOut();

                waveOut.Init(audioReader);
                waveOut.Play();
            });

        }

        /// <summary>
        /// 슬라이더바 타이머 추가
        /// </summary>
        public void AttachTimer()
        {
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        /// <summary>
        /// 슬라이더바 타이머 제거
        /// </summary>
        public void DetachTimer()
        {
            timer.Tick -= Timer_Tick;   
        }


        /// <summary>
        /// 슬라이더 진행 타이머(0.5초 간격) 
        /// </summary>
        private void Timer_Tick(object sender, EventArgs e)
        {
            if(songInfo.IsRun)
            {
                if (SongInfo.Position.TotalSeconds != DisplayInfo.SliderValue) // 유저가 노래 진행시간 임의로 바꿨을 때
                {
                    waveOut.Pause();
                    audioReader.CurrentTime = TimeSpan.FromSeconds(DisplayInfo.SliderValue);   
                    waveOut.Play();
                }

                SongInfo.Position = audioReader.CurrentTime;
                DisplayInfo.SliderValue = SongInfo.Position.TotalSeconds;

                if (SongInfo.Position >= SongInfo.TotalLength) // 두개의 값이 같아지면 타이머 스탑
                {
                    timer.Stop();
                    playEndEvent(null);
                }
            }
        }

        /// <summary>
        /// 미디어 플레이어 정보 업데이트
        /// </summary>
        /// <param name="state"> 미디어 상태, Play와 Pause 두가지 존재 </param>
        public void ChangeMediaState(State state)
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                if (state == State.Play)
                {
                    DisplayInfo.ButtonImage = (ImageSource)c.ConvertFrom("./Resource/stop.png");
                    SongInfo.IsRun = true;
                    waveOut.Resume();
                }

                else
                {
                    DisplayInfo.ButtonImage = (ImageSource)c.ConvertFrom("./Resource/play.png");
                    SongInfo.IsRun = false;
                    waveOut.Pause();
                }
            });          
        }

        /// <summary>
        /// 이전 곡, 다음 곡 정보 등록
        /// </summary>
        /// <param name="mediaFile"> BaseViewModel에 있는 내가 가지고 있는 미디어 파일 목록 </param>
        public void UpdateWaitList(Dictionary<string, int> mediaFile)
        {
            // 셔플 재생 모드일 때
            if (DisplayInfo.IsShuffle) 
            {
                Random rand = new Random();

                int nextIndex = mediaFile[DisplayInfo.NowSong];
                int preIndex = mediaFile[DisplayInfo.NowSong];

                // 같은 곡이 또 뽑히지 않게, 현재 인덱스와 다른 숫자가 나올때까지 반복
                while (nextIndex == mediaFile[DisplayInfo.NowSong])
                {
                   nextIndex = rand.Next(0, mediaFile.Count);
                }

                while(preIndex == mediaFile[DisplayInfo.NowSong] || preIndex == nextIndex)
                {
                    preIndex = rand.Next(0, mediaFile.Count);
                }                
               
                DisplayInfo.NextSong = mediaFile.FirstOrDefault(f => f.Value == nextIndex).Key;
                DisplayInfo.PreSong = mediaFile.FirstOrDefault(f => f.Value == preIndex).Key;
            }

            // 순차 재생 모드일 때
            else
            {
                int nextIndex = mediaFile[DisplayInfo.NowSong] + 1;
                DisplayInfo.NextSong = mediaFile.FirstOrDefault(f => f.Value == nextIndex).Key;

                // 다음 인덱스가 비었을 때 (지금 재생되는 곡이 플레이리스트의 마지막 곡일경우)
                if (string.IsNullOrEmpty(DisplayInfo.NextSong))
                {
                    // 맨 처음 곡으로 다음 인덱스 설정
                    nextIndex = 0;
                    DisplayInfo.NextSong = mediaFile.FirstOrDefault(f => f.Value == nextIndex).Key;
                }

                int preIndex = mediaFile[DisplayInfo.NowSong] - 1;
                DisplayInfo.PreSong = mediaFile.FirstOrDefault(f => f.Value == preIndex).Key;

                // 이전 인덱스가 비었을 때 (지금 재생되는 곡이 플레이리스트의 첫번째 곡일 경우)
                if (string.IsNullOrEmpty(DisplayInfo.PreSong))
                {
                    // 맨 끝곡으로 이전 인덱스 설정
                    preIndex = mediaFile.Count - 1;
                    DisplayInfo.PreSong = mediaFile.FirstOrDefault(f => f.Value == preIndex).Key;
                }
            }
            

            DisplayInfo.PreSong = DisplayInfo.PreSong.Replace(".mp3", string.Empty);
            DisplayInfo.NextSong = DisplayInfo.NextSong.Replace(".mp3", string.Empty);
            DisplayInfo.NowSong = DisplayInfo.NowSong.Replace(".mp3", string.Empty);
        }

        /// <summary>
        /// 노래 재생 방식 변경
        /// </summary>
        public void ChangePlayMode()
        {
            if(DisplayInfo.IsShuffle)
            {
                DisplayInfo.PlayModeImage = (ImageSource)c.ConvertFrom("./Resource/shuffle.png");
                DisplayInfo.IsShuffle = false;
            }

            else
            {
                DisplayInfo.PlayModeImage = (ImageSource)c.ConvertFrom("./Resource/round.png");
                DisplayInfo.IsShuffle = true;
            }
        }


    }
}
