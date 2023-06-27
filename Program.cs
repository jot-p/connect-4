using System;

namespace ConnectFour
{
    class Program
    {
        static char[,] board;
        static int currentPlayer;
        static bool gameOver;

        static void Main(string[] args)
        {
            InitializeGame();
            PlayGame();
        }

        static void InitializeGame()
        {
            // Initialize the game state
            board = new char[6, 7];
            currentPlayer = 1;
            gameOver = false;

            // Initialize the board with empty spaces
            for (int row = 0; row < 6; row++)
            {
                for (int col = 0; col < 7; col++)
                {
                    board[row, col] = ' ';
                }
            }
        }

        static void PlayGame()
        {
            while (!gameOver)
            {
                Console.Clear();
                PrintBoard();
                Console.WriteLine($"Player {currentPlayer}'s turn. Enter column number (1-7):");

                bool validInput = false;
                int column = 0;
                while (!validInput)
                {
                    string input = Console.ReadLine();
                    validInput = Int32.TryParse(input, out column) && column >= 1 && column <= 7;
                    if (!validInput)
                    {
                        Console.WriteLine("Invalid input. Please enter a valid column number (1-7).");
                    }
                }

                column--; // Adjust column number to match array index

                if (IsValidMove(column))
                {
                    int row = GetLowestEmptyRow(column);
                    board[row, column] = currentPlayer == 1 ? 'X' : 'O';

                    if (IsWinningMove(row, column))
                    {
                        Console.Clear();
                        PrintBoard();
                        Console.WriteLine($"Player {currentPlayer} wins! It's a Connect 4!");
                        gameOver = true;
                    }
                    else if (IsBoardFull())
                    {
                        Console.Clear();
                        PrintBoard();
                        Console.WriteLine("It's a draw!");
                        gameOver = true;
                    }
                    else
                    {
                        currentPlayer = currentPlayer == 1 ? 2 : 1; // Switch to the other player
                    }
                }
                else
                {
                    Console.WriteLine("Invalid move. Please choose another column.");
                }
            }

            // Ask the user if they want to restart the game
            Console.WriteLine("Restart? Yes(1) No(0):");
            string restartInput = Console.ReadLine();
            int restartChoice = 0;
            Int32.TryParse(restartInput, out restartChoice);
            if (restartChoice == 1)
            {
                InitializeGame();
                PlayGame();
            }
            else
            {
                Console.WriteLine("Press any key to exit.");
                Console.ReadKey();
            }
        }

        static void PrintBoard()
        {
            // Print the current state of the board
            for (int row = 5; row >= 0; row--)
            {
                Console.Write("| ");
                for (int col = 0; col < 7; col++)
                {
                    Console.Write(board[row, col] + " ");
                }
                Console.WriteLine("|");
            }
            Console.WriteLine("  1 2 3 4 5 6 7");
            Console.WriteLine();
        }

        static bool IsValidMove(int column)
        {
            // Check if the move is valid (within the board boundaries and the selected column is not full)
            return column >= 0 && column < 7 && board[5, column] == ' ';
        }

        static int GetLowestEmptyRow(int column)
        {
            // Find the lowest empty row in the selected column
            for (int row = 0; row < 6; row++)
            {
                if (board[row, column] == ' ')
                {
                    return row;
                }
            }
            return -1; // Column is full, should not happen if IsValidMove is checked first
        }

        static bool IsWinningMove(int row, int col)
        {
            // Check if the current move results in a winning configuration

            char symbol = currentPlayer == 1 ? 'X' : 'O';

            // Check horizontal
            int count = 0;
            for (int c = 0; c < 7; c++)
            {
                if (board[row, c] == symbol)
                {
                    count++;
                    if (count >= 4)
                    {
                        return true;
                    }
                }
                else
                {
                    count = 0;
                }
            }

            // Check vertical
            count = 0;
            for (int r = 0; r < 6; r++)
            {
                if (board[r, col] == symbol)
                {
                    count++;
                    if (count >= 4)
                    {
                        return true;
                    }
                }
                else
                {
                    count = 0;
                }
            }

            // Check diagonal (top-left to bottom-right)
            count = 0;
            int startRow = row - Math.Min(row, col);
            int startCol = col - Math.Min(row, col);
            for (int i = 0; i < 6; i++)
            {
                if (startRow + i >= 6 || startCol + i >= 7)
                {
                    break;
                }

                if (board[startRow + i, startCol + i] == symbol)
                {
                    count++;
                    if (count >= 4)
                    {
                        return true;
                    }
                }
                else
                {
                    count = 0;
                }
            }

            // Check diagonal (bottom-left to top-right)
            count = 0;
            startRow = row + Math.Min(5 - row, col);
            startCol = col - Math.Min(5 - row, col);
            for (int i = 0; i < 6; i++)
            {
                if (startRow - i < 0 || startCol + i >= 7)
                {
                    break;
                }

                if (board[startRow - i, startCol + i] == symbol)
                {
                    count++;
                    if (count >= 4)
                    {
                        return true;
                    }
                }
                else
                {
                    count = 0;
                }
            }

            return false;
        }

        static bool IsBoardFull()
        {
            // Check if the board is full (no empty spaces left)
            for (int row = 0; row < 6; row++)
            {
                for (int col = 0; col < 7; col++)
                {
                    if (board[row, col] == ' ')
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}