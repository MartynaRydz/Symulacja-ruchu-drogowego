using System.Text;
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
using System.Drawing;

namespace Projekcik
{
    public partial class MainWindow : Window
    {
        private Random _random = new Random();  
        private bool _isMoving;  //czy ciufcia się porusza


        public MainWindow()
        {
            InitializeComponent();
            _isMoving = false;  // Na początku ciufcia się nie porusza
        }

        private void CiufciaJedzie()
        {

            while (true)
            {
                CiufCiuf _ciufciuf = null;
                int tloCiufWidth = 0;
                int ciufciufWidth = 0;

                Dispatcher.Invoke(() => 
                {
                    _ciufciuf = new CiufCiuf();
                    tloCiuf.Children.Add(_ciufciuf.CiufciufRectangle);// dodajemy ciufciuf (Rectangle) na canvas
                    tloCiufWidth = (int)tloCiuf.Width;
                    ciufciufWidth = (int)(_ciufciuf.CiufciufRectangle as FrameworkElement).Width;
                    Canvas.SetLeft(_ciufciuf.CiufciufRectangle, _ciufciuf.X);
                    Canvas.SetTop(_ciufciuf.CiufciufRectangle, _ciufciuf.Y);
                });


                for(int i = 0; i < tloCiufWidth + ciufciufWidth*2; i += Math.Abs(_ciufciuf.CiufciowaPredkosc))
                {
                    _ciufciuf.X += _ciufciuf.CiufciowaPredkosc;
                    Dispatcher.Invoke(() =>
                    {
                        Canvas.SetLeft(_ciufciuf.CiufciufRectangle, _ciufciuf.X);
                        Canvas.SetTop(_ciufciuf.CiufciufRectangle, _ciufciuf.Y);
                    });
                    Thread.Sleep(50);
                }

                
                Dispatcher.Invoke(()=> tloCiuf.Children.Remove(_ciufciuf.CiufciufRectangle));

                switch(_random.Next(3))
                {
                    case 0:
                        Thread.Sleep(5000);
                        break;
                    case 1:
                        Thread.Sleep(7500);
                        break;
                    case 2:
                        Thread.Sleep(10000);
                        break;

                }

            }
        }
        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            Thread wateczek = new Thread(CiufciaJedzie);
            wateczek.Start();
        }

        
        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
  
        }
    }
}