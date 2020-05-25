using System;
using System.Collections.Generic;
using System.Text;

namespace NNCore
{
    class Link
    {
        public Neuron Source { get; }
        public Neuron Target { get; }

        public double Weight { get; set; }

        public double PrevWeight { get; set; }
        public double Delta { get; set; }

        public Link(Neuron source, Neuron target)
        {
            Source = source;
            Target = target;
            Weight = Algorithms.GetNumber();
        }
    }
}
