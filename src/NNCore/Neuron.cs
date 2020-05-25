using System;
using System.Linq;

namespace NNCore
{
    class Neuron
    {
        private readonly Func<double, double> Activation = Algorithms.Sigmoid;

        private readonly Func<double, double> Derivative = Algorithms.DerivativeSigmoid;

        private readonly double _learningRatio = 0.01;

        private readonly bool _withBias;

        private readonly NeuronType _type;

        private double _bias;

        private double _error;

        public Link[] InputLinks { get; }

        public Link[] OutputLinks { get; }

        public double InputData { get; set; }
        public double OutputData { get; private set; }

        public Neuron(int inputLinkCount, int outputLinkCount, bool withBias = false)
        {
            InputLinks = new Link[inputLinkCount];
            OutputLinks = new Link[outputLinkCount];

            if (!Equals(GetNeuronType(), NeuronType.Input) && withBias)
            {
                _withBias = true;
                _bias = Algorithms.GetNumber();
            }

            _type = GetNeuronType();
        }

        public double ForwardPass(double input)
        {
            if (Equals(_type, NeuronType.Input))
                return OutputData = InputData = input;

            InputData = InputLinks.Select(s => s.Source.OutputData * s.Weight).Sum() + _bias;

            return OutputData = Activation.Invoke(InputData);
        }

        public double SetError(double data)
        {
            if (Equals(_type, NeuronType.Input))
                return default;

            if (Equals(_type, NeuronType.Output))
                return _error = (data - OutputData);

            return _error = OutputLinks.Select(s => s.Weight * s.Target._error).Sum();
        }

        public void CoerceLinks()
        {
            if (Equals(_type, NeuronType.Input))
                return;

            var gradient = _error * Derivative(OutputData) * _learningRatio;

            foreach (var link in InputLinks)
            {
                var delta = gradient * link.Source.OutputData;

                link.Weight += delta;
            }

            if (_withBias)
                _bias += gradient;
        }
        
        private NeuronType GetNeuronType()
        {
            return InputLinks.Length == 0
                ? NeuronType.Input
                : OutputLinks.Length == 0
                    ? NeuronType.Output
                    : NeuronType.Hidden;
        }
    }
}