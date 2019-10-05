using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    public GameObject target;

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
    }
    private void LateUpdate()
    {
        transform.position = new Vector3(target.transform.position.x, target.transform.position.y, -10);
    }
}
