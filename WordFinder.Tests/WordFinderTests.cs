namespace WordFinder.Tests
{
    public class WordFinderTests
    {
        [Fact]
        public void Setting_Valid_Search_Words()
        {
            // Arrange
            WordFinder wordFinder = new WordFinder(string.Empty, string.Empty);
            wordFinder.WordsToFind = new List<string>(new string[] { "rug", "tiger", "candle", "box" });

            // Act
            bool actualResult = wordFinder.ValidateSearchWords();

            // Assert
            Assert.True(actualResult);
        }

        [Fact]
        public void Setting_Search_Word_Less_Than_Two_Chars_Throws()
        {
            // Arrange
            WordFinder wordFinder = new WordFinder(string.Empty, string.Empty);
            wordFinder.WordsToFind = new List<string>(new string[] { "rug", "tiger", "candle", "box", "I" });

            // Act + Assert
            Assert.Throws<Exception>(() => wordFinder.ValidateSearchWords());
        }

        [Fact]
        public void Setting_Search_Word_More_Than_Ten_Chars_Throws()
        {
            // Arrange
            WordFinder wordFinder = new WordFinder(string.Empty, string.Empty);
            wordFinder.WordsToFind = new List<string>(new string[] { "rug", "tiger", "candle", "box", "I", "Intercontinental" });

            // Act + Assert
            Assert.Throws<Exception>(() => wordFinder.ValidateSearchWords());
        }

        [Fact]
        public void Invalid_Path_To_Search_Words_File_Throws()
        {
            // Arrange
            var invalidSearchPath = @"D:\temp\testFiles\searchWords.input.test0.txt";
            WordFinder wordFinder = new WordFinder(string.Empty, invalidSearchPath);

            // Act + Assert
            Assert.Throws<FileNotFoundException>(() => wordFinder.LoadWordsToFind());
            
        }

        [Fact]
        public void Invalid_Path_To_Matrix_Input_File_Throws()
        {
            // Arrange
            var invalidMatrixFilePath = @"D:\temp\testFiles\garbarge.txt";
            WordFinder wordFinder = new WordFinder(invalidMatrixFilePath, string.Empty);

            // Act + Assert
            Assert.Throws <FileNotFoundException>(() => wordFinder.LoadWordMatrix());
        }

        [Fact]
        public void UnEqual_Matrix_Row_Lengths_Throws()
        {
            // Arrange
            WordFinder wordFinder = new WordFinder(string.Empty, string.Empty);
            wordFinder.MatrixAsStrings = new List<string>(new string[] { "XXXXX", "XXXXXX", "XXXXX", "BBBBB", "YYYYYY" });

            // Act + Assert
            Assert.Throws<Exception>(() => wordFinder.SetMatrixSize());
        }

        [Fact]
        public void FindWords_Executes_Successfully_With_Valid_Inputs()
        {
            // Arrange
            WordFinder wordFinder = new WordFinder(string.Empty, string.Empty);
            wordFinder.MatrixAsStrings = new List<string>(new string[] { "XCARX", "XAXXX", "XBXXX", "BLBBB", "YEVAN" });
            wordFinder.WordsToFind = new List<string>(new string[] { "CAR", "VAN", "CABLE", "MACHINE", "TRUCK" });
            wordFinder.SetMatrixSize();
            wordFinder.ValidateSearchWords();
            wordFinder.CreateMatrix();
            var expectedWordsFoundCount = 3;
            var expectedWordsNotFoundCount = 2;

            // Act
            wordFinder.FindWords();

            // Assert
            Assert.True(expectedWordsFoundCount == wordFinder.WordsFound.Count() &&
                        expectedWordsNotFoundCount == wordFinder.WordsNotFound.Count());

        }
    }
}
