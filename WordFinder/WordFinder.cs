using System.Text;

namespace WordFinder
{
    public class WordFinder
    {
        #region [ CLASS FIELDS ]

        private int _matrixSize;
        private List<string> _wordsToFind;
        private List<string> _wordsFound;
        private List<string> _wordsNotFound;
        private List<string> _matrixAsStrings;
        private string _pathToMatrixFile;
        private string _pathToSearchWordFile;

        /*
         * PER Synapse Health no words with length under 2 chars
         */
        private readonly int MIN_WORD_LEN = 2;

        /*
         * PER Synapse Health no words with length over 10 chars
         */
        private readonly int MAX_WORD_LEN = 10;

        private string[,] _wordMatrix;

        #endregion

        #region [ CONSTRUCTORS ]

        private WordFinder()
        {
            this._wordsToFind = new List<string>();
            this._wordsFound = new List<string>();
            this._wordsNotFound = new List<string>();
            this._matrixAsStrings = new List<string>();
            this._pathToMatrixFile = string.Empty;
            this._pathToSearchWordFile = string.Empty;
            this._matrixSize = 0;
        }

        public WordFinder(string pathToMatrixFile, string pathToSearchWordFile) : this()
        {
            this._pathToMatrixFile = pathToMatrixFile;
            this._pathToSearchWordFile = pathToSearchWordFile;
        }

        #endregion

        #region [ PROPERTIES ]

        public int MatrixSize { get => _matrixSize; set => _matrixSize = value; }


        public List<string> WordsToFind { get => _wordsToFind; set => _wordsToFind = value; }

        public List<string> WordsFound { get => _wordsFound; set => _wordsFound = value; }

        public List<string> WordsNotFound { get => _wordsNotFound; set => _wordsNotFound = value; }

        public List<string> MatrixAsStrings { get => _matrixAsStrings; set => _matrixAsStrings = value; }

        #endregion

        #region [ METHODS ]

        /// <summary>
        /// scan the word matrix for the list of search words.
        /// create list of words found in the matrix and a list of search words
        /// that were provided but not found in the matrix
        /// </summary>
        public void FindWords()
        {
            StringBuilder sbCols = new StringBuilder();
            StringBuilder sbRows = new StringBuilder();

            /*
             * use nested BRUTE FORCE for loops
             * to search the rows and columns
             * at the same time.
             */
            for (int i = 0; i < this._matrixSize; i++)
            {

                for (int j = 0; j < this._matrixSize; j++)
                {
                    sbCols.Append(this._wordMatrix[i, j]);
                    sbRows.Append(this._wordMatrix[j, i]);
                }

                if (sbCols.Length > 0 && sbRows.Length > 0)
                {
                    foreach (var w in this._wordsToFind)
                    {
                        if (sbCols.ToString().IndexOf(w.ToUpper()) > -1)
                        { this._wordsFound.Add(w); }

                        if (sbRows.ToString().IndexOf(w.ToUpper()) > -1)
                        { this._wordsFound.Add(w); }
                    }
                }
                else
                { 
                    throw new Exception("Invalid length for search string.");
                }

                sbCols.Clear();
                sbRows.Clear();
            }

            this._wordsNotFound = this._wordsToFind.Except(this._wordsFound, StringComparer.InvariantCultureIgnoreCase)
                                                    .ToList();
        }

        /// <summary>
        /// Load the matrix input file
        /// </summary>
        /// <exception cref="FileNotFoundException"></exception>
        public void LoadWordMatrix()
        {
            
            if(File.Exists(this._pathToMatrixFile))
            {
                this._matrixAsStrings = File.ReadAllLines(this._pathToMatrixFile).ToList();
            }
            else
            {
                throw new FileNotFoundException($"Invalid file: {this._pathToMatrixFile}.");
            }

            if(this._matrixAsStrings.Count < 1)
            {
                throw new Exception("Matrix input file must contain strings for matrix. File cannot be empty.");
            }
        }

        /// <summary>
        /// Set the size of the matrix
        /// check each line in the input file, all must be equal to
        /// create a "square" matrix, throw exception if file is empty. PER
        /// Synapse Health: cannot search for nothing
        /// </summary>
        /// <exception cref="Exception"></exception>
        public void SetMatrixSize()
        {
            this._matrixSize = this._matrixAsStrings.Count;

            this._matrixAsStrings.ForEach(s =>
            {
                if (s.Length != this._matrixSize)
                {
                    throw new Exception("Invalid string length, cannot create matrix.");
                }
            });
        }

        /// <summary>
        /// Load the words to find from a text input file
        /// </summary>
        public void LoadWordsToFind()
        {
            if(File.Exists(this._pathToSearchWordFile))
            {
                this._wordsToFind = File.ReadAllLines(this._pathToSearchWordFile).ToList();
            }
            else
            {
                throw new FileNotFoundException($"Invalid file: {this._pathToSearchWordFile}");
            }

            if(this._wordsToFind.Count < 1)
            {
                throw new Exception("Search words input file cannot be empty.");
            }  
        }

        /// <summary>
        /// Check each search word, PER Synapse Health words must be 2 or more characters
        /// and 10 or less characters, throw exception if file is empty. PER
        /// Synapse Health: cannot search for nothing  
        /// </summary>
        /// <returns>bool to indicate valid search words</returns>
        /// <exception cref="Exception"></exception>
        public bool ValidateSearchWords()
        {
            var wordsOK = true;

            this._wordsToFind.ForEach(w =>
            {
                if (!this._validSearchWord(w))
                {
                    throw new Exception($"Invalid length for search word: {w}.");
                }
            });

            return wordsOK;
        }

        /// <summary>
        /// Set the size of the 2D array
        /// create the 2D array from the strings in the text input file
        /// using String.Substring() with a length of 1 
        /// </summary>
        public void CreateMatrix()
        {
            this._wordMatrix = new string[this._matrixSize, this._matrixSize];

            for(int j = 0; j < this._matrixSize; j++)
            {
                for(int k = 0; k < this._matrixSize; k++)
                {
                    this._wordMatrix[j, k] = this._matrixAsStrings[j].Substring(k, 1);
                }
            }
        }

        /// <summary>
        /// Validate word length
        /// </summary>
        /// <param name="theWord"></param>
        /// <returns></returns>
        private bool _validSearchWord(string theWord)
        {
            bool wordOK = false;

            if (theWord.Length >= MIN_WORD_LEN && theWord.Length <= MAX_WORD_LEN)
            { 
                wordOK = true;
            }

            return wordOK;
        }
        #endregion
    }
}
