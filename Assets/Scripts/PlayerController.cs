using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
   
    public static PlayerController instance;

    [SerializeField] private Animator anim;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float force = 2;
    [SerializeField] private float centerForce = .5f;
    [SerializeField] private float hangDistance = 5f;
    [SerializeField] private GameObject protectionParticle;
    [SerializeField] private Rigidbody ropeHolderObject;   //Karakterin ipi tutacağı nokta.
    [SerializeField] private GameObject handPosition;
    [SerializeField] private GameObject windParticle;

    
    private AudioSource a_source;

    private Vector3 direction = Vector3.zero;   // Karakter için force uygulanacak directionun tutulduğu değer. //TODO vektör yerine float tutuabilir.
    private Vector3 basePos;    // Merkez nokta değeri.
    private Vector3 windParticleOffset;

    private bool playerDied = false;
    private bool playerProtectionOn = false;

    private float protectionTimer,protectionTime;

    private void Awake() 
    {
        instance = this;
        a_source = GetComponent<AudioSource>();
    }

    private void Start() 
    {
        GameplayManager.instance.GameWinAction += OnGameWinAction;

        // Wind particle efektinin offset'ini belirlenmesi.
        windParticleOffset = windParticle.transform.position - transform.position;
        windParticle.SetActive(true);

        protectionParticle.SetActive(false);
        //Rigidbody kısıtlamalarının set edilmesi.
        rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
        
        basePos = transform.position;   //Merkez nokta değerinin başlangıçta alınması.

        ropeHolderObject.transform.position = handPosition.transform.position;
    }

    private void Update()
    {
        if(playerDied) return;

        

        // Protection süresinin hesaplandığı yer.
        protectionTimer+= Time.deltaTime;
        if(protectionTimer >= protectionTime)
        {
            playerProtectionOn = false;
            protectionParticle.SetActive(false);
        }

       
        // Windparticle objesinin offset değeriyle pozisyonunun güncellenmesi.
        windParticle.transform.position = transform.position + windParticleOffset;

        // Salınma animasyonu için parametrenin belirlenmesi.
        float xDelta = transform.position.x - basePos.x;
        anim.SetFloat("hangParameter",(xDelta/hangDistance));

        ropeHolderObject.transform.position = handPosition.transform.position;
    }

    private void FixedUpdate() 
    {
        if(playerDied) return;
        
        //Her zaman merkeze doğru uygulanacak force.
        float xdistance = basePos.x - transform.position.x; //Anlık karakter ile merkez nokta arasında ki farkın bulunması.
        rb.AddForce(Mathf.Sign(xdistance) * Vector3.right * centerForce,ForceMode.Acceleration);   //X ekseni için merkez noktaya standart belli force uygulanması.

        rb.AddForce(direction * force,ForceMode.Acceleration);   //Kullanıcının uyguladığı force.

        rb.velocity = new Vector3(rb.velocity.x,rb.velocity.y, ShipController.instance.shipSpeed);
        

    }

    private void OnTriggerEnter(Collider other) 
    {
        // Playerinteraction objesinin olup olmadığının kontrolü ve varsa ilgili fonksiyonun çağırılması.
        if(other.TryGetComponent<PlayerInteractionObject>(out PlayerInteractionObject obj))
        {
            obj.OnPlayerInteraction();
        }
    }

    private void OnCollisionEnter(Collision other) 
    {
        // Playerinteraction objesinin olup olmadığının kontrolü ve varsa ilgili fonksiyonun çağırılması.
        if(other.gameObject.TryGetComponent<PlayerInteractionObject>(out PlayerInteractionObject obj))
        {
            obj.OnPlayerInteraction();
        }
    }

    // UI managerden çağırılan fonksiyon direction değerinin set edilmesi.
    public void SetControlDirection(int xDir)
    {
        direction = Vector3.right * xDir;
    }

    // Protection buff'ının aktive edilmesi.
    public void ActivatePlayerProtection(float time)
    {
        protectionParticle.SetActive(true);
        playerProtectionOn = true;
        protectionTimer = 0;
        protectionTime = time;
    }

    public void OnGameWinAction()
    {
        // Fizik layerinin değişimi.
        gameObject.layer = LayerMask.NameToLayer("PlayerPassive");
    }

    // Oyuncunun ölmesi için çağrılacak fonksiyon.
    public void PlayerDie()
    {
        if(playerProtectionOn || playerDied) return;

        a_source.Play();    // Ölme ses efekti.
        windParticle.SetActive(false);
        CameraShaker.instance.StartCameraShaking(.2f);  // Kamera sarsılması.
        gameObject.layer = LayerMask.NameToLayer("PlayerPassive");  //Fizik layer değişimi.
        playerDied = true;
        //Rigidbody ayarları.
        rb.constraints = rb.constraints & ~(RigidbodyConstraints.FreezePositionY|RigidbodyConstraints.FreezeRotationZ);     
        rb.velocity = Vector3.zero;
        rb.useGravity = true;
        anim.enabled = false;   // Ragdoll aktif olması için animasyon etkisinin kapatılması.
        GameplayManager.instance.GameLose();    
    }
}
