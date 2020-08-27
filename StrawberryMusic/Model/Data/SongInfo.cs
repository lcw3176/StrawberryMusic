using System;
using System.ComponentModel;

namespace StrawberryMusic.Model.Data
{
    /// <summary>
    /// 현재 재생중인 노래 정보
    /// </summary>
    class SongInfo : INotifyPropertyChanged
    {
        private bool isRun = false;
        private float volume = 100;
        private TimeSpan position;
        private TimeSpan totalLength;


        /// <summary>
        /// 현재 노래 재생 여부 
        /// </summary>
        public bool IsRun
        {
            get { return isRun; }
            set
            {
                isRun = value;
            }
        }

        /// <summary>
        /// 미디어 플레이어 볼륨 값
        /// </summary>
        public float Volume
        {
            get { return volume; }
            set
            {
                volume = value;
                OnPropertyUpdate("Volume");
            }
        }

        /// <summary>
        /// 현재 재생되는 위치
        /// </summary>
        public TimeSpan Position
        {
            get { return position; }
            set
            {
                position = value;
                OnPropertyUpdate("Position");
            }
        }


        /// <summary>
        /// 노래 실제 길이
        /// </summary>
        public TimeSpan TotalLength
        {
            get { return totalLength; }
            set
            {
                totalLength = value;
                OnPropertyUpdate("TotalLength");
            }
        }



        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyUpdate(string param)
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(param));
            }
        }
    }
}
