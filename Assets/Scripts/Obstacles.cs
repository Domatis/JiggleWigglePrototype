using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacles : PlayerInteractionObject
{
    
    [SerializeField] protected PoolingObject poolObjRef;    // Kendi pool referansı.
    [SerializeField] protected PoolingObject interactionEffectPoolObj;  // Player etkileşiminde particle effectinin pool referansı
    [SerializeField] protected PoolingObject interactionSoundPoolObj;   // Player etkileşiminde sound effectinin pool referansı
    [SerializeField] protected GameObject particlePos;
    
    public override void OnPlayerInteraction()
    {
        // Oyuncu enemy ile etkileşime girince direkt ölme fonksiyonu çağırılacak.
        PlayerController.instance.PlayerDie();

            // Particle efektinin poolingden alınıp kullanılması.
            GameObject particle = ObjectPoolingManager.instance.RequestFromPool(interactionEffectPoolObj);
            particle.transform.position = particlePos.transform.position;
            particle.SetActive(true);

            // Sound efektinin pool'dan alınıp kullanılması.
            GameObject sound = ObjectPoolingManager.instance.RequestFromPool(interactionSoundPoolObj);
            sound.transform.position = transform.position;
            sound.SetActive(true);

        DisableObject();
    }

    public override void DisableObject()
    {   
        // Objenin pool'a gönderilmesi.
        ObjectPoolingManager.instance.AddToThePool(gameObject,poolObjRef);
    }
    
}
