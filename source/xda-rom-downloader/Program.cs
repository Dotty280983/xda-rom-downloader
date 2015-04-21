﻿using System;
using System.IO;
using System.Linq;
using cm12.i9300.downloader;
using Fclp;

namespace xda.rom.downloader
{
    class Program
    {
        static int Main(string[] args)
        {
            var options = Parse(args);
            if (options == null)
                return ExitWithError("Commandline parameter failure");
            var dstFolder = GetDestinationFolderFrom(options.OutputFolder);
            if (dstFolder == null)
                return 1;
            return DownloadLatest(options.XdaProjectId, dstFolder);
        }

        private static Args Parse(string[] args)
        {
            var parser = new FluentCommandLineParser<Args>();
            parser.Setup(o => o.OutputFolder)
                .As('o', "output")
                .WithDescription("Output folder")
                .Required();
            parser.Setup(o => o.XdaProjectId)
                .As('p', "project-id")
                .SetDefault(Downloader.CM12_1_PROJECTID)
                .WithDescription("XDA project id (get it from the forum download page url)");

            var result = parser.Parse(args);
            return result.HasErrors ? null : parser.Object;
        }

        private static int DownloadLatest(int projectId, string dstFolder)
        {
            var downloader = new Downloader();
            try
            {
                downloader.ProjectId = projectId;
                downloader.DownloadLatestTo(dstFolder);
            }
            catch (Exception ex)
            {
                return ExitWithError("Download fails: " + ex.Message);
            }
            return 0;
        }

        private static string GetDestinationFolderFrom(string folderPath)
        {
            if (!Directory.Exists(folderPath))
            {
                try
                {
                    Directory.CreateDirectory(folderPath);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Unable to find or create folder at '" + folderPath + "': " + ex.Message);
                    return null;
                }
            }
            return folderPath;
        }

        private static int ExitWithError(string message)
        {
            Console.WriteLine(message);
            return 1;
        }
    }
}