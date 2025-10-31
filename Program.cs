// See https://aka.ms/new-console-template for more information
using System;

namespace TicTacToe
{
    class Program
    {
        static char[,] board = new char[3, 3];
        static Random rnd = new Random();

        static void Main(string[] args)
        {
            InitializeBoard();
            Console.Title = "TicTacToe - Console";

            Console.WriteLine("TicTacToe (3x3) - Console Version\n");
            Console.Write("Play against the computer? (y/n): ");
            bool playAI = Console.ReadLine()?.Trim().ToLower() == "y";

            bool playerIsX = true;
            if (!playAI)
            {
                Console.WriteLine("Two players: Player 1 = X, Player 2 = O\n");
            }
            else
            {
                Console.WriteLine("You are X (go first). Computer is O.\n");
            }

            char current = 'X';
            while (true)
            {
                PrintBoard();

                if (current == 'X' || !playAI)
                {
                    Console.WriteLine($"Player {current}, it's your move.");
                    (int r, int c) = GetPlayerMove();
                    board[r, c] = current;
                }
                else
                {
                    Console.WriteLine("Computer is thinking...");
                    (int r, int c) = GetAIMove();
                    Console.WriteLine($"Computer places O at row {r + 1}, col {c + 1}.");
                    board[r, c] = current;
                }
                
                if (CheckWin(current))
                {
                    PrintBoard();
                    Console.WriteLine($"Player {current} wins!");
                    break;
                }
                
                if (IsBoardFull())
                {
                    PrintBoard();
                    Console.WriteLine("It's a draw!");
                    break;
                }
                
                current = (current == 'X') ? 'O' : 'X';
            }

            Console.WriteLine("Game over. Press any key to exit.");
            Console.ReadKey();
        }

        static void InitializeBoard()
        {
            for (int r = 0; r < 3; r++)
                for (int c = 0; c < 3; c++)
                    board[r, c] = ' ';
        }

        static void PrintBoard()
        {
            Console.Clear();
            Console.WriteLine("     1   2   3");
            Console.WriteLine("   -------------");
            for (int r = 0; r < 3; r++)
            {
                Console.Write($" {r + 1} |");
                for (int c = 0; c < 3; c++)
                {
                    Console.Write(" " + board[r, c] + " |");
                }
                Console.WriteLine();
                Console.WriteLine("   -------------");
            }
            Console.WriteLine();
        }

        static (int, int) GetPlayerMove()
        {
            while (true)
            {
                Console.Write("Enter row (1-3) and column (1-3) separated by space: ");
                string input = Console.ReadLine()?.Trim();
                if (string.IsNullOrEmpty(input))
                {
                    Console.WriteLine("Invalid input. Try again.");
                    continue;
                }

                string[] parts = input.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length != 2)
                {
                    Console.WriteLine("Please enter two numbers like: 2 3");
                    continue;
                }

                if (!int.TryParse(parts[0], out int r) || !int.TryParse(parts[1], out int c))
                {
                    Console.WriteLine("Numbers only. Try again.");
                    continue;
                }

                if (r < 1 || r > 3 || c < 1 || c > 3)
                {
                    Console.WriteLine("Numbers must be between 1 and 3.");
                    continue;
                }

                r--; c--;
                if (board[r, c] != ' ')
                {
                    Console.WriteLine("That cell is already taken. Choose another.");
                    continue;
                }

                return (r, c);
            }
        }

        static bool CheckWin(char player)
        {
            for (int r = 0; r < 3; r++)
                if (board[r, 0] == player && board[r, 1] == player && board[r, 2] == player)
                    return true;
            
            for (int c = 0; c < 3; c++)
                if (board[0, c] == player && board[1, c] == player && board[2, c] == player)
                    return true;
            
            if (board[0, 0] == player && board[1, 1] == player && board[2, 2] == player)
                return true;
            if (board[0, 2] == player && board[1, 1] == player && board[2, 0] == player)
                return true;

            return false;
        }

        static bool IsBoardFull()
        {
            for (int r = 0; r < 3; r++)
                for (int c = 0; c < 3; c++)
                    if (board[r, c] == ' ') return false;
            return true;
        }
        static (int, int) GetAIMove()
        {
            var win = TryFindWinningMove('O');
            if (win.HasValue) return win.Value;
            
            var block = TryFindWinningMove('X');
            if (block.HasValue) return block.Value;
            
            if (board[1, 1] == ' ') return (1, 1);
            
            int[][] corners = new int[][] { new int[] { 0, 0 }, new int[] { 0, 2 }, new int[] { 2, 0 }, new int[] { 2, 2 } };
            foreach (var corner in corners)
                if (board[corner[0], corner[1]] == ' ') return (corner[0], corner[1]);
            
            int[][] sides = new int[][] { new int[] { 0, 1 }, new int[] { 1, 0 }, new int[] { 1, 2 }, new int[] { 2, 1 } };
            foreach (var side in sides)
                if (board[side[0], side[1]] == ' ') return (side[0], side[1]);
            
            for (int r = 0; r < 3; r++)
                for (int c = 0; c < 3; c++)
                    if (board[r, c] == ' ') return (r, c);

            return (0, 0);
        }
        
        static (int, int)? TryFindWinningMove(char player)
        {
            for (int r = 0; r < 3; r++)
            {
                for (int c = 0; c < 3; c++)
                {
                    if (board[r, c] != ' ') continue;
                    
                    board[r, c] = player;
                    bool wins = CheckWin(player);
                    board[r, c] = ' ';

                    if (wins) return (r, c);
                }
            }
            return null;
        }
    }
}
