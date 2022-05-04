using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtectionPickup : PlayerInteractionObject
{
    
    [SerializeField] private PoolingObject poolObjRef;
    [SerializeField] private PoolingObject interactionSoundPoolObj;
    [SerializeField] private float protectionTime = 4f;
    
    public override void OnPlayerInteraction()
    {
         // Player ile etkileşiminde protection'ın başlması.
        PlayerController.instance.ActivatePlayerProtection(protectionTime);
        
        // Pool'dan ses efektini istenmesi.
        GameObject sound = ObjectPoolingManager.instance.RequestFromPool(interactionSoundPoolObj);
        sound.transform.position = transform.position;
        sound.SetActive(true);

        DisableObject();
    }

    // Pool'a gönderilmesi.
    public override void DisableObject()
    {
        ObjectPoolingManager.instance.AddToThePool(gameObject,poolObjRef);
    }

}
