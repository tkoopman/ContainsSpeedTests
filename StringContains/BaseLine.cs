using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using StringContains;

namespace StringContains
{
    public class BaseLine : ITestCase
    {
        public string[] SearchFor;

        public void Load (string[] searchFor) => SearchFor = [.. searchFor];

        public int FindAll(string str)
        {
            int c = 0;
            foreach (string s in SearchFor)
            {
                if (str.Contains(s))
                {
                    //Console.WriteLine($"Baseline found {s} in {str}");
                    c++;
                }
            }

            return c;
        }

    }
}
