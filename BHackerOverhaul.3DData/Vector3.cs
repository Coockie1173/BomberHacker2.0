using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHackerOverhaul._3DData
{
    public class Vector3
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public void RoundValues(int Decimals)
        {
            double Buf = Math.Round(X, Decimals);
            X = Convert.ToSingle(Buf);
            Buf = Math.Round(Y, Decimals);
            Y = Convert.ToSingle(Buf);
            Buf = Math.Round(Z, Decimals);
            Z = Convert.ToSingle(Buf);
        }

        public override string ToString()
        {
            return X.ToString() + " " + Y.ToString() + " " + Z.ToString();
        }
    }
}
