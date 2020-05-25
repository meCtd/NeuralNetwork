using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


namespace NNCore
{
    static class Algorithms
    {
        private const double MinRandomWeight = -1;
        private const double MaxRandomWeight = 1;

        private static readonly Random _random = new Random((int)DateTime.Now.Ticks);

        public static double GetNumber() => _random.NextDouble() * (MaxRandomWeight - MinRandomWeight) + MinRandomWeight;

        public static Func<double, double> Sigmoid { get; } = (s) => 1 / (1 + Math.Exp(-s));

        public static Func<double, double> DerivativeSigmoid { get; } = (s) => s * (1 - s);

        public static Func<double, double> TanH { get; } = Math.Tanh;

        public static Func<double, double> DerivativeTanH { get; } = (s) => 1 - Math.Pow(s, 2);


        public static Func<double[], double[], double> MSE { get; } = (expected, actual) =>
         {
             if (expected.Length != actual.Length)
                 throw new ArgumentException(nameof(actual));

             return expected.Zip(actual, (e, a) => (Math.Pow(e - a, 2))).Sum() / expected.Length;
         };
    }
}
