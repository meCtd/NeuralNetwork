using System;
using System.Linq;

namespace NNCore
{
    public class NeuronalNetwork
    {
        private readonly Neuron[][] _neurons;

        public NeuronalNetwork((int Neurons, bool WithBias)[] layers)
        {
            if (layers.Any(s => s.Neurons < 1))
                throw new ArgumentException(nameof(layers));

            _neurons = new Neuron[layers.Length][];

            layers.ForEach((ctx, i) => _neurons[i] = new Neuron[ctx.Neurons]);

            SetupNetwork(layers);
        }

        private void SetupNetwork((int Neurons, bool WithBias)[] layers)
        {
            for (int i = 0; i < _neurons.Length - 1; i++)
            {
                for (int source = 0; source < _neurons[i].Length; source++)
                {
                    var nextNeuronsCount = _neurons[i + 1].Length;

                    var sNeuron = i == 0
                        ? _neurons[i][source] = new Neuron(0, nextNeuronsCount, false)
                        : _neurons[i][source];

                    for (int target = 0; target < nextNeuronsCount; target++)
                    {
                        var innerNextNeuronsCount = i + 1 == _neurons.Length - 1
                            ? 0
                            : _neurons[i + 2].Length;

                        var tNeuron = source == 0
                            ? _neurons[i + 1][target] = new Neuron(_neurons[i].Length, innerNextNeuronsCount, layers[i + 1].WithBias)
                            : _neurons[i + 1][target];

                        var link = new Link(sNeuron, tNeuron);

                        sNeuron.OutputLinks[target] = link;
                        tNeuron.InputLinks[source] = link;
                    }

                }
            }
        }

        public double[] ForwardPass(double[] input)
        {
            if (_neurons[0].Length != input.Length)
                throw new ArgumentException(nameof(input));

            _neurons[0].ForEach((neuron, i) => neuron.ForwardPass(input[i]));

            _neurons.Skip(1).ForEach(s => s.ForEach(ss => ss.ForwardPass(default)));

            return _neurons.Last().Select(s => s.OutputData).ToArray();
        }

        public (double Error, double[] Result) Study(double[] input, double[] expected)
        {
            var result = ForwardPass(input);

            var error = Algorithms.MSE(expected, result);

            for (int i = _neurons.Length - 1; i >= 1; i--)
            {
                if (i == _neurons.Length - 1)
                {
                    if (_neurons[i].Length != expected.Length)
                        throw new Exception(nameof(expected));

                    _neurons[i].ForEach((outNeuron, s) => outNeuron.SetError(expected[s]));
                }
                else
                    _neurons[i].ForEach(s => s.SetError(default));

            }

            for (int i = _neurons.Length - 1; i >= 1; i--)
            {
                _neurons[i].ForEach(s => s.CoerceLinks());
            }

            return (error, result);
        }
    }

}
