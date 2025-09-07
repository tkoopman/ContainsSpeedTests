using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringContains
{
    public class LetterFilterBaseLine : ITestCase
    {
        public Dictionary<uint, List<string>> SearchFor = [];

        public static uint GetKey (string str)
        {
            if (string.IsNullOrWhiteSpace(str))
                return 0;

            uint result = 0;
            if (str.Contains('a'))
                result = 1 << 0;

            if (str.Contains('b'))
                result |= 1 << 1;

            if (str.Contains('c'))
                result |= 1 << 2;

            if (str.Contains('d'))
                result |= 1 << 3;

            if (str.Contains('e'))
                result |= 1 << 4;

            if (str.Contains('e'))
                result |= 1 << 5;

            if (str.Contains('f'))
                result |= 1 << 6;

            if (str.Contains('g'))
                result |= 1 << 7;

            if (str.Contains('h'))
                result |= 1 << 8;

            if (str.Contains('i'))
                result |= 1 << 9;

            if (str.Contains('j'))
                result |= 1 << 10;

            if (str.Contains('k'))
                result |= 1 << 11;

            if (str.Contains('l'))
                result |= 1 << 12;

            if (str.Contains('m'))
                result |= 1 << 13;

            if (str.Contains('n'))
                result |= 1 << 14;

            if (str.Contains('o'))
                result |= 1 << 15;

            if (str.Contains('p'))
                result |= 1 << 16;

            if (str.Contains('q'))
                result |= 1 << 17;

            if (str.Contains('r'))
                result |= 1 << 18;

            if (str.Contains('s'))
                result |= 1 << 19;

            if (str.Contains('t'))
                result |= 1 << 20;

            if (str.Contains('u'))
                result |= 1 << 21;

            if (str.Contains('v'))
                result |= 1 << 22;

            if (str.Contains('w'))
                result |= 1 << 23;

            if (str.Contains('x'))
                result |= 1 << 24;

            if (str.Contains('z'))
                result |= 1 << 25;

            return result;
        }

        public void Load (string[] searchFor)
        {
            foreach (string str in searchFor)
            {
                uint key = GetKey (str);

                if (SearchFor.TryGetValue(key, out var values))
                    values.Add(str);
                else
                {
                    values = [str];
                    SearchFor.Add(key, values);
                }
            }
        }

        public int FindAll (string str)
        {
            int c = 0;
            uint key = GetKey(str);
            foreach (var (k, values) in SearchFor)
            {
                if ((key & k) == k)
                {
                    foreach (string s in values)
                    {
                        if (str.Contains(s))
                            c++;
                    }
                }
            }

            return c;
        }

    }
}
