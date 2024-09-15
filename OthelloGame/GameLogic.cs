using System;

namespace OthelloGame
{
    public class GameLogic
    {
        private readonly GameBoard m_GameBoard;
        public Player CurrentPlayer { get; private set; }
        public Player Player1 { get; }
        public Player Player2 { get; }
        public GameBoard Board => m_GameBoard;

        public GameLogic(GameBoard i_GameBoard, Player i_Player1, Player i_Player2)
        {
            m_GameBoard = i_GameBoard;
            Player1 = i_Player1;
            Player2 = i_Player2;
            CurrentPlayer = Player1; // Assume player 1 starts
        }

        public bool IsGameOver(char firstTypeOfToken, char secondTypeOfToken)
        {
            return !m_GameBoard.AnyValidMoves(Player1.PlayerToken, firstTypeOfToken, secondTypeOfToken) &&
                   !m_GameBoard.AnyValidMoves(Player2.PlayerToken, firstTypeOfToken, secondTypeOfToken);
        }

        public string GetWinner()
        {
            int player1Tokens = m_GameBoard.CountTokens(Player1.PlayerToken);
            int player2Tokens = m_GameBoard.CountTokens(Player2.PlayerToken);

            if (player1Tokens > player2Tokens)
            {
                return Player1.PlayerName;
            }
            else if (player2Tokens > player1Tokens)
            {
                return Player2.PlayerName;
            }
            else
            {
                return "It's a tie!";
            }
        }

        public void ApplyMove(int i_Row, int i_Col, char firstTypeOfToken, char secondTypeOfToken)
        {
            if (!m_GameBoard.IsMoveValid(i_Row, i_Col, CurrentPlayer.PlayerToken, firstTypeOfToken, secondTypeOfToken))
            {
                throw new InvalidOperationException("Invalid move. Please choose a valid position that flips opponent's tokens.");
            }

            m_GameBoard.ApplyMove(i_Row, i_Col, CurrentPlayer.PlayerToken, firstTypeOfToken, secondTypeOfToken);
        }

        public void SwitchTurn()
        {
            CurrentPlayer = (CurrentPlayer == Player1) ? Player2 : Player1;
        }
    }
}
