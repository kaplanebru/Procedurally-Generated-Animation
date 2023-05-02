using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{


    public float speed = 0.01f;
    void FixedUpdate()
    {
        Utilities.CheckGround(transform, true);
        Move();
    }
    

    void Move()
    {
        transform.position += new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * speed;
    }
}
