using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static ConsoleApp.ReferenciaErtek;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ConsoleApp
{
    public static class TombokForeach
    {
        public static void ForeachPelda()
        {
            int[] szamok = { 1, 2, 3, 4, 5, 6, 7 };
            //Olyan osztályok esetén működik amik megvalósítják az IEnumerable
            foreach (int value in szamok)
            {
                Console.WriteLine(value);
                //Nem tudjuk settelni. Fordítási hiba.
                //value = 0;
                //value = Console.ReadKey();
            }

            //Nem szép kijátszása a rendszernek
            foreach (var (value, i) in szamok.Select((value, i) => (value, i)))
            {
                Console.WriteLine(value + " is at index " + i);
                //A value továbbra sem módosítható, de az index érték tudatában a kollekció igen
                //value = 0;
                szamok[i] = 0;
            }
        }
        //Plusz pontos -> saját IEnumerable osztály

        public static void TombokPelda()
        {
            //Tömb létrehozása elemekkel szintaxisa
            var gyumolcsok = new string[]
            {
                "alma", "körte", "szilva"
            };

            //Egyszerű tömb létrehozásának a szintaxisa
            var bevitelek = new string[3];

            for (int i = 0; i < bevitelek.Length; i++)
            {
                Console.WriteLine("{0}. bevitel: ", i);
                bevitelek[i] = Console.ReadLine();
            }

            foreach (var gyumolcs in gyumolcsok)
            {
                Console.WriteLine(gyumolcs);
            }
            foreach (var bevitel in bevitelek)
            {
                Console.WriteLine(bevitel);
            }

            Console.ReadLine();

            //1.bevitel: első
            //2.bevitel: második
            //3.bevitel: harmadik
            //alma
            //körte
            //szilva
            //első
            //második
            //harmadik
        }

        //A main metódus egy szöveg tömböt vesz át. Ezen szövegtömb az operációs rendszer által a programnak átadott argumentumokattartalmazza.
        //Ez alapján készíthetünk argumentumokkal vezérelt programokat is.  
        //Ez nem lesz belépési pont mert top level statementes a a projekt
        public static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Nem elég argumentum!");
                return;
            }

            try
            {
                switch (args[0])
                {
                    case "hello":
                        Console.WriteLine("Hello");
                        break;
                    case "hellow":
                        Console.WriteLine("Hello, {0}!", args[1]);
                        break;
                    default:
                        Console.WriteLine("Ismeretlen argumentum!");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Hiba történt: {0}", ex.Message);
            }
            Console.ReadLine();

            //A példaprogramot parancssorból indítsuk el a következő paraméterekkel: 
            //pelda_argumentumok.exe hello 
        }      

        public static void TombokReferenciakbol()
        {
            //a tömb példányosítása még 
            //nem példányosítja az elemeket! 
            var tomb = new Demo[4];

            tomb[0] = new Demo("Teszt", 42);

            //Object initializer szintaxis 
            tomb[3] = new Demo()
            {
                Szoveg = "Masik",
                Szam = 11
            };

            foreach (var elem in tomb)
            {
                if (elem == null)
                {
                    Console.WriteLine("null");
                }
                else
                {
                    Console.WriteLine("{0} ; {1}", elem.Szoveg, elem.Szam);
                }
            }

            Console.ReadLine();
            //Teszt; 42
            //null
            //null
            //Masik; 11
        }

        public static void TombSegitseg()
        {
            int[] szamok = { 1, 2, 3, 4, 5, 6, 7 };

            //Visszaadja az aktuális tömb elemeinek a számát. 
            int elemSzamok = szamok.Length;

            //Visszaadja az aktuális tömb elemeinek a számát hosszú egész típusban. Akkor jön jól, ha nagyon nagy méretű tömböket szeretnénk kezelni.
            long nagySzamok = szamok.LongLength;

            //Visszaadja a tömb dimenzióinak a számát. 
            int rank = szamok.Rank;

            //További segítségek az Array osztályból

            int index = 2;
            int length = 3;
            //Visszaállítja egy tömb elemeinek értéket az alapértelmezettre.
            //Első paramétere a tömböt, második paramétere a visszaállítás kezdő indexét, utolsó paramétere pedig a visszaállítandó elemek számát adja meg. 
            Array.Clear(szamok, index, length);

            //A tömb elemeit másolja egy másik tömbbe.
            //Az első paraméter a forrástömb, a második paraméter a cél tömb, a harmadik paraméter pedig a másolandó elemek számát adja meg.
            //Másik változatában a kezdő index is megadható
            int[] masoltszamok = new int[2];
            Array.Copy(szamok, masoltszamok, 2);

            //Visszaadja, hogy az első paraméter által meghatározott tömbben a második paraméterben megadott objektum hányadik helyen szerepel.
            //A három paraméteres  változatban az utolsó paraméter a kezdőindexet határozza meg, amelytől kezdve a keresés indul. 
            //Van LastIndexOf is amely pedig az utolsó előfordulást adja meg.
            index = Array.IndexOf(szamok, 5);

            //Megfordija az elemek sorrendjét
            Array.Reverse(szamok);
            //Nem ad vissza semmit, az beadott tömböt rendezi. Forditasi hiba.
            //var reversed = Array.Reverse(szamok);

            //A tömb elemeinek sorbarendezése növekvő sorrendben
            //Egyedi osztályokat tartalmazó tömb esetén csak akkor fog működni, ha az osztály implementálja az IComparable<T> interfészt.
            Array.Sort(szamok);
        }
    }

    //demo osztály 2 adattaggal 
    public class Demo
    {
        public string Szoveg { get; set; }
        public int Szam { get; set; }

        //alapértelmezett konstruktor 
        public Demo()
        {
            Szoveg = "";
            Szam = -1;
        }

        //paraméteres konstruktor 
        public Demo(string szoveg, int szam)
        {
            Szoveg = szoveg;
            Szam = szam;
        }
    }
}

