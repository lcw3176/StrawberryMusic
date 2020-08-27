using System;
using System.ComponentModel;
using System.Windows.Media;

namespace StrawberryMusic.Model.Data
{
    /// <summary>
    /// 미디어 플레이어에 부가적으로 표시되는 데이터 클래스
    /// </summary>
    class DisplayInfo : INotifyPropertyChanged
    {
        private string preSong;
        private string nextSong;
        private string nowSong;
        private bool isShuffle = false;
        private double sliderLength = 0;
        private double sliderValue = 0;
        private ImageSource buttonImage;
        private ImageSource playModeImage;


        public bool IsShuffle
        {
            get { return isShuffle; }
            set { isShuffle = value; }
        }


        /// <summary>
        /// 이전 노래 제목
        /// </summary>
        public string PreSong
        {
            get { return preSong; }
            set
            {
                preSong = value;
                OnPropertyUpdate("PreSong");
            }
        }

        /// <summary>
        /// 다음 노래 제목
        /// </summary>
        public string NextSong
        {
            get { return nextSong; }
            set
            {
                nextSong = value;
                OnPropertyUpdate("NextSong");
            }
        }

        /// <summary>
        /// 현재 재생중인 노래
        /// </summary>
        public string NowSong
        {
            get { return nowSong; }
            set
            {
                nowSong = value;
                OnPropertyUpdate("NowSong");
            }
        }


        /// <summary>
        /// 재생, 정지 버튼 이미지
        /// </summary>
        public ImageSource ButtonImage
        {
            get { return buttonImage; }
            set
            {
                buttonImage = value;
                OnPropertyUpdate("buttonImage");
            }
        }

        /// <summary>
        /// 플레이 모드 버튼 이미지
        /// </summary>
        public ImageSource PlayModeImage
        {
            get { return playModeImage; }
            set
            {
                playModeImage = value;
                OnPropertyUpdate("PlayModeImage");
            }
        }

        /// <summary>
        /// 슬라이더 바 value
        /// </summary>        
        public double SliderValue
        {
            get { return sliderValue; }
            set
            {
                sliderValue = value;
                OnPropertyUpdate("SliderValue");
            }
        }

        /// <summary>
        /// 슬라이더 바 Maximum Value
        /// </summary>
        public double SliderLength
        {
            get { return sliderLength; }
            set
            {
                sliderLength = value;
                OnPropertyUpdate("SliderLength");
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyUpdate(string param)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(param));
            }
        }
    }
}
