using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Player ile etkileşime geçecek objeler için ana class.
public abstract class PlayerInteractionObject : MonoBehaviour
{


    protected virtual void OnEnable() 
    {
        GameplayManager.instance.GameWinAction += DisableObject;    // Oyun kazanılması durumunda aktif objelerin disable olması.
    }

    protected virtual void OnDisable() 
    {
        GameplayManager.instance.GameWinAction -= DisableObject;    
    }

    public abstract void OnPlayerInteraction();     // Player etkileşiminde çağırılacak fonksiyon.
    public abstract void DisableObject();

}
