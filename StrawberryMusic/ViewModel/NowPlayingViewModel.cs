using StrawberryMusic.Command;
using StrawberryMusic.Model;
using StrawberryMusic.Model.Data;
using System;
using System.Threading;
using System.Windows.Input;

namespace StrawberryMusic.ViewModel
{
    class NowPlayingViewModel : BaseViewModel
    {
        private NowPlayingModel model = new NowPlayingModel();

        public ICommand nextSongCommand { get; set; }
        public ICommand preSongCommand { get; set; }
        public ICommand playButtonCommand { get; set; }
        public ICommand playModeCommand { get; set; }


        /// <summary>
        /// 현재 재생중인 노래 정보 클래스
        /// </summary>
        public SongInfo SongInfo
        {
            get { return model.SongInfo; }
            set { model.SongInfo = value; }
        }

        /// <summary>
        /// 디스플레이 되는 부가적인 정보 클래스
        /// </summary>
        public DisplayInfo DisplayInfo
        {
            get { return model.DisplayInfo; }
            set { model.DisplayInfo = value; }
        }


        public NowPlayingViewModel()
        {
            model.playEnd += NextSongExecuteMethod;
            DisplayInfo.PropertyChanged += DisplayInfo_PropertyChanged;
            SongInfo.PropertyChanged += SongInfo_PropertyChanged;

            playButtonCommand = new RelayCommand(playButtonExecuteMethod);
            nextSongCommand = new RelayCommand(NextSongExecuteMethod);
            preSongCommand = new RelayCommand(PreSongExecuteMethod);
            playModeCommand = new RelayCommand(PlayModeExecuteMethod);

            Thread queueThread = new Thread(ChangeSongThread);
            queueThread.IsBackground = true;            

            queueThread.Start();
        }

        /// <summary>
        /// 노래 재생 방식 변경 커맨드 ( 셔플, 전체 반복재생 )
        /// </summary>
        private void PlayModeExecuteMethod(object obj)
        {
            model.ChangePlayMode();
        }

        /// <summary>
        /// SongInfo 클래스 변동 시 알림
        /// </summary>
        private void SongInfo_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(e.PropertyName == "Volume" && model.AudioReader != null)
            {
                model.AudioReader.Volume = SongInfo.Volume / 100;
            }

            OnPropertyChanged(e.PropertyName);
        }


        /// <summary>
        /// DisplayInfo 클래스 변동 시 알림
        /// </summary>
        private void DisplayInfo_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            OnPropertyChanged(e.PropertyName);
        }


        /// <summary>
        /// 이전 노래로 넘기기
        /// </summary>
        private void PreSongExecuteMethod(object obj)
        {
            RegisterSongToPlay(DisplayInfo.PreSong + ".mp3");
        }

        /// <summary>
        /// 다음 노래로 넘기기
        /// </summary>
        private void NextSongExecuteMethod(object obj)
        {
            RegisterSongToPlay(DisplayInfo.NextSong + ".mp3");
        }

        /// <summary>
        /// 재생, 정지 버튼 누르면 실행, 상황에 따라 미디어와 버튼 이미지를 바꿈.
        /// </summary>
        private void playButtonExecuteMethod(object obj)
        {
            if(string.IsNullOrEmpty(DisplayInfo.NowSong))
            {
                return;
            }

            if (SongInfo.IsRun)
            {
                model.ChangeMediaState(State.Pause);
            }

            else
            {
                model.ChangeMediaState(State.Play);
            }

        }

        /// <summary>
        /// 큐에 노래목록 감시, 채워지면 해당 노래 실행
        /// </summary>
        private void ChangeSongThread()
        {
            while (true)
            {
                while (songName.Count > 0)                     // 큐에 노래 제목이 차면
                {
                    model.DetachTimer();                       // 먼저 이전에 존재하는 슬라이더 바의 타이머를 해제하고
                    model.UpdateMedia(songName.Dequeue());     // 큐에서 빼낸 미디어 파일 이름을 미디어 플레이어에 등록한 후
                    model.UpdateWaitList(mediaFileDictionary); // 이전 곡 제목과 다음곡 제목을 등록해주고
                    model.AttachTimer();                       // 슬라이더 바 타이머를 다시 붙여준다
                }

                controller.Reset();                            // 스레드를 중지한 후
                controller.WaitOne(Timeout.Infinite);          // 무한 대기
            }
        }

     
    }
}
