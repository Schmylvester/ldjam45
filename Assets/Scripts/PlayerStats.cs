using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    static float baseDamage = 1;
    float damageModifier = 0;
    List<float> multiplicativeDamageMultipliers = new List<float>();
    public float GetActualDamage()
    {
        float multiplier = 1.0f;
        foreach (float mult in multiplicativeDamageMultipliers)
        {
            multiplier *= 1.0f + mult;
        }

        return (baseDamage + damageModifier) * multiplier;
    }

    static float baseMoveSpeed = 50;
    float moveSpeedModifier = 0;
    List<float> multiplicateMoveSpeedMultipliers = new List<float>();
    public float GetActualMovespeed()
    {
        float multiplier = 1.0f;
        foreach (float mult in multiplicateMoveSpeedMultipliers)
        {
            multiplier *= 1.0f + mult;
        }

        return (baseMoveSpeed + moveSpeedModifier) * multiplier;
    }
}
