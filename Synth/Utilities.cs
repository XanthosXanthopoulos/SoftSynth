using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synth
{
    public class Utilities
    {
        public static int Pow(int x, int y)
        {
            int result = 1;
            while (y > 0)
            {
                result *= x;
                --y;
            }

            return result;
        }
    }
}
