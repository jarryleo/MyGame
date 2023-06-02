using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikedBall : MonoBehaviour
{

    private Rigidbody2D rb;
    [Tooltip("°Ú´¸³õÊ¼Á¦¶È")]
    public float startTorque = 1000;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.AddTorque(startTorque);
    }
}
