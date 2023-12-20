using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Progression
{
    static int runExp;
    static int progressionLevel;
    public static int nextLevelUp = 10;
    static float spawnTime;

    public static void SetExp(int exp)
    {
        runExp += exp;
    }

    public static int GetExp()
    {
        return runExp;
    }

    public static int GetProgressionLevel()
    {
        return progressionLevel;
    }

    public static void LevelUp()
    {
        runExp = 0;
        progressionLevel++;
        GameManager.Instance.UpdateExpSlider();
    }

    public static void FreshRun()
    {
        runExp = 0;
        progressionLevel = 0;
    }

    public static void SetSpawnSettings()
    {

    }

}
