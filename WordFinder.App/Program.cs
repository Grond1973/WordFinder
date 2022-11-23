/***
 * program usage: WordFinder.App.exe [path to matrix input file] [path to search word file]
 *              ex: WordFinder.App.exe "D:\SynapseHealth\WordMatrixTest\InputFiles\SynapseHealthMatrix0.txt" "D:\SynapseHealth\WordMatrixTest\InputFiles\SynapseHealthInputWords0.txt"
 */

try
{
    WordFinder.WordFinder wordFinder = null;

    if (args.Length == 0 || args.Length == 1)
    {
        Console.WriteLine("Input parameters missing.");
        Console.WriteLine("Usage: WordFinder.App.Exe path_to_matrix_input_file path_to_search_word_file");
    }
    else
    {
        wordFinder = new WordFinder.WordFinder(args[0], args[1]);
        wordFinder.LoadWordMatrix();
        wordFinder.SetMatrixSize();
        wordFinder.LoadWordsToFind();
        wordFinder.ValidateSearchWords();
        wordFinder.CreateMatrix();
        wordFinder.FindWords();

        Console.WriteLine("WORDS FOUND: ");
        wordFinder.WordsFound.ForEach(x => Console.WriteLine(x));

        Console.WriteLine("WORDS NOT FOUND:");
        wordFinder.WordsNotFound.ForEach(x => Console.WriteLine(x));
    }

    Console.WriteLine("Operation complete...");
}
catch (Exception ex)
{ 
    Console.WriteLine($"ERROR: {ex.ToString()}");
}

return 0;