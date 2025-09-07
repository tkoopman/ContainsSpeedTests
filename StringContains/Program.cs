using System.Diagnostics;
using System.Threading;

using StringContains;
using StringContains.List;

class Program
{
    private readonly static string[] searchIn = File.ReadAllLines("SearchIn.txt");

    private readonly static TimeSpan TimePerRun = TimeSpan.FromSeconds(3);
    private readonly static int[] InstanceCounts = [1, 2, 3, 4, 5];
    private readonly static int[] MinWords = [1, 3];
    private readonly static Type[] TestCaseTypes =
        [
            typeof(BaseLine),
            typeof(TreeTest),
            //typeof(TreePlusTest),
            //typeof(ListSearcher),
            //typeof(VowelFilterBaseLine),
            //typeof(LetterFilterBaseLine),
            typeof(BaseLineDeDup),
            //typeof(TreeDeDupTest),
        ];
    private readonly static int[] ValidateList = [0, 100, 1000, 10000, 100000];

    private static ITestCase create (Type type) => (ITestCase)Activator.CreateInstance(type)!;

    public static void Main (string[] _)
    {
        Console.WriteLine($"{searchIn.Length:N0} entries loaded to search in");
        List<(string name, int mwl, int instances, TimeSpan timeToLoad, int count, int instanceAvg, long found)> runs = [];



        if (!validateMethod(ValidateList))
            return;

        foreach (var testCaseType in TestCaseTypes)
        {
            foreach (int threads in InstanceCounts)
            {
                foreach (int min in MinWords)
                    runs.Add(runMultipleWithTimeout(create(testCaseType), min, threads));
            }
        }

        runs.Sort((l, r) => l.instanceAvg.CompareTo(r.instanceAvg));

        Console.WriteLine($"{"Name",20} | M | I | {"Load In",7} | {"Searches Performed",18} | {"Search PSPI",11} | {"Found",18} | {"Found/Performed",15} |");
        foreach (var (name, mwl, instances, timeToLoad, count, instanceAvg, found) in runs)
            Console.WriteLine($"{name,20} | {mwl,1} | {instances,1} | {timeToLoad.TotalSeconds,7:N2} | {count,18:N0} | {instanceAvg,11:N0} | {found,18:N0} | {found / count,15:N0} |");
    }

    private static bool validateMethod (int[] indexes)
    {
        int[] results = new int[indexes.Length];

        var testCases = TestCaseTypes.Select(create).ToArray();

        foreach (var testCase in testCases)
            testCase.Load(File.ReadAllLines("SearchForMin1.txt"));

        bool failed = false;

        for (int i = 0; i < indexes.Length; i++)
        {
            int[] r = new int[testCases.Length];
            int c = 0;
            foreach (var testCase in testCases)
            {
                r[c++] = testCase.FindAll(searchIn[indexes[i]]);
            }

            if (r.Any(x => x != r[0]))
            {
                failed = true;
                Console.WriteLine($"Validation failed on {i}");

                c = 0;
                foreach (var testCase in testCases)
                {
                    Console.WriteLine($"  {testCase.GetType().Name} = {r[c++]:N0}");
                }
            }
        }

        return !failed;
    }

    private static (string name, int mwl, int instances, TimeSpan timeToLoad, int count, int instanceAvg, long found) runMultipleWithTimeout (ITestCase testCase, int mwl, int instances)
    {
        string name = testCase.GetType().Name;
        var sw = new Stopwatch();
        sw.Start();
        testCase.Load(File.ReadAllLines($"SearchForMin{mwl}.txt"));
        sw.Stop();
        Console.WriteLine($"{name} x {instances} (MWL: {mwl}) loaded in {sw.Elapsed}");

        // Define the cancellation token.
        var source = new CancellationTokenSource();
        var token = source.Token;

        List<Task<(int, long)>> tasks = [];
        for (int i = 0; i < instances; i++)
        {
            var t = new Task<(int, long)>(() => performSearches(testCase, i, token));
            tasks.Add(t);
            t.Start();
        }

        Thread.Sleep(TimePerRun);
        source.Cancel();

        int count = 0;
        long found = 0;
        foreach (var task in tasks)
        {
            task.Wait();

            var (c, f) = task.Result;
            count += c;
            found += f;
        }

        Console.WriteLine($"{name} x {instances} searched {count:N0} lines in {TimePerRun}. Found {found:N0} results.");
        Console.WriteLine();

        return (name, mwl, instances, sw.Elapsed, count, count / instances / (int)TimePerRun.TotalSeconds, found);
    }

    private static (int, long) performSearches (ITestCase test, int instance, CancellationToken token)
    {
        int startAt = instance*10000;
        int count = 0;
        long found = 0;
        while (true)
        {
            for (int i = startAt; i < searchIn.Length; i++)
            {
                if (token.IsCancellationRequested)
                    return (count, found);
                found += test.FindAll(searchIn[i]);
                count++;
            }

            startAt = 0;
        }

    }
}
