using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Projekcik.Models;

public class Train
{
    public double X { get; set; }
    public double Y { get; set; }
    public int Direction { get; set; }
    public UIElement TrainImage { get; set; }
    public int TrainSpeed { get; set; }
    private Random random = new Random();

    public Train()
    {
        Direction = random.Next(0, 2);

            TrainImage = new Image
            {
                Width = 350,  
                Height = 150, 
            };

        if (Direction == 0)
        {
            Y = 500;
            X = 1200;
            TrainSpeed = -random.Next(5, 11);
            (TrainImage as Image).Source = new BitmapImage(new Uri("pack://application:,,,/items/ciufciufBezTla.png"));
            (TrainImage as Image).Stretch = Stretch.Uniform;
        }
        else if (Direction == 1)
        {
            Y = 500;
            X = -600;
            TrainSpeed = random.Next(2, 8);
            (TrainImage as Image).Source = new BitmapImage(new Uri("pack://application:,,,/items/ciufciufBezTlaPrawo.png"));
            (TrainImage as Image).Stretch = Stretch.Uniform;
        }
    }
}
