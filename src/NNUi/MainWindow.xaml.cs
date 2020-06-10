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
        private const int Size = 100;

        private List<double[]> _input = new List<double[]>();
        private List<double[]> _expected = new List<double[]>();

        private NeuronalNetwork _network = new NeuronalNetwork(new (int Neurons, bool WithBias)[]
        {
            (2,false),
            //(2,false),
            (20,true),
            (10,true),
            //(2,false),
            //(2,false),
            (1,true)
        });

        private bool _isSuspended;


        public MainWindow()
        {
            InitializeComponent();
            SetupGrid(Root, true);
            SetupGrid(Net, false);
        }

        private void SetupGrid(Grid grid, bool withEvents)
        {
            grid.Children.Clear();
            for (int i = 0; i < Size; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(3) });

                for (int j = 0; j < Size; j++)
                {
                    grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(3) });
                    var item = new Grid() { Background = new SolidColorBrush(Colors.Transparent) };
                    if (withEvents)
                    {
                        item.MouseLeftButtonDown += MouseClick;
                        item.MouseRightButtonDown += MouseClick;
                    }

                    grid.Children.Add(item);

                    Grid.SetRow(item, i);
                    Grid.SetColumn(item, j);
                }
            }
        }

        private void MouseClick(object sender, MouseButtonEventArgs e)
        {
            var grid = (Panel)sender;
            double row = Grid.GetRow(grid);
            double column = Grid.GetColumn(grid);

            double data;
            Color color;
            if (e.ChangedButton == MouseButton.Left)
            {
                data = 1;
                color = Colors.Green;
            }
            else
            {
                data = 0;
                color = Colors.Red;
            }

            grid.Background = new SolidColorBrush(color);

            _input.Add(new[] { row / 100d, column / 100d });
            _expected.Add(new[] { data });
        }

        private async void Study(object sender, RoutedEventArgs e)
        {
            if (_input.Count == 0 || _isSuspended)
                return;

            try
            {
                _isSuspended = true;

                await Task.Run(() =>
                {
                    for (int i = 0, j = 0; i < 10000; i++, j++)
                    {
                        double error = 0;
                        var data = _input.Zip(_expected, Tuple.Create).ToArray() /*.OrderBy(s => _random.Next())*/;
                        var counter = 0;

                        foreach (var tuple in data)
                        {
                            error += Math.Pow(_network.Study(tuple.Item1, tuple.Item2).Error, 2);
                            counter++;
                        }

                        if (j == 500)
                            j = 0;

                        if (j == 0)
                            App.Current.Dispatcher.Invoke(Draw);
                        

                        Debug.WriteLine(error / counter);
                        if (error < .01)
                        {
                            Debug.WriteLine(i);
                            return;
                        }
                    }
                });
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
            SetupGrid(Root, true);
            SetupGrid(Net, false);
        }

        private void Draw()
        {
            if (_expected.Count == 0)
                return;

            var index = 0;
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    var item = (Panel)Net.Children[index];

                    double row = Grid.GetRow(item);
                    double column = Grid.GetColumn(item);

                    var result = _network.ForwardPass(new[] { row / 100d, column / 100d });

                    if (result[0] > 0.5)
                        item.Background = new SolidColorBrush(Colors.Orange);
                    else
                        item.Background = new SolidColorBrush(Colors.Pink);

                    index++;
                }
            }
        }
    }
}
