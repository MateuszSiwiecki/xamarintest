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
        private static string TestImageDirectory(string additionalInfo = "", [CallerMemberName] string testName = "")
        {
            var path = @"C:\Users\mateu\OneDrive\Desktop\QR\documents";
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);

            path = Path.Combine(path, testName);
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);

            var nameOfFile = string.IsNullOrWhiteSpace(additionalInfo) ? testName : $"{additionalInfo}_{testName}.png";

            return Path.Combine(path, nameOfFile);
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
        private static void SaveAndCheckIfSavedCorrect(OpenCvSharp.Mat result, string additionalInfo = "", [CallerMemberName] string name = "")
        {
            var savePath = TestImageDirectory(additionalInfo, name);
            MainClass.SaveImage(savePath, result);
            Assert.True(File.Exists(savePath));
        }

        [Fact()]
        public void ProccessToGrayContuourTest()
        {
            var testObject = MainClass.LoadImage(TestImage);
            var result = MainClass.ProccessToGrayContuour(testObject);

            SaveAndCheckIfSavedCorrect(result);
        }

        [InlineData(300, 300)]
        [InlineData(3000, 300)]
        [InlineData(300, 3000)]
        [InlineData(100, 400)]
        [InlineData(400, 100)]
        [InlineData(1000, 1000)]
        [Theory()]
        public void ResizeTest(int dimensionX, int dimensionY)
        {
            var testObject = MainClass.LoadImage(TestImage);
            var result = MainClass.Resize(testObject, dimensionX, dimensionY);

            SaveAndCheckIfSavedCorrect(result, $"{dimensionX}x{dimensionY}");
        }
    }
}