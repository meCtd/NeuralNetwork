using System;
using System.Linq;

namespace NNCore
{
    class Neuron
    {
        private readonly Func<double, double> Activation = Algorithms.Sigmoid;

        private readonly Func<double, double> Derivative = Algorithms.DerivativeSigmoid;

        private readonly double _learningRatio = 0.01;

        public Link[] InputLinks { get; }

        public Link[] OutputLinks { get; }

        private readonly bool _withBias;

        public double Error { get; private set; } = 0;

        public NeuronType Type => InputLinks.Length == 0
            ? NeuronType.Input
            : OutputLinks.Length == 0
                ? NeuronType.Output
                : NeuronType.Hidden;

        public double Bias { get; set; } = 0;

        public Neuron(int inputLinkCount, int outputLinkCount, bool withBias = false)
        {
            InputLinks = new Link[inputLinkCount];
            OutputLinks = new Link[outputLinkCount];

            if (!Equals(Type, NeuronType.Input) && withBias)
            {
                _withBias = true;
                Bias = Algorithms.GetNumber();
            }
        }


        public double InputData { get; set; }
        public double OutputData { get; private set; }

        public double ForwardPass(double input)
        {
            if (Equals(Type, NeuronType.Input))
                return OutputData = InputData = input;

            InputData = InputLinks.Select(s => s.Source.OutputData * s.Weight).Sum() + Bias;

            return OutputData = Activation.Invoke(InputData);
        }

        public double SetError(double data)
        {
            if (Equals(Type, NeuronType.Input))
                return default;

            if (Equals(Type, NeuronType.Output))
                return Error = (data - OutputData);

            return Error = OutputLinks.Select(s => s.Weight * s.Target.Error).Sum();
        }

        public void CoerceLinks()
        {
            if (Equals(Type, NeuronType.Input))
                return;

            var gradient = Error * Derivative(OutputData) * _learningRatio;

            foreach (var link in InputLinks)
            {
                var delta = gradient * link.Source.OutputData;

                link.Weight += delta;
            }

            if (_withBias)
                Bias += gradient;
        }
    }
}