using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppNullable
{
    public static class Nullable
    {
        public static void NullablePelda()
        {
            string s = null;
            //Figyeljük meg, hogy a csprojban enable a NullableReferenceTypes
            //Warningot kapunk, hogy lehetséges null reference
            Console.WriteLine(s.Length);

            //Ami lehet null, azt explicit jelöljük, mert a vezető hiba ok a NullReferenceException
        }
    }
}
