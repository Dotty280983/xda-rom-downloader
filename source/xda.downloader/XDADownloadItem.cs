namespace cm12.i9300.downloader
{
    public class XDADownloadItem
    {
        public string MD5;
        public string Url { get; set; }
        public string Filename { get; set; }
        public string DownloadUrl { get; set; }

        public XDADownloadItem(string relativeUrl, string filename)
        {
            Url = "http://forum.xda-developers.com" + relativeUrl;
            DownloadUrl = Url + "&task=get";
            Filename = filename;
        }
    }
}