using StrawberryMusic.Command;
using StrawberryMusic.Model;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace StrawberryMusic.ViewModel
{
    class BrowseViewModel : BaseViewModel
    {
        public ICommand searchCommand { get; set; }
        private BrowseModel model;

        public ICommand downloadCommand
        {
            get { return model.DownloadCommand; }
            set { model.DownloadCommand = value; }
        }

        public ObservableCollection<YoutubeSearch> youtubeSearch
        {
            get { return model.YoutubeSearch; }
            set
            {
                model.YoutubeSearch = value;
                OnPropertyChanged("youtubeSearch");
            }
        }

        public string searchText
        {
            get { return model.SearchText; }
            set
            {
                model.SearchText = value;
                OnPropertyChanged("searchText");
            }
        }

        public BrowseViewModel()
        {
            model = new BrowseModel();
            model.alert += Alert;
            searchCommand = new RelayCommand(searchExecuteMethod);
            downloadCommand = new RelayCommand(downloadExecuteMethod);
        }

        /// <summary>
        /// 다운로드 불가능 메세지 띄우기 (접근 제한 동영상, 나이 제한 동영상)
        /// </summary>
        private void Alert()
        {
            MessageBox.Show("접근이 제한된 동영상입니다.");
        }

        /// <summary>
        /// 다운로드 버튼 커맨드
        /// </summary>
        /// <param name="videoTitle">클릭한 비디오 제목</param>        
        private void downloadExecuteMethod(object videoTitle)
        {
            string id = youtubeSearch.FirstOrDefault(e => e.title == videoTitle.ToString()).id;
            model.Download(id, videoTitle.ToString());
        }

        /// <summary>
        /// 검색 버튼 커맨드
        /// </summary>
        private void searchExecuteMethod(object obj)
        {
            model.Search();
        }
    }
}
