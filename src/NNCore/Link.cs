namespace NNCore
{
    class Link
    {
        public Neuron Source { get; }
        public Neuron Target { get; }

        public double Weight { get; set; }

        public Link(Neuron source, Neuron target)
        {
            Source = source;
            Target = target;
            Weight = Algorithms.GetNumber();
        }
    }
}
