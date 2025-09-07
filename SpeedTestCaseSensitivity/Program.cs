using System.Diagnostics;

internal class Program
{
    private static readonly TimeSpan TimePerRun = TimeSpan.FromSeconds(10);

    private static readonly string[] SearchFor = File.ReadAllLines("SearchForMin1.txt");
    private static readonly string[] RC_SearchFor = File.ReadAllLines("RC_SearchForMin1.txt");
    private static readonly string[] SearchIn = File.ReadAllLines("SearchIn.txt");
    private static readonly string[] RC_SearchIn = File.ReadAllLines("RC_SearchIn.txt");

    private static readonly (string Name, StringComparer Comparer, string[] SearchFor, string[] SearchIn)[] TestCases = [
        (nameof(StringComparer.Ordinal), StringComparer.Ordinal, SearchFor, SearchIn),
        (nameof(StringComparer.OrdinalIgnoreCase), StringComparer.OrdinalIgnoreCase, RC_SearchFor, RC_SearchIn),
        (nameof(StringComparer.CurrentCulture), StringComparer.CurrentCulture, SearchFor, SearchIn),
        (nameof(StringComparer.CurrentCultureIgnoreCase), StringComparer.CurrentCultureIgnoreCase, RC_SearchFor, RC_SearchIn),
        (nameof(StringComparer.InvariantCulture), StringComparer.InvariantCulture, SearchFor, SearchIn),
        (nameof(StringComparer.InvariantCultureIgnoreCase), StringComparer.InvariantCultureIgnoreCase, RC_SearchFor, RC_SearchIn),
        ];

    public static void Main (string[] _)
    {
        Thread.Sleep(TimePerRun);

        foreach (var (name, comparer, searchFor, searchIn) in TestCases)
        {
            int c = runMultipleWithTimeout(searchFor, searchIn, comparer);
            Console.WriteLine($"{name}: {c:N0}");
        }
    }

    private static int runMultipleWithTimeout (string[] lookFor, string[] searchIn, StringComparer comparer)
    {
        var sw = new Stopwatch();

        // Define the cancellation token.
        var source = new CancellationTokenSource();
        var token = source.Token;

        var task = new Task<int>(() => performSearches(lookFor, searchIn, comparer, token));
        task.Start();

        Thread.Sleep(TimePerRun);
        source.Cancel();

        task.Wait();

        return task.Result;
    }

    private static int performSearches (string[] lookFor, string[] searchIn, StringComparer comparer, CancellationToken token)
    {
        int count = 0;
        int found = 0;

        while (true)
        {
            foreach (string si in searchIn)
            {
                foreach (string lf in lookFor)
                {
                    if (token.IsCancellationRequested)
                        return count;

                    if (searchIn.Contains(lf, comparer))
                        found++;

                    count++;
                }
            }
        }
    }
}