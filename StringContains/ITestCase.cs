using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringContains
{
    public interface ITestCase
    {
        public void Load (string[] searchFor);
        public int FindAll (string str);
    }
}
