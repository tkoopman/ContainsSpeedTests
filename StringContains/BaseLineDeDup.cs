namespace StringContains
{
    public class BaseLineDeDup : ITestCase
    {
        public readonly struct WordEntry (string word, List<int> ints)
        {
            public string Word { get; } = word;
            public List<int> Ints { get; } = ints;
            public List<WordEntry> Words { get; } = [];
        }

        public List<WordEntry> SearchFor = [];

        public void Load (string[] searchFor)
        {
            int c = 0;
            Dictionary<string, List<int>> tmp = [];
            foreach (string s in searchFor)
            {
                if (tmp.TryGetValue(s, out var ints))
                {
                    ints.Add(c++);
                }
                else
                {
                    ints = [c++];
                    tmp.Add(s, ints);
                }
            }

            foreach (var (k, w) in tmp)
                SearchFor.Add(new WordEntry(k, w));

            Optimize(SearchFor);
        }

        public void Optimize (List<WordEntry> words)
        {
            words.Sort((l, r) => l.Word.Length.CompareTo(r.Word.Length));

            int i = 0;
            while (i < words.Count)
            {
                var wi = words[i];
                int len = wi.Word.Length;

                int x = i+1;
                while (x < words.Count)
                {
                    var wx = words[x];

                    if (wx.Word.Length != len && wx.Word.Contains(wi.Word))
                    {
                        wi.Words.Add(wx);
                        words.RemoveAt(x);
                        continue;
                    }

                    x++;
                }

                i++;
            }

            foreach (var word in words)
            {
                if (word.Words.Count > 1)
                    Optimize(word.Words);
            }
        }

        public int FindAll (string str) => FindAll(str, SearchFor);

        public static int FindAll (string str, List<WordEntry> words)
        {
            int c = 0;
            foreach (var w in words)
            {
                if (!str.Contains(w.Word))
                    continue;

                c += w.Ints.Count;
                c += FindAll(str, w.Words);
            }

            return c;
        }
    }
}