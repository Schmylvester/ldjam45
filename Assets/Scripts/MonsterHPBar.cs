using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterHPBar : MonoBehaviour
{
    PlayerStats stats;

    float originalWidth = 0;
    private void Start()
    {
        stats = transform.parent.parent.GetComponent<PlayerStats>();
        originalWidth = GetComponent<RectTransform>().rect.width;
    }

    private void Update()
    {
        float monsterHPPercent = 0;
        if (stats.currentHealth > 0)
        {
            monsterHPPercent = stats.currentHealth / stats.GetActualMaxHealth();
        }

        Vector3 scale = GetComponent<RectTransform>().transform.localScale;
        scale.x = monsterHPPercent;
        GetComponent<RectTransform>().transform.localScale = scale;
    }
}
