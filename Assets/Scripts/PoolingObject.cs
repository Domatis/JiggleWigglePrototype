using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/* Scriptable object oluşturulan nesneler editor'da oluşturulduğunda tekil instanceId'si oluşuyor ve
   bu id objele için referans olarak kullanılıyor.
*/
[CreateAssetMenu(fileName = "PoolingObject", menuName = "PoolingObject", order = 3)]
public class PoolingObject : ScriptableObject   
{     
        public GameObject prefab;       // Pool objesinin örnek prefabi.
}

