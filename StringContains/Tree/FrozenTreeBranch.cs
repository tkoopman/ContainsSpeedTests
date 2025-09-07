using System.Collections.Frozen;

namespace StringContains.Tree
{
    public readonly struct FrozenTreeBranch (int keySize, FrozenSet<int>? leafs, FrozenDictionary<string, FrozenTreeBranch>? branches)
    {
        public FrozenDictionary<string, FrozenTreeBranch>? Branches { get; } = branches;
        public int KeySize { get; } = keySize;
        public FrozenSet<int>? Leafs { get; } = leafs;

        public int Count()
        {
            int c = Leafs?.Count ?? 0;

            if (Branches is null)
                return c;

            foreach(var branch in Branches)
            {
                c += branch.Value.Count();
            }

            return c;
        }

        public IEnumerable<int> FindStartingWith (string input, int startAt)
        {
            if (Leafs is not null)
            {
                foreach (int i in Leafs)
                    yield return i;
            }

            int endAt = startAt + KeySize;

            if (Branches is not null && input.Length >= endAt && Branches.TryGetValue(input[startAt..endAt], out var child))
            {
                foreach (int i in child.FindStartingWith(input, endAt))
                    yield return i;
            }
        }
    }
}