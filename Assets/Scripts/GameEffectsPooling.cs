using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Aktif edildikten belli bir süre sonra kendini poola gönderen objeler efektler,soundlar.
public class GameEffectsPooling : MonoBehaviour
{
    [SerializeField] private PoolingObject poolObjRef;  //Kendi pool object referansı
    [SerializeField] private float timeToSendPool = 2f;


    private float timer = 0;

    private void OnEnable() 
    {
        timer = 0;
    }

    
    private void Update() 
    {

        timer += Time.deltaTime;
        if(timer >= timeToSendPool)
        {
            ObjectPoolingManager.instance.AddToThePool(gameObject,poolObjRef);
        }
    }
}
