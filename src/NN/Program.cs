using System;
using System.Linq;
using NNCore;

namespace NN
{
    class Program
    {
        static readonly Random _a = new Random();

        static void Main(string[] args)
        {
            var outputs = new double[] { .0, .1, .2, .3, .4, .5, .6, .7, .8, .9, .2, .3, .4, .5, .6, .7, .8, .9, .2, .3, .4 };
            var inputs = new double[][]
            {
                new double[]{ 0,0},
                new double[]{ 0,1},
                new double[]{ 0,2},
                new double[]{ 0,3},
                new double[]{ 0,4},
                new double[]{ 0,5},
                new double[]{ 0,6},
                new double[]{ 0,7},
                new double[]{ 0,8},
                new double[]{ 0,9},
                new double[]{ 1,1},
                new double[]{ 1,2},
                new double[]{ 1,3},
                new double[]{ 1,4},
                new double[]{ 1,5},
                new double[]{ 1,7},
                new double[]{ 1,8},
                new double[]{ 1,9},
                new double[]{ 2,0},
                new double[]{ 2,1},
                new double[]{ 2,2},
            };


            //outputs = new double[] { 1, 1, 0, 0 };
            //inputs = new double[][] {
            //    new double[] { 1,0},
            //    new double[] { 0,1},
            //    new double[] { 0,0},
            //    new double[] { 1,1},

            //};


            var network = new Network(new[] { 2, 10, 5, 1 });


            for (int i = 0; i < 1000000; i++)
            {
                for (int a = 0; a < outputs.Length; a++)
                {
                    network.Learn(inputs[a], new Double[] { outputs[a] });
                }

            }
        }


    }
}

