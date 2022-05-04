using System.Collections;
using System.Collections.Generic;
using UnityEngine;
    
    public class ObjectPoolingManager : MonoBehaviour
    {
            public static ObjectPoolingManager instance;


            /* Pooling object scriptable objesinin tekil instance id'si kullanılarak oluşturlan pool dizileri */
            Dictionary<int,Queue<GameObject>> poolDictionary = new Dictionary<int, Queue<GameObject>>();

            private void Awake() 
            {
                instance = this;
            }

            // Pooldan obje talebi.
            public GameObject RequestFromPool(PoolingObject poolObj)
            {
                // Öncelikle poolobjesinin id'si ile daha önce oluşturulup oluşturulmadığının kontrolü.
                if(poolDictionary.ContainsKey(poolObj.GetInstanceID()))
                {          
                    //Eğer obje için pool var ise, içinde nesne var mı yok mu kontrol edilmesi.
                    if(poolDictionary[poolObj.GetInstanceID()].Count > 0)      
                    {
                        //Var ise sıradaki objenin pooldan çıkarılıp döndürülmesi.
                        return poolDictionary[poolObj.GetInstanceID()].Dequeue();
                    }   

                    else    
                    {
                        // Pool içinde nesne kalmamışsa, aynı türde nesne oluşturulup onun döndürülmesi.
                        return Instantiate(poolObj.prefab,transform.position,poolObj.prefab.transform.rotation);
                    }                 

                }
                else // Eğer bu obje için pool oluşturulmamışsa pool oluşturulup dictionary'e eklendikten sonra aynı türde nesne üretilip döndürülmesi.
                {
                    Queue<GameObject> newStack = new Queue<GameObject>();
                    poolDictionary.Add(poolObj.GetInstanceID(),newStack);

                    return Instantiate(poolObj.prefab,transform.position,poolObj.prefab.transform.rotation);
                }

            }

            // Objenin poola eklenmesi.
            public void AddToThePool(GameObject obj,PoolingObject poolObj)  // Objenin kendisi , objenin pool referansı.
            {
                obj.transform.parent = null;    

                // Önce bu obje için pool var mı yok mu kontrol edilmesi.
                if(poolDictionary.ContainsKey(poolObj.GetInstanceID()))      
                {
                    //Var ise deactive edilip, pool'a eklenmesi.
                    obj.SetActive(false);
                    obj.transform.position = transform.position;
                    poolDictionary[poolObj.GetInstanceID()].Enqueue(obj);
                }

                // Eğer pool yoksa bu obje tipi için yeni pool üretilip, ardından objenin pool'a eklenmesi.
                else 
                {      
                    Queue<GameObject> newStack = new Queue<GameObject>();
                    poolDictionary.Add(poolObj.GetInstanceID(),newStack);

                    obj.SetActive(false);
                    obj.transform.position = transform.position;
                    poolDictionary[poolObj.GetInstanceID()].Enqueue(obj);
                }

            }

    }

    

