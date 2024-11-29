using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace Game
{
    public partial class MainWindow : Window
    {
        Runer.Game game;
        Label[,] labels;
        public MainWindow()
        {
            InitializeComponent();
            int width = 10;
            int height = 10;
            for(int y = 0; y < height; y++)
            {
                GridGameField.RowDefinitions.Add(new RowDefinition());
            }
            for(int x = 0; x < width; x++)
            {
                GridGameField.ColumnDefinitions.Add(new ColumnDefinition());
            }

            labels = new Label[height, width];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var label = new Label() { Content = "Txt",
                        VerticalContentAlignment = VerticalAlignment.Center,
                        HorizontalContentAlignment = HorizontalAlignment.Center,
                        FontSize = 30,
                        Width = 50,
                        Height = 50};
                    Grid.SetRow(label, y);
                    Grid.SetColumn(label, x);
                    GridGameField.Children.Add(label);
                    labels[y, x] = label;
                    label.Background = new SolidColorBrush(Colors.Gray);

                }
            }
            game = new Runer.Game(width, height, scoreLabel, labels, DeathMessageBorder);
            game.SimulateAndRender();
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (!game.IsGameOver)
            {
                if (e.Key == Key.NumPad5)
                {
                    game.Move(new Runer.Offset(0, 0));
                    game.SimulateAndRender();
                }
                else if (e.Key == Key.NumPad7 || e.Key == Key.Q)
                {
                    game.Move(new Runer.Offset(-1, -1));
                    game.SimulateAndRender();
                }
                else if (e.Key == Key.NumPad8 || e.Key == Key.W)
                {
                    game.Move(new Runer.Offset(0, -1));
                    game.SimulateAndRender();
                }
                else if (e.Key == Key.NumPad9 || e.Key == Key.E)
                {
                    game.Move(new Runer.Offset(1, -1));
                    game.SimulateAndRender();
                }
                else if (e.Key == Key.NumPad4 || e.Key == Key.A)
                {
                    game.Move(new Runer.Offset(-1, 0));
                    game.SimulateAndRender();
                }
                else if (e.Key == Key.NumPad6 || e.Key == Key.D)
                {
                    game.Move(new Runer.Offset(1, 0));
                    game.SimulateAndRender();
                }
                else if (e.Key == Key.NumPad1 || e.Key == Key.Z)
                {
                    game.Move(new Runer.Offset(-1, 1));
                    game.SimulateAndRender();
                }
                else if (e.Key == Key.NumPad2 || e.Key == Key.X)
                {
                    game.Move(new Runer.Offset(0, 1));
                    game.SimulateAndRender();
                }
                else if (e.Key == Key.NumPad3 || e.Key == Key.C)
                {
                    game.Move(new Runer.Offset(1, 1));
                    game.SimulateAndRender();
                }
                else
                {
                    game.Move(new Runer.Offset(0, 0));
                    game.SimulateAndRender();
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            game.Reset();
            game.SimulateAndRender();
        }
    } 
}
