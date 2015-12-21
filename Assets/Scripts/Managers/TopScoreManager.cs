using UnityEngine;

public class TopScoreManager : MonoSingleton<TopScoreManager>
{
    public const int TOTAL_TOPSCORES = 5;

    private string[] m_TopScoreIndex = { "TopScore1", "TopScore2", "TopScore3", "TopScore4", "TopScore5" };

    protected TopScoreManager()
    {
    }

    // Check if new topscore was achieved
    public int CheckForTopScore(int score)
    {
        int scoreRank = -1;

        for (int i = 0; i < TOTAL_TOPSCORES; ++i)
        {
            if (score > PlayerPrefs.GetInt(m_TopScoreIndex[i], 0))
            {
                scoreRank = i;

                // Update list of topscores
                AddTopScore(score, scoreRank);

                break;
            }
        }

        return scoreRank;
    }

    public int GetTopScore(int rank)
    {
        return PlayerPrefs.GetInt(m_TopScoreIndex[rank], 0);
    }

    private void AddTopScore(int score, int scoreRank)
    {
        if (scoreRank < TOTAL_TOPSCORES)
        {
            int oldScore = PlayerPrefs.GetInt(m_TopScoreIndex[scoreRank]);
            PlayerPrefs.SetInt(m_TopScoreIndex[scoreRank], score);

            AddTopScore(oldScore, ++scoreRank);
        }
    }
}