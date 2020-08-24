using System;

namespace Technique
{
    public class Code
    {
        public bool[] bools;
        public int[] ints;
        public int length;

        public Code(bool[] bools, int[] ints)
        {
            this.bools = bools;
            this.ints = ints;
            length = ints.Length;
        }
    }
}
