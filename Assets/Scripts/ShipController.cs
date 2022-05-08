using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    public static ShipController instance;

    public Action<Vector3> shipMovementAction;

    [SerializeField] private float speed;
    private Rigidbody rb;
    private Vector3 lastPos;
    
    public float shipSpeed => speed;

    private void Awake() 
    {
        instance = this;
        rb = GetComponent<Rigidbody>();
    }

    private void Start() 
    {
        lastPos  = transform.position;
    }

    private void Update() 
    {
        // Geminin hıznın set edilmesi ve aldığı mesafenin hesaplanması.
        rb.velocity = Vector3.forward * speed;
        Vector3 deltaDistance = transform.position - lastPos;   //Calculate delta distance.
        shipMovementAction?.Invoke(deltaDistance);
        lastPos = transform.position;
    }
}
