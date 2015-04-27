using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace cm12.i9300.downloader
{
    public class XDADownloader
    {
        public const int CM12_PROJECTID = 6661; // deprecated
        public const int CM12_1_PROJECTID = 8923;
        public const string DOWNLOAD_URL_TEMPLATE = "http://forum.xda-developers.com/devdb/project/?id={0}#downloads"; 
        private const string CONTENT_LENGTH_HEADER = "Content-Length";
        public int ProjectId { get; set; }

        public XDADownloader()
        {
            ProjectId = CM12_1_PROJECTID;
        }

        public XDADownloadItem GetLatestDownload()
        {
            var req = WebRequest.Create(String.Format(DOWNLOAD_URL_TEMPLATE, ProjectId));
            using (var stream = req.GetResponse().GetResponseStream())
            {
                var doc = new HtmlDocument();
                doc.Load(stream);
                foreach (var anchor in doc.DocumentNode.SelectNodes("//a[starts-with(@href,  '/devdb/project/dl/?id=')]"))
                {
                    if (Path.GetExtension(anchor.InnerText) == ".zip")
                    {
                        var dl =  new XDADownloadItem(anchor.Attributes["href"].Value, anchor.InnerText);
                        dl.MD5 = GetMD5For(dl.Url);
                        return dl;
                    }
                }
                return null;
            }
        }

        public string GetMD5For(string downloadPageUrl)
        {
            var req = WebRequest.Create(downloadPageUrl);
            using (var stream = req.GetResponse().GetResponseStream())
            {
                var doc = new HtmlDocument();
                doc.Load(stream);
                return doc.DocumentNode
                            .SelectNodes("//div[@id='project_download__md5']/div")
                            .First()
                                .InnerText.Trim();
            }
        }
        
        public string DownloadLatestTo(string destPath)
        {
            var dl = GetLatestDownload();
            var outFile = Path.Combine(destPath, dl.Filename);
            var req = WebRequest.Create(dl.DownloadUrl) as HttpWebRequest;
            Console.WriteLine("Attempt download: {0} from {1} ({2})", dl.Filename, dl.DownloadUrl, dl.MD5);
            long existingSize = 0;
            if (File.Exists(outFile))
            {
                var fullDownloadSize = GetContentLengthFor(dl.DownloadUrl);
                var finfo = new FileInfo(outFile);
                existingSize = finfo.Length;
                if (fullDownloadSize == existingSize)
                {
                    Console.WriteLine("Already fully downloaded");
                    Verify(outFile, dl.MD5);
                    return outFile;
                }
                Console.WriteLine("Resuming download from byte: {0}", finfo.Length);
                req.AddRange(finfo.Length);
            }
            req.Timeout = 90000;
            using (var response = req.GetResponse())
            {
                var expectedSize = long.Parse(response.Headers[CONTENT_LENGTH_HEADER]);
                Console.WriteLine("Should get {0} bytes to {1}", expectedSize, outFile);
                DownloadFile(response, outFile, expectedSize, expectedSize + existingSize, existingSize);
                return Verify(outFile, dl.MD5) ? outFile : null;
            }
        }

        private long GetContentLengthFor(string downloadUrl)
        {
            var req = WebRequest.Create(downloadUrl) as HttpWebRequest;
            using (var response = req.GetResponse())
            {
                return long.Parse(response.Headers[CONTENT_LENGTH_HEADER]);
            }
        }

        private bool Verify(string filePath, string md5Sum)
        {
            using (var md5 = new MD5CryptoServiceProvider())
            {
                var buffer = md5.ComputeHash(File.ReadAllBytes(filePath));
                var sb = new StringBuilder();
                for (int i = 0; i < buffer.Length; i++)
                {
                    sb.Append(buffer[i].ToString("x2"));
                }
                var fileMd5 = sb.ToString();
                if (md5Sum.ToLower() != fileMd5.ToLower())
                {
                    Console.WriteLine("MD5Sum of downloaded file does not match expected hash ): ");
                    DeleteFile(filePath);
                    return false;
                }
                Console.WriteLine("Download checks out fine!");
                return true;
            }
        }

        private void DeleteFile(string outFile)
        {
            try { File.Delete(outFile); } catch { }
        }

        private void DownloadFile(WebResponse response, string outFile, long expectedSize, long totalSize, long offset)
        {
            var started = DateTime.Now;
            using (var reader = new BinaryReader(response.GetResponseStream()))
            {
                using (var outStream = new FileStream(outFile, FileMode.Append))
                {
                    var haveRead = 0;
                    using (var writer = new BinaryWriter(outStream))
                    {
                        while (haveRead < expectedSize)
                        {
                            var toRead = expectedSize - haveRead;
                            if (toRead > 8192)
                                toRead = 8192;
                            var readBuf = reader.ReadBytes((int)toRead);
                            haveRead += readBuf.Length;
                            writer.Write(readBuf, 0, readBuf.Length);
                            writer.Flush();
                            ReportProgress(expectedSize, haveRead, started, totalSize, offset);
                        }
                    }
                }
            }
        }

        private const string BLANKLINE = "                                                ";

        private void ReportProgress(long expectedSize, int haveRead, DateTime started, long totalSize, long offset)
        {
            var totalPerc = (haveRead + offset)*100M/totalSize;
            var dlTime = (decimal)(DateTime.Now - started).TotalSeconds;
            var perc = haveRead*100M/expectedSize;
            var rate = (haveRead/1024M)/dlTime;
            var etaInSeconds = 100M*(dlTime/perc);
            Console.Write("\r{0}\r{1:0.0} % {2:0.0} Kb/s  ETA: {3}", BLANKLINE, totalPerc, rate, HumanTime((int)etaInSeconds));
        }

        public string HumanTime(int etaInSeconds)
        {
            var hours = (int) (etaInSeconds/3600);
            var minutes = (int) ((etaInSeconds%3600)/60);
            var seconds = etaInSeconds%60;
            Func<int, string> zeropad = i => i < 10 ? "0" + i : i.ToString();
            if (hours > 0)
                return string.Format("{0}h{1}m{2}s", zeropad(hours), zeropad(minutes), zeropad(seconds));
            return string.Format("{0}m{1}s", zeropad(minutes), zeropad(seconds));
        }
    }
}
