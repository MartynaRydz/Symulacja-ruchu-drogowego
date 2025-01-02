using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Projekcik
{
    public partial class MainWindow : Window
    {
        private Random _random = new Random();
        private DispatcherTimer _ciufciufTimer; // Timer do obsługi ruchu ciufci
        private CiufCiuf _ciufciuf; // Obecnie poruszająca się ciufcia
        private bool _isMoving; // Czy ciufcia jest w ruchu
        private int _tloCiufWidth; // Szerokość obszaru tła
        private int _ciufciufWidth; // Szerokość ciufci

        public MainWindow()
        {
            InitializeComponent();
            InitializeTimer();
            _isMoving = false;
        }

        private void InitializeTimer()
        {
            // Inicjalizacja timera
            _ciufciufTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(30)
            };
            _ciufciufTimer.Tick += CiufciufTimer_Tick; //obsługa zdarzenia tick
        }

        private void CiufciaJedzie()
        {
            _ciufciuf = new CiufCiuf();

            tloCiuf.Children.Add(_ciufciuf.CiufciufRectangle);

            //pobranie szerokości obszaru tła i ciufci
            _tloCiufWidth = (int)tloCiuf.Width;
            _ciufciufWidth = (int)(_ciufciuf.CiufciufRectangle as FrameworkElement).Width;

            //ustawienie początkowej pozycji na Canvas
            Canvas.SetLeft(_ciufciuf.CiufciufRectangle, _ciufciuf.X);
            Canvas.SetTop(_ciufciuf.CiufciufRectangle, _ciufciuf.Y);

            _ciufciufTimer.Start();
        }

        private void CiufciufTimer_Tick(object sender, EventArgs e)
        {
            if (_ciufciuf == null) return;

            //poruszanko
            _ciufciuf.X += _ciufciuf.CiufciowaPredkosc;

            // aktualizacja pozycji ciufci na Canvas
            Canvas.SetLeft(_ciufciuf.CiufciufRectangle, _ciufciuf.X);

            //czy ciufcia wyjechała poza ekran
            if ((_ciufciuf.kieruneczek == 0 && _ciufciuf.X < -_ciufciufWidth) ||
                (_ciufciuf.kieruneczek == 1 && _ciufciuf.X > _tloCiufWidth))
            {
                //zatrzymanie timera i usunięcie ciufci
                _ciufciufTimer.Stop();
                tloCiuf.Children.Remove(_ciufciuf.CiufciufRectangle);
                _ciufciuf = null;

                //losowanie czasu przed kolejną ciufcią
                int opuznienieCiuf = _random.Next(5000, 10001);
                Task.Delay(opuznienieCiuf).ContinueWith(_ => Dispatcher.Invoke(CiufciaJedzie));
            }
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (_isMoving) return; // Zapobiega wielokrotnemu uruchamianiu
            _isMoving = true;
            CiufciaJedzie();
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            _ciufciufTimer.Stop(); // Zatrzymanie timera
            _isMoving = false;

            // Usunięcie ciufci, jeśli istnieje
            if (_ciufciuf != null)
            {
                tloCiuf.Children.Remove(_ciufciuf.CiufciufRectangle);
                _ciufciuf = null;
            }
        }
    }
}
