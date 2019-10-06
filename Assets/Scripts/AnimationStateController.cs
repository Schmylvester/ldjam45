using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationStateController : MonoBehaviour
{
    private List<GameObject> children = new List<GameObject>();
    private GameObject activeState;

    private void Awake()
    {
        for (int i = 0; i < transform.childCount; ++i)
        {
            children.Add(transform.GetChild(i).gameObject);
            children[i].SetActive(false);
        }

        children[0].SetActive(true);
        activeState = children[0];
    }

    public bool StateExists(string stateName)
    {
        foreach (GameObject child in children)
        {
            if (child.name == stateName)
            {
                return true;
            }
        }

        return false;
    }

    public void SetState(string stateName)
    {
        foreach (GameObject child in children)
        {
            if (child.name == stateName)
            {
                if (activeState) activeState.SetActive(false);
                activeState = child;
                activeState.SetActive(true);
                return;
            }
        }

        //Debug.LogError("Invalid state " + stateName);
    }

    public void ChangeFPS(float fps)
    {
        activeState.GetComponent<Animator>().SetFPS(fps);
    }

    public void NextFrame(float fps)
    {
        activeState.GetComponent<Animator>().NextFrame();
    }

    public string GetState()
    {
        return activeState.name;
    }
}
