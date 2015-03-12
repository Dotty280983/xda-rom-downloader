using System;
using System.IO;
using System.Linq;
using cm12.i9300.downloader;

namespace cm12.i9300.UNOFFICIAL.downloader
{
    class Program
    {
        static int Main(string[] args)
        {
            if (args.Length < 1)
                return ExitWithError("No destination folder specified");
            var dstFolder = GetDestinationFolderFrom(args);
            if (dstFolder == null)
                return 1;
            return DownloadLatestTo(dstFolder);
        }

        private static int DownloadLatestTo(string dstFolder)
        {
            var downloader = new Downloader();
            try
            {
                downloader.DownloadLatestTo(dstFolder);
            }
            catch (Exception ex)
            {
                return ExitWithError("Download fails: " + ex.Message);
            }
            return 0;
        }

        private static string GetDestinationFolderFrom(string[] args)
        {
            var dstFolder = args.First();
            if (!Directory.Exists(dstFolder))
            {
                try
                {
                    Directory.CreateDirectory(dstFolder);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Unable to find or create folder at '" + dstFolder + "': " + ex.Message);
                    return null;
                }
            }
            return dstFolder;
        }

        private static int ExitWithError(string message)
        {
            Console.WriteLine(message);
            return 1;
        }
    }
}
