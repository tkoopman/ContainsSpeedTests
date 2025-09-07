using System.Collections.Frozen;

namespace StringContains.TreeDeDup
{
    internal sealed class TreeBranch (int depth)
    {
        public Dictionary<char, TreeBranch> Branches { get; private set; } = [];
        public int Depth { get; } = depth;
        public TreeEntry? Entry { get; private set; }

        public void Add (string key, TreeEntry entry)
        {
            if (key.Length == Depth)
            {
                Entry = entry;
                return;
            }

            char c = key[Depth];
            if (!Branches.TryGetValue(c, out var branch))
            {
                branch = new(Depth + 1);
                Branches.Add(c, branch);
            }

            branch.Add(key, entry);
        }

        public FrozenTreeBranch Optimize ()
        {
            if (Branches.Count == 0)
                return new FrozenTreeBranch(-1, Entry, null);

            Dictionary<string, FrozenTreeBranch> branches = [];
            bool anyLeafs = false;
            int keySize = 0;
            foreach (var (key, branch) in Branches)
            {
                var o = branch.Optimize();
                branches.Add(key.ToString(), o);
                anyLeafs |= o.Entry is not null;
                keySize = keySize switch
                {
                    -1 => keySize,
                    0 => o.KeySize,
                    _ => keySize == o.KeySize ? keySize : -1,
                };
            }

            if (anyLeafs || keySize == -1)
                // Can not combine branches as a branch contained leafs
                return new FrozenTreeBranch(1, Entry, branches.ToFrozenDictionary());

            keySize++;
            Dictionary<string, FrozenTreeBranch> newBranches = [];
            foreach (var (key, branch) in branches)
            {
                foreach (var (key2, branch2) in branch.Branches!)
                    newBranches.Add($"{key}{key2}", branch2);
            }

            return new FrozenTreeBranch(keySize, Entry, newBranches.ToFrozenDictionary());
        }
    }
}