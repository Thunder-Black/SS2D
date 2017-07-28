using UnityEngine;
using System.Collections;

public class SlightlyMove : MonoBehaviour
{
    public Vector3 vec;
    public Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        rb.AddForce(vec);
    }
}
