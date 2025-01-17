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

public class Szlabanik
{
    public double X { get; set; }
    public double Y { get; set; }
    public bool SzlabanikOtwarty { get; set; }
    public UIElement SzlabanikImage { get; set; }

    public Szlabanik()
    {
        SzlabanikImage = new Image
        {
            Width = 200,  // szerokość
            Height = 100,  // wysokość 
        };

        X = 300;
        Y = 600;

        SzlabanikOtwarty = true;

        (SzlabanikImage as Image).Source = new BitmapImage(new Uri("pack://application:,,,/items/szlabanOtwarty.png"));
        (SzlabanikImage as Image).Stretch = Stretch.Uniform;
    }
}
