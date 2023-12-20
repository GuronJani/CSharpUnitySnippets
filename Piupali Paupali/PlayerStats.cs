using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerStats
{

    // Upgrading stats -> first get value, then add to it in the set value parameters

    // MAX HEALTH
    public static int GetMaxHealth()
    {
        return PlayerPrefs.GetInt("maxHealth", 100);
    }
    public static void SetMaxHealth(int newMaxHealth)
    {
        PlayerPrefs.SetInt("maxHealth", newMaxHealth);
    }

    // DAMAGE MULTIPLIER
    public static float GetDamageMultiplier()
    {
        return PlayerPrefs.GetFloat("damageMultiplier", 1f);
    }
    public static void SetDamageMultiplier(float newDamageMultiplier)
    {
        PlayerPrefs.SetFloat("damageMultiplier", newDamageMultiplier);
    }

    // COOLDOWN MULTIPLIER
    public static float GetCooldownMultiplier()
    {
        return PlayerPrefs.GetFloat("damageMultiplier", 1f);
    }
    public static void SetCooldownMultiplier(float newCooldownMultiplier)
    {
        PlayerPrefs.SetFloat("cooldownMultiplier", newCooldownMultiplier);
    }

    // MOVE SPEED
    public static float GetMoveSpeed()
    {
        return PlayerPrefs.GetFloat("moveSpeed", 1f);
    }
    public static void SetMoveSpeed(float newMoveSpeed)
    {
        PlayerPrefs.SetFloat("moveSpeed", newMoveSpeed);
    }

    // PROJECTILE SPEED
    public static float GetProjectileSpeed()
    {
        return PlayerPrefs.GetFloat("projectileSpeed", 1f);
    }
    public static void SetProjectileSpeed(float newProjectileSpeed)
    {
        PlayerPrefs.SetFloat("projectileSpeed", newProjectileSpeed);
    }

    // This method will clear all progression and put starting values in
    public static void ResetAllStats()
    {
        PlayerPrefs.SetInt("maxHealth", 100);
        PlayerPrefs.SetFloat("damageMultiplier", 1f);
        PlayerPrefs.SetFloat("cooldownMultiplier", 1f);
        PlayerPrefs.SetFloat("moveSpeed", 5f);
        PlayerPrefs.SetFloat("projectileSpeed", 1f);
    }
}
