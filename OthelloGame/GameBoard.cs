using System;

namespace OthelloGame
{
    public class GameBoard
    {
        private readonly char[,] m_Board;
        private readonly int m_Rows;
        private readonly int m_Cols;
        private readonly int[] m_Directions = { -1, 0, 1 }; // To check in all directions

        public GameBoard(int i_Rows, int i_Cols)
        {
            m_Rows = i_Rows;
            m_Cols = i_Cols;
            m_Board = new char[m_Rows, m_Cols];
        }
        
        public int Rows => m_Rows;

        public int Cols => m_Cols;

        public void InitializeBoard(char i_TokenPlayer1, char i_TokenPlayer2)
        {
            for (int i = 0; i < m_Rows; i++)
            {
                for (int j = 0; j < m_Cols; j++)
                {
                    m_Board[i, j] = ' '; // Empty value
                }
            }

            // Initialize 4 tokens in the center
            int middleRow = m_Rows / 2 - 1;
            int middleCol = m_Cols / 2 - 1;
            m_Board[middleRow, middleCol] = i_TokenPlayer2;
            m_Board[middleRow + 1, middleCol + 1] = i_TokenPlayer2;
            m_Board[middleRow, middleCol + 1] = i_TokenPlayer1;
            m_Board[middleRow + 1, middleCol] = i_TokenPlayer1;
        }

        public char[,] GetBoard()
        {
            return m_Board;
        }

        public bool IsMoveValid(int i_Row, int i_Col, char i_PlayerToken, char i_FirstTypeOfToken, char i_SecondTypeOfToken)
        {
            if (i_Row < 0 || i_Row >= m_Rows || i_Col < 0 || i_Col >= m_Cols)
            {
                return false; // Out of bounds
            }

            if (m_Board[i_Row, i_Col] != ' ')
            {
                return false; // Space is already occupied
            }

            foreach (int dRow in m_Directions)
            {
                foreach (int dCol in m_Directions)
                {
                    if (dRow == 0 && dCol == 0) continue; // Skip the current cell
                    if (isBlockingOpponent(i_Row, i_Col, dRow, dCol, i_PlayerToken, i_FirstTypeOfToken, i_SecondTypeOfToken))
                    {
                        return true; // Valid move if it can flip opponent's tokens
                    }
                }
            }

            return false;
        }

        private bool isBlockingOpponent(int i_Row, int i_Col, int i_DRow, int i_DCol, char i_PlayerToken, char i_FirstTypeOfToken, char i_SecondTypeOfToken) 
        {
            int row = i_Row + i_DRow;
            int col = i_Col + i_DCol;
            char opponentToken = (i_PlayerToken == i_FirstTypeOfToken) ? i_SecondTypeOfToken : i_FirstTypeOfToken;
            bool foundOpponent = false;

            // Traverse in the given direction to look for opponent's tokens.
            while (row >= 0 && row < m_Rows && col >= 0 && col < m_Cols) 
            {
                if (m_Board[row, col] == opponentToken) 
                {
                    foundOpponent = true;
                } 
                else if (m_Board[row, col] == i_PlayerToken && foundOpponent) 
                {
                    return true; // Found player's token after opponent's token(s)
                } 
                else 
                {
                    return false; // Invalid move
                }

                row += i_DRow;
                col += i_DCol;
            }

            return false;
        }

        public void ApplyMove(int i_Row, int i_Col, char i_PlayerToken, char i_FirstTypeOfToken, char i_SecondTypeOfToken)
        {
            if (i_Row < 0 || i_Row >= m_Board.GetLength(0) || i_Col < 0 || i_Col >= m_Board.GetLength(1))
            {
                throw new IndexOutOfRangeException("Attempted to apply a move outside the bounds of the board.");
            }

            m_Board[i_Row, i_Col] = i_PlayerToken;

            foreach (int dRow in m_Directions)
            {
                foreach (int dCol in m_Directions)
                {
                    if (dRow == 0 && dCol == 0) continue;

                    if (isBlockingOpponent(i_Row, i_Col, dRow, dCol, i_PlayerToken, i_FirstTypeOfToken, i_SecondTypeOfToken))
                    {
                        flipOpponentTokens(i_Row, i_Col, dRow, dCol, i_PlayerToken, i_FirstTypeOfToken, i_SecondTypeOfToken);
                    }
                }
            }
        }

        public int GetMoveScore(int i_Row, int i_Col, char i_PlayerToken, char i_FirstTypeOfToken, char i_SecondTypeOfToken)
        {
            int score = 0;
            char opponentToken = (i_PlayerToken == i_FirstTypeOfToken) ? i_SecondTypeOfToken : i_FirstTypeOfToken;

            foreach (int dRow in m_Directions)
            {
                foreach (int dCol in m_Directions)
                {
                    if (dRow == 0 && dCol == 0) continue;

                    int row = i_Row + dRow;
                    int col = i_Col + dCol;
                    int currentScore = 0;

                    while (row >= 0 && row < m_Rows && col >= 0 && col < m_Cols && m_Board[row, col] == opponentToken)
                    {
                        currentScore++;
                        row += dRow;
                        col += dCol;
                    }

                    if (row >= 0 && row < m_Rows && col >= 0 && col < m_Cols && m_Board[row, col] == i_PlayerToken)
                    {
                        score += currentScore;
                    }
                }
            }

            return score;
        }

        private void flipOpponentTokens(int i_Row, int i_Col, int i_DRow, int i_DCol, char i_PlayerToken, char i_FirstTypeOfToken, char i_SecondTypeOfToken)
        {
            int row = i_Row + i_DRow;
            int col = i_Col + i_DCol;

            char opponentToken = (i_PlayerToken == i_FirstTypeOfToken) ? i_SecondTypeOfToken : i_FirstTypeOfToken;

            while (row >= 0 && row < m_Rows && col >= 0 && col < m_Cols && m_Board[row, col] == opponentToken)
            {
                m_Board[row, col] = i_PlayerToken; // Flip the token
                row += i_DRow;
                col += i_DCol;
            }
        }

        public bool AnyValidMoves(char i_PlayerToken, char i_FirstTypeOfToken, char i_SecondTypeOfToken)
        {
            for (int i = 0; i < m_Rows; i++)
            {
                for (int j = 0; j < m_Cols; j++)
                {
                    if (IsMoveValid(i, j, i_PlayerToken, i_FirstTypeOfToken, i_SecondTypeOfToken))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public int CountTokens(char i_PlayerToken)
        {
            int count = 0;
            for (int i = 0; i < m_Rows; i++)
            {
                for (int j = 0; j < m_Cols; j++)
                {
                    if (m_Board[i, j] == i_PlayerToken)
                    {
                        count++;
                    }
                }
            }

            return count;
        }
    }
}