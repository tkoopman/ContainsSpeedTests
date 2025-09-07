# Goals

To find the fastest method of doing a string contains search when looking for 100k+ words contained in 100k+ strings.

# Requirements
- Case Insensitive.
- Neither list is unique. Duplicates will exist.
- Given datasets also chance that a search word will be contained within other words being searched for.
- Must index the list of words we looking for, not the strings we searching for contained words in.
- For each string, must search that list of words and return ALL matching contained words, including all duplicates.
- Strings being searched will come from records, where each record can have multiple words attached to it, so some threaded searching can assist. So will run tests with a few different number of instances.
- For this performance testing can just return count of total found including duplicates.
- All methods should return same counts for same data set to be valid. Doing check at start on a few records to do basic validation of this.

# Context
How I will be using this, is the list of words will be linked to "rules" by index int. The rules are stored in text file format, and can be loaded quickly.
The "records" will then be processed one at a time, finding all matching rules, and taking actions on that record based on matched rules. 
Records are not in a database but proprietary format, that is not indexed. To limit time taken to read records and modify them if required, 
we only want to read the record once, action it and move on. Hence why the words linked to rules need to be indexed.

While rules are more complex than just word matches (allOf, anyOf, oneOf, not), and different fields. 
The searching for words points to what words in a rule were found so don't have to check each individually.