using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerProjectile : PlayerInteractionObject
{

    [SerializeField] private PoolingObject poolObjRef;  // Kendi pool referansı.
    [SerializeField] private GameObject smokeParticleObj;
    [SerializeField] private PoolingObject hitParticlePoolObj;  // Çarpma durumunda oluşacak particle efektin pool referansı.
    [SerializeField] private AudioClip launcSound;
    [SerializeField] private PoolingObject hitSoundPoolObj;     // Çarpma durumunda oluşacak sound efektin pool referansı.
    [SerializeField] private float speed;

    private AudioSource a_source;
    private Rigidbody rb;
    private bool projectileOn;

    private void Awake() 
    {
        a_source = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
    }

    private void Start() 
    {
        smokeParticleObj.SetActive(false);
    }

    private void Update() 
    {
        if(projectileOn)
        {
            // Boss ile aradaki yönün bulunması ve hızın set edilmesi.
            Vector3 distance = Boss.instance.hitPoint.transform.position - transform.position;
            Vector3 direction = distance.normalized;
            rb.velocity = direction * speed;
            transform.forward = direction;
        }
    }

    private void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.TryGetComponent<Boss>(out Boss boss))
        {
            
            // Sound efektinin pool'dan istenmesi.
            GameObject sound = ObjectPoolingManager.instance.RequestFromPool(hitSoundPoolObj);
            sound.transform.position = transform.position;
            sound.SetActive(true);
            
            // Particle efektinin pool'dan istenmesi.
            GameObject hitParticle = ObjectPoolingManager.instance.RequestFromPool(hitParticlePoolObj);
            hitParticle.transform.position = transform.position;
            hitParticle.SetActive(true);

            //Hasar vermesi ve disable durumu.
            boss.TakeDamage(1);
            DisableObject();   
        }
    }

    //Disable edilip pool'a gönderilmesi.
    public override void DisableObject()
    {   
        smokeParticleObj.SetActive(false);
        projectileOn = false;
        rb.velocity = Vector3.zero;
        transform.rotation = poolObjRef.prefab.transform.rotation;
        ObjectPoolingManager.instance.AddToThePool(gameObject,poolObjRef);
    }

    public override void OnPlayerInteraction()
    {   
        // Player etkileşiminde ses ve particle efektinin aktif edilmesi ve hareketin başlangıcı.
        a_source.clip = launcSound;
        a_source.Play();
        projectileOn = true;
        smokeParticleObj.SetActive(true);
    }
}
