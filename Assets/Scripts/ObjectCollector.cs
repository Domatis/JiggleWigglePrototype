using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class ObjectCollector : MonoBehaviour
{

    private bool movingOn = true;

    private void Start() 
    {
        ShipController.instance.shipMovementAction += CollectorMoving;
        GameplayManager.instance.GameLoseAction += () => {movingOn = false;};
    }
    
    private void OnTriggerEnter(Collider other) 
    {
        // Oyuncunun geçip arkada kalan objelerin collector tarafından testip edilip disable edilmesi.
        if(other.TryGetComponent<PlayerInteractionObject>(out PlayerInteractionObject obj))
        {   
            obj.DisableObject();
        }
    }

    public void CollectorMoving(Vector3 deltamov)
    {
        if(!movingOn) return;
        transform.position = transform.position + deltamov;
    }

    public void OnGameLose()
    {
        movingOn = false;
    }

}
