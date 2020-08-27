using System.Collections.ObjectModel;
using System.IO;

namespace StrawberryMusic.Model
{
    class PlaylListModel
    {
        /// <summary>
        /// playList 폴더에 있는 파일들 불러오기
        /// </summary>
        /// <returns>mp3 확장자 파일들만 읽어옴</returns>
        public FileInfo[] GetPlayList()
        {
            DirectoryInfo directory = new DirectoryInfo("playlist");

            if (!directory.Exists)
            {
                directory.Create();
            }


            return directory.GetFiles("*.mp3");
        }
    }
}
