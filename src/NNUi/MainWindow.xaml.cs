using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

using NNCore;

namespace NNUi
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<double[]> _input = new List<double[]>();
        private List<double[]> _expected = new List<double[]>();

        private NeuronalNetwork _network = new NeuronalNetwork(new (int Neurons, bool WithBias)[]
        {
            (2,false),
            (5,true),
            (5,false),
            //(2,false),
            //(2,false),
            (2,true)

        });

        private double[] _temp = new double[2];

        private bool _isSuspended;


        public MainWindow()
        {
            InitializeComponent();
        }

        private void Draw(object sender, EventArgs e)
        {
            if (_input.Count == 0 || _isSuspended)
                return;

            Net.Children.Clear();

            try
            {
                _isSuspended = true;

                for (int i = 0; i < 300; i += 3)
                {
                    for (int j = 0; j < 300; j += 3)
                    {
                        _temp[0] = i / 300d;
                        _temp[1] = j / 300d;


                        var result = _network.ForwardPass(_temp);

                        Color color;

                        if (result[0] < result[1])
                            color = Colors.Orange;
                        else
                        {
                            color = Colors.Pink;
                        }



                        Draw(Net, 3, color, i, j);

                    }
                }
            }
            finally
            {
                _isSuspended = false;
            }
        }

        private void MouseClick(object sender, MouseButtonEventArgs e)
        {
            var position = Mouse.GetPosition(this);

            double data;
            double data1;
            Color color;
            if (e.ChangedButton == MouseButton.Left)
            {
                data = 0;
                data1 = 1;

                color = Colors.Green;
            }
            else
            {
                data = 1;
                data1 = 0;
                color = Colors.Red;
            }

            Draw(Root, 3, color, position.X, position.Y);

            var a = position.X / 300d;
            var b = position.Y / 300d;

            _input.Add(new[] { a, b });
            _expected.Add(new[] { data, data1 });
        }

        private void Draw(Canvas canvas, double size, Color color, double x, double y)
        {
            var brush = new SolidColorBrush(color);

            var point = new Ellipse()
            {
                Width = size,
                Height = size,
                SnapsToDevicePixels = true,
                StrokeThickness = 0,
                Stroke = brush,
                Fill = brush

            };

            Canvas.SetLeft(point, x);
            Canvas.SetTop(point, y);

            canvas.Children.Add(point);
        }

        private void Study(object sender, RoutedEventArgs e)
        {
            if (_input.Count == 0 || _isSuspended)
                return;

            try
            {
                _isSuspended = true;

                for (int i = 0; i < 10000; i++)
                {
                    double error = 0;
                    var asd = 0;
                    foreach (var tuple in _input.Zip(_expected, Tuple.Create)/*.OrderBy(s => _random.Next())*/)
                    {
                        error += Math.Pow(_network.Study(tuple.Item1, tuple.Item2).Error, 2);
                        asd++;
                    }


                    Debug.WriteLine(error / asd);
                    if (error < .01)
                    {
                        Debug.WriteLine(i);
                        return;
                    }

                }
            }
            finally
            {
                _isSuspended = false;
            }
        }

        private void Clear(object sender, RoutedEventArgs e)
        {
            _input.Clear();
            _expected.Clear();
            Root.Children.Clear();
            Net.Children.Clear();
        }

        private void Aa(object sender, RoutedEventArgs e)
        {
            Root.Visibility = Root.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
        }
    }
}
