using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EnemyProjectile : PlayerInteractionObject
{
    [SerializeField] private PoolingObject poolObjRef;  //Kendi pool object referansı
    [SerializeField] private float speed;


    private Rigidbody rb;
    private Vector3 direction;
    private bool projectileReady;

    private void Awake() 
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update() 
    {
        if(projectileReady)
        {
            rb.velocity = direction * speed;
            transform.forward = direction;
        }
    }

    // Player etkileşimi
    public override void OnPlayerInteraction()
    {
        PlayerController.instance.PlayerDie();
        DisableObject();
    }


    public override void DisableObject()
    {
        //Disable olması ve poola gönderilmesi.
        projectileReady = false;
        ObjectPoolingManager.instance.AddToThePool(gameObject,poolObjRef);
    }
    
    public void SetDirection(Vector3 dir)
    {
        direction = dir;
        projectileReady = true;
    }
}
