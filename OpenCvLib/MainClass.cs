﻿using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenCvLib
{
    public static class MainClass
    {
        public static Mat ProcessImage(string path) => ProcessImage(new Mat(path));
        public static Mat ProcessImage(Mat image)
        {
            using var grayImage = ProccessToGrayContuour(image.Clone());
            var contoursOfDocument = FindContours_BiggestContourFloat(grayImage);
            using var transformedImage = Transform(image, contoursOfDocument);
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

            grayOutput.Dispose();
            bilateralFilter.Dispose();
            edgedResult.Dispose();

            //return edgedResult;
            return dilateResult;
        }
        public static List<Point[]> FindContours_SortedContours(Mat image)
        {
            Cv2.FindContours(image, out var foundedContour, out var hierarchy, RetrievalModes.Tree, ContourApproximationModes.ApproxNone);
            var sortedContour = foundedContour.OrderByDescending(ContourArea).ToArray();

            List<Point[]> result = new List<Point[]>();
            foreach (var contour in sortedContour)
            {
                var peri = Cv2.ArcLength(contour, true);

                var approx = Cv2.ApproxPolyDP(contour.AsEnumerable(), 0.015 * peri, true);

                result.Add(approx);
            }
            return result;
        }
        public static Point2f[] FindContours_BiggestContourFloat(Mat image)
        {
            var points = FindContours_BiggestContourInt(image);
            var output = new Point2f[points.Length];
            for (int i = 0; i < points.Length; i++)
            {
                output[i] = points[i];
            }
            return output;
        }
        public static Point[] FindContours_BiggestContourInt(Mat image)
        {
            Cv2.FindContours(image, out var foundedContours, out var hierarchy, RetrievalModes.Tree, ContourApproximationModes.ApproxNone);
            var contourOfDocument = foundedContours.OrderByDescending(ContourArea).First();
            var peri = Cv2.ArcLength(contourOfDocument, true);

            var approx = Cv2.ApproxPolyDP(contourOfDocument.AsEnumerable(), 0.015 * peri, true);
            return approx;
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
        public static Mat Transform(Mat inputImage, Point2f[] toTransform) 
            => Transform(inputImage, toTransform, new Point2f[]
                {
                    new Point2f(0, 0),
                    new Point2f(inputImage.Width, 0),
                    new Point2f(inputImage.Width, inputImage.Height),
                    new Point2f(0, inputImage.Height)
                }, new Size(inputImage.Width, inputImage.Height));
        public static Mat Transform(Mat inputImage, Point2f[] toTransform, Point2f[] destination, double width, double heigth) 
            => Transform(inputImage, toTransform, destination, new Size(width, heigth));
        public static Mat Transform(Mat inputImage, Point2f[] toTransform, Point2f[] destination, Size size)
        {
            var output = new Mat();

            var m = Cv2.GetPerspectiveTransform(toTransform, destination);
            Cv2.WarpPerspective(inputImage, output, m, size);

            return output;
        }
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
