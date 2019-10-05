using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animator : MonoBehaviour
{
    public Sprite[] frames;
    public float fps = 10.0f;

    private int currentFrame = 0;
    private float timer = 0;
    private float delay = 0;

    private void Start()
    {
        SetFPS(fps);
    }

    public void SetFPS(float FPS)
    {
        if (fps == 0) delay = 0;

        delay = 1.0f / fps;
    }

    public void NextFrame()
    {
        if (currentFrame == frames.Length - 1)
        {
            currentFrame = 0;
        }
        else
        {
            currentFrame++;
        }

        GetComponent<SpriteRenderer>().sprite = frames[currentFrame];
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > delay)
        {
            timer -= delay;
            NextFrame();
        }
    }

    private void OnDisable()
    {
        currentFrame = 0;
        timer = 0;
        GetComponent<SpriteRenderer>().sprite = frames[currentFrame];
    }
}
