using System;
using NNCore;

namespace NNConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var a = new NeuronalNetwork(new[] { (2,false),(5,true), (5, true), (1, true) });


            var aa = a.ForwardPass(new double[]{1,3});
            var asd= a.Study(new double[] {1,3}, new[] {.3});

             aa = a.ForwardPass(new double[] { 2, 4});
        }
    }
}
