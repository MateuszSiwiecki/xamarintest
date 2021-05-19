using Xunit;
using OpenCvLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenCvLib.Tests
{
    public class MainClassTests
    {
        private static string TestImage => @"C:\Users\mateu\OneDrive\Desktop\QR\documents\recepta_test1.png";
        [Fact()]
        public void RotateTest()
        {
            // MainClass.
        }

        [Fact()]
        public void LoadImageTest()
        {
            var result = MainClass.LoadImage(TestImage);

            Assert.NotNull(result);
        }

        [Fact()]
        public void InitTest()
        {
            
            Assert.True(MainClass.Init());
        }
    }
}