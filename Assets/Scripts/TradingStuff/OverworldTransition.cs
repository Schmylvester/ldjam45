using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldTransition : MonoBehaviour
{
    [SerializeField] GameObject toHide;
    [SerializeField] GameObject toShow;
    [SerializeField] Vector3 playerStart;
    [SerializeField] GameObject player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.parent.gameObject == player)
        {
            toHide.SetActive(false);
            toShow.SetActive(true);
            player.transform.position = playerStart;
        }
    }
}
