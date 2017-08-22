using VideoLibrary;
using System.IO;

namespace BDLib.Youtube
{
    public static class YoutubeDownloader
    {

        private static YouTube YT;

        public static void INIT()
        {
            YT = YouTube.Default;
        }

        public static void DownloadVideoToFile(string URI, string Path)
        {
            Video vid = YT.GetVideo(URI);
            File.WriteAllBytes(Path + "//" + vid.FullName, vid.GetBytes());
        }
        public static byte[] GetVideoBytes(string URI)
        {
            Video vid = YT.GetVideo(URI);
            return vid.GetBytes();
        }
        public static Stream GetVideoStream(string URI)
        {
            return (YT.GetVideo(URI)).Stream();
        }
    }
}
