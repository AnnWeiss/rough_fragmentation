using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace App1
{
    class Pair : IEquatable<Pair>
    {
        public int IndexPoint1 { get; set; }
        public int IndexPoint2 { get; set; }

        public Pair(int p1, int p2)
        {
            IndexPoint1 = p1;
            IndexPoint2 = p2;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Pair);
        }

        public bool Equals(Pair other)
        {
            return other != null &&
                   (IndexPoint1 == other.IndexPoint1 &&
                   IndexPoint2 == other.IndexPoint2) ||
                   (IndexPoint1 == other.IndexPoint2 &&
                   IndexPoint2 == other.IndexPoint1);
        }

        public override int GetHashCode()
        {
            int hashCode = -1555726615;
            hashCode = hashCode * -1521134295 + IndexPoint1.GetHashCode();
            hashCode = hashCode * -1521134295 + IndexPoint2.GetHashCode();
            return hashCode;
        }
    }

    
}
