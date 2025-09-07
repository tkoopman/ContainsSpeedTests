<#+
public class Words
{
    private readonly int count;
    private readonly int MinWordSize;
    private readonly Random random;
    private readonly Random randomCase;

    public Words (int minWordSize)
    {
        Lines = File.ReadAllLines("data/words_alpha.txt");
        count = Lines.Length;
        random = new Random(13);
        randomCase = new Random(17);
        MinWordSize = minWordSize;
    }

    public string[] Lines { get; }

    public string RandomWord => FirstUpper(RandomWordLower);

    public string RandomWordAndCase => randomCase.Next(4) switch
    {
        0 => RandomWord,
        1 => RandomWordLower.ToUpperInvariant(),
        2 => RandomWordLower,
        3 => RandomizeCase(RandomWordLower),
        _ => RandomWord,
    };

    public string RandomWordLower
    {
        get
        {
            while (true)
            {
                string word = Lines[random.Next(count)];
                if (word.Length >= MinWordSize)
                    return word;
            }
        }
    }

    public string RandomWords => random.Next(4) switch
    {
        0 => RandomWord,
        1 => $"{RandomWord}{RandomWord}",
        2 => $"{RandomWord} {RandomWord}",
        3 => $"{RandomWord}{RandomWord}{RandomWord}",
        _ => $"{RandomWord} {RandomWord} {RandomWord}",
    };

    public string RandomWordsAndCase => random.Next(4) switch
    {
        0 => RandomWordAndCase,
        1 => $"{RandomWordAndCase}{RandomWordAndCase}",
        2 => $"{RandomWordAndCase} {RandomWordAndCase}",
        3 => $"{RandomWordAndCase}{RandomWordAndCase}{RandomWordAndCase}",
        _ => $"{RandomWordAndCase} {RandomWordAndCase} {RandomWordAndCase}",
    };

    public string RandomWordsLower => random.Next(4) switch
    {
        0 => RandomWordLower,
        1 => $"{RandomWordLower}{RandomWordLower}",
        2 => $"{RandomWordLower} {RandomWordLower}",
        3 => $"{RandomWordLower}{RandomWordLower}{RandomWordLower}",
        _ => $"{RandomWordLower} {RandomWordLower} {RandomWordLower}",
    };

    public static string FirstUpper (string word) => word.Length > 1 ? word[0].ToString().ToUpperInvariant() + word.Substring(1) : word.ToUpperInvariant();

    public string RandomizeCase (string str)
    {
        char[] chars = new char[str.Length];
        for (int i = 0; i < chars.Length; i++)
        {
            chars[i] = randomCase.Next(4) switch
            {
                0 => str[i],
                1 => char.ToLowerInvariant(str[i]),
                2 => char.ToUpperInvariant(str[i]),
                _ => str[i]
            };
        }

        return new(chars);
    }
}
#>