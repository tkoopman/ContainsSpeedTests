using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

namespace StringContains.List
{
    public class ListSearcher : ITestCase
    {
        public readonly List<(string, int)> list = [];
        public int MinLen { get; private set; } = int.MaxValue;

        public int FindAll (string str)
        {
            HashSet<int> results = [];

            int lastI = str.Length - MinLen;

            for (int i = 0; i < lastI; i++)
            {
                foreach (int r in StartsWith(str, i))
                    _ = results.Add(r);
            }

            return results.Count;
        }

        public IEnumerable<int> StartsWith (string str, int startAt)
        {
            str = str[startAt..];

            int i = list.BinarySearch((str, 0), Comparer<(string, int)>.Create((l, r) => l.Item1.CompareTo(r.Item1)));

            if (i >= 0)
            {
                // Match found
                yield return list[i].Item2;

                // Check forward for more matches
                for (int x = i + 1; x < list.Count; x++)
                {
                    var y = list[x];
                    if (CompareMinLength(str, y.Item1) < 0)
                        break;

                    if (str.StartsWith(y.Item1))
                        yield return y.Item2;
                }
            }

            if (i < 0)
                i = ~i;
            // Check Backwards
            for (int x = i - 1; x >= 0; x--)
            {
                var y = list[x];
                if (CompareMinLength(str, y.Item1) > 0)
                    break;

                if (str.StartsWith(y.Item1))
                    yield return y.Item2;
            }

        }

        public int CompareMinLength (string? left, string? right)
        {
            if (left is null && right is null)
                return 0;

            if (left is null || right is null)
                return left is null ? -1 : 1;

            for (int i = 0; i < MinLen; i++)
            {
                char l = left[i];
                char r = right[i];
                if (l != r)
                    return l < r ? -1 : 1;
            }

            return 0;
        }

        public void Load (string[] searchFor)
        {
            int c = 0;
            foreach (string str in searchFor)
            {
                MinLen = int.Min(MinLen, str.Length);
                list.Add((str, c++));
            }

            list.Sort((l, r) => l.Item1.CompareTo(r.Item1));
        }
    }
}
