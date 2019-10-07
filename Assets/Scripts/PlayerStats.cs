using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    //todo: rename to just stats?
    //todo: make monster stat seperate?

    public float baseDamage = 1;
    public float damageModifier = 0;
    public List<float> multiplicativeDamageMultipliers = new List<float>();
    public float GetActualDamage()
    {
        float multiplier = 1.0f;
        foreach (float mult in multiplicativeDamageMultipliers)
        {
            multiplier *= 1.0f + mult;
        }
        return (baseDamage + damageModifier) * multiplier;
    }

    public float baseMoveSpeed = 50;
    public float moveSpeedModifier = 0;
    public List<float> multiplicateMoveSpeedMultipliers = new List<float>();
    public float GetActualMovespeed()
    {
        float multiplier = 1.0f;
        foreach (float mult in multiplicateMoveSpeedMultipliers)
        {
            multiplier *= 1.0f + mult;
        }

        return Mathf.Max((baseMoveSpeed + moveSpeedModifier) * multiplier, 5);
    }

    public float baseMaxHealth = 10;
    float maxHealthModifier = 0;
    public void maxHealthMod(float by)
    {
        maxHealthModifier += by;
        if (by > 0)
            currentHealth += by;
    }
    public List<float> multiplicateMaxHealthMultipliers = new List<float>();
    public float GetActualMaxHealth()
    {
        float multiplier = 1.0f;
        foreach (float mult in multiplicateMaxHealthMultipliers)
        {
            multiplier *= 1.0f + mult;
        }

        return Mathf.Max((baseMaxHealth + maxHealthModifier) * multiplier, 1);
    }

    public float currentHealth = 10;
    public void setCurrentHealth(float to) { currentHealth = to; }
    public float getCurrentHealth() { return currentHealth; }
    public void changeCurrentHealth(float by)
    {
        currentHealth += by;
        currentHealth = Mathf.Min(currentHealth, GetActualMaxHealth());
    }

    public float baseWeaponRange = 20;
    public float weaponRangeModifier = 0;
    public List<float> multiplicativeWeaponRangeModifier = new List<float>();
    public float GetActualWeaponRange()
    {
        float multiplier = 1.0f;
        foreach (float mult in multiplicativeWeaponRangeModifier)
        {
            multiplier *= 1.0f + mult;
        }

        return (baseWeaponRange + weaponRangeModifier) * multiplier;
    }

    public float baseArmour = 0;
    public float armourModifier = 0;
    public List<float> multiplicativeArmourModifier = new List<float>();
    public float GetActualArmour()
    {
        float multiplier = 1.0f;
        foreach (float mult in multiplicativeArmourModifier)
        {
            multiplier *= 1.0f + mult;
        }

        return (baseArmour + armourModifier) * multiplier;
    }

    public enum Status
    {
        Fine,
        Fire,
        Poison,
    }

    public Status status;
}
