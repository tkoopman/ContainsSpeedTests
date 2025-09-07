using System.Collections.Frozen;

namespace StringContains.TreeDeDup
{
    public readonly struct FrozenTreeBranch
    {
        public FrozenDictionary<string, FrozenTreeBranch>? Branches { get; }
        public int KeySize { get; }
        public TreeEntry? Entry { get; }

        public FrozenTreeBranch (int keySize, TreeEntry? entry, FrozenDictionary<string, FrozenTreeBranch>? branches)
        {
            if (branches is not null && keySize == -1)
                throw new Exception();
            Branches = branches;
            KeySize = keySize;
            Entry = entry;
        }
        public int Count ()
        {
            if (Branches is not null && KeySize < 1)
                throw new Exception();

            if (Branches is null && Entry is null)
                throw new Exception();

            int c = Entry?.Ints.Count ?? 0;

            if (Branches is not null)
            {
                foreach (var branch in Branches)
                {
                    if (branch.Key.Length != KeySize)
                        throw new Exception();

                    c += branch.Value.Count();
                }
            }

            if (Entry?.Branches is not null)
            {
                c += Entry.Branches.Value.Count();
                foreach (var branch in Entry.Branches.Value.Branches)
                {
                    if (branch.Key.Length != Entry.Branches.Value.KeySize)
                        throw new Exception();
                }
            }

            return c;
        }

        public int FindAll (string str)
        {
            var ints = FindAll(str, 0, this, [], [], "");

            return ints.Count();
        }

        public static IEnumerable<int> FindAll (string str, int startAt, FrozenTreeBranch branch, HashSet<int> found, HashSet<FrozenTreeBranch> processed, string path)
        {
            if (branch.Branches is null || !processed.Add(branch))
                yield break;


            int i = startAt;

            while ((str.Length - i) >= branch.KeySize)
            {
                path = $"{path}{str[i]}";
                foreach (int result in SearchBranch(str, branch, found, processed, path, i++))
                    yield return result;
            }
        }

        private static IEnumerable<int> SearchBranch (string str, FrozenTreeBranch branch, HashSet<int> found, HashSet<FrozenTreeBranch> processed, string path, int i)
        {
            foreach (var twig in branch.FindStartingWith(str, i, path))
            {
                if (twig.Entry is not null)
                {
                    bool addedAny = false;
                    foreach (int ei in twig.Entry.Ints ?? [])
                    {
                        if (found.Add(ei))
                        {
                            addedAny = true;
                            yield return ei;
                        }
                    }

                    if (addedAny && twig.Entry.Branches.HasValue)
                    {

                        foreach (int ti in FindAll(str, 0, twig.Entry.Branches.Value, found, processed, $"{path}\\{twig.Entry.Word}\\"))
                            yield return ti;
                    }
                }

                foreach (int bi in SearchBranch(str, twig, found, processed, $"{path}>", i+branch.KeySize))
                    yield return bi;
            }
        }

        public IEnumerable<FrozenTreeBranch> FindStartingWith (string str, int startAt, string path)
        {
            int endAt = startAt + KeySize;

            if (Branches is not null && str.Length >= endAt)
            {
                string find = str[startAt..endAt];
                if (Branches.TryGetValue(find, out var child))
                    yield return child;
            }
        }
    }
}