using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Runtime.Remoting.Lifetime;

class Program
{
    // Function to display Foreground Colors
    private static void ShowMessageToPlayer(ConsoleColor color, string msg)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(msg);
        Console.ResetColor();
    }

    // Method to check if the input is a valid number and convert it
    static bool IsValidNumber(string input, out int result)
    {
        bool isValid = int.TryParse(input, out result);
        return isValid;
    }

    // Function to play the guessing game for one round
    static void PlayGame(List<string> players, int minRange, int maxRange)
    {
        Random rand = new Random();
        int mysteryNumber = rand.Next(minRange, maxRange + 1); // Random number within the defined range
        bool gameWon = false;
        int currentPlayerIndex = 0;

        Console.WriteLine($"\nA new game has started! The mystery number is between {minRange} and {maxRange}.");
        while (!gameWon)
        {
            string currentPlayer = players[currentPlayerIndex];
            Console.WriteLine($"\n{currentPlayer}, it's your turn to guess.");

            int playerGuess = -1;
            bool validInput = false;

            while (!validInput)
            {
                Console.Write("Enter your guess: ");
                string input = Console.ReadLine();
                validInput = int.TryParse(input, out playerGuess);

                // Check if the player guess is whithin the defined range
                if (playerGuess < minRange)
                {
                    ShowMessageToPlayer(ConsoleColor.Yellow, $"{currentPlayer}, your guess is below the minimum allowed. Please enter a number between {minRange} and {maxRange}.");
                    validInput = false;
                }
                else if (playerGuess > maxRange)
                {
                    ShowMessageToPlayer(ConsoleColor.Yellow, $"{currentPlayer}, your guess is above the maximum allowed. Please enter a number between {minRange} and {maxRange}.");
                    validInput = false;
                }
            }

            // Check if the guess is correct
            if (playerGuess == mysteryNumber)
            {
                ShowMessageToPlayer(ConsoleColor.Green, $"\nCongratulations {currentPlayer}, you've guessed the correct number!");
                ShowMessageToPlayer(ConsoleColor.Green, $"The mystery number was {mysteryNumber}.");
                gameWon = true;
            }
            else if (playerGuess < mysteryNumber)
            {
                ShowMessageToPlayer(ConsoleColor.Yellow, $"{currentPlayer}, too low! Try again.");
            }
            else
            {
                ShowMessageToPlayer(ConsoleColor.Yellow, $"{currentPlayer}, too high! Try again.");
            }

            // Move to the next player
            currentPlayerIndex = (currentPlayerIndex + 1) % players.Count;
        }
    }

    static void Main()
    {
        // Game Presentation and Instructions
        Console.BackgroundColor = ConsoleColor.White;
        ShowMessageToPlayer(ConsoleColor.Black, " Welcome to the High-Low G1 Multiplayer Guessing Game! \n");
        Console.ResetColor();
        ShowMessageToPlayer(ConsoleColor.Blue, "Instructions:");
        Console.WriteLine("In this game, each player will take turns guessing a mystery number.");
        Console.WriteLine("The mystery number is randomly generated within a defined range.");
        Console.WriteLine("Each player will have one guess per round, and they will be told if their guess is too low, too high, or correct.");
        Console.WriteLine("The first player to guess the correct number wins the game.");
        Console.WriteLine("Click any key to continue...");
        Console.ReadLine();

        bool playAgain = true;
        bool isValidInput = false;
        int numberOfPlayers = 0;

        // Start the game loop
        while (playAgain)
        {
            // Ask if user is brave enough yo play and validate if the input is valid or not
            string braveResponse = "";
            while (braveResponse != "y" && braveResponse != "n")
            {
                Console.Write("Are you brave enough to play? (y/n): ");
                braveResponse = Console.ReadLine().ToLower(); // Convert to lowercase

                if (braveResponse != "y" & braveResponse != "n")
                {
                    ShowMessageToPlayer(ConsoleColor.Yellow, "Invalid input! Please enter 'y' or 'n'.");
                }
            }

            if (braveResponse == "n")
            {
                Console.WriteLine("\nOk! Maybe next time! Goodbye.");
                break; // Exit the game if the player is not brave enough
            }

            List<string> players = new List<string>();
            // Ask if the player wants to play single-player or multiplayer
            string modeChoice = "";
            while (modeChoice != "s" && modeChoice != "m")
            {
                Console.Write("\nDo you want to play in single-player or multiplayer mode? (s/m): ");
                modeChoice = Console.ReadLine().ToLower();

                if (modeChoice == "s")
                {
                    // Single-player mode
                    Console.Write("Enter your name: ");
                    string playerName = Console.ReadLine();
                    players.Add(playerName); // Only one player in single-player mode
                }
                else if (modeChoice == "m")
                {
                    // Multiplayer mode

                    while (!isValidInput)
                    {
                        Console.Write("Enter the number of players: ");
                        string input = Console.ReadLine();

                        // Check if the input is a valid number
                        isValidInput = IsValidNumber(input, out numberOfPlayers);

                        if (!isValidInput || numberOfPlayers < 1)
                        {
                            Console.WriteLine("Invalid input! Please enter a valid number of players (at least 1).");
                            isValidInput = false;  // Reset isValidInput to false for re-entry
                        }
                    }

                    // Get player names
                    for (int i = 1; i <= numberOfPlayers; i++)
                    {
                        Console.Write($"Enter the name of player {i}: ");
                        string playerName = Console.ReadLine();
                        players.Add(playerName);
                    }
                }
                else
                {
                    // If the user didn't choose valid mode, this message should appear and ask the player to enter a valid game mode
                    ShowMessageToPlayer(ConsoleColor.Yellow, "Invalid choice! Please choose 's' for single-player or 'm' for multiplayer.");
                }
            }

            // Ask for a random range for the mystery number (this could be modified for specific rules)
            Random rand = new Random();
            int minRange = rand.Next(1, 51); // Random min range between 1 and 50
            int maxRange = rand.Next(minRange + 50, minRange + 101); // Random max range, ensuring it is larger than min range

            // Play the game for the current set of players with the chosen range
            PlayGame(players, minRange, maxRange);

            // Ask if they want to play again
            Console.Write("\nDo you want to play again? (y/n): ");
            string playAgainInput = Console.ReadLine().ToLower();

            if (playAgainInput != "y")
            {
                playAgain = false; // Exit the game loop
                Console.WriteLine("Thanks for playing! Goodbye!");
            }
            else
            {
                Console.WriteLine("\nA new game is starting...\n");
            }
        }
        Console.ReadLine();
    }
}
