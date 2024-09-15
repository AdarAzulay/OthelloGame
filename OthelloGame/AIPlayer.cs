using System;
using System.Collections.Generic;

namespace OthelloGame
{
    public class AIPlayer
    {
        private readonly Random m_RandomGenerator;
        private Player m_Player;

        public AIPlayer(string i_PlayerName, char i_PlayerToken)
        {
            m_Player = new Player(i_PlayerName, i_PlayerToken);  // Use composition to hold player data
            m_RandomGenerator = new Random();
        }

        public char PlayerToken => m_Player.PlayerToken;
        public string PlayerName => m_Player.PlayerName;

        public Tuple<int, int> ChooseMove(GameBoard i_Board, char firstTypeOfToken, char secondTypeOfToken)
        {
            List<Tuple<int, int, int>> validMovesWithScores = new List<Tuple<int, int, int>>();

            for (int row = 0; row < i_Board.Rows; row++)
            {
                for (int col = 0; col < i_Board.Cols; col++)
                {
                    if (i_Board.IsMoveValid(row, col, this.PlayerToken, firstTypeOfToken, secondTypeOfToken))
                    {
                        int moveScore = i_Board.GetMoveScore(row, col, this.PlayerToken, firstTypeOfToken, secondTypeOfToken);
                        validMovesWithScores.Add(Tuple.Create(row, col, moveScore));
                    }
                }
            }

            if (validMovesWithScores.Count == 0)
            {
                throw new InvalidOperationException("No valid moves available for the AI.");
            }

            validMovesWithScores.Sort((move1, move2) => move2.Item3.CompareTo(move1.Item3));
            var bestMove = validMovesWithScores[0];

            return Tuple.Create(bestMove.Item1, bestMove.Item2);
        }
    }
}