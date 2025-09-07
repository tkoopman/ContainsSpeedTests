using StringContains.Tree;

namespace StringContains
{
    public class TreeTest : ITestCase
    {
        private FrozenTreeBranch _branch;

        public int FindAll (string str)
        {
            HashSet<int> c = [];

            while (str.Length >= _branch.KeySize)
            {
                foreach (int i in _branch.FindStartingWith(str, 0))
                    _ = c.Add(i);
                str = str[1..];
            }

            return c.Count;
        }

        public void Load (string[] searchFor)
        {
            var tree = new TreeBranch(0);
            int count = 0;
            foreach (string item in searchFor)
                tree.Add(item, count++);

            _branch = tree.Optimize();
        }
    }
}