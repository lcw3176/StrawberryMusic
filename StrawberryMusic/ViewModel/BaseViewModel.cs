using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;

namespace StrawberryMusic.ViewModel
{
    class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private static protected Queue<string> songName = new Queue<string>();
        private static protected ManualResetEvent controller = new ManualResetEvent(false);
        private static protected Dictionary<string, int> mediaFileDictionary = new Dictionary<string, int>();


        protected void OnPropertyChanged(string param)
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(param));
            }
        }

        protected void RegisterSongNameToParent(Dictionary<string, int> _mediaFile)
        {
            mediaFileDictionary = _mediaFile;
        }

        /// <summary>
        /// 재생할 노래 제목 등록
        /// </summary>
        protected void RegisterSongToPlay(string fileName)
        {
            songName.Enqueue(fileName);
            controller.Set();
        }

    }
}
