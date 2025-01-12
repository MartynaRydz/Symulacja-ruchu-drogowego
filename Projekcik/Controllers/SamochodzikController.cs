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
        if ((samochodzik.KieruneczekDrogi == Kieruneczek.Prawo && samochodzik.X >= 200) || (samochodzik.KieruneczekDrogi == Kieruneczek.Lewo && samochodzik.X <= 800))
        {
            return true;
        }
        else
        { 
            return false;
        }
    }
    public void DodawanieSamochodziku(Samochodzik samochodzik)
    {
        samodziki.Add(samochodzik);
    }
    public void DostosowanieSamochodzikowejPredkosci(Samochodzik samochodzik1, Samochodzik samochodzik2)
    {
        if (samochodzik1.KieruneczekDrogi == samochodzik2.KieruneczekDrogi)
        {
            samochodzik1.SamochodzikowaPredkosc = samochodzik2.SamochodzikowaPredkosc;
        }
    }
    public void AktualizacjaSamochodziku(Samochodzik samochodzik)
    {
        samochodzik.Ruch();
    }
}
