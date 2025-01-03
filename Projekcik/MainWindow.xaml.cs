using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace Projekcik;

public partial class MainWindow : Window
{
    private Random _random = new Random();
    private DispatcherTimer _ciufciufTimer; //timer do obsługi ruchu ciufci
    private CiufCiuf _ciufciuf;
    private DispatcherTimer _samochodzikTimer; //timer do obsługi ruchu aut
    private Samochodzik _samochodzik;
    private bool _isMoving; //czy pojazdy jest w ruchu
    private int _tloDrogaWidth;
    private int _ciufciufWidth;
    private int _samochodzikWidth;

    public MainWindow()
    {
        InitializeComponent();
        InitializeCiufTimer();
        InitializeSamochodzikTimer();
        _isMoving = false;
    }
    #region InitializeTimer
    private void InitializeCiufTimer()
    {
        //inicjalizacja timera
        _ciufciufTimer = new DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(30)
        };
        _ciufciufTimer.Tick += CiufciufTimer_Tick; //obsługa zdarzenia tick
    }
    private void InitializeSamochodzikTimer()
    {
        //inicjalizacja timera
        _samochodzikTimer = new DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(30)
        };
        _samochodzikTimer.Tick += SamochodzikTimer_Tick; //obsługa zdarzenia tick
    }
    #endregion
    #region Ciufcia
    private void CiufciaJedzie()
    {
        _ciufciuf = new CiufCiuf();

        tloDroga.Children.Add(_ciufciuf.CiufciufImage);

        // pobranie szerokości obszaru tła i ciufci
        _tloDrogaWidth = (int)tloDroga.Width;
        _ciufciufWidth = (int)(_ciufciuf.CiufciufImage as FrameworkElement).Width;

        // ustawienie początkowej pozycji na Canvas
        Canvas.SetLeft(_ciufciuf.CiufciufImage, _ciufciuf.X);
        Canvas.SetTop(_ciufciuf.CiufciufImage, _ciufciuf.Y);

        _ciufciufTimer.Start();
    }

    private void CiufciufTimer_Tick(object sender, EventArgs e)
    {
        if (_ciufciuf == null) return;

        // poruszanko
        _ciufciuf.X += _ciufciuf.CiufciowaPredkosc;

        // aktualizacja pozycji ciufci na canvas
        Canvas.SetLeft(_ciufciuf.CiufciufImage, _ciufciuf.X);

        // czy ciufcia wyjechała poza ekran
        if ((_ciufciuf.Kieruneczek == 0 && _ciufciuf.X < -_ciufciufWidth) ||
            (_ciufciuf.Kieruneczek == 1 && _ciufciuf.X > _tloDrogaWidth))
        {
            // zatrzymanie timera i usunięcie ciufci
            _ciufciufTimer.Stop();
            tloDroga.Children.Remove(_ciufciuf.CiufciufImage);
            _ciufciuf = null;

            // losowanie czasu przed kolejną ciufcią
            int opuznienieCiuf = _random.Next(5, 11) * 1000;
            Task.Delay(opuznienieCiuf).ContinueWith(_ => Dispatcher.Invoke(CiufciaJedzie));
        }
    }
    #endregion
    #region Samochodzik
    private void SamochodzikJedzie()
    {
        _samochodzik = new Samochodzik();

        tloDroga.Children.Add(_samochodzik.SamochodzikImage);

        // Pobranie szerokości obszaru tła i samochodziku
        _tloDrogaWidth = (int)tloDroga.Width;
        _samochodzikWidth = (int)(_samochodzik.SamochodzikImage as FrameworkElement).Width;

        // Ustawienie początkowej pozycji na Canvas
        Canvas.SetLeft(_samochodzik.SamochodzikImage, _samochodzik.X);
        Canvas.SetTop(_samochodzik.SamochodzikImage, _samochodzik.Y);

        _samochodzikTimer.Start();
    }


    private void SamochodzikTimer_Tick(object sender, EventArgs e)
    {
        if (_samochodzik == null) return;

        // Poruszanko
        _samochodzik.X += _samochodzik.SamochodzikowaPredkosc;

        // Aktualizacja pozycji samochodziku na Canvas
        Canvas.SetLeft(_samochodzik.SamochodzikImage, _samochodzik.X);

        // Czy samochodzik wyjechał poza ekran
        if ((_samochodzik.X < -_samochodzikWidth && _samochodzik.Kieruneczek == 0) ||
            (_samochodzik.X > _tloDrogaWidth && _samochodzik.Kieruneczek == 1))
        {
            // Zatrzymanie timera i usunięcie samochodziku
            _samochodzikTimer.Stop();
            tloDroga.Children.Remove(_samochodzik.SamochodzikImage);
            _samochodzik = null;

            // Losowanie czasu przed kolejnym samochodzikiem
            int opoznienieSamochodzik = _random.Next(5, 11) * 1000;
            Task.Delay(opoznienieSamochodzik).ContinueWith(_ => Dispatcher.Invoke(SamochodzikJedzie));
        }
    }

    #endregion

    private void StartButton_Click(object sender, RoutedEventArgs e)
    {
        if (_isMoving) return; //zapobiega wielokrotnemu uruchamianiu
        _isMoving = true;
        CiufciaJedzie();
        SamochodzikJedzie();
    }

    private void StopButton_Click(object sender, RoutedEventArgs e)
    {
        _ciufciufTimer.Stop();
        _samochodzikTimer.Stop();
        _isMoving = false;

        // usunięcie istniejącej ciufci
        if (_ciufciuf != null)
        {
            tloDroga.Children.Remove(_ciufciuf.CiufciufImage);
            _ciufciuf = null;
        }
        // usunięcie istniejącej samochodu
        if (_samochodzik != null)
        {
            tloDroga.Children.Remove(_samochodzik.SamochodzikImage);
            _samochodzik = null;
        }
    }
}