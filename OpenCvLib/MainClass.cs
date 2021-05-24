using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenCvLib
{
    public static class MainClass
    {        

        public static Mat ProcessImage(Mat image)
        {
            return image;
        }
        public static Mat ProccessToGrayContuour(Mat image)
        {
            var grayOutput = new Mat();
            var bilateralFilter = new Mat();
            var edgedResult = new Mat();
            var dilateResult = new Mat();
            Cv2.CvtColor(image, grayOutput, ColorConversionCodes.BGR2GRAY);
            Cv2.BilateralFilter(grayOutput, bilateralFilter, 11, 17, 17);
            Cv2.Canny(bilateralFilter, edgedResult, 30, 200);
            Cv2.Dilate(edgedResult, dilateResult, new Mat());

            //return edgedResult;
            return dilateResult;
        }
        public static List<Point[]> FindContours_SortedContours(Mat image)
        {
            Cv2.FindContours(image, out var foundedContour, out var hierarchy, RetrievalModes.Tree, ContourApproximationModes.ApproxNone);
            var sortedContour = foundedContour.OrderByDescending(ContourArea).Where((x, y) => y < 1).ToArray();

            List<Point[]> result = new List<Point[]>();
            foreach (var contour in sortedContour)
            {
                var contourArea = ContourArea(contour);
                var peri = Cv2.ArcLength(contour, true);

                var approx = Cv2.ApproxPolyDP(contour.AsEnumerable(), 0.015 * peri, true);

                result.Add(approx);
            }


            return result;
        }
        public static Point[] FindContours_BiggestContour(Mat image)
        {
            Cv2.FindContours(image, out var foundedContour, out var hierarchy, RetrievalModes.Tree, ContourApproximationModes.ApproxNone);
            return foundedContour.OrderByDescending(ContourArea).First();
        }
        public static double ContourArea(Point[] x) => Cv2.ContourArea(x, true);
        public static Mat DrawContour(Mat image, IEnumerable<IEnumerable<Point>> contours)
        {
            Cv2.DrawContours(image, contours, -1, Scalar.Red, 5);

            return image;
        }
        public static Mat DrawContour(Mat image, IEnumerable<Point> countour)
        {
            Cv2.DrawContours(image, new List<IEnumerable<Point>> { countour }, -1, Scalar.Red, 5);

            return image;
        }
        public static Mat LoadImage(string filePath) => Cv2.ImRead(filePath);
        public static void SaveImage(string filePath, Mat imageToSave) => Cv2.ImWrite(filePath, imageToSave);
        public static Mat Rotate(Mat image, double angle, Point2f? center = null, double scale = 1.0)
        {
            // grab the dimensions of the image
            var w = image.Width;
            var h = image.Height;

            // if the center is None, initialize it as the center of
            // the image
            if (center == null) center = new Point2f { X = w / 2, Y = h / 2 };

            // perform the rotation
            var rotationMatrix2d = Cv2.GetRotationMatrix2D(center.Value, angle, scale);

            var warpAffineResult = new Mat();
            Cv2.WarpAffine(image, warpAffineResult, rotationMatrix2d, new Size((int)(w * scale),(int)( h * scale)));
            // return the rotated image
            return warpAffineResult;
        }
        public static Mat Resize(Mat image, double newWidth, double newHigh, InterpolationFlags inter = InterpolationFlags.Linear)
        {
            // grab the dimensions of the image
            var w = image.Width;
            var h = image.Height;

            var result = new Mat();
            Cv2.Resize(image, result, new Size(newWidth, newHigh), interpolation: inter);

            return result;
        }

    }
}
