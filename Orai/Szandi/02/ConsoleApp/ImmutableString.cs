using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ConsoleApp.ReferenciaErtek;

namespace ConsoleApp
{
    public static class ImmutableString
    {
        public static void StringBovites()
        {
            //Szövegek karakterek tömbjeként tárolódnak
            //Három külön memóriaterület fogalalódik le, egy "Ez egy szep", kettő "Ez egy szep nap" és "Ez egy szep nap!" értékkel.
            //Nem az eredetileg lefoglalt memóriaterület értéke módosul
            //Nem optimális megoldás nagy méretű, vagy sokszor elvégzett ilyen típusú műveletek esetén
            var szoveg = "Ez egy szep";
            szoveg += " nap";
            szoveg += "!";
        }

        //Jobb teljesítmény érhető el a StringBuilder segítségével.
        //Ez belsőleg a List osztályhoz hasonló struktúrával tárolja az adatokat, ami nagyságrendekkel nagyobb teljesítményt jelent.
        public static void StringOptimalisBovites()
        {
            var szoveg = "";
            var stringBuilder = new StringBuilder();
            var random = new Random();

            Console.WriteLine("100 000 db random karater összefűzése szövegbe");

            Stopwatch watch = Stopwatch.StartNew(); //algoritmusok sebességének  mérésére használható osztály
            for (int i = 0; i < 100000; i++)
            {
                szoveg += (char)random.Next(32, 255);
            }
            watch.Stop();
            var stringIdo = watch.Elapsed.TotalMilliseconds;

            watch = Stopwatch.StartNew();
            for (int i = 0; i < 1000000; i++)
            {
                stringBuilder.Append((char)random.Next(32, 255)); //Hozzáfüzés
                //AppendLine és AppendFormat metódusok is gyakran használatosak
            }
            string builderesSzoveg = stringBuilder.ToString(); //Tényleges stringé konvertálás
            watch.Stop();
            var stringBuilderIdo = watch.Elapsed.TotalMilliseconds;

            Console.WriteLine("Eddig tartott String-el: {0} ms", stringIdo);
            Console.WriteLine("Eddig tartott StringBuilder-el: {0} ms", stringBuilderIdo);
            Console.ReadKey();

            //100 000 db random karater összefuzése szövegbe
            //Eddig tartott String-el: 6429,7839 ms
            //Eddig tartott StringBuilder-el: 31,9724 ms
        }
    }
}
