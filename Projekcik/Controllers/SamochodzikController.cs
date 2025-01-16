using Projekcik.Enum;
using Projekcik.Events;
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
    public bool CzyMogeDodacSamochodzik(Samochodzik samochodzik)
    {
        var samochodzikList = samodziki.Where(c => c.Kieruneczek == samochodzik.Kieruneczek && c.ObecnySegment == samochodzik.ObecnySegment).ToList();
        foreach (Samochodzik samochodzik2 in samochodzikList)
        {
            if (samochodzik2.PrzejechanaOdlegloscWSegmencie > 200)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return true;
    }

    public void DodawanieSamochodziku(Samochodzik samochodzik)
    {
        samodziki.Add(samochodzik);
    }
    public void DostosowanieSamochodzikowejPredkosci(Samochodzik samochodzik1)
    {
        var samochodzikList = samodziki.Where(c => c.Kieruneczek == samochodzik1.Kieruneczek && c.ObecnySegment == samochodzik1.ObecnySegment).ToList();
        foreach (Samochodzik samochodzik2 in samochodzikList)
        {
            if (Math.Abs(samochodzik2.PrzejechanaOdlegloscWSegmencie - samochodzik1.PrzejechanaOdlegloscWSegmencie) < 200)
            {
                samochodzik1.SamochodzikowaPredkosc = samochodzik2.SamochodzikowaPredkosc;
            }
        }
    }

    public bool AktualizacjaSamochodziku(Samochodzik samochodzik)
    {
        if (samochodzik.ObecnySegment < numberOfRoadSegments && samochodzik.ObecnySegment >= 0)
        {
            var kierunekIDystansSamochodziku = samochodzik.KieruneczekDrogi == Kieruneczek.Prawo ? kierunekIDystansPrawo : kierunekIDystansLewo;
            DostosowanieSamochodzikowejPredkosci(samochodzik);

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
