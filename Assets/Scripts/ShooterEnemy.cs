using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterEnemy : PlayerInteractionObject
{

    [SerializeField] private float shootEverySec;
    [SerializeField] protected PoolingObject poolObjRef;
    [SerializeField] protected PoolingObject interactionEffectPoolObj;
    [SerializeField] protected GameObject particlePos;
    [SerializeField] private PoolingObject projectilePoolObj;
    [SerializeField] private Transform projectileSpawnPos;

    private AudioSource a_source;
    private Animator anim;
    private float shootTimer;
    private Vector3 direction;
    private bool shootingOn  = true;

    private void Awake() 
    {
        a_source = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
    }

    protected override void OnEnable() 
    {
        base.OnEnable();
        GameplayManager.instance.GameLoseAction += OnGameLoseAction;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        GameplayManager.instance.GameLoseAction -= OnGameLoseAction;
    }

    private void Update() 
    {
        if(!shootingOn) return;
        // Karakterin her zaman oyuncuya bakmasının ayarlanması.
        Vector3 distance = PlayerController.instance.transform.position - transform.position;
        distance.y = 0; // Yükseklik değeri yok sayılıyor.
        direction = distance.normalized;
        transform.forward = direction;

        // Atış için timer.
        shootTimer += Time.deltaTime;
        if(shootTimer >= shootEverySec)
        {
            anim.SetBool("shoot",true);
            shootTimer = 0;
        }
    }

    // Animasyondan çağırılan fonksiyon.
    public void Shoot()
    {
        GameObject projectile = ObjectPoolingManager.instance.RequestFromPool(projectilePoolObj);
        projectile.transform.position = projectileSpawnPos.position;
        projectile.transform.rotation = projectilePoolObj.prefab.transform.rotation;
        projectile.GetComponent<EnemyProjectile>().SetDirection(direction);
        projectile.SetActive(true);
    }

    // Atış animasyonu bittiğinde çağırılan fonksiyon.
    public void ShootEnds()
    {
        anim.SetBool("shoot",false);
    }

    public void OnGameLoseAction()
    {
        shootingOn = false;
        anim.SetBool("shoot",false);
    }

    public override void OnPlayerInteraction()
    {      
        shootingOn = false;
    
        // Particle efektinin pool'dan istenmesi.
        GameObject particle = ObjectPoolingManager.instance.RequestFromPool(interactionEffectPoolObj);
        particle.transform.position = particlePos.transform.position;
        particle.SetActive(true);

        a_source.Play();

        // Layer,hız ve animasyon ayarları.
        PlayerController.instance.PlayerDie();
        gameObject.layer = LayerMask.NameToLayer("EnemiesPassive");
        anim.SetBool("death",true);
    }

    public override void DisableObject()
    {
        // Değerlerin resetlenmesi ve pool'a gönderilmesi.
        ObjectPoolingManager.instance.AddToThePool(gameObject,poolObjRef);
        shootTimer = 0;
        shootingOn = true;
    }

}
