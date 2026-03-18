using System.Security.Cryptography;

namespace JogoDaForca.ConsoleApp;

class Program
{
    static void Main(string[] args)
    {
        while (true)
        {
            Console.CursorVisible = false;
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
        HashSet<char> guessedLetters = [];
        while (!playerWon && !playerLost)
        {
            PrintInfo(string.Join("", guessedWord), guessesLeft);
            Console.WriteLine($"Letras já inseridas: {string.Join(" ", guessedLetters.Order())}");
            Console.WriteLine("\nDigite uma letra, ou pressione [Tab↹] para tentar adivinhar a palavra inteira…");

            var keyInfo = Console.ReadKey(true);
            char guess = char.ToUpperInvariant(keyInfo.KeyChar);

            if (keyInfo.Key == ConsoleKey.Tab)
            {
                string? fullWordGuess = null;
                PrintInfo(string.Join("", guessedWord), guessesLeft);
                Console.WriteLine($"Letras já inseridas: {string.Join(" ", guessedLetters.Order())}");
                Console.Write("\nDigite a palavra inteira: ");
                fullWordGuess = Console.ReadLine()?.ToUpperInvariant();
                if (fullWordGuess == randomWord)
                {
                    guessedWord = randomWord.ToCharArray();
                    playerWon = true;
                }
                else
                {
                    guessesLeft--;
                    playerLost = guessesLeft == 0;
                }


                if (playerWon)
                    PrintInfo(string.Join("", guessedWord), msg: "Você ganhou!");
                else if (playerLost)
                    PrintInfo(string.Join("", guessedWord), msg: $"Você perdeu…\nA palavra era {randomWord}.");

                continue;
            }

            if (char.IsLetter(guess))
            {
                if (guessedLetters.Contains(guess)) continue;
                if (randomWord.Contains(guess)) RevealLetters(randomWord, guessedWord, guess);
                else guessesLeft--;
                guessedLetters.Add(guess);
            }

            playerWon = !guessedWord.Contains('_');
            playerLost = guessesLeft == 0;

            if (playerWon)
                PrintInfo(word: string.Join("", guessedWord), msg: "Você ganhou!");
            else if (playerLost)
                PrintInfo(word: string.Join("", guessedWord), msg: $"Você perdeu…\nA palavra era {randomWord}.");
        }
    }
    static char[] InitializeWord(string word)
    {
        return Enumerable.Repeat('_', word.Length).ToArray();
    }

    static void RevealLetters(string word, char[] guessedWord, char guess)
    {
        for (int i = 0; i < word.Length; i++) if (word[i] == guess) guessedWord[i] = guess;
    }

    static string GetRandomWord()
    {
        string[] words = [
            "ABACATE",
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
    static void PrintInfo(string word, int? count = null, string? msg = null)
    {
        Console.Clear();
        Console.WriteLine($"Palavra: {word}");
        if (count != null) Console.WriteLine($"Tentativas: {count}");
        if (msg != null) Console.WriteLine(msg);
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
