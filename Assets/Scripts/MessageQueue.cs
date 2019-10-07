using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageQueue : MonoBehaviour
{
    static List<string> messageQueue = new List<string>();
    [SerializeField] Text text;
    [SerializeField] Image box;

    void Update()
    {
        if (messageQueue.Count > 0)
        {
            box.enabled = true;
            text.enabled = true;
            text.text = messageQueue[0];
            GameObservables.gamePaused = true;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                GameObservables.gamePaused = false;
                messageQueue.RemoveAt(0);
                box.enabled = false;
                text.enabled = false;
            }
        }
    }

    public static void addToQueue(string message)
    {
        messageQueue.Add(message);
    }
}
