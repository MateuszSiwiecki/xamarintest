using Xunit;
using OpenCvLib;
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.CompilerServices;
using System.IO;

namespace OpenCvLib.Tests
{
    public class MainClassTests
    {
        private static string TestImage => @"C:\Users\mateu\OneDrive\Desktop\QR\documents\recepta_test1.png";
        private static string TestImageDirectory([CallerMemberName] string testName = "") => TestImageDirectory(0, testName);
        private static string TestImageDirectory(int iteration, [CallerMemberName] string testName = "")
        {
            var path = @"C:\Users\mateu\OneDrive\Desktop\QR\documents";
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);

            path = Path.Combine(path, testName);
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);

            return Path.Combine(path, $"{iteration}test.png");
        }
        [Fact()]
        public void RotateTest()
        {
            var testObject = MainClass.LoadImage(TestImage);
            var result = MainClass.Rotate(testObject, 90);

            Assert.NotEqual(result, testObject);

            SaveAndCheckIfSavedCorrect(result);
        }


        [Fact()]
        public void OpenCvCorrectLoadTest_ShouldLoadAndSaveImage_NoChangesToImage()
        {
            var testObject = MainClass.LoadImage(TestImage);            
            Assert.NotNull(testObject);

            SaveAndCheckIfSavedCorrect(testObject);
        }
        private static void SaveAndCheckIfSavedCorrect(OpenCvSharp.Mat result, [CallerMemberName] string name = "")
        {
            var savePath = TestImageDirectory(name);
            MainClass.SaveImage(savePath, result);
            Assert.True(File.Exists(savePath));
        }
    }
}