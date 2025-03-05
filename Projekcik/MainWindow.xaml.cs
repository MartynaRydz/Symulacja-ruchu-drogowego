using Projekcik.Controllers;
using Projekcik.Enum;
using Projekcik.Events;
using Projekcik.Models;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace Projekcik;

public partial class MainWindow : Window
{
    private Random _random = new Random();
    private bool _isMoving; 
    CarController _carController = new CarController();
    TrainController _trainController = new TrainController();
    TrafficLights lights = new TrafficLights();
    Models.Barrier barrier = new Models.Barrier();
    Thread barrierThread;
    Thread trainThread;
    Thread[] carThread;
    Thread lightsThread;

    public MainWindow()
    {
        InitializeComponent();
        Canvas.SetZIndex(closeBarrier, 1);
        _isMoving = false;  
        TrafficLights.IsTrafficLightsOn = false;
    }

    #region Pojazdy
    
    private void TrainGoing()
    {

        while (_isMoving)
        {
            Dispatcher.Invoke(() => { _trainController.Train = new Train(); });
            int backgroundWayWidth = 0;
            int trainWidth = 0;

            Dispatcher.Invoke(() =>
            {
                backgroundWay.Children.Add(_trainController.Train.TrainImage);
                backgroundWayWidth = (int)backgroundWay.Width;
                trainWidth = (int)(_trainController.Train.TrainImage as FrameworkElement).Width;
                Canvas.SetLeft(_trainController.Train.TrainImage, _trainController.Train.X);
                Canvas.SetTop(_trainController.Train.TrainImage, _trainController.Train.Y);
            });


            for (int i = 0; i < backgroundWayWidth + trainWidth * 2; i += Math.Abs(_trainController.Train.TrainSpeed))
            {
                _trainController.Train.X += _trainController.Train.TrainSpeed;
                Dispatcher.Invoke(() =>
                {
                    Canvas.SetLeft(_trainController.Train.TrainImage, _trainController.Train.X);
                    Canvas.SetTop(_trainController.Train.TrainImage, _trainController.Train.Y);
                });
                Thread.Sleep(50);
            }

            Dispatcher.Invoke(() => backgroundWay.Children.Remove(_trainController.Train.TrainImage));

            Thread.Sleep(_random.Next(4, 6) * 1000);

        }
    }
    private void CarDirectionHasChanged(object? sender, DirectionHasChangedEventArgs e)
    {

        switch (e.directionChanegCar.Direction)
        {
            case Direction.Up:
                Dispatcher.Invoke(() => (e.directionChanegCar.CarImage as Image).Source = new BitmapImage(new Uri("pack://application:,,,/items/samochodGora.png")));
                break;

            case Direction.Right:
                Dispatcher.Invoke(() => (e.directionChanegCar.CarImage as Image).Source = new BitmapImage(new Uri("pack://application:,,,/items/samochodPrawo.png")));
                break;

            case Direction.Down:
                Dispatcher.Invoke(() => (e.directionChanegCar.CarImage as Image).Source = new BitmapImage(new Uri("pack://application:,,,/items/samochodDol.png")));
                break;

            case Direction.Left:
                Dispatcher.Invoke(() => (e.directionChanegCar.CarImage as Image).Source = new BitmapImage(new Uri("pack://application:,,,/items/samochodLewo.png")));
                break;
        }
    }
    private void CarGoing()
    {
        Car car = null;
        while (_isMoving)
        {
           Dispatcher.Invoke(()=> car = new Car());
            car.DirectionHasChanged += CarDirectionHasChanged;

            while (!_carController.CanAddCar(car))
            {
                Thread.Sleep(1000);
            }
            _carController.AddCar(car);

            Dispatcher.Invoke(() => 
            {
                backgroundWay.Children.Add(car.CarImage);
                Canvas.SetLeft(car.CarImage,car.X);
                Canvas.SetTop(car.CarImage,car.Y);
            });

            while(_carController.CarUpdate(car))
            {

                Thread.Sleep(50);
                Dispatcher.Invoke(() =>
                {
                    Canvas.SetLeft(car.CarImage, car.X);
                    Canvas.SetTop(car.CarImage, car.Y);
                });
            }

      
            if (_carController.DeleteCar(car))
            {
                Dispatcher.Invoke(() => { backgroundWay.Children.Remove(car.CarImage); });
            }
        }
    }
    #endregion

    #region Syganlizacja
    public void SygnalizacjaSwiatelkowa()
    {
        while (_isMoving)
        {
            if (_trainController.Train is not null)
            {

                if (_trainController.Train.X > -500 && _trainController.Train.X < 500)
                {
                    TrafficLights.IsTrafficLightsOn = true;

                    Dispatcher.Invoke(() =>
                    {
                        Light.Source = new BitmapImage(new Uri("pack://application:,,,/items/semafor2.png"));
                    });

                    Thread.Sleep(300);

                    Dispatcher.Invoke(() =>
                    {
                        Light.Source = new BitmapImage(new Uri("pack://application:,,,/items/semafor1.png"));
                    });
                    Thread.Sleep(300);
                }
                else
                {
                    TrafficLights.IsTrafficLightsOn = false;

                    Dispatcher.Invoke(() =>
                    {
                        Light.Source = new BitmapImage(new Uri("pack://application:,,,/items/semaforZgaszony.png"));
                    });
                }
            }
        }
    }

    public void ZamykanieSzlabanu()
    {

        while (_isMoving)
        {
            
            if (TrafficLights.IsTrafficLightsOn)
            {
                Thread.Sleep(1100);
                Dispatcher.Invoke(() =>
                {
                    openBarrier.Source = new BitmapImage(new Uri("pack://application:,,,/items/szlabanZamkniety.png"));
                    closeBarrier.Source = new BitmapImage(new Uri("pack://application:,,,/items/szlabanZamkniety.png"));
                });
            }
            else
            {
                Thread.Sleep(20);
                Dispatcher.Invoke(() =>
                {
                    openBarrier.Source = new BitmapImage(new Uri("pack://application:,,,/items/szlabanOtwarty.png"));
                    closeBarrier.Source = new BitmapImage(new Uri("pack://application:,,,/items/szlabanOtwarty.png"));
                });
            }
        }
    }

    #endregion

    #region Buttoniki
    private void StartButton_Click(object sender, RoutedEventArgs e)
    {
        if (_isMoving) return;
        _isMoving = true;
        lightsThread = new Thread(SygnalizacjaSwiatelkowa);
        trainThread = new Thread(TrainGoing);
        barrierThread = new Thread(ZamykanieSzlabanu);
        lightsThread.Start();
        trainThread.Start();
        barrierThread.Start();
        carThread = new Thread[5];
        for (int i = 0; i < 5; i++)
        {
            carThread[i] = new Thread(CarGoing);
            carThread[i].Name = $"samochodzik {i + 1}";
            carThread[i].Start();
        }
    }
    private async void StopButton_Click(object sender, RoutedEventArgs e)
    {
        await Task.Run(() => trainThread?.Join());

        var tasks = new Task[5];
        for (int i = 0; i < 5; i++)
        {
            int index = i; 
            tasks[i] = Task.Run(() => carThread[index]?.Join());
        }

        await Task.WhenAll(tasks);

        await Task.Run(() => trainThread?.Join());

        await Task.Run(() => barrierThread?.Join());

        await Task.Run(() => lightsThread?.Join());

        _isMoving = false;

        Dispatcher.Invoke(() => { backgroundWay.Children.Clear(); });
    }
    #endregion
}