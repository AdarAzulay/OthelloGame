namespace OthelloGame
{
    public class Player
    {
        private readonly string m_PlayerName;
        private readonly char m_PlayerToken;

        public Player(string i_PlayerName, char i_PlayerToken)
        {
            m_PlayerName = i_PlayerName;
            m_PlayerToken = i_PlayerToken;
        }

        public string PlayerName
        {
            get { return m_PlayerName; }
        }

        public char PlayerToken
        {
            get { return m_PlayerToken; }
        }
    }
}