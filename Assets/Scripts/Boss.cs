using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    public static Boss instance;


    
    [SerializeField] private Image hpImage;
    [SerializeField] private int maxHealth;

    public GameObject hitPoint; // Füzelerin isabet aldığı yer.
    
    
    private Rigidbody rb;
    private int currentHealth;
    private bool movingOn = true;


    private void Awake() 
    {  
        rb = GetComponent<Rigidbody>();
        instance = this;
    }


    private void Start() 
    {
        currentHealth = maxHealth;

        // Ship ilerledikçe ilerlemesi ve oyun bittiğinde yapılacaklar için aksiyon kayıtları.
        ShipController.instance.shipMovementAction += MoveBoss;
        GameplayManager.instance.GameLoseAction += () => {movingOn = false;};
        
        // Başlangıçta son üretilen yol objesinden daha ileride pozisyon almasının sağlanması.
        List<GameObject> roads = RoadManager.instance.CurrentRoads;
        transform.position = roads[roads.Count-1].transform.position + (Vector3.forward * RoadManager.instance.OffsetPrefabs);
    }
    
    
    public void MoveBoss(Vector3 deltaMov)
    {
        if(!movingOn) return;
        transform.position = transform.position + deltaMov;
    }

    public void TakeDamage(int val)
    {
        currentHealth = Mathf.Max(0,currentHealth-val);
        if(currentHealth <= 0)
        {
            //Boss ölür ve oyun kazanılır.
            GameplayManager.instance.GameWin();
        }

        hpImage.fillAmount = (float)currentHealth/(float)maxHealth;
    }

    
}
