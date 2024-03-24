using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    public class AlapTipusok
    {
        //10-es számrendszerben megadott egész 16 biten.
        short kicsi_egesz = 8;

        //10-es számrendszerben megadott egész.
        //Az int minden platformon 32 bitet foglal el.
        int egesz_szam = 42;

        //10-es számrendszerben nagyobb megadott egész.
        long nagy_szam = 4300000000000000000;

        //az f jelzés jelöli a fordító számára, hogy ez egy float típus.
        float lebegopontos = 3.14f;

        //double esetén nem kell külön jelölni.
        double d = 1124.333;

        //fordítási hibát eredményez mert alapjáraton double ha nincs lebegőpontos jelölés.
        //float fhibapelda = 3.14;

        //egész szám hexadecimális formában.
        int hexa = 0xff;

        //hosszú egész oktális formátumban.
        long okta = 07123235;

        //decimal típus esetén m betű jelzi, hogy a szám egy decimal típus .
        decimal penz = 1224.3m;

        //var kulcszsó esetén a fordító kitalálja a jobb oldalból a típust. Fordításnál már eldől.
        //A fordító a változó típusának string-et fog adni. Nem használható csak local variable ként. Forditási hiba.
        //Ezt a kulcsszót osztályban elhelyezkedő tag változó definiálására nem használhatjuk, ott minden esetben egyértelműen kifejezve kell jelölni annak a típusát.
        //var valtozo = "Ez egy szöveg";

        //futtatás közben fog eldőlni a típus.
        //az eredmény típus szöveg lesz. A 44 szöveggé fog konvertálódni.
        dynamic dinamikus = "Ez egy " + 44;

        //Ha a szövegünket @ karakterrel kezdjük, akkor szó szerint kerül értelmezésre, vagyis a string fordításból eltávolítja a vezérlő karakter értelmezést.
        string szoszerint = @"Ez egy speciális\különlege's \t string";

        //Fontosabb speckó verérlők:
        //   \b – Backspace. Törli az előtte szereplő karaktert.
        //   \t – Tab.
        //   \n – Új sor
        //   \r – Sor elejére helyezi a kurzort
        //   \’ – Aposztróf karakter elhelyezése
        //   \” – Idézőjel karakter elhelyezése
        //   \\ – Visszafelé per jel

        public static void InterpolationPelda()
        {
            //Az interpolált szövegek a $ karakterrel kezdődnek
            int valtozo = 13;
            string interpolalt = $"{nameof(valtozo)} értéke: {valtozo}";

            //az x hexa, a 2 pedig, hogy 2db számjegyet szeretnénk
            //a kimenete: "valtozo értéke hexadecimálisan: 0d"
            string interpolalt2 = $"{nameof(valtozo)} értéke hexadecimálisan: {valtozo:x2}";

            //Sok esetben már fordítási időben kigenerálja a szöveget, ha lehetősége van rá, illetve csak a legvégső esetben hívja a String.Format metódust.
            //Ebben az esetben, mivel a nev változó a végén helyezkedik el a szövegnek, így valójában a generált kód egy szimpla + operátoros összefűzés lesz, ami jelen esetben gyorsabb, mint egy komplex String.Format hívás.
            //Ha viszont a nev konstans (const) módosító, akkor már fordítási időben a hello értéke felveszi a "Hello World" szöveget, anélkül, hogy futási időben bármiféle metódushívás történne.
            string nev = "World";
            string hello = $"Hello {nev}!";
        }

        public void IntLeszBelole()
        {
            //C# esetén, ha aritmetikai műveletet végzünk kettő, az int típusnál kevesebb bitszámmal rendelkező változón, akkor az eredmény mindig Int lesz.
            //Túlcsordulásvédelem
            var osszeadas = kicsi_egesz + kicsi_egesz;
            //Nem enegedi beleerőszakolni kisebbe. Fordítási hiba.
            //short beleferaz = kicsi_egesz + kicsi_egesz;
        }

        //Fordító túlcsordulás ügyileg ellenőrzi a számokat  abban az esetben, ha a szám értékéről már fordítási időben megállapítható, hogy túl fog csordulni.
        //Ellenörzés kikapcsolása fordításnál az unchecked kód blokkban lehetséges.
        //Ez az egész ellenörzés csak a fordítási időben meghatározható számokra vonatkozik. Ha futtatási időben is szeretnénk ezt az ellenőrzést érvényesíteni, akkor checked kód blokkban kell elvégeznünk a műveletünket.
        public static void BeleferAz()
        {
            unchecked
            {
                //nem okoz problémát, mert unchecked blokk 
                int ertek = int.MaxValue + 100;
                checked
                {
                    //fordítási hibát okoz 
                    //int ertek2 = int.MaxValue +100; 
                }
                Console.WriteLine(ertek);
                Console.ReadKey();
            }
        }
    }
}
