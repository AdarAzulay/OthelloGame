using System;
using System.Threading;

namespace OthelloGame
{
    public class GameManager
    {
        private char m_firstTypeOFToken = 'X';
        private char m_secondTypeOfToken = 'O';
        private ConsoleUI m_consoleUI = new ConsoleUI();

        public void Start()
        {
            bool playAgain = true;

            while (playAgain)
            {
                PlayGame();
                Console.WriteLine("Do you want to play again? (Y/N)");
                string response = Console.ReadLine()?.ToUpper();

                while (response != "Y" && response != "N")
                {
                    Console.WriteLine("Invalid input. Please enter 'Y' for Yes or 'N' for No:");
                    response = Console.ReadLine()?.ToUpper();
                }

                if (response == "N")
                {
                    playAgain = false;
                    Console.WriteLine("Thank you for playing! Goodbye.");
                }
            }
        }

        private void PlayGame()
        {
            Console.WriteLine("Enter the name of Player 1:");
            string player1Name = Console.ReadLine();
            while (string.IsNullOrWhiteSpace(player1Name))
            {
                Console.WriteLine("Invalid input. Please enter a valid name:");
                player1Name = Console.ReadLine();
            }

            Player player1 = new Player(player1Name, m_firstTypeOFToken);

            Console.WriteLine("Do you want to play against another player (P) or the computer (C)?");
            string opponentChoice = Console.ReadLine()?.ToUpper();

            while (opponentChoice != "P" && opponentChoice != "C")
            {
                Console.WriteLine("Invalid input. Please enter 'P' for another player or 'C' for computer:");
                opponentChoice = Console.ReadLine()?.ToUpper();
            }

            AIPlayer aiPlayer = null;
            Player player2 = null;

            if (opponentChoice == "P")
            {
                Console.WriteLine("Enter the name of Player 2:");
                string player2Name = Console.ReadLine();

                while (string.IsNullOrWhiteSpace(player2Name))
                {
                    Console.WriteLine("Invalid input. Please enter a valid name:");
                    player2Name = Console.ReadLine();
                }
                player2 = new Player(player2Name, m_secondTypeOfToken);
            }
            else
            {
                aiPlayer = new AIPlayer("Computer", m_secondTypeOfToken);
            }

            int boardSize = m_consoleUI.GetBoardSize();
            GameBoard board = new GameBoard(boardSize, boardSize);

            board.InitializeBoard(m_firstTypeOFToken, m_secondTypeOfToken);
            GameLogic game = new GameLogic(board, player1, aiPlayer == null ? player2 : new Player(aiPlayer.PlayerName, aiPlayer.PlayerToken));
            bool player1CanMove, player2CanMove;

            while (!game.IsGameOver(m_firstTypeOFToken, m_secondTypeOfToken))
            {
                ClearScreen();
                m_consoleUI.PrintBoard(board);
                Tuple<int, int> move;
                bool validMove = false;

                player1CanMove = board.AnyValidMoves(player1.PlayerToken, m_firstTypeOFToken, m_secondTypeOfToken);
                player2CanMove = board.AnyValidMoves(aiPlayer?.PlayerToken ?? player2.PlayerToken, m_firstTypeOFToken, m_secondTypeOfToken);

                if (!player1CanMove && !player2CanMove)
                {
                    break;
                }

                if (game.CurrentPlayer == player1 && player1CanMove)
                {
                    while (!validMove)
                    {
                        try
                        {
                            move = m_consoleUI.GetPlayerMove(player1, boardSize);
                            game.ApplyMove(move.Item1, move.Item2, m_firstTypeOFToken, m_secondTypeOfToken);
                            Console.WriteLine($"{player1.PlayerName} made a move at ({move.Item1 + 1},{(OthelloGame.ConsoleUI.columnLetterToIndex)(move.Item2)})");
                            validMove = true;
                            Thread.Sleep(1000);
                        }
                        catch (InvalidOperationException ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
                else if (player2CanMove)
                {
                    while (!validMove)
                    {
                        try
                        {
                            if (aiPlayer != null)
                            {
                                Tuple<int, int> aiMove = aiPlayer.ChooseMove(board, m_firstTypeOFToken, m_secondTypeOfToken);
                                move = aiMove;
                                game.ApplyMove(move.Item1, move.Item2, m_firstTypeOFToken, m_secondTypeOfToken);
                                Console.WriteLine($"The Computer played its turn at ({move.Item1 + 1},{(OthelloGame.ConsoleUI.columnLetterToIndex)move.Item2})");
                                Thread.Sleep(1500);
                            }
                            else
                            {
                                move = m_consoleUI.GetPlayerMove(player2, boardSize);
                                game.ApplyMove(move.Item1, move.Item2, m_firstTypeOFToken, m_secondTypeOfToken);
                                Console.WriteLine($"{player2.PlayerName} played its turn at ({move.Item1 + 1},{(OthelloGame.ConsoleUI.columnLetterToIndex)move.Item2})");
                            }
                            validMove = true;
                        }
                        catch (InvalidOperationException ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"{game.CurrentPlayer.PlayerName} has no valid moves. Skipping turn.");
                }

                game.SwitchTurn();
            }

            string winner = game.GetWinner();
            m_consoleUI.DisplayWinner(winner);
        }

        private static void ClearScreen()
        {
            #if WINDOWS
                Ex02.ConsoleUtils.Screen.Clear();
            #else
                Console.Clear();
            #endif
        }
    }
}
