using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringContains
{
    public class VowelFilterBaseLine : ITestCase
    {
        public Dictionary<byte, List<string>> SearchFor = [];

        public static byte GetKey (string str)
        {
            if (string.IsNullOrWhiteSpace(str))
                return 0;

            byte result = 0;
            if (str.Contains('a'))
                result = 0b00001;

            if (str.Contains('e'))
                result |= 0b00010;

            if (str.Contains('i'))
                result |= 0b00100;

            if (str.Contains('o'))
                result |= 0b01000;

            if (str.Contains('u'))
                result |= 0b10000;

            return result;
        }

        public void Load (string[] searchFor)
        {
            foreach (string str in searchFor)
            {
                byte key = GetKey (str);

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
            byte key = GetKey(str);
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
