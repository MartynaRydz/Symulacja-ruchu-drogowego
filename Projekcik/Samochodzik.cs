using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Projekcik;

public class Samochodzik
{
    public double X { get; set; }
    public double Y { get; set; }
    public int Kieruneczek { get; set; } // 0 - góra, 1 - prawo, 2 - dół, 3 - lewo
    public UIElement SamochodzikImage { get; set; }
    public int SamochodzikowaPredkosc { get; set; }
    private Random random = new Random();

    public Samochodzik()
    {
        Kieruneczek = random.Next(0, 2);

        SamochodzikImage = new Image
        {
            Width = 100,  // szerokość samochodzika
            Height = 80,  // wysokość samochodzika
        };

        SamochodzikowaPredkosc = random.Next(4, 8); //losowanie prędkości 

        if (Kieruneczek == 0)
        {
            Y = 590;
            X = 1200;
            
            (SamochodzikImage as Image).Source = new BitmapImage(new Uri("pack://application:,,,/items/samochodLewo.png"));
            (SamochodzikImage as Image).Stretch = Stretch.Uniform;
        }
        else if (Kieruneczek == 1)
        {
            Y = 220;
            X = -200;
            
            (SamochodzikImage as Image).Source = new BitmapImage(new Uri("pack://application:,,,/items/samochodPrawo.png"));
            (SamochodzikImage as Image).Stretch = Stretch.Uniform;
        }
    }
}


