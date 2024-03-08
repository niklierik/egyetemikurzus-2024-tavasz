using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    public static class Konvertalas
    {
        public static void BoxingUnboxing()
        {
            int x = 33;
            object o = x; //boxing 

            int szam = (int)o; //unboxing 

            //jelen esetben kivédhető a Convert osztály használatával  
            var szam2 = (short)o; //hibát fog eredményezni 
                                 
        }

        public static void PeldaAs()
        {
            short ertek = 8;

            //Castolas as segítségével
            var a = ertek as int?;
            if (a == null)
            {
                // a cast nem sikerült 
            }
            else
            {
                //az a változó int? típusú 
            }

            //Rövidebben as helyett is
            var b = ertek is int? ? (int?)ertek : (int?)null;
        }

        public static void BoxingUnboxingHibakezeles()
        {
            int ertek = 11;
            object boxed = ertek;

            if (boxed is int)
            {
                //explicit unbox 
                int unboxed = (int)boxed;
                Console.WriteLine("A csomagolt adat int. Erteke: {0}", unboxed);
            }
            try
            {
                //mivel úgyis hibát dob 
                short short_unbox = (short)boxed;
                //ez nem fog lefutni 
                Console.WriteLine(short_unbox);
            }
            catch (InvalidCastException)
            {
                //nem szép megoldás, igyekezzünk ne így használni. 
                Console.WriteLine("Konvertálási hiba");
            }

            //csak referencia típusok és nullable esetén 
            //használható az as operátor! 
            short? short_unbox2 = boxed as short?;
            if (short_unbox2 == null)
            {
                Console.WriteLine("Short-ra nem sikerült konvertálni");
                int? un_int = boxed as int?;
                Console.WriteLine("A csomagolt adat int. Erteke: {0}", (int)un_int);
            }
            Console.ReadLine();

            //A csomagolt adat int. Erteke: 11 
            //Konvertálási hiba
            //Short - ra nem sikerült konvertálni
            //A csomagolt adat int.Erteke: 11
        }

        public static void Parse()
        {
            //Futas idejű hiba
            string szoveg = "valami szöveg";
            int toSzam = Convert.ToInt32(szoveg);
            Console.WriteLine(toSzam);
            Console.ReadKey();

            //Hibas formatum, futasi hiba
            int parseSzam = int.Parse(szoveg);
            Console.WriteLine(parseSzam);
            Console.ReadKey();

            //Hibakezelese nem try-catch -> az kivetelkezelesre való
            //Alaptipusok esetén kb mindig a TryParse konverió az ajánlott
            int szam;
            if (int.TryParse("123", out szam))
            {
                //sikeres volt a feldolgozás
                //ebben a blokkban a szam változó
                //biztos, hogy helytálló értékkel rendelkezik.
            }
        }
    }
}
