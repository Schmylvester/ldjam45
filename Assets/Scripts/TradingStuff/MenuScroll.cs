using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScroll : MonoBehaviour
{
    float initPos = 0;
    [SerializeField] float scrollSpeed = 0;
    [SerializeField] bool hz = false;


    void Update()
    {
        if (hz)
        {
            if (gameObject.activeSelf && Input.GetKey(KeyCode.D))
            {
                transform.position += Vector3.left * Time.deltaTime * scrollSpeed;
            }
            if (gameObject.activeSelf && Input.GetKey(KeyCode.A))
            {
                transform.position += Vector3.right * Time.deltaTime * scrollSpeed;
            }
        }
        else
        {
            if (gameObject.activeSelf && Input.GetKey(KeyCode.S))
            {
                transform.position += Vector3.up * Time.deltaTime * scrollSpeed;
            }
            if (gameObject.activeSelf && Input.GetKey(KeyCode.W))
            {
                transform.position += Vector3.down * Time.deltaTime * scrollSpeed;
            }
        }
    }
}