# C# Contains Speed Tests

## Speed Test Case Sensitivity
This was just an extra test to see how much using different StringComparer values affect performance.
Which basically indicates if you doing lots of compares against the same set of strings you should pre-load them with all the same case.

### Results
- Ordinal: 4,436
- OrdinalIgnoreCase: 4,291
- CurrentCulture: 818
- CurrentCultureIgnoreCase: 815
- InvariantCultureIgnoreCase: 813
- InvariantCulture: 806

Larger is better.

## Different methods of doing string.Contains
Here all tests were done on the same dataset that was all lower case.
Each test run was only over 3 seconds of actual searching, but that was enough to see differences.

Ground Rules:
- Case Insensitive.
- Neither list is unique. Duplicates will exist.
- Given datasets also chance that a search word will be contained within other words being searched for.
- Must index the list of words we looking for, not the strings we searching for contained words in.
- For each string, must search that list of words and return ALL matching contained words, including all duplicates.
- Strings being searched will come from records, where each record can have multiple words attached to it, so some threaded searching can assist. So will run tests with a few different number of instances.
- For this performance testing can just return count of total found including duplicates.
- All methods should return same counts for same data set to be valid. Doing check at start on a few records to do basic validation of this.

Each method was run with the following differences:
- Min length of words looking to be contained in other strings.
  - 1 & 3 (Matches my actual data set)
- Number of threads doing searches over the same dataset (each thread searching different subset of strings)
  - 1 - 5 threads

This gives total of 10 runs per method.

### Method 1: Base Line
This just uses string.Contains.

### Method 2: Base Line DeDup
DeDeup probably not correct term, but while loading the words we looking for into memory, it finds if any of the words are contained in other words.
Main search is then only done on the smallest length words, and when a match is found if that word was also found in others, those others would then be searched.
So basically reducing the total number of string.Contains calls we need to do for each string being looked for.

### Method 3: Tree
In this method I create a tree structure using Dictionaries. This does mean we are only doing Equals matches against the dictionary keys,
so for each string being searched we repeat the search removing the first char each time, giving the end result of contains.
This is what I was already doing so has a couple other optimizations, mainly dynamic key size, so not needing to always search a dictionary for a single letter.

### Other Methods
You can see my other attempts. I either never got them fully working (List binary searching), and/or times were just not that impressive to warrant continued investigations in to them.

### Results

|          Name | M | I | Load In | Searches Performed | Search PSPI |
| ------------: | - | - | ------: | -----------------: | ----------: |
|          Tree | 3 | 2 |    2.23 |            572,771 |      95,461 |
|          Tree | 3 | 3 |    2.11 |            844,044 |      93,782 |
|          Tree | 3 | 4 |    2.28 |          1,114,720 |      92,893 |
|          Tree | 3 | 1 |    2.33 |            271,797 |      90,599 |
|          Tree | 3 | 5 |    2.17 |          1,321,679 |      88,111 |
|          Tree | 1 | 5 |    2.27 |          1,039,245 |      69,283 |
|          Tree | 1 | 3 |    2.10 |            616,879 |      68,542 |
|          Tree | 1 | 2 |    2.07 |            402,786 |      67,131 |
|          Tree | 1 | 4 |    2.27 |            801,378 |      66,781 |
|          Tree | 1 | 1 |    2.04 |            195,512 |      65,170 |
| BaseLineDeDup | 1 | 4 |   13.54 |             99,830 |       8,319 |
| BaseLineDeDup | 1 | 5 |   13.37 |            122,676 |       8,178 |
| BaseLineDeDup | 1 | 2 |   13.48 |             47,603 |       7,933 |
| BaseLineDeDup | 1 | 3 |   13.56 |             70,929 |       7,881 |
| BaseLineDeDup | 1 | 1 |   13.50 |             21,994 |       7,331 |
| BaseLineDeDup | 3 | 4 |   13.88 |             82,842 |       6,903 |
| BaseLineDeDup | 3 | 5 |   12.76 |            102,196 |       6,813 |
| BaseLineDeDup | 3 | 1 |   12.89 |             20,320 |       6,773 |
| BaseLineDeDup | 3 | 3 |   12.84 |             59,512 |       6,612 |
| BaseLineDeDup | 3 | 2 |   13.91 |             39,189 |       6,531 |
|      BaseLine | 3 | 1 |    0.10 |                291 |          97 |
|      BaseLine | 3 | 4 |    0.11 |              1,153 |          96 |
|      BaseLine | 3 | 5 |    0.06 |              1,426 |          95 |
|      BaseLine | 1 | 5 |    0.06 |              1,426 |          95 |
|      BaseLine | 1 | 4 |    0.16 |              1,149 |          95 |
|      BaseLine | 3 | 3 |    0.04 |                847 |          94 |
|      BaseLine | 1 | 3 |    0.16 |                843 |          93 |
|      BaseLine | 1 | 2 |    0.08 |                560 |          93 |
|      BaseLine | 1 | 1 |    0.10 |                279 |          93 |
|      BaseLine | 3 | 2 |    0.08 |                556 |          92 |

#### Legend
M = Min word length  
I = Instances (Threads)  
PSPI = Searches Per Second / Per Instance (Avg)