﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sign : MonoBehaviour
{
    [SerializeField] string[] m_messages;
    int m_activeMessage = 0;
    bool m_inTrigger = false;
    bool m_boxActive = false;

    private void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space) && m_inTrigger)
        {
            m_boxActive = !m_boxActive;
            if (m_boxActive)
            {
                GameObservables.gamePaused = true;
                showMessage();
            }
            else
            {
                GameObservables.gamePaused = false;
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.parent.tag == "Player")
            m_inTrigger = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        m_inTrigger = false;
    }

    public void setMessage(string[] to)
    {
        m_messages = to;
    }

    protected virtual void showMessage()
    {
        MessageQueue.addToQueue(m_messages[m_activeMessage]);
        ++m_activeMessage;
        m_activeMessage %= m_messages.Length;
    }
}
