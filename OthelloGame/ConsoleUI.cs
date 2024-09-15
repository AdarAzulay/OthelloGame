using System;
using System.Text;

namespace OthelloGame
{
    public class ConsoleUI
    {
        private char m_FirstTypeOfToken = 'X';
        private char m_SecondTypeOfToken = 'O';

        public int GetBoardSize()
        {
            int dimension;

            Console.WriteLine("Please choose board size by entering the corresponding number:");
            Console.WriteLine("1 - 6X6");
            Console.WriteLine("2 - 8X8");
            while (!int.TryParse(Console.ReadLine(), out dimension) || (dimension != 1 && dimension != 2))
            {
                Console.WriteLine("Invalid input. Please enter a valid choice");
            }

            return dimension == 1 ? 6 : 8;
        }

        public Tuple<int, int> GetPlayerMove(Player i_Player, int i_BoardSize)
        {
            Console.WriteLine($"{i_Player.PlayerName}, place your Token ({i_Player.PlayerToken}) by entering your move in the format 'row,column' (e.g., 2,B) or 'Q' to quit:");
            string input = Console.ReadLine();

            if (input.ToUpper() == "Q")
            {
                Environment.Exit(0); // Quit the game
            }

            string[] tokens = input.Split(',');
            columnLetterToIndex colOption;

            while (tokens.Length != 2 || !int.TryParse(tokens[0], out int row) || !Enum.TryParse(tokens[1], true, out colOption) ||
                   row < 1 || row > i_BoardSize || (int)colOption < 0 || (int)colOption > i_BoardSize - 1)
            {
                Console.WriteLine("Invalid input. Please enter your move in the correct format 'row,column' (e.g., 2,B) and in the correct range:");
                input = Console.ReadLine();
                tokens = input.Split(',');
            }

            int columnAsInt = (int)colOption;
            return Tuple.Create(int.Parse(tokens[0]) - 1, columnAsInt);
        }

        public void DisplayBoard(GameBoard i_Board)
        {
            char[,] board = i_Board.GetBoard();

            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    Console.Write($"| {board[i, j]} ");
                }
                Console.WriteLine("|");
            }
        }

        public void DisplayWinner(string i_Winner)
        {
            Console.WriteLine($"The winner is: {i_Winner}");
        }

        public void PrintBoard(GameBoard i_Board)
        {
            char[,] board = i_Board.GetBoard();
            int boardWidth = board.GetLength(0);
            int boardHeight = board.GetLength(1);
            StringBuilder rowInBoard = new StringBuilder("");
            string cellInBoard;
            int rowNumber = 1;

            PrintABCHeaderInBoard(boardWidth);
            PrintLineBetweenRows(boardWidth);
            for (int i = 0; i < boardWidth; i++)
            {
                Console.Write($"{rowNumber} |");
                for (int j = 0; j < boardHeight; j++)
                {
                    cellInBoard = string.Format(" {0} |", board[i, j]);
                    rowInBoard.Append(cellInBoard);
                }

                Console.WriteLine(rowInBoard);
                rowInBoard.Clear();
                PrintLineBetweenRows(boardWidth);
                rowNumber++;
            }
        }

        public void PrintABCHeaderInBoard(int i_BoardWidth)
        {
            char character = 'A';
            StringBuilder abcHeader = new StringBuilder("");

            for (int i = 0; i < i_BoardWidth; i++)
            {
                abcHeader.AppendFormat("{0}   ", character);
                character++;
            }

            Console.WriteLine($"    {abcHeader}");
        }

        public void PrintLineBetweenRows(int i_BoardWidth)
        {
            StringBuilder lineHeader = new StringBuilder("");

            for (int i = 0; i < i_BoardWidth; i++)
            {
                lineHeader.Append("====");
            }
            lineHeader.Append("=");

            Console.WriteLine($"  {lineHeader}");
        }

        public enum columnLetterToIndex
        {
            A = 0,
            B,
            C,
            D,
            E,
            F,
            G,
            H
        }
    }
}