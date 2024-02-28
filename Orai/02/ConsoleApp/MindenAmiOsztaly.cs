using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    //Lathatosagi módosítók
    //private - protected - public szokás szerint
    //Érdekesség a "private protected" amely módosítóval megjelölt elemek protected mezőként viselkednek, de csak egy adott szerelvényen belül.


    //internal láthatóság
    //Ez a láthatósági szint C# specifikus. 
    //Ezzel a kulcsszóval megjelölt elemek csak az adott szerelvényen belül lesznek láthatóak.
    //A szerelvényen kívül úgy viselkednek, mint ha privát adattagok lennének, vagyis nem lehet hozzájuk férni
    //Szerelvényen belül publikus láthatósággal rendelkeznek.
    //Lehet még "protected internal" ami szerelvenyen belül protected nem public
    internal static class MindenAmiOsztaly
    {
        public static void Peldak()
        {
            var a = new KonstruktorPelda();
            var b = new KonstruktorPelda(33);
            a.Kiir();
            b.Kiir();
            Console.ReadKey();
            //42 
            //33


            var a2 = new KonstruktorLancPelda();
            var b2 = new KonstruktorLancPelda(33);
            a2.Kiir();
            b2.Kiir();
            Console.ReadKey();
            //42 
            //33


            var a3 = new ThisPelda(1, 2);
            for (int i = 0; i < 5; i++)
            {
                PeldaStaticKonstruktor.TesztMetodus();
            }
            Console.ReadKey();
            //Még ennyi van hátra: 5 
            //Még ennyi van hátra: 4
            //Még ennyi van hátra: 3
            //Még ennyi van hátra: 2
            //Még ennyi van hátra: 1


            var pelda = new GetSetMetodus();
            pelda.SetAdattag(48000);
            Console.WriteLine(pelda.GetAdattag());
            pelda.SetAdattag(1200000);
            Console.WriteLine(pelda.GetAdattag());
            Console.ReadKey();
            //48000 
            //60000

            //Inites példányosítás 
            Point point = new Point
            {
                X = 1,
                Y = 2
            };

            //New operátor
            ThisPelda regimodivagyokbruhu = new ThisPelda(1, 2);
            ThisPelda menoujvagyok = new(1, 2);

            //Requred paraméterek megadása nélkül nem is lehet inicializálni, hiába a paraméter nélküli konstruktor. Fordítási hiba.           
            //var Szandi = new Szemely();
            //Object initializer syntax
            var Szandi = new Szemely()
            {
                Keresztnev = "Szandi",
                VezetekNev = "Apró"
            };
        }

    }

    internal class KonstruktorPelda
    {
        private int _szam;

        //paraméter nélküli konstruktor 
        public  KonstruktorPelda()
        {
            _szam = 42;
        }

        //paraméteres konstruktor 
        public KonstruktorPelda(int szam)
        {
            _szam = szam;
        }

        public void Kiir()
        {
            Console.WriteLine(_szam);
        }
    }

    internal class KonstruktorLancPelda
    {
        private int _szam;

        //paraméter nélküli konstruktor 
        public KonstruktorLancPelda() : this(42)
        {
            //Ha ide kódot tennénk, akkor az a Paraméteres 
            //konstruktor futása után futna le 
        }

        //paraméteres konstruktor 
        public KonstruktorLancPelda(int szam)
        {
            _szam = szam;
        }

        public void Kiir()
        {
            Console.WriteLine(_szam);
        }
    }

    internal class ThisPelda
    {
        private int a;
        private int b;
        public ThisPelda(int a, int b)
        {
            //A fordító azt fogja feltételezni, hogy a paraméterként kapott a változót egyenlővé kell tennie a paraméterként kapott a változóval.
            a = a;
            //Megoldás rá a this kulcsszó 
            this.a = a;
            this.b = b;
        }
    }

    internal class PeldaStaticKonstruktor
    {
        private static int Szam;
        //statikus konstruktor 
        static PeldaStaticKonstruktor()
        {
            Szam = 5;
        }
        public static void TesztMetodus()
        {
            Console.WriteLine("Még ennyi van hátra: {0}", Szam);
            --Szam;
        }
    }

    internal class GetSetMetodus
    {
        //backing field
        //backing field-et nem publikálunk ki soha!
        private int _adattag;

        public int GetAdattag()
        {
            return _adattag;
        }

        public void SetAdattag(int adat)
        {
            if (adat > 60000) _adattag = 60000;
            else _adattag = adat;
        }
    }

    //Elveszős végtelen parmaméteres konstruktorok helyett init
    internal record Point
    {
        public double X { get; init; }
        public double Y { get; init; }
    }

    internal sealed class BelolemNemOrokolsz
    {
        private int _adattag;

        public BelolemNemOrokolsz(int adattag)
        {
            _adattag = adattag;
        }
    }

    internal class A
    {
        private int _adattag;

        public A(int adattag)
        {
            _adattag = adattag;
        }
    }

    //B öröklődik A-ból
    internal class B :A
    {
        public B(int adattag) :base(adattag)
        {
        }
    }

    //C öröklődik selaedből
    //Fordítási hiba
    //internal class C : BelolemNemOrokolsz
    //{
    //    public C(int adattag) : base(adattag)
    //    {
    //    }
    //}

    public class Szemely
    {
        public Szemely() { }
     
        public required string VezetekNev { get; init; }
        public required string Keresztnev { get; init; }

        public int? Kor { get; set; }
    }
}

