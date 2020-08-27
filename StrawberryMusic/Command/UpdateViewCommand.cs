using StrawberryMusic.ViewModel;
using System;
using System.Windows.Input;

namespace StrawberryMusic.Command
{
    class UpdateViewCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private MainViewModel mainViewModel;

        PlayListViewModel playList = new PlayListViewModel();
        NowPlayingViewModel nowPlaying = new NowPlayingViewModel();
        BrowseViewModel browse = new BrowseViewModel();

        public UpdateViewCommand(MainViewModel _mainViewModel)
        {
            mainViewModel = _mainViewModel;

            playList.RefreshExecuteMethod(null);
            mainViewModel.selectedViewModel = playList;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        /// <summary>
        /// MainWindow에서 좌측 버튼들 클릭할 때 마다 알맞은 View 연결
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object parameter)
        {
            if(parameter.ToString().Equals("playList"))
            {
                playList.RefreshExecuteMethod(null);
                mainViewModel.selectedViewModel = playList;
            }

            if(parameter.ToString().Equals("nowPlaying"))
            {
                mainViewModel.selectedViewModel = nowPlaying;
            }

            if(parameter.ToString().EndsWith("browse"))
            {
                mainViewModel.selectedViewModel = browse;
            }
        }
    }
}
