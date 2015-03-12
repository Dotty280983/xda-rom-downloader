using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace cm12.i9300.downloader.tests
{
    [TestFixture]
    public class TestDownloader
    {
        [Test]
        public void GetLatestDownloadUrl()
        {
            //---------------Set up test pack-------------------
            var sut = new Downloader();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = sut.GetLatestDownload();

            //---------------Test Result -----------------------
            Console.WriteLine(result);
        }

        [Test]
        public void DownloadLatestTo()
        {
            //---------------Set up test pack-------------------
            var sut = new Downloader();

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var dstFile = sut.DownloadLatestTo("C:\\tmp");

            //---------------Test Result -----------------------
            Console.WriteLine("Done: {0}", dstFile);
        }

        [Test]
        public void HumanTime_Given1_ShouldReturn_00m01s()
        {
            //---------------Set up test pack-------------------
            var input = 1;
            var expected = "00m01s";
            var sut = Create();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = sut.HumanTime(input);

            //---------------Test Result -----------------------
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void HumanTime_Given60_ShouldReturn_01m00s()
        {
            //---------------Set up test pack-------------------
            var input = 60;
            var expected = "01m00s";
            var sut = Create();
            
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = sut.HumanTime(input);

            //---------------Test Result -----------------------
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void HumanTime_Given61_ShouldReturn_01m01s()
        {
            //---------------Set up test pack-------------------
            var input = 61;
            var expected = "01m01s";
            var sut = Create();
            
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = sut.HumanTime(input);

            //---------------Test Result -----------------------
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void HumanTime_Given3600_ShouldReturn_01h00m00s()
        {
            //---------------Set up test pack-------------------
            var input = 3600;
            var expected = "01h00m00s";
            var sut = Create();
            
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = sut.HumanTime(input);

            //---------------Test Result -----------------------
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void HumanTime_Given3660_ShouldReturn_01h01m00s()
        {
            //---------------Set up test pack-------------------
            var input = 3660;
            var expected = "01h01m00s";
            var sut = Create();
            
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = sut.HumanTime(input);

            //---------------Test Result -----------------------
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void HumanTime_Given3666_ShouldReturn_01h01m06s()
        {
            //---------------Set up test pack-------------------
            var input = 3666;
            var expected = "01h01m06s";
            var sut = Create();
            
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = sut.HumanTime(input);

            //---------------Test Result -----------------------
            Assert.AreEqual(expected, result);
        }


        private Downloader Create()
        {
            return new Downloader();
        }
    }
}
