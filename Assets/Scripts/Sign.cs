using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sign : MonoBehaviour
{
    [SerializeField] GameObject m_messageBox = null;
    [SerializeField] UnityEngine.UI.Text m_messageText = null;
    [SerializeField] string[] m_messages;
    int m_activeMessage = 0;
    bool m_inTrigger = false;
    bool m_boxActive = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && m_inTrigger)
        {
            m_boxActive = !m_boxActive;
            if (m_boxActive)
            {
                showMessage();
            }
            else
            {
                GameObservables.gamePaused = false;
                m_messageBox.SetActive(false);
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
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
        GameObservables.gamePaused = true;
        m_messageBox.SetActive(true);
        m_messageText.text = m_messages[m_activeMessage];
        ++m_activeMessage;
        m_activeMessage %= m_messages.Length;
    }
}
