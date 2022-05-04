using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    public static CameraShaker instance;

    [Range(0,5)]
    [SerializeField] private float shakePower;  

    private Vector3 basePos;
    private float shaketimer,shakeTime;
    private bool shakeOn;


    private void Awake() 
    {
        instance = this;
    }

    private void Start() 
    {
        basePos = transform.position;
    }

    private void Update() 
    {
        if(shakeOn)
        {
            
            // Shakepower değeri kullanılarak x ve y eksenleri için rastgele değerler alınıp kamera base pozisyonuna eklenmesi.
            Vector3 currentPos = basePos;
            float randX = Random.Range(0,shakePower);
            float randY = Random.Range(0,shakePower);

            transform.position = basePos + (Vector3.right * randX) + (Vector3.up * randY);

            shaketimer += Time.deltaTime;
            if(shaketimer >= shakeTime)
            {
                //Bitiş ve kameranın ana pozisyona set edilmesi.
                shakeOn = false;
                transform.position = basePos;
            }
        }
    }

    //Kamera sallanma fonksiyonu saniye parametresi ile.
    public void StartCameraShaking(float sec)
    {
        if(shakeOn) return;
        shakeOn = true;
        shaketimer = 0;
        shakeTime = sec;
    }
}
