﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScroll : MonoBehaviour
{
    float maxY;
    float minY;
    float initPos = 0;
    [SerializeField] float scrollSpeed = 0;

    private void Start()
    {
        minY = (Screen.height / 2) + 50;
        maxY = float.MaxValue;
    }

    public void reset()
    {
        transform.position = new Vector3(transform.position.x, minY);
    }

    void Update()
    {
        if (transform.position.y < maxY && gameObject.activeSelf && Input.GetKey(KeyCode.S))
        {
            transform.position += Vector3.up * Time.deltaTime * scrollSpeed;
        }
        if (transform.position.y > minY && gameObject.activeSelf && Input.GetKey(KeyCode.W))
        {
            transform.position += Vector3.down * Time.deltaTime * scrollSpeed;
        }
    }
}