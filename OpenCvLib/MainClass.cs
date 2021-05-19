using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenCvLib
{
    public static class MainClass
    {        
        public static void ProcessImage(string filePath)
        {
            var image = LoadImage(filePath);
            var result = image.PerspectiveWrapping().TransformImage();
        }
        public static Mat LoadImage(string filePath) => Cv2.ImRead(filePath);
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
            return image ;
        }

        public static void ScreenFinder(Mat image)
        {
            Cv2.Resize(image, image, new Size());
        }
        public static double[] OrderPoints(IEnumerable<Tuple<double, double>> coordinates)
        {
            var table = new double[4];
            //table[0] = coordinates.OrderBy(x => x.Item1).Max(x => x)

            return table;
        }
        public static Mat PerspectiveWrapping(this Mat image)
        {

            return image;
        }
        public static Mat TransformImage(this Mat image)
        {

            return image;
        }
    }
}
