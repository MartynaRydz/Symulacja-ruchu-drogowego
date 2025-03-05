using Projekcik.Enum;
using Projekcik.Events;
using Projekcik.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekcik.Controllers;

public class CarController
{
    List<Car> cars = new List<Car>();
    int numberOfRoadSegments = 5;

    readonly List<Tuple<Direction, int>> directionAndDistanceRight = new List<Tuple<Direction, int>>()
    {
      Tuple.Create(Direction.Right, 950),
      Tuple.Create(Direction.Down, 150),  
      Tuple.Create(Direction.Left, 690), 
      Tuple.Create(Direction.Down, 260),  
      Tuple.Create(Direction.Right, 1000)   
    };

    readonly List<Tuple<Direction, int>> directionAndDistanceLeft = new List<Tuple<Direction, int>>()
    {
      Tuple.Create(Direction.Left, 1080), 
      Tuple.Create(Direction.Up, 165),  
      Tuple.Create(Direction.Right, 670),   
      Tuple.Create(Direction.Up, 250),   
      Tuple.Create(Direction.Left, 900)    
    };

    public bool DeleteCar(Car car) 
    {
        return cars.Remove(car);
    }
    public bool CanAddCar(Car car)
    {
        var carList = cars.Where(c => !c.Equals(car) && c.WayDirection == car.WayDirection && c.CurrentSegment == car.CurrentSegment).ToList();

        if (carList.Count == 0)
        {
            return true;
        }

        foreach (Car carFromList in carList)
        {
            if (carFromList.DistanceTraveledInSegment > 280)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        
        return false;
    }
    public void AddCar(Car car)
    {
        cars.Add(car);
    }
    public void carSpeedAdjustment(Car car)
    {
        var carList = cars.Where(c => !c.Equals(car) && c.WayDirection == car.WayDirection && c.CurrentSegment == car.CurrentSegment).ToList();
        foreach (Car carFromList in carList)
        {
            if (Math.Abs(car.DistanceTraveledInSegment - carFromList.DistanceTraveledInSegment) <= 180 && car.CarsSpeed > carFromList.CarsSpeed) 
            {
                car.CarsSpeed = carFromList.CarsSpeed;
            }
        }
    }
    public bool CarUpdate(Car car)
    {
        if (car.CurrentSegment < numberOfRoadSegments && car.CurrentSegment >= 0)
        {
            var carDirectionAndDistance = car.WayDirection == Direction.Right ? directionAndDistanceRight : directionAndDistanceLeft;
            carSpeedAdjustment(car);

            if (car.DistanceTraveledInSegment < carDirectionAndDistance[car.CurrentSegment].Item2)
            {
                car.Move();
            }
            else
            {
                car.CurrentSegment++;
                if (car.CurrentSegment < 5 && car.CurrentSegment >= 0)
                {
                    car.ChangeDirestion(carDirectionAndDistance[car.CurrentSegment].Item1);
                }
            }
            return true;
        }
        else
        {
            return false;
        }
    }
}
