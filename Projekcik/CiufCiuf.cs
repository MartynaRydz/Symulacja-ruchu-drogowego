using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Controls;

namespace Projekcik;

public class CiufCiuf
{
    public double X { get; set; }
    public double Y { get; set; }
    public int Kieruneczek { get; set; } // 1 - prawo, 0 - lewo
    public UIElement CiufciufImage { get; set; }
    public int CiufciowaPredkosc { get; set; }
    private Random random = new Random();

    public CiufCiuf()
    {
        Kieruneczek = random.Next(0, 2);

        // Zamiana na Image zamiast Rectangle
        CiufciufImage = new Image
        {
            Width = 350,  // szerokość ciufci
            Height = 150, // wysokość ciufci
        };

        if (Kieruneczek == 0)
        {
            Y = 500;
            X = 1200;
            CiufciowaPredkosc = -random.Next(5, 11);
            (CiufciufImage as Image).Source = new BitmapImage(new Uri("pack://application:,,,/items/ciufciufBezTla.png"));
            (CiufciufImage as Image).Stretch = Stretch.Uniform;
        }
        else if (Kieruneczek == 1)
        {
            Y = 500;
            X = -450;
            CiufciowaPredkosc = random.Next(2, 8);
            (CiufciufImage as Image).Source = new BitmapImage(new Uri("pack://application:,,,/items/ciufciufBezTlaPrawo.png"));
            (CiufciufImage as Image).Stretch = Stretch.Uniform;
        }
    }
}
