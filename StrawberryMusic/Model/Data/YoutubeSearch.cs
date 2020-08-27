using System.Windows.Input;

namespace StrawberryMusic.Model
{
    /// <summary>
    /// 유튜브에서 검색된 비디오 관리 클래스
    /// </summary>
    class YoutubeSearch
    {
 
        /// <summary>
        /// 비디오 고유 id
        /// </summary>
        public string id { get; set; }

        /// <summary>
        /// 비디오 제목
        /// </summary>
        public string title { get; set; }

        /// <summary>
        /// 클릭 시 비디오 다운로드
        /// </summary>
        public ICommand downloadCommand { get; set; } 
    }
}
