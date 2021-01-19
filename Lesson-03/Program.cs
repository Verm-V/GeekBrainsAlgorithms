using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace Lesson_03
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
        }
    }

    public class BechmarkClass
    {
        

        #region ---- DATA TYPES ----
        public class PointClassFloat
        {
            public float X { get; set; }
            public float Y { get; set; }
        }

        public struct PointStructFloat
        {
            public float X;
            public float Y;

        }
        public struct PointStructDouble
        {
            public double X;
            public double Y;

        }

        #endregion

        #region ---- METHODS ----

        public float PointDistanceClassFloat(PointClassFloat pointOne, PointClassFloat pointTwo)
        {
            float x = pointOne.X - pointTwo.X;
            float y = pointOne.Y - pointTwo.Y;
            return MathF.Sqrt((x * x) + (y * y));
        }

        public float PointDistanceStructFloat(PointStructFloat pointOne, PointStructFloat pointTwo)
        {
            float x = pointOne.X - pointTwo.X;
            float y = pointOne.Y - pointTwo.Y;
            return MathF.Sqrt((x * x) + (y * y));
        }

        public double PointDistanceStructDouble(PointStructDouble pointOne, PointStructDouble pointTwo)
        {
            double x = pointOne.X - pointTwo.X;
            double y = pointOne.Y - pointTwo.Y;
            return Math.Sqrt((x * x) + (y * y));
        }

        public float PointDistanceShortStructFloat(PointStructFloat pointOne, PointStructFloat pointTwo)
        {
            float x = pointOne.X - pointTwo.X;
            float y = pointOne.Y - pointTwo.Y;
            return (x * x) + (y * y);
        }

        #endregion

        #region ---- BENCHMARKS ----

        [Benchmark]
        public void TestPointDistanceClassFloat()
        {
            PointClassFloat pointOne = new PointClassFloat() { X = 0, Y = 0 };
            PointClassFloat pointTwo = new PointClassFloat() { X = 3, Y = 4 };
            PointDistanceClassFloat(pointOne, pointTwo);
        }

        [Benchmark]
        public void TestPointDistanceStructFloat()
        {
            PointStructFloat pointOne = new PointStructFloat() { X = 0, Y = 0 };
            PointStructFloat pointTwo = new PointStructFloat() { X = 3, Y = 4 };
            PointDistanceStructFloat(pointOne, pointTwo);
        }

        [Benchmark]
        public void TestPointDistanceStructDouble()
        {
            PointStructDouble pointOne = new PointStructDouble() { X = 0, Y = 0 };
            PointStructDouble pointTwo = new PointStructDouble() { X = 3, Y = 4 };
            PointDistanceStructDouble(pointOne, pointTwo);
        }

        [Benchmark]
        public void TestPointDistanceShortStructFloat()
        {
            PointStructFloat pointOne = new PointStructFloat() { X = 0, Y = 0 };
            PointStructFloat pointTwo = new PointStructFloat() { X = 3, Y = 4 };
            PointDistanceShortStructFloat(pointOne, pointTwo);
        }

        #endregion

    }
}