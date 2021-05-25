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
        public void FindContours_SortedContoursTest()
        {
            using var testObject = MainClass.LoadImage(TestImage);
            using var testObject2 = MainClass.ProccessToGrayContuour(testObject);
            var contours = MainClass.FindContours_SortedContours(testObject2);


            using var result = MainClass.DrawContour(testObject.Clone(), contours);
            SaveAndCheckIfSavedCorrect(result);
        }

        private static Point[] BiggestContourExec(Mat testObject)
        {
            using var testObject2 = MainClass.ProccessToGrayContuour(testObject);
            return MainClass.FindContours_BiggestContour(testObject2);
        }
        [Fact()]
        public void FindContours_DrawContour_BiggestContourTest()
        {
            using var testObject = MainClass.LoadImage(TestImage);
            var contours = BiggestContourExec(testObject);

            using var result = MainClass.DrawContour(testObject.Clone(), contours);
            SaveAndCheckIfSavedCorrect(result);
        }
        [Fact()]
        public void FindContours_NumberOfPoints_BiggestContourTest()
        {
            using var testObject = MainClass.LoadImage(TestImage);
            var contours = BiggestContourExec(testObject);

            Assert.True(contours.Length == 4);
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

        [InlineData(10, 1000, 10, 1000)]
        [InlineData(2000, 1000, 10, 1000)]
        [InlineData(2000, 4000, 2000, 4000)]
        [InlineData(8000, 1000, 8000, 1000)]
        [Theory()]
        public void TransformTest_Rectangle(int xLeft, int xRight, int yUp, int yDown)
        {
            using var testObject = MainClass.LoadImage(TestImage);

            var pointsOfFragment = new Point2f[]
            {
                new Point2f(xLeft, yUp) ,
                new Point2f(xRight, yUp) ,
                new Point2f(xRight, yDown),
                new Point2f(xLeft, yDown) ,
            };
            var pointsOfDestination = new Point2f[]
            {
                new Point2f(0, 0),
                new Point2f(testObject.Width, 0),
                new Point2f(testObject.Width, testObject.Height),
                new Point2f(0, testObject.Height)
            };

            using var contouredImage = MainClass.DrawContour(testObject.Clone(), new List<Point>
            {
                new Point(xLeft, yUp) ,
                new Point(xRight, yUp) ,
                new Point(xRight, yDown),
                new Point(xLeft, yDown) 
            });
            using var result = MainClass.Transform(contouredImage, pointsOfFragment, pointsOfDestination, testObject.Width, testObject.Height);


            SaveAndCheckIfSavedCorrect(contouredImage, $"{xLeft}x{xRight}x{yUp}x{yDown}_orgin");
            SaveAndCheckIfSavedCorrect(result, $"{xLeft}x{xRight}x{yUp}x{yDown}_result");
        }

        [InlineData(10, 10, 1000, 1000, 1200, 2000, 500, 1800)]
        [Theory()]
        public void TransformTest_UnnormalShape(int point1x, int point1y, int point2x, int point2y, int point3x, int point3y, int point4x, int point4y)
        {
            using var testObject = MainClass.LoadImage(TestImage);

            var pointsOfFragment = new Point2f[]
            {
                new Point2f(point1x, point1y) ,
                new Point2f(point2x, point2y) ,
                new Point2f(point3x, point3y),
                new Point2f(point4x, point4y) 
            };
            var pointsOfDestination = new Point2f[]
            {
                new Point2f(0, 0),
                new Point2f(testObject.Width, 0),
                new Point2f(testObject.Width, testObject.Height),
                new Point2f(0, testObject.Height)
            };

            using var contouredImage = MainClass.DrawContour(testObject.Clone(), new List<Point>
            {
                new Point(point1x, point1y) ,
                new Point(point2x, point2y) ,
                new Point(point3x, point3y),
                new Point(point4x, point4y)
            });
            using var result = MainClass.Transform(contouredImage, pointsOfFragment, pointsOfDestination, testObject.Width, testObject.Height);


            SaveAndCheckIfSavedCorrect(contouredImage, $"{point1x}x{point2x}x{point3x}x{point4x}_orgin");
            SaveAndCheckIfSavedCorrect(result, $"{point1x}x{point2x}x{point3x}x{point4x}_result");
        }
    }
}