using System;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.Windows.Shapes;


namespace Projekcik
{
    public class CiufCiuf
    {
        public double X { get; set; }
        public double Y { get; set; }
        public int kieruneczek { get; set; } // 1 - prawo, 0 - lewo
        public UIElement CiufciufRectangle { get; set; }
        public int CiufciowaPredkosc { get; set; }
        Random random = new Random();

        public CiufCiuf()
        {
            kieruneczek = random.Next(0, 2);
            CiufciufRectangle = new System.Windows.Shapes.Rectangle
            {
                Width = 350,  // szerokość ciufci
                Height = 150, // wysokość ciufci
            };

            if (kieruneczek == 0)
            {
                Y = 500;
                X = 1200;
                CiufciowaPredkosc = -random.Next(5, 11);
                (CiufciufRectangle as System.Windows.Shapes.Rectangle).Fill = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/items/ciufciufBezTla.png"))) { Stretch = Stretch.Uniform };
            }
            else if(kieruneczek == 1)
            {
                Y = 500;
                X = -350;
                CiufciowaPredkosc = random.Next(2, 8);
                (CiufciufRectangle as System.Windows.Shapes.Rectangle).Fill = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/items/ciufciufBezTlaPrawo.png"))) { Stretch = Stretch.Uniform };
            }
        }

    }
}
