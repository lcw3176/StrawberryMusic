using System.Windows.Input;

namespace StrawberryMusic.Model
{
    /// <summary>
    /// 내가 가지고 있는 노래 파일 클래스
    /// </summary>
    class MediaFile
    {
        /// <summary>
        /// 실제 노래 파일 제목
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 파일 제목에서 확장명 제거된 값, 유저한테 디스플레이 되는 값
        /// </summary>
        public string visualName { get; set; }

        /// <summary>
        /// 클릭 시 재생 커맨드
        /// </summary>
        public ICommand playSongCommand { get; set; }
    }
}
