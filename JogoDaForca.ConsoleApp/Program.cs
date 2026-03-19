using System.Globalization;
using System.Text;
using static JogoDaForca.ConsoleApp.WordsRepository;

namespace JogoDaForca.ConsoleApp;

static class Program
{
    static void Main(string[] args)
    {
        Console.CursorVisible = false;
        while (true)
        {
            var selection = SelectMenuOption();
            string randomWord = GetRandomWord(selection.category);
            char[] guessedWordState = InitializeWord(randomWord);
            RunGame(randomWord, guessedWordState);
            Thread.Sleep(750);
            Console.WriteLine();
            if (!ContinuePrompt()) break;
        }
    }

    static void RunGame(string randomWord, char[] guessedWord)
    {
        bool playerWon = false;
        bool playerLost = false;
        int guessesLeft = 5;
        string normalizedWord = NormaliseString(randomWord);
        HashSet<char> guessedLetters = [];
        while (!playerWon && !playerLost)
        {
            PrintInfo(word: new string(guessedWord), count: guessesLeft, guessedLetters: guessedLetters, drawAttemptCounter: true);
            Console.WriteLine("\nDigite uma letra, ou pressione [Tab↹] para\ntentar adivinhar a palavra inteira…");

            var keyInfo = Console.ReadKey(true);
            char guess = char.ToUpperInvariant(keyInfo.KeyChar);
            if (keyInfo.Key == ConsoleKey.Tab)
            {
                PrintInfo(word: new string(guessedWord), count: guessesLeft, guessedLetters: guessedLetters, drawAttemptCounter: true);
                Console.Write("\nDigite a palavra inteira: ");
                string? fullWordGuess = Console.ReadLine()?.ToUpperInvariant();
                if (!string.IsNullOrWhiteSpace(fullWordGuess) && NormaliseString(fullWordGuess) == normalizedWord)
                {
                    guessedWord = randomWord.ToCharArray();
                    playerWon = true;
                }
                else
                {
                    guessesLeft--;
                    playerLost = guessesLeft == 0;
                }

                PrintEnd(playerWon, playerLost, randomWord, new string(guessedWord), guessesLeft);
                continue;
            }

            if (char.IsLetter(guess))
            {
                char normalizedGuess = NormaliseString(guess.ToString())[0];
                if (guessedLetters.Contains(normalizedGuess)) continue;
                if (normalizedWord.Contains(normalizedGuess)) RevealLetters(randomWord, guessedWord, guess);
                else guessesLeft--;
                guessedLetters.Add(normalizedGuess);
            }

            playerWon = !guessedWord.Contains('_');
            playerLost = guessesLeft == 0;

            PrintEnd(playerWon, playerLost, randomWord, new string(guessedWord), guessesLeft);
        }
    }
    static char[] InitializeWord(string word)
    {
        return word.Select(c => c == ' ' ? ' ' : '_').ToArray();
    }
    static string NormaliseString(string input)
    {
        var normalized = input.Normalize(NormalizationForm.FormD);
        var sb = new StringBuilder();

        foreach (char c in normalized)
        {
            if (char.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                sb.Append(c);
        }

        return sb.ToString().Normalize(NormalizationForm.FormC);
    }
    static void RevealLetters(string word, char[] guessedWord, char guess)
    {
        char normalizedGuess = NormaliseString(guess.ToString())[0];
        for (int i = 0; i < word.Length; i++)
        {
            char normalizedWordChar = NormaliseString(word[i].ToString())[0];

            if (normalizedWordChar == normalizedGuess) guessedWord[i] = word[i]; // keep original accent
        }
    }
    static void DrawLittleGuy(int guessesLeft)
    {
        char[,] art =
        {
            { ' ', '╔','═','═','╤','═','═','═','═','═','═','╕',' ' },
            { ' ', '╟','─','─','╯',' ',' ',' ',' ',' ',' ','│',' ' },
            { ' ', '║',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ' },
            { ' ', '║',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ' },
            { ' ', '║',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ' },
            { ' ', '║',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ' },
            { ' ', '║',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ' },
            { '╔', '╩','╗',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ' }
        };
        if (guessesLeft <= 4) art[2, 11] = '☺';
        if (guessesLeft <= 3) art[3, 11] = '|';
        if (guessesLeft <= 2) art[3, 10] = '/';
        if (guessesLeft <= 1) art[3, 12] = '\\';
        if (guessesLeft <= 0)
        {
            art[4, 10] = '/';
            art[4, 12] = '\\';
        }
        for (int i = 0; i < art.GetLength(0); i++)
        {
            for (int j = 0; j < art.GetLength(1); j++)
            {
                Console.Write(art[i, j]);
            }
            Console.WriteLine();
        }
    }
    static void PrintInfo(string word, int count, string? msg = null, IEnumerable<char>? guessedLetters = null, bool drawAttemptCounter = false)
    {
        Console.Clear();
        DrawLittleGuy(count);
        Console.WriteLine($"Palavra: {word}");
        if (guessedLetters != null) Console.WriteLine($"Letras já inseridas: {string.Join(" ", guessedLetters.Order())}");
        if (drawAttemptCounter) Console.WriteLine($"Tentativas: {count}");
        if (msg != null) Console.WriteLine(msg);
    }
    static void PrintEnd(bool playerWon, bool playerLost, string randomWord, string guessedWord, int guessesLeft)
    {
        if (playerWon)
            PrintInfo(word: guessedWord, msg: "Você ganhou!", count: guessesLeft);
        else if (playerLost)
            PrintInfo(word: guessedWord, msg: $"Você perdeu…\nA palavra era {randomWord}.", count: guessesLeft);
    }
    static (Difficulty difficulty, Category category) SelectMenuOption()
    {
        Difficulty[] difficulties = [Difficulty.Easy, Difficulty.Medium, Difficulty.Hard];

        int difficultyIndex = 0;
        int categoryIndex = 0;

        while (true)
        {
            Console.Clear();

            DrawDifficultyMenu(difficulties, difficultyIndex);

            Category[] categories = GetCategories(difficulties[difficultyIndex]);

            DrawCategories(categories, categoryIndex);

            Console.WriteLine("\nUse ← → para dificuldade, ↑ ↓ para categoria, ENTER para confirmar.");

            ConsoleKey key = Console.ReadKey(true).Key;

            switch (key)
            {
                case ConsoleKey.LeftArrow:
                    if (difficultyIndex > 0)
                    {
                        difficultyIndex--;
                        categoryIndex = 0;
                    }
                    break;

                case ConsoleKey.RightArrow:
                    if (difficultyIndex < difficulties.Length - 1)
                    {
                        difficultyIndex++;
                        categoryIndex = 0;
                    }
                    break;

                case ConsoleKey.UpArrow:
                    if (categoryIndex > 0)
                        categoryIndex--;
                    break;

                case ConsoleKey.DownArrow:
                    if (categoryIndex < categories.Length - 1)
                        categoryIndex++;
                    break;

                case ConsoleKey.Enter:
                    return (difficulties[difficultyIndex], categories[categoryIndex]);
            }
        }
    }
    static void DrawDifficultyMenu(Difficulty[] difficulties, int selectedIndex)
    {
        string[] top = new string[difficulties.Length];
        string[] middle = new string[difficulties.Length];
        string[] bottom = new string[difficulties.Length];

        for (int i = 0; i < difficulties.Length; i++)
        {
            string label = difficulties[i] switch
            {
                Difficulty.Easy => " FÁCIL ",
                Difficulty.Medium => " MÉDIO ",
                Difficulty.Hard => " DIFÍCIL ",
                _ => ""
            };

            bool selected = i == selectedIndex;

            if (selected)
            {
                top[i] = $"╔{new string('═', label.Length)}╗";
                middle[i] = $"║{label}║";
                bottom[i] = $"╚{new string('═', label.Length)}╝";
            }
            else
            {
                top[i] = $"┌{new string('─', label.Length)}┐";
                middle[i] = $"│{label}│";
                bottom[i] = $"└{new string('─', label.Length)}┘";
            }
        }

        Console.WriteLine(string.Join("  ", top));
        Console.WriteLine(string.Join("  ", middle));
        Console.WriteLine(string.Join("  ", bottom));
    }
    static void DrawCategories(Category[] categories, int selectedIndex)
    {
        Console.WriteLine("Categorias:\n");

        for (int i = 0; i < categories.Length; i++)
        {
            bool selected = i == selectedIndex;
            Console.WriteLine(selected ? $" → {categories[i].Name}" : $"   {categories[i].Name}");
        }
    }
    static bool ContinuePrompt()
    {
        Console.Write("Pressione ENTER para jogar novamente ou ESC para sair…");
        while (true)
        {
            ConsoleKey key = Console.ReadKey(true).Key;
            switch (key)
            {
                case ConsoleKey.Enter: return true;
                case ConsoleKey.Escape: return false;
                default: break;
            }
        }
    }
}