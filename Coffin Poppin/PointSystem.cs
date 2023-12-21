using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PointSystem
{
    static int points;

    public static void ResetPoints()
    {
        points = 0;
    }

    public static void AddPoints(int addedPoints)
    {
        points += addedPoints;
    }

    public static int GetPoints()
    {
        return points;
    }

    public static int GetHighScore()
    {
        return PlayerPrefs.GetInt("HighScore", 0);
    }

    public static void SetHighScore(int pScore)
    {
        if (pScore > PlayerPrefs.GetInt("HighScore", 0))
        {
            PlayerPrefs.SetInt("HighScore", pScore);
        }
    }

    public static void ResetHighScore()
    {
        PlayerPrefs.DeleteKey("HighScore");
    }
}
