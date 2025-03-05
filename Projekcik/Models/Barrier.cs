using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace Projekcik.Models;

public class Barrier
{
    public double X { get; set; }
    public double Y { get; set; }
    public bool IsBarrierOpen { get; set; }
    public UIElement BarrierImage { get; set; }

    public Barrier()
    {
        BarrierImage = new Image
        {
            Width = 200,  
            Height = 100,  
        };

        X = 300;
        Y = 600;

        IsBarrierOpen = true;

        (BarrierImage as Image).Source = new BitmapImage(new Uri("pack://application:,,,/items/szlabanOtwarty.png"));
        (BarrierImage as Image).Stretch = Stretch.Uniform;
    }
}
