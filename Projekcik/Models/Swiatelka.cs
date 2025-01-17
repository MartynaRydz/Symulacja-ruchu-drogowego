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

internal class Swiatelka
{
    public double X { get; set; }
    public double Y { get; set; }
    public bool SwiatelkaSwieca { get; set;  }
    public UIElement SwiatelkaImage { get; set; }

    public Swiatelka()
    {
        SwiatelkaImage = new Image
        {
            Width = 200,  // szerokość
            Height = 100,  // wysokość 
        };

        X = 300;
        Y = 600;

        SwiatelkaSwieca = false;

        (SwiatelkaImage as Image).Source = new BitmapImage(new Uri("pack://application:,,,/items/semafor1.png"));
        (SwiatelkaImage as Image).Stretch = Stretch.Uniform;
    }

    public void SwiecaceSwiatelka()
    {
        while(SwiatelkaSwieca)
        {
            (SwiatelkaImage as Image).Source = new BitmapImage(new Uri("pack://application:,,,/items/semafor2.png"));
            (SwiatelkaImage as Image).Stretch = Stretch.Uniform;

            Thread.Sleep(500);

            (SwiatelkaImage as Image).Source = new BitmapImage(new Uri("pack://application:,,,/items/semafor1.png"));
            (SwiatelkaImage as Image).Stretch = Stretch.Uniform;

            Thread.Sleep(500);
        }

        (SwiatelkaImage as Image).Source = new BitmapImage(new Uri("pack://application:,,,/items/semafor1.png"));
        (SwiatelkaImage as Image).Stretch = Stretch.Uniform;
    }
}
