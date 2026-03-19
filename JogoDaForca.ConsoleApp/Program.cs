using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace JogoDaForca.ConsoleApp;

static class Program
{
    static void Main(string[] args)
    {
        Console.CursorVisible = false;
        while (true)
        {
            string randomWord = GetRandomWord();
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
        return Enumerable.Repeat('_', word.Length).ToArray();
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
    static string GetRandomWord()
    {
        string[] words = [
            "ABACAXI",
            "ACEROLA",
            "AÇAÍ",
            "ARAÇÁ",
            "ABACATE",
            "BACABA",
            "BACURI",
            "BANANA",
            "CAJÁ",
            "CAJU",
            "CARAMBOLA",
            "CUPUAÇU",
            "GRAVIOLA",
            "GOIABA",
            "JABUTICABA",
            "JENIPAPO",
            "MAÇÃ",
            "MANGABA",
            "MANGA",
            "MARACUJÁ",
            "MURICI",
            "PEQUI",
            "PITANGA",
            "PITAYA",
            "SAPOTI",
            "TANGERINA",
            "UMBU",
            "UVA",
            "UVAIA"
        ];

        return words[RandomNumberGenerator.GetInt32(words.Length)];
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