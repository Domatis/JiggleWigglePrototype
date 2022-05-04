using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RunnerEnemy : PlayerInteractionObject   
{
    [SerializeField] protected PoolingObject poolObjRef;    // Kendi pool referansı.
    [SerializeField] protected PoolingObject particleEffectPoolObj;    // particle efekti pool referansı.
    [SerializeField] protected GameObject particlePos;
    [SerializeField] private float speed;

    private Animator anim;
    private Rigidbody rb;
    private AudioSource a_source;
    private bool enemyMovingOn  = true;

    private void Awake() 
    {
        a_source = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
    }

    // Enable disable durumlarında aksiyon kayıtları ve silinmesi.
    protected override void OnEnable() 
    {
        base.OnEnable();
        GameplayManager.instance.GameLoseAction += OnGameLoseAction;
        anim.SetBool("running",true);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        GameplayManager.instance.GameLoseAction -= OnGameLoseAction;
    }

    private void Start() 
    {     
        rb = GetComponent<Rigidbody>();
        transform.forward = Vector3.forward * -1;       // Objenin yüzünün local olarak forward vektörüne bakıldığı varsayılmıştır.
    }

    private void FixedUpdate() 
    {
        if(!enemyMovingOn) return;
        rb.velocity = (transform.forward * speed) + (Vector3.up * rb.velocity.y);
    }

    public void OnGameLoseAction()
    {
        anim.SetBool("running",false);
        enemyMovingOn = false;
        rb.velocity = Vector3.zero;
    }

    public override void OnPlayerInteraction()
    {
         enemyMovingOn = false;

         // Particle efektinin pool'dan istenmesi.
         GameObject particle = ObjectPoolingManager.instance.RequestFromPool(particleEffectPoolObj);
         particle.transform.position = particlePos.transform.position;
         particle.SetActive(true);

         a_source.Play();   // Çarpışma durumunda sound çalınması.

         PlayerController.instance.PlayerDie();
         // Layer,hız ve animasyon ayarları.
         gameObject.layer = LayerMask.NameToLayer("EnemiesPassive");
         rb.velocity = Vector3.zero;
         anim.SetBool("death",true);
    }

    public override void DisableObject()
    {
        // Değerlerin resetlenmesi ve pool'a gönderilmesi.
        anim.SetBool("death",false);
        enemyMovingOn = true;
        ObjectPoolingManager.instance.AddToThePool(gameObject,poolObjRef);
    }
    
}
