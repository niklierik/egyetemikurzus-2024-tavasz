using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    public static class ReferenciaErtek
    {
        public static void Pelda1()
        {
            //nem kell hívni konstruktort 
            Pont pont;

            //de cserébe minden tagot inicializálni kell értékkel 
            //ha ez elmarad, akkor fordítási hibát kapunk, ha le akarjuk kérni később az értékét 
                 //pl kommenteld ki a pont.x = 23; sort 
            pont.x = 23;
            pont.y = 11;

            Console.WriteLine("x: {0}, y: {1}", pont.x, pont.y);
            Console.ReadKey();
        }
     
        //Érték típusú hívásnál a hívó érték másolatát kapja
        //Csak lokális változás
        public static void ErtekatadoPelda(double ertek)
        {
            ertek = 2.1;
        }
        //Referencia típus hívásnál ténylegesen a változó kerül átadásra
        //Megváltozik az érték
        public static void ReferenciaPelda(Osztaly referencia)
        {
            referencia.Ertek = 2.1;
        }

        //A metódus meg tudja változtatni a paraméterül kapott referencia típusú objektumot, viszont a referenciát nem tudja egy másik objektumhoz rendelni, erre lesz majd jó a ref módosító.
        //Csak lokális változás 
        public static void KomplikaltReferenciaPelda(Osztaly osztaly)
        {
            osztaly = new Osztaly(99);
        }

        //Referenciát másik objektumhoz rendeli
        //Megváltozik az érték 
        public static void RefReferenciaPelda(ref Osztaly osztaly)
        {
            osztaly = new Osztaly(100);
        }

        //A ref kulcsszó értéktípusoknál is működik
        //Megváltozik az érték 
        static void RefErtekPelda(ref double ertek)
        {
            ertek = 2.1;
        }

        //Ha a cél több érték visszaadása, akkor ref helyett out
        //Itt a return utasítás előtt értéket kell adni a out módosítóval megjelölt változónak. Elmaradása fordítási hiba.
        public static bool OutPelda(out int variable)
        {
            //hibát eredényez: 
            //variable = variable + 1;  
            variable = 41;
            return true;
        }

        //A ref és az out érdekessége, hogy metódusok polimorfizmusánál két metódus azonos.
        public static int Refes(ref int parameter)
        {
            parameter *= 2;
            return parameter + 1;
        }
        //A két definíció azonos lenne fordító szemszögéből, ha a fgv neve ugyanaz lenne.
        //Próbáld meg úgyanugy nevezni. Fordítási hiba.
        public static int Outos(out int parameter)
        {
            parameter = 41;
            return 41 + 1;
        }

        public static void Pelda2()
        {
            Console.WriteLine("Érték típus példa");
            Console.WriteLine();

            double ertek = 3.14;
            Console.WriteLine("Függvényhívás előtt az ertek: {0}", ertek);
            ErtekatadoPelda(ertek);
            Console.WriteLine("Függvényhívás után az ertek: {0}", ertek);

            //Függvényhívás előtt az ertek: 3,14
            //Függvényhívás után az ertek: 3,14

            Console.WriteLine();
            Console.WriteLine("Referencia típus példa");
            Console.WriteLine();

            Osztaly o = new Osztaly(3.14);
            Console.WriteLine("Függvényhívás előtt az ertek: {0}", o.Ertek);
            ReferenciaPelda(o);
            Console.WriteLine("Függvényhívás után az ertek: {0}", o.Ertek);
            Console.ReadKey();

            //Függvényhívás előtt az ertek: 3,14
            //Függvényhívás után az ertek: 2,1

            Console.WriteLine();
            Console.WriteLine("Komplikalt referencia típus példa");
            Console.WriteLine();

            var pelda = new Osztaly(44);
            Console.WriteLine("Függvényhívás előtt az ertek: {0}", pelda.Ertek);
            KomplikaltReferenciaPelda(pelda);
            Console.WriteLine("Függvényhívás előtt az ertek: {0}", pelda.Ertek);
            Console.ReadKey();

            //Függvényhívás előtt az ertek: 44
            //Függvényhívás után az ertek: 44

            Console.WriteLine();
            Console.WriteLine("Ref kulcsszavas referencia típus példa");
            Console.WriteLine();

            var refpelda = new Osztaly(44);
            Console.WriteLine("Függvényhívás előtt az ertek: {0}", refpelda.Ertek);
            //Hívásnál is jelöljük a referencia átadását
            RefReferenciaPelda(ref refpelda);
            Console.WriteLine("Függvényhívás előtt az ertek: {0}", refpelda.Ertek);
            Console.ReadKey();

            //Függvényhívás előtt az ertek: 44
            //Függvényhívás után az ertek: 100

            Console.WriteLine();
            Console.WriteLine("Érték típus referenciaként példa");
            Console.WriteLine();

            double megvaltozoertek = 3.14;
            Console.WriteLine("Függvény hívás előtt az ertek: {0}", megvaltozoertek);
            RefErtekPelda(ref megvaltozoertek);
            Console.WriteLine("Függvény hívás utan az ertek: {0}", megvaltozoertek);
            Console.ReadKey();

            //Függvény hívás előtt az ertek: 3,14
            //Függvény hívás utan az ertek: 2,1

            Console.WriteLine();
            Console.WriteLine("Out példa");
            Console.WriteLine();
            int test;
            var result = OutPelda(out test);
            Console.WriteLine(result);
            Console.WriteLine(test);
            Console.ReadLine();

            //True 
            //41
        }
    }

    public struct Pont
    {
        //16 byte adat :) 
        public double x, y;
    }
    public class Osztaly
    {
        public double Ertek { get; set; }

        public Osztaly(double ertek)
        {
            Ertek = ertek;
        }
    }
}
