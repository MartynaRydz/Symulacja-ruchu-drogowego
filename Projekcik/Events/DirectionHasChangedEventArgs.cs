using Projekcik.Enum;
using Projekcik.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekcik.Events;

public class DirectionHasChangedEventArgs : EventArgs
{
    public Car directionChanegCar { get; set;}
    //public Kieruneczek PoprzedniKieruneczek { get; set;}
}
