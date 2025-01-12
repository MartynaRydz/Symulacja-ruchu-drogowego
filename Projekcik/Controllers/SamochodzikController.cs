using Projekcik.Enum;
using Projekcik.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekcik.Controllers;

public class SamochodzikController
{
    List<Samochodzik> samodziki = new List<Samochodzik>();
    int numberOfRoadSegments = 5;

    readonly List<Tuple<Kieruneczek, int>> kierunekIDystansPrawo = new List<Tuple<Kieruneczek, int>>()
    {
      Tuple.Create(Kieruneczek.Prawo, 950),  //prawo1
      Tuple.Create(Kieruneczek.Dol, 1510),  //dół
      Tuple.Create(Kieruneczek.Lewo, 700),  //lewo
      Tuple.Create(Kieruneczek.Dol, 255),  //dół2
      Tuple.Create(Kieruneczek.Prawo, 950)   //prawo2
    };

    readonly List<Tuple<Kieruneczek, int>> kierunekIDystansLewo = new List<Tuple<Kieruneczek, int>>()
    {
      Tuple.Create(Kieruneczek.Lewo, 1080),  //lewo1
      Tuple.Create(Kieruneczek.Gora, 165),   //góra1
      Tuple.Create(Kieruneczek.Prawo, 670),   //prawo
      Tuple.Create(Kieruneczek.Gora, 250),   //góra2
      Tuple.Create(Kieruneczek.Lewo, 1000)    //lewo2
    };

    public bool UsuwankoSamchodziku(Samochodzik samochodzik) // usówa i zwraca bool bo chcę zrobić tak że jeśłi usunie z listy to wtedy w main sprawdzę czy true czy false i usunę wtedy obrazek z tym zamochodem :D
    {
        return samodziki.Remove(samochodzik);
    }
    public bool CzyMogeDodacSamochodziku(Samochodzik samochodzik) //żeby nie stworzyły się dwa samochodziki na sobie
    {
        foreach(Samochodzik samochidzik2 in samodziki.Where(c => c.Kieruneczek == samochodzik.Kieruneczek))
        {
            if (Math.Abs(samochidzik2.X - samochodzik.X) < 200)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        return true;
    }
    public void DodawanieSamochodziku(Samochodzik samochodzik)
    {
        samodziki.Add(samochodzik);
    }
    public void DostosowanieSamochodzikowejPredkosci(Samochodzik samochodzik1, Samochodzik samochodzik2) //samochodzik 1 to zamochodzik sprawdzany a 2 to ten do którego porównujemy
    {
        foreach (Samochodzik samochidzik2 in samodziki.Where(c => c.Kieruneczek == samochodzik1.Kieruneczek))
        {
            if (Math.Abs(samochidzik2.X - samochodzik1.X) < 200)
            {
                samochodzik1.SamochodzikowaPredkosc = samochidzik2.SamochodzikowaPredkosc;
            }
        }
    }
    public bool AktualizacjaSamochodziku(Samochodzik samochodzik)
    {
        if (samochodzik.ObecnySegment < 5 && samochodzik.ObecnySegment >= 0)
        {
            var kierunekIDystansSamochodziku = samochodzik.KieruneczekDrogi == Kieruneczek.Prawo ? kierunekIDystansPrawo : kierunekIDystansLewo;

            if (samochodzik.PrzejechanaOdlegloscWSegmencie < kierunekIDystansSamochodziku[samochodzik.ObecnySegment].Item2)
            {
                samochodzik.Ruch();
            }
            else
            {
                samochodzik.ObecnySegment++;
                if (samochodzik.ObecnySegment < 5 && samochodzik.ObecnySegment >= 0)
                {
                    samochodzik.ZmaianaKieruneczku(kierunekIDystansSamochodziku[samochodzik.ObecnySegment].Item1);
                }
            }
            return true;
        }
        else
        {
            return false;
        }

    }
}
