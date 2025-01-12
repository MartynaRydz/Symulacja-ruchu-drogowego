﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows.Threading;
using System.Diagnostics;
using System.Linq;
using Projekcik.Models;
using Projekcik.Controllers;
using System.ComponentModel.DataAnnotations;

namespace Projekcik
{
    public partial class MainWindow : Window
    {
        private Random _random = new Random();
        private bool _isMoving;  //czy ciufcia się porusza
        SamochodzikController _samochodzikController = new SamochodzikController();
        Thread ciufciaThread;
        Thread[] samochodzikThread;

        

        public MainWindow()
        {
            InitializeComponent();
            _isMoving = false;
        }
        #region CiufCiuf
        private void CiufciaJedzie()
        {

            while (_isMoving)
            {
                CiufCiuf ciufciuf = null;
                int tloDrogaWidth = 0;
                int ciufciufWidth = 0;

                Dispatcher.Invoke(() =>
                {
                    ciufciuf = new CiufCiuf();
                    tloDroga.Children.Add(ciufciuf.CiufciufImage);// dodajemy ciufciuf (image) na canvas
                    tloDrogaWidth = (int)tloDroga.Width;
                    ciufciufWidth = (int)(ciufciuf.CiufciufImage as FrameworkElement).Width;
                    Canvas.SetLeft(ciufciuf.CiufciufImage, ciufciuf.X);
                    Canvas.SetTop(ciufciuf.CiufciufImage, ciufciuf.Y);
                });


                for (int i = 0; i < tloDrogaWidth + ciufciufWidth * 2; i += Math.Abs(ciufciuf.CiufciowaPredkosc))
                {
                    ciufciuf.X += ciufciuf.CiufciowaPredkosc;
                    Dispatcher.Invoke(() =>
                    {
                        Canvas.SetLeft(ciufciuf.CiufciufImage, ciufciuf.X);
                        Canvas.SetTop(ciufciuf.CiufciufImage, ciufciuf.Y);
                    });
                    Thread.Sleep(50);
                }

                Dispatcher.Invoke(() => tloDroga.Children.Remove(ciufciuf.CiufciufImage));

                Thread.Sleep(_random.Next(2, 5) * 1000);

            }
        }

        // 0 - góra, 1 - prawo, 2 - dół, 3 - lewo
        #endregion
        private void SamochodzikJedzie()
        {
            Samochodzik samochodzik = null;
            while(_isMoving)
            {
                samochodzik = new Samochodzik();

                //Segment dodawania
                while(!_samochodzikController.CzyMogeDodacSamochodziku(samochodzik))
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



                //Segment usuwania
                if (_samochodzikController.UsuwankoSamchodziku(samochodzik))
                {
                    Dispatcher.Invoke(() => { tloDroga.Children.Remove(samochodzik.SamochodzikImage); });
                }
            }


            /*while (_isMoving)
            {
                List<Tuple<int, int>> obecneJechanko = null;//_random.Next(1, 3) == 1 ? kierunekIDystansLewo : kierunekIDystansPrawo;
                Samochodzik samochodzik = null;
                int tloDrogaWidth = 0;
                int smochodzikWidth = 0;

                Dispatcher.Invoke(() =>
                {
                    samochodzik = new Samochodzik();
                    tloDroga.Children.Add(samochodzik.SamochodzikImage);// dodajemy ciufciuf (image) na canvas
                    tloDrogaWidth = (int)tloDroga.Width;
                    smochodzikWidth = (int)(samochodzik.SamochodzikImage as FrameworkElement).Width;
                    Canvas.SetLeft(samochodzik.SamochodzikImage, samochodzik.X);
                    Canvas.SetTop(samochodzik.SamochodzikImage, samochodzik.Y);
                });

                foreach (var odcinekDrogi in obecneJechanko)
                {
                    Dispatcher.Invoke(() => tloDroga.Children.Remove(samochodzik.SamochodzikImage));
                    switch (odcinekDrogi.Item1)
                    {
                        case 0:
                            Dispatcher.Invoke(() => (samochodzik.SamochodzikImage as Image).Source = new BitmapImage(new Uri("pack://application:,,,/items/samochodGora.png")));
                            Dispatcher.Invoke(() => (samochodzik.SamochodzikImage as Image).Stretch = Stretch.Uniform);
                            break;

                        case 1:
                            Dispatcher.Invoke(() => (samochodzik.SamochodzikImage as Image).Source = new BitmapImage(new Uri("pack://application:,,,/items/samochodPrawo.png")));
                            Dispatcher.Invoke(() => (samochodzik.SamochodzikImage as Image).Stretch = Stretch.Uniform);
                            break;

                        case 2:
                            Dispatcher.Invoke(() => (samochodzik.SamochodzikImage as Image).Source = new BitmapImage(new Uri("pack://application:,,,/items/samochodDol.png")));
                            Dispatcher.Invoke(() => (samochodzik.SamochodzikImage as Image).Stretch = Stretch.Uniform);
                            break;

                        case 3:
                            Dispatcher.Invoke(() => (samochodzik.SamochodzikImage as Image).Source = new BitmapImage(new Uri("pack://application:,,,/items/samochodLewo.png")));
                            Dispatcher.Invoke(() => (samochodzik.SamochodzikImage as Image).Stretch = Stretch.Uniform);
                            break;

                    }

                    
                }

            }*/
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (_isMoving) return;
            _isMoving = true;
            ciufciaThread = new Thread(CiufciaJedzie);
            //samochodzikThread = new Thread(SamochodzikJedzie);
            ciufciaThread.Start();
            //samochodzikThread.Start();
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
            _isMoving = false;
            await Task.Run(() => ciufciaThread?.Join());
            for (int i = 0; i < 5; i++)
            {
                await Task.Run(() => samochodzikThread[i]?.Join());
            }
            //await Task.Run(() => samochodzikThread?.Join());
            Dispatcher.Invoke(() => { tloDroga.Children.Clear(); });
        }
    }
}