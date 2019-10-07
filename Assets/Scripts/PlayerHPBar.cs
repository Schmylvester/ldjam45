using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHPBar : MonoBehaviour
{
    PlayerStats stats;

    float originalWidth = 0;
    private void Start()
    {
        stats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
        originalWidth = GetComponent<RectTransform>().rect.width;
    }

    private void Update()
    {
        stats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();

        float playerHpPercent = 0;
        if (stats.currentHealth > 0)
        {
            playerHpPercent = stats.currentHealth / stats.GetActualMaxHealth();
        }

        Vector3 scale = GetComponent<RectTransform>().transform.localScale;
        scale.x = playerHpPercent;
        GetComponent<RectTransform>().transform.localScale = scale;
    }
}
