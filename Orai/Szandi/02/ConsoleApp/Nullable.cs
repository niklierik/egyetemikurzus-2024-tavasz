using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    public class Nullable
    {
        public static void NullablePelda()
        {
            string s = null;
            //Ez futási időben NullReferenceException kivételt fog generálni, mert s értéke null. 
            //Figyeljük meg, hogy a csprojban ki van kappcsolva a NullableReferenceTypes
            Console.WriteLine(s.Length);
        }
    }
}
