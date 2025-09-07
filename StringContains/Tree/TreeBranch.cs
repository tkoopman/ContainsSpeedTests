using System.Collections.Frozen;

namespace StringContains.Tree
{
    public sealed class TreeBranch (int depth)
    {
        public Dictionary<char, TreeBranch> Branches { get; private set; } = [];
        public int Depth { get; } = depth;
        public HashSet<int> Leafs { get; } = [];

        public void Add (string key, int index)
        {
            if (key.Length == Depth)
            {
                _ = Leafs.Add(index);
                return;
            }

            char c = key[Depth];
            if (!Branches.TryGetValue(c, out var branch))
            {
                branch = new(Depth + 1);
                Branches.Add(c, branch);
            }

            branch.Add(key, index);
        }

        public FrozenTreeBranch Optimize ()
        {
            if (Branches.Count == 0)
                return new FrozenTreeBranch(-1, Leafs.ToFrozenSet(), null);

            Dictionary<string, FrozenTreeBranch> branches = [];
            bool anyLeafs = false;
            int keySize = 0;
            foreach (var (key, branch) in Branches)
            {
                var o = branch.Optimize();
                branches.Add(key.ToString(), o);
                anyLeafs |= o.Leafs is not null;
                keySize = keySize switch
                {
                    -1 => keySize,
                    0 => o.KeySize,
                    _ => keySize == o.KeySize ? keySize : -1,
                };
            }

            if (anyLeafs || keySize == -1)
                // Can not combine branches as a branch contained leafs
                return new FrozenTreeBranch(1, Leafs.Count == 0 ? null : Leafs.ToFrozenSet(), branches.ToFrozenDictionary());

            keySize++;
            Dictionary<string, FrozenTreeBranch> newBranches = [];
            foreach (var (key, branch) in branches)
            {
                foreach (var (key2, branch2) in branch.Branches!)
                    newBranches.Add($"{key}{key2}", branch2);
            }

            return new FrozenTreeBranch(keySize, Leafs.Count == 0 ? null : Leafs.ToFrozenSet(), newBranches.ToFrozenDictionary());
        }
    }
}