using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement : MonoBehaviour
{
    public float speed;
    private Rigidbody2D body;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.D))
        {
            body.linearVelocity = new Vector2(speed,body.linearVelocity.y);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            body.linearVelocity = new Vector2(-speed,body.linearVelocity.y);
        }
    }
}