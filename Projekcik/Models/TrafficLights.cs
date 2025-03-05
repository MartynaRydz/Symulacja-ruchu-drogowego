using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows.Controls;

namespace Projekcik.Models;

internal class TrafficLights
{

    public static bool IsTrafficLightsOn { get; set; }

    public TrafficLights()
    {
        IsTrafficLightsOn = false;
    }

}
