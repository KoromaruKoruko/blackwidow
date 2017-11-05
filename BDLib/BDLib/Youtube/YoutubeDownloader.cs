using VideoLibrary;
using System.IO;
using BDLib.BDLibInfo;

namespace BDLib.Youtube
{
    public static class YoutubeDownloader
    {
        private static YouTube YT;

        /// <summary>
        /// initulizes the class for use,
        /// only needs to be done once
        /// </summary>
        public static void INIT()
        {
            if (!Info.Moduls.Contains("Youtube/YoutubeDownloader.cs"))
                Info.Moduls.Add("Youtube/YoutubeDownloader.cs");
            YT = YouTube.Default;
        }

        /// <summary>
        /// Downloads a video from youtube using YOUR PC
        /// </summary>
        /// <param name="URI">uri/url to video</param>
        /// <param name="Path">output directory</param>
        public static void DownloadVideoToFile(string URI, string Path)
        {
            Video vid = YT.GetVideo(URI);
            File.WriteAllBytes(Path + "//" + vid.FullName, vid.GetBytes());
        }
        /// <summary>
        /// downloads a video from youtube using YOUR PC and returns the file data
        /// </summary>
        /// <param name="URI">uri/url to video</param>
        /// <returns>video data</returns>
        public static byte[] GetVideoBytes(string URI)
        {
            Video vid = YT.GetVideo(URI);
            return vid.GetBytes();
        }
        /// <summary>
        /// creates a stream to a youtube video for reading
        /// </summary>
        /// <param name="URI">uri/url to video</param>
        /// <returns>video stream</returns>
        public static Stream GetVideoStream(string URI)
        {
            return YT.GetVideo(URI).Stream();
        }
    }
}
