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
    public int Kieruneczek { get; set; } // 1 - prawo, 0 - lewo
    public UIElement SamochodzikImage { get; set; }
    public int SamochodzikowaPredkosc { get; set; }
    private Random random = new Random();

    public Samochodzik()
    {
        Kieruneczek = random.Next(0, 2);

        SamochodzikImage = new Image
        {
            Width = 110,  // szerokość samochodzika
            Height = 110,  // wysokość samochodzika
        };

        SamochodzikowaPredkosc = random.Next(4, 8); //losowanie prędkości od 1 do 5

        if (Kieruneczek == 0)
        {
            Y = 580;
            X = 1200;
            SamochodzikowaPredkosc = -SamochodzikowaPredkosc;
            (SamochodzikImage as Image).Source = new BitmapImage(new Uri("pack://application:,,,/items/samochodLewo.png"));
            (SamochodzikImage as Image).Stretch = Stretch.Uniform;
        }
        else if (Kieruneczek == 1)
        {
            Y = 200;
            X = -200;
            (SamochodzikImage as Image).Source = new BitmapImage(new Uri("pack://application:,,,/items/samochodPrawo.png"));
            (SamochodzikImage as Image).Stretch = Stretch.Uniform;
        }
    }
}


