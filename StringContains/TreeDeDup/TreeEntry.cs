namespace StringContains.TreeDeDup
{
    public class TreeEntry (string word, List<int> ints, FrozenTreeBranch? branch)
    {
        public string Word { get; } = word;
        public List<int> Ints { get; } = ints;
        public FrozenTreeBranch? Branches { get; } = branch;
    }
}