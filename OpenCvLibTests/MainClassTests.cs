using Xunit;
using OpenCvLib;
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.CompilerServices;
using System.IO;
using OpenCvSharp;
using Xunit.Abstractions;

namespace OpenCvLib.Tests
{
    public class MainClassTests
    {
        private readonly ITestOutputHelper output;

        public MainClassTests(ITestOutputHelper output)
        {
            this.output = output;
        }
        private static string TestPath => @"C:\Users\mateu\OneDrive\Desktop\QR\documents";
        private static string TestImage => Path.Combine(TestPath, "recepta_test1.png");

        private static string TestImageDirectory(string additionalInfo = "", [CallerMemberName] string testName = "")
        {
            var path = TestPath;
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);

            path = Path.Combine(path, testName);
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);

            var nameOfFile = string.IsNullOrWhiteSpace(additionalInfo) ? $"{testName}.png" : $"{additionalInfo}_{testName}.png";

            return Path.Combine(path, nameOfFile);
        }
        [Fact()]
        public void RotateTest()
        {
            using var testObject = MainClass.LoadImage(TestImage);
            var result = MainClass.Rotate(testObject, 90);

            Assert.NotEqual(result, testObject);

            SaveAndCheckIfSavedCorrect(result);
        }


        [Fact()]
        public void OpenCvCorrectLoadTest_ShouldLoadAndSaveImage_NoChangesToImage()
        {
            using var testObject = MainClass.LoadImage(TestImage);
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
            using var testObject = MainClass.LoadImage(TestImage);
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
            using var testObject = MainClass.LoadImage(TestImage);
            var result = MainClass.Resize(testObject, dimensionX, dimensionY);

            SaveAndCheckIfSavedCorrect(result, $"{dimensionX}x{dimensionY}");
        }

        [Fact()]
        public void FindContoursTest()
        {
            using var testObject = MainClass.LoadImage(TestImage);
            using var testObject2 = MainClass.ProccessToGrayContuour(testObject);
            var contours = MainClass.FindContours_SortedContours(testObject2);


            using var result = MainClass.DrawContour(testObject.Clone(), contours);
            SaveAndCheckIfSavedCorrect(result);

        }

        [InlineData(10, 1000, 10, 1000)]
        [Theory()]
        public void DrawContourTest_RandomContour(int xLeft, int xRight, int yUp, int yDown)
        {
            using var testObject = MainClass.LoadImage(TestImage);
            using var result = MainClass.DrawContour(testObject.Clone(), new List<Point> 
            { 
                new Point(xLeft, yUp) , 
                new Point(xRight, yUp) ,
                new Point(xRight, yDown),
                new Point(xLeft, yDown) ,
            });

            SaveAndCheckIfSavedCorrect(result, $"{xLeft}x{xRight}x{yUp}x{yDown}");
        }

        [Fact]
        public void FactMethodName()
        {
            output.WriteLine($"{ Cv2.GetVersionMajor() }. {Cv2.GetVersionMinor()}");
            Assert.True(
            Cv2.GetVersionMajor() == 3);
        }
    }
}