using StrawberryMusic.Command;
using StrawberryMusic.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;

namespace StrawberryMusic.ViewModel
{
    class PlayListViewModel : BaseViewModel
    {
        private PlaylListModel model;
        private ObservableCollection<MediaFile> mediaFile = new ObservableCollection<MediaFile>();
        public ICommand playSongCommand { get; set; }
        public ICommand refreshCommand { get; set; }


        public ObservableCollection<MediaFile> MediaFile
        {
            get { return mediaFile; }
            set
            {
                mediaFile = value;
                OnPropertyChanged("MediaFile");
            }
        }

        public PlayListViewModel()
        {
            model = new PlaylListModel();
            playSongCommand = new RelayCommand(playSongExecuteMethod);
            refreshCommand = new RelayCommand(RefreshExecuteMethod);
        }

        /// <summary>
        /// 새로고침 버튼 커맨드, 노래 목록 불러오기
        /// </summary>
        public void RefreshExecuteMethod(object obj)
        {
            mediaFile.Clear();
            Dictionary<string, int> songName = new Dictionary<string, int>();
            FileInfo[] fileinfo = model.GetPlayList();

            for (int i = 0; i < fileinfo.Length; i++)
            {
                MediaFile.Add(new MediaFile()
                {
                    name = fileinfo[i].Name,
                    visualName = fileinfo[i].Name.Replace(fileinfo[i].Extension, string.Empty),
                    playSongCommand = this.playSongCommand
                });

                songName.Add(fileinfo[i].Name, i);
            }

            RegisterSongNameToParent(songName);
        }

        /// <summary>
        /// 클릭한 노래 재생
        /// </summary>
        private void playSongExecuteMethod(object obj)
        {
            string songName = MediaFile.FirstOrDefault(e => e.name == obj.ToString()).name;
            RegisterSongToPlay(songName);
        }

        
    }
}
