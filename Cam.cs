using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam : MonoBehaviour
{
    public Transform okobot;
    private Vector3 offset;
    void Start()
    {
        offset = okobot.position - transform.position;
    }
    
    void Update()
    {
        FollowPlayer();
    }

    void FollowPlayer()
    {
        Vector3 pos = transform.position;
        pos.z = Mathf.Lerp(pos.z,okobot.position.z - offset.z, 0.5f);
        pos.x = Mathf.Lerp(pos.x,okobot.position.x - offset.x, 0.5f);
        transform.position = pos;
    }
}
