using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using StringContains.TreeDeDup;

namespace StringContains
{
    public class TreeDeDupTest : ITestCase
    {
        private FrozenTreeBranch _branch;

        public int FindAll (string str)
        {
            int c = _branch.FindAll(str);

            return c;
        }

        public void Load (string[] searchFor)
        {
            BaseLineDeDup deDup = new();
            deDup.Load (searchFor);

            _branch = Load(deDup.SearchFor)!.Value;
        }

        public static FrozenTreeBranch? Load (List<BaseLineDeDup.WordEntry> wordEntries)
        {
            var result = new TreeBranch(0);
            foreach (var w in wordEntries)
            {
                var wb = w.Words.Count > 0 ? Load(w.Words) : null;
                result.Add(w.Word, new TreeEntry(w.Word, w.Ints, wb));
            }

            return result.Optimize();
        }
    }
}
