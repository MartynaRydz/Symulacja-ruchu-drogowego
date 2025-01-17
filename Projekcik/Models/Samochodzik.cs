using Projekcik.Enum;
using Projekcik.Events;
using System;
using System.Security.Cryptography.X509Certificates;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Projekcik.Models;

public class Samochodzik
{
    public double X { get; set; }
    public double Y { get; set; }
    public Kieruneczek Kieruneczek { get; set; }
    public Kieruneczek KieruneczekDrogi { get; set; }  
    public UIElement SamochodzikImage { get; set; }
    public int ObecnySegment { get; set; }
    public int SamochodzikowaPredkosc { get; set; }
    public int PrzejechanaOdlegloscWSegmencie { get; set; }

    private Random random = new Random();

    public event EventHandler<KierunekZmienilSieEventArgs> KieruneczekZmienlSie;

    protected virtual void OnKieruneczekZmienilSie()
    {
        KieruneczekZmienlSie?.Invoke(this, new KierunekZmienilSieEventArgs() { SamochodzikZmienajacyKierunek = this});
    }

    public Samochodzik()
    {
        SamochodzikImage = new Image
        {
            Width = 100,  // szerokość samochodzika
            Height = 80,  // wysokość samochodzika
        };

        ObecnySegment = 0;
        PrzejechanaOdlegloscWSegmencie = 0;

        SamochodzikowaPredkosc = random.Next(3, 8); //losowanie prędkości 

        int randomValue = random.Next(0, 2);  // Losuje 0 lub 1
        Kieruneczek startKieruneczek = (randomValue == 0) ? Kieruneczek.Lewo : Kieruneczek.Prawo;

        if (startKieruneczek == Kieruneczek.Lewo)
        {
            Y = 590;
            X = 1200;
            Kieruneczek = startKieruneczek;//dodane w prubir naprawy(pomohłó)

            KieruneczekDrogi = startKieruneczek;


            (SamochodzikImage as Image).Source = new BitmapImage(new Uri("pack://application:,,,/items/samochodLewo.png"));
            (SamochodzikImage as Image).Stretch = Stretch.Uniform;
        }
        else if (startKieruneczek == Kieruneczek.Prawo)
        {
            Y = 220;
            X = -200;
            Kieruneczek = startKieruneczek;//dodane w prubie narawy(pomogło)

            KieruneczekDrogi = startKieruneczek;//myśłę ze to po to żeby dobieraó dobrą ścieszke w aktualizacjisamochodziku (donrze myślałam)

            (SamochodzikImage as Image).Source = new BitmapImage(new Uri("pack://application:,,,/items/samochodPrawo.png"));
            (SamochodzikImage as Image).Stretch = Stretch.Uniform;
        }
    }
    public void ZmaianaKieruneczku(Kieruneczek kieruneczek)
    {
        Kieruneczek = kieruneczek;
        PrzejechanaOdlegloscWSegmencie = 0;
        OnKieruneczekZmienilSie();
    }
    
    public void Ruch()
    {
        switch (Kieruneczek)
        {
            case Kieruneczek.Gora:
                Y -= SamochodzikowaPredkosc;
                break;

            case Kieruneczek.Prawo: 
                X += SamochodzikowaPredkosc;
                break;

            case Kieruneczek.Dol:
                Y += SamochodzikowaPredkosc;
                break;

            case Kieruneczek.Lewo:
                X -= SamochodzikowaPredkosc;
                break;
        }
        PrzejechanaOdlegloscWSegmencie += SamochodzikowaPredkosc;
    }
}


