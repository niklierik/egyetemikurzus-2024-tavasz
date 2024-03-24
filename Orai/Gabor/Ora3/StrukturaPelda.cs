using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ora3
{
    //stuktúra maximum 16 byte 
    internal struct StrukturaPelda :
        IComparable<StrukturaPelda>, IEquatable<StrukturaPelda>
    {
        public int X { get; set; }
        public int Y { get; set; }

        public int CompareTo(StrukturaPelda other)
        {
            //-1
            //1
            //0
            throw new NotImplementedException();
        }

        public override bool Equals(object? obj)
        {
            return obj is StrukturaPelda pelda && Equals(pelda);
        }

        public bool Equals(StrukturaPelda other)
        {
            return X == other.X &&
                   Y == other.Y;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }

        public static bool operator ==(StrukturaPelda left, StrukturaPelda right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(StrukturaPelda left, StrukturaPelda right)
        {
            return !(left == right);
        }
    }
}
