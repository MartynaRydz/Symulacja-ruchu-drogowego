using Projekcik.Enum;
using Projekcik.Events;
using System;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Projekcik.Models;

public class Car
{
    public double X { get; set; }
    public double Y { get; set; }
    public Direction Direction { get; set; }
    public Direction WayDirection { get; set; }  
    public UIElement CarImage { get; set; }
    public int CurrentSegment { get; set; }
    public int CarsSpeed { get; set; }
    public int DistanceTraveledInSegment { get; set; }

    private Random random = new Random();

    public event EventHandler<DirectionHasChangedEventArgs> DirectionHasChanged;


    protected virtual void OnDirectionHasChanged()
    {
        DirectionHasChanged?.Invoke(this, new DirectionHasChangedEventArgs() { directionChanegCar = this});
    }

    public Car()
    {
        CarImage = new Image
        {
            Width = 100, 
            Height = 80, 
        };

        CurrentSegment = 0;
        DistanceTraveledInSegment = 0;

        CarsSpeed = random.Next(3, 8);  

        int randomValue = random.Next(0, 2);  
        Direction startDirection = (randomValue == 0) ? Direction.Left : Direction.Right;

        if (startDirection == Direction.Left)
        {
            Y = 590;
            X = 1200;
            Direction = startDirection;

            WayDirection = startDirection;


            (CarImage as Image).Source = new BitmapImage(new Uri("pack://application:,,,/items/samochodLewo.png"));
            (CarImage as Image).Stretch = Stretch.Uniform;
        }
        else if (startDirection == Direction.Right)
        {
            Y = 220;
            X = -200;
            Direction = startDirection;
            WayDirection = startDirection;

            (CarImage as Image).Source = new BitmapImage(new Uri("pack://application:,,,/items/samochodPrawo.png"));
            (CarImage as Image).Stretch = Stretch.Uniform;
        }
    }
    public void ChangeDirestion(Direction direction)
    {
        Direction = direction;
        DistanceTraveledInSegment = 0;
        OnDirectionHasChanged();
    }
    
    public void Move()
    {

        if (TrafficLights.IsTrafficLightsOn && (Direction == Direction.Left && CurrentSegment == 2 || Direction == Direction.Left && CurrentSegment == 0) && X <= 200)
        {
            CarsSpeed = 0;
        }
        else
        {

            switch (Direction)
            {
                case Direction.Up:
                    Y -= CarsSpeed;
                    break;

                case Direction.Right:
                    X += CarsSpeed;
                    break;

                case Direction.Down:
                    Y += CarsSpeed;
                    break;

                case Direction.Left:
                    X -= CarsSpeed;
                    break;
            }
            DistanceTraveledInSegment += CarsSpeed;

        }

        if (CarsSpeed == 0 && !TrafficLights.IsTrafficLightsOn)
        {
            CarsSpeed = random.Next(3, 8);
        }

    }
}


