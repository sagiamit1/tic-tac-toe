using System;

class TicTacToe
{
    static string GetInput()
    {
        return Console.ReadLine();
    }

    static char[] GetInitializedBoard()
    {
        return new char[] { '-', '-', '-', '-', '-', '-', '-', '-', '-' };
    }

    static void PrintBoard(char[] board)
    {
        Console.WriteLine("  1 2 3");
        for (int i = 0; i < 3; i++)
        {
            Console.Write((i + 1) + " ");
            for (int j = 0; j < 3; j++)
            {
                Console.Write(board[i * 3 + j] + " ");
            }
            Console.WriteLine();
        }
    }

    static bool IsValidMove(char[] board, int row, int col)
    {
        if (row < 1 || row > 3 || col < 1 || col > 3)
            return false;
        int index = (row - 1) * 3 + (col - 1);
        return board[index] == '-';
    }

    static void UserMove(char[] board)
    {
        int row, col;
        do
        {
            Console.WriteLine("Please enter the row (1-3):");
            row = GetValidNumber();
            Console.WriteLine("Please enter the column (1-3):");
            col = GetValidNumber();
        } while (!IsValidMove(board, row, col));

        int index = (row - 1) * 3 + (col - 1);
        board[index] = 'X';
    }

    static int GetValidNumber()
    {
        while (true)
        {
            string input = Console.ReadLine();

            // Check if input is empty or null
            if (input == null || input.Length == 0)
            {
                Console.WriteLine("Invalid input. Please enter a number between 1 and 3:");
                continue;
            }

            // Remove leading whitespaces
            int startIndex = 0;
            while (startIndex < input.Length && input[startIndex] == ' ')
            {
                startIndex++;
            }

            // Check if input contains only digits
            bool isValid = true;
            int number = 0;
            for (int i = startIndex; i < input.Length; i++)
            {
                if (input[i] < '0' || input[i] > '9')
                {
                    isValid = false;
                    break;
                }
                number = number * 10 + (input[i] - '0');
            }

            // If input is valid, check the range
            if (isValid && number >= 1 && number <= 3)
            {
                return number;
            }

            // If input is not valid, prompt again
            Console.WriteLine("Invalid input. Please enter a number between 1 and 3:");
        }
    }



    static bool IsNumeric(string input)
    {
        foreach (char c in input)
        {
            if (c < '0' || c > '9')
                return false;
        }
        return true;
    }


    static bool IsWinner(char[] board, char symbol)
    {
        for (int i = 0; i < 3; i++)
        {
            if (board[i * 3] == symbol && board[i * 3 + 1] == symbol && board[i * 3 + 2] == symbol)
                return true;
        }

        for (int i = 0; i < 3; i++)
        {
            if (board[i] == symbol && board[i + 3] == symbol && board[i + 6] == symbol)
                return true;
        }

        if ((board[0] == symbol && board[4] == symbol && board[8] == symbol) ||
            (board[2] == symbol && board[4] == symbol && board[6] == symbol))
            return true;

        return false;
    }

    static bool IsDraw(char[] board)
    {
        foreach (char c in board)
        {
            if (c == '-')
                return false;
        }
        return true;
    }

    static void ComputerMove(char[] board)
    {
        // First, try to win
        if (TryWinOrBlock(board, 'O'))
            return;

        // Then, block the player if they can win
        if (TryWinOrBlock(board, 'X'))
            return;

        // Handle specific block for described scenario
        if (HandleSpecificBlock(board))
            return;

        // Handle corner cases
        if (HandleCornerCases(board))
            return;

        // If no specific strategy, pick the first available spot
        for (int i = 0; i < 9; i++)
        {
            if (board[i] == '-')
            {
                board[i] = 'O';
                break;
            }
        }
    }

    static bool TryWinOrBlock(char[] board, char symbol)
    {
        for (int i = 0; i < 9; i++)
        {
            if (board[i] == '-')
            {
                board[i] = symbol;
                if (IsWinner(board, symbol))
                {
                    if (symbol == 'O')
                    {
                        board[i] = 'O'; // Computer wins
                    }
                    else
                    {
                        board[i] = 'O'; // Block player
                    }
                    return true;
                }
                board[i] = '-';
            }
        }
        return false;
    }

    static bool HandleCornerCases(char[] board)
    {
        int center = 4;
        int[] corners = { 0, 2, 6, 8 };
        int[] sides = { 1, 3, 5, 7 };

        // Specific block scenarios based on player's X positions
        if (board[1] == 'X' && board[5] == 'X' && board[2] == '-')
        {
            board[2] = 'O';
            return true;
        }
        if (board[5] == 'X' && board[7] == 'X' && board[8] == '-')
        {
            board[8] = 'O';
            return true;
        }
        if (board[7] == 'X' && board[3] == 'X' && board[6] == '-')
        {
            board[6] = 'O';
            return true;
        }
        if (board[1] == 'X' && board[3] == 'X' && board[0] == '-')
        {
            board[0] = 'O';
            return true;
        }

        // If center is available, take it
        if (board[center] == '-')
        {
            board[center] = 'O';
            return true;
        }

        // If player has taken center, take a corner
        if (board[center] == 'X')
        {
            foreach (var corner in corners)
            {
                if (board[corner] == '-')
                {
                    board[corner] = 'O';
                    return true;
                }
            }
        }

        // If center is taken by computer, take a side
        if (board[center] == 'O')
        {
            foreach (var side in sides)
            {
                if (board[side] == '-')
                {
                    board[side] = 'O';
                    return true;
                }
            }
        }

        return false;
    }

    static bool HandleSpecificBlock(char[] board)
    {
        // Check for the specific scenario where player has made the following moves:
        // Player: (1,3), (3,2), (3,3)
        // Block this by placing 'O' in (2,3)
        if (board[2] == 'X' && board[7] == 'X' && board[8] == 'X' && board[5] == '-')
        {
            board[5] = 'O';
            return true;
        }

        // Check for the specific scenario where player has made the following moves:
        // Player: (1,3), (3,2)
        // Block this by placing 'O' in (3,3)
        if (board[2] == 'X' && board[7] == 'X' && board[8] == '-')
        {
            board[8] = 'O';
            return true;
        }

        // Check for the specific scenario where player has made the following moves:
        // Player: (1,3), (2,2), (3,2)
        // Block this by placing 'O' in (3,3)
        if (board[2] == 'X' && board[4] == 'X' && board[7] == 'X' && board[8] == '-')
        {
            board[8] = 'O';
            return true;
        }

        // No specific block needed
        return false;
    }

    static int Main(string[] args)
    {
        char[] board = GetInitializedBoard();
        int turnCounter = 0;

        while (true)
        {
            PrintBoard(board);

            if (turnCounter % 2 == 0)
            {
                UserMove(board);
                if (IsWinner(board, 'X'))
                {
                    Console.WriteLine("Player wins!");
                    PrintBoard(board);
                    return 1;
                }
            }
            else
            {
                ComputerMove(board);
                if (IsWinner(board, 'O'))
                {
                    Console.WriteLine("Computer wins!");
                    PrintBoard(board);
                    return 2;
                }
            }

            if (IsDraw(board))
            {
                Console.WriteLine("It's a draw!");
                PrintBoard(board);
                return 0;
            }

            turnCounter++;
        }
    }
}