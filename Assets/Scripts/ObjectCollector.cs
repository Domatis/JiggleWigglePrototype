using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider),typeof(Rigidbody))]
public class ObjectCollector : MonoBehaviour
{

    private bool movingOn = true;

    private Rigidbody rb;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
    }

    private void Start() 
    {
        GetComponent<BoxCollider>().isTrigger = true;
        GameplayManager.instance.GameLoseAction += () => 
        {
            movingOn = false;
            rb.velocity = Vector3.zero;
        };
    }
    
    private void Update() 
    {
        if(!movingOn) return;
        rb.velocity = Vector3.forward * ShipController.instance.shipSpeed;
    }


    private void OnTriggerEnter(Collider other) 
    {
        // Oyuncunun geçip arkada kalan objelerin collector tarafından testip edilip disable edilmesi.
        if(other.TryGetComponent<PlayerInteractionObject>(out PlayerInteractionObject obj))
        {   
            obj.DisableObject();
        }
    }

    public void OnGameLose()
    {
        movingOn = false;
    }

}
