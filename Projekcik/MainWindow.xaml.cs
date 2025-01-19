using Projekcik.Controllers;
using Projekcik.Enum;
using Projekcik.Events;
using Projekcik.Models;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Threading;

namespace Projekcik;

public partial class MainWindow : Window
{
    private Random _random = new Random();
    private bool _isMoving;  //czy ciufcia i samochodziki się poruszaja
    SamochodzikController _samochodzikController = new SamochodzikController();
    CiufciufController _ciufciufController = new CiufciufController();
    Swiatelka swiatelka = new Swiatelka();
    Szlabanik szlabanik = new Szlabanik();
    Thread szlabanikThread;
    Thread ciufciaThread;
    Thread[] samochodzikThread;
    Thread swiatelkaThread;
    bool swieca = false;
    //Thread samochodzikThread;



    public MainWindow()
    {
        InitializeComponent();
        Canvas.SetZIndex(szlabanikDol, 1);
        _isMoving = false;  
        swieca = false;
    }

    #region Pojazdy
    
    private void CiufciaJedzie()
    {

        while (_isMoving)
        {
            Dispatcher.Invoke(() => { _ciufciufController.CiufCiuf = new CiufCiuf(); });
            int tloDrogaWidth = 0;
            int ciufciufWidth = 0;

            Dispatcher.Invoke(() =>
            {
                tloDroga.Children.Add(_ciufciufController.CiufCiuf.CiufciufImage);// dodajemy ciufciuf (image) na canvas
                tloDrogaWidth = (int)tloDroga.Width;
                ciufciufWidth = (int)(_ciufciufController.CiufCiuf.CiufciufImage as FrameworkElement).Width;
                Canvas.SetLeft(_ciufciufController.CiufCiuf.CiufciufImage, _ciufciufController.CiufCiuf.X);
                Canvas.SetTop(_ciufciufController.CiufCiuf.CiufciufImage, _ciufciufController.CiufCiuf.Y);
            });


            for (int i = 0; i < tloDrogaWidth + ciufciufWidth * 2; i += Math.Abs(_ciufciufController.CiufCiuf.CiufciowaPredkosc))
            {
                _ciufciufController.CiufCiuf.X += _ciufciufController.CiufCiuf.CiufciowaPredkosc;
                Dispatcher.Invoke(() =>
                {
                    Canvas.SetLeft(_ciufciufController.CiufCiuf.CiufciufImage, _ciufciufController.CiufCiuf.X);
                    Canvas.SetTop(_ciufciufController.CiufCiuf.CiufciufImage, _ciufciufController.CiufCiuf.Y);
                });
                Thread.Sleep(50);
            }

            Dispatcher.Invoke(() => tloDroga.Children.Remove(_ciufciufController.CiufCiuf.CiufciufImage));

            Thread.Sleep(_random.Next(4, 6) * 1000);

        }
    }
    private void Samochodzik_KieruneczekZmienlSie(object? sender, KierunekZmienilSieEventArgs e)
    {
        // Dispatcher.Invoke(() => tloDroga.Children.Remove(e.SamochodzikZmienajacyKierunek.SamochodzikImage));

        switch (e.SamochodzikZmienajacyKierunek.Kieruneczek)
        {
            case Kieruneczek.Gora:
                Dispatcher.Invoke(() => (e.SamochodzikZmienajacyKierunek.SamochodzikImage as Image).Source = new BitmapImage(new Uri("pack://application:,,,/items/samochodGora.png")));
                break;

            case Kieruneczek.Prawo:
                Dispatcher.Invoke(() => (e.SamochodzikZmienajacyKierunek.SamochodzikImage as Image).Source = new BitmapImage(new Uri("pack://application:,,,/items/samochodPrawo.png")));
                break;

            case Kieruneczek.Dol:
                Dispatcher.Invoke(() => (e.SamochodzikZmienajacyKierunek.SamochodzikImage as Image).Source = new BitmapImage(new Uri("pack://application:,,,/items/samochodDol.png")));
                break;

            case Kieruneczek.Lewo:
                Dispatcher.Invoke(() => (e.SamochodzikZmienajacyKierunek.SamochodzikImage as Image).Source = new BitmapImage(new Uri("pack://application:,,,/items/samochodLewo.png")));
                break;
        }
        /*Dispatcher.Invoke(() =>
        {
            tloDroga.Children.Add(e.SamochodzikZmienajacyKierunek.SamochodzikImage);
            Canvas.SetLeft(e.SamochodzikZmienajacyKierunek.SamochodzikImage, e.SamochodzikZmienajacyKierunek.X);
            Canvas.SetTop(e.SamochodzikZmienajacyKierunek.SamochodzikImage, e.SamochodzikZmienajacyKierunek.Y);
        });*/
    }
    private void SamochodzikJedzie()
    {
        Samochodzik samochodzik = null;
        while (_isMoving)
        {
           Dispatcher.Invoke(()=> samochodzik = new Samochodzik());
            samochodzik.KieruneczekZmienlSie += Samochodzik_KieruneczekZmienlSie;

            //Segment dodawania
            while (!_samochodzikController.CzyMogeDodacSamochodzik(samochodzik))
            {
                Thread.Sleep(1000);
            }
            _samochodzikController.DodawanieSamochodziku(samochodzik);

            Dispatcher.Invoke(() => 
            {
                tloDroga.Children.Add(samochodzik.SamochodzikImage);
                Canvas.SetLeft(samochodzik.SamochodzikImage,samochodzik.X);
                Canvas.SetTop(samochodzik.SamochodzikImage,samochodzik.Y);
            });

            //Segment aktualizowania
            while(_samochodzikController.AktualizacjaSamochodziku(samochodzik))
            {
                if (swieca && ((samochodzik.Kieruneczek == Kieruneczek.Gora && samochodzik.ObecnySegment == 2)||((samochodzik.Kieruneczek == Kieruneczek.Dol && samochodzik.ObecnySegment == 4))))
                {
                    samochodzik.SamochodzikowaPredkosc = 0;
                }
                else 
                {
                    samochodzik.SamochodzikowaPredkosc = _random.Next(3, 8);
                }

                Thread.Sleep(50);
                Dispatcher.Invoke(() =>
                {
                    Canvas.SetLeft(samochodzik.SamochodzikImage, samochodzik.X);
                    Canvas.SetTop(samochodzik.SamochodzikImage, samochodzik.Y);
                });
            }

            //Segment usuwania
            if (_samochodzikController.UsuwankoSamchodziku(samochodzik))
            {
                Dispatcher.Invoke(() => { tloDroga.Children.Remove(samochodzik.SamochodzikImage); });
            }
        }
    }
    #endregion

    public void SygnalizacjaSwiatelkowa()
    {
        Dispatcher.Invoke(() =>
        {
            tloDroga.Children.Add(swiatelka.SwiatelkaImage);
            Canvas.SetLeft(swiatelka.SwiatelkaImage, swiatelka.X);
            Canvas.SetTop(swiatelka.SwiatelkaImage, swiatelka.Y);
        });

        while (_isMoving)
        {
            if (_ciufciufController.CiufCiuf is not null)
            {               

                if (_ciufciufController.CiufCiuf.X > -400 && _ciufciufController.CiufCiuf.X < 500)
                {
                    swieca = true;

                    Dispatcher.Invoke(() =>
                    {
                        (swiatelka.SwiatelkaImage as Image).Source = new BitmapImage(new Uri("pack://application:,,,/items/semafor2.png"));
                        (swiatelka.SwiatelkaImage as Image).Stretch = Stretch.Uniform;
                    });

                    Thread.Sleep(300);

                    Dispatcher.Invoke(() =>
                    {
                        (swiatelka.SwiatelkaImage as Image).Source = new BitmapImage(new Uri("pack://application:,,,/items/semafor1.png"));
                        (swiatelka.SwiatelkaImage as Image).Stretch = Stretch.Uniform;
                    });
                    Thread.Sleep(300);
                }

                swieca = false;

                Dispatcher.Invoke(() =>
                {
                    (swiatelka.SwiatelkaImage as Image).Source = new BitmapImage(new Uri("pack://application:,,,/items/semaforZgaszony.png"));
                    (swiatelka.SwiatelkaImage as Image).Stretch = Stretch.Uniform;
                });
            }
        }
    }

    public void ZamykanieSzlabanu()
    {

        while (_isMoving)
        {
            

            if (swieca)
            {
                Thread.Sleep(1000);
                Dispatcher.Invoke(() =>
                {
                    szlabanikGora.Source = new BitmapImage(new Uri("pack://application:,,,/items/szlabanZamkniety.png"));
                    szlabanikDol.Source = new BitmapImage(new Uri("pack://application:,,,/items/szlabanZamkniety.png"));
                });
            }
            else
            {
                Dispatcher.Invoke(() =>
                {
                    szlabanikGora.Source = new BitmapImage(new Uri("pack://application:,,,/items/szlabanOtwarty.png"));
                    szlabanikDol.Source = new BitmapImage(new Uri("pack://application:,,,/items/szlabanOtwarty.png"));
                });
            }
        }
    }

    #region Buttoniki
    private void StartButton_Click(object sender, RoutedEventArgs e)
    {
        if (_isMoving) return;
        _isMoving = true;
        swiatelkaThread = new Thread(SygnalizacjaSwiatelkowa);
        ciufciaThread = new Thread(CiufciaJedzie);
        szlabanikThread = new Thread(ZamykanieSzlabanu);
        //samochodzikThread = new Thread(SamochodzikJedzie);
        //samochodzikThread.Start();
        swiatelkaThread.Start();
        ciufciaThread.Start();
        szlabanikThread.Start();
        samochodzikThread = new Thread[5];
        for (int i = 0; i < 5; i++)
        {
            samochodzikThread[i] = new Thread(SamochodzikJedzie);
            samochodzikThread[i].Name = $"samochodzik {i + 1}";
            samochodzikThread[i].Start();
        }
    }
    private async void StopButton_Click(object sender, RoutedEventArgs e)
    {
        await Task.Run(() => ciufciaThread?.Join());

        var tasks = new Task[5];
        for (int i = 0; i < 5; i++)
        {
            int index = i; 
            tasks[i] = Task.Run(() => samochodzikThread[index]?.Join());
        }

        await Task.WhenAll(tasks);

        await Task.Run(() => ciufciaThread?.Join());

        await Task.Run(() => szlabanikThread?.Join());

        await Task.Run(() => swiatelkaThread?.Join());

        _isMoving = false;

        Dispatcher.Invoke(() => { tloDroga.Children.Clear(); });
    }
    #endregion
}