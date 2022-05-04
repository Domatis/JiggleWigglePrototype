using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadManager : MonoBehaviour
{
    public static RoadManager instance;
    
    [SerializeField] private GameObject roadPrefab;     // Yol prefabi
    [SerializeField] private Transform startPos;        // Yol için başlangıç noktası
    [SerializeField] private float offsetPrefabs;       // Üretilirken ve yerleştirilirken yollar arasında ki mesafe
    [SerializeField] private float distanceThreshold;   // Oyuncu ve diğer bileşenler ne kadar hareket ettiğinde, yeni prefabin yerleştirileceği.
    [SerializeField] private int numberOfRoadsAtStart;  // Başlangıçta oluşturulacak yol sayısı.
    [SerializeField] private int emptyRoadsAtStart;
    [Header("Objects Settings")]
    [SerializeField] private float zRadiusForspawn;     // Oluşturulacak objenin z ekseni için maksimum mesafesi
    [SerializeField] private float xRadiusForSpawn;     // Oluşturulacak objenin x ekseni için maksimum mesafesi
    [SerializeField] private InteractableObjects[] objectsPrefabs;  // Oluşturulacak objeler listesi.

    private List<GameObject> currentRoads = new List<GameObject>(); 

    private float currentDistance;
    private int behindIndex,furthestIndex;  // Yollar yerleştirilirken en öndeki ve en arkada ki yolun dizi içindeki indexlerinin tutulması.
    private bool createRoadsOn = true;
    private bool createObjectsOn = true;
    private int totalPriorityVal;

    public List<GameObject> CurrentRoads => currentRoads;
    public float OffsetPrefabs => offsetPrefabs;

    private void Awake() 
    {
        instance = this;
    }

    private void Start() 
    {
        ShipController.instance.shipMovementAction += IncreaseCurrentDistance;   
        GameplayManager.instance.GameLoseAction += () => {createRoadsOn = false;};  // Kaybedilmesi durumunda yeni yollar üretilmeyecek.
        GameplayManager.instance.GameWinAction += () => {createObjectsOn = false;}; // Kazanılması durumunda yeni yollar üretilecek fakat, objeler üretimeyecek.

        // Toplam öncelik değerlerinin toplanması.
        for(int i =0; i< objectsPrefabs.Length;i++)
        {
            totalPriorityVal += objectsPrefabs[i].priority;
        }

        CreateRoads();
    }


    private void CreateRoads()
    {
        for(int i=0; i < numberOfRoadsAtStart ;i++)
        {
            GameObject road = Instantiate(roadPrefab,(startPos.position + (i*offsetPrefabs *(Vector3.forward))),roadPrefab.transform.rotation);
            currentRoads.Add(road);
            // Yollar üretilirken belli sayıda yol üzerinde objeler üretilmeden oluşturulacak.
            if(i >= emptyRoadsAtStart)  
            {
                CreateObjectOnRoad(road);   
            }
        }

        behindIndex = 0;
        furthestIndex = currentRoads.Count-1;
    }

    // Oyuncu arkasında kalan yolun, en sona taşınması.
    private void PlaceRoadToFront()
    {
        GameObject replaceRoad = currentRoads[behindIndex];
        replaceRoad.transform.position = currentRoads[furthestIndex].transform.position + Vector3.forward * offsetPrefabs;

        CreateObjectOnRoad(replaceRoad);    // Taşınan yolun üzerinde yeni objeler üretilmesi.

        // Indexleri güncelle.
        furthestIndex = behindIndex;
        behindIndex++;
        if(behindIndex >= currentRoads.Count) behindIndex = 0;
    }

    //Alınan yolu hesaplayıp ve değere göre yeni yol eklenmesinin çağırılması.
    public void IncreaseCurrentDistance(Vector3 deltamov)
    {
        if(!createRoadsOn) return;      

        currentDistance += deltamov.z;  
        if(currentDistance >= distanceThreshold)
        {
            PlaceRoadToFront();
            currentDistance = 0;
        }   
    }

    // Yollar üzerinde rastgele objeler oluşturulması.
    public void CreateObjectOnRoad(GameObject road)
    {
        if(!createObjectsOn) return;

        // Öncelik değeri kullanılarak, rastgele bir sayı seçilmesi 
        int randomVal = Random.Range(0,totalPriorityVal);

        int selectedIndex = 0;
        int cumulativePriority = 0;

        /* Her objenin priority değerinin kontrol edilip, yüksek priority'e sahip objelerin şansı yüksek olacak şekilde rastgele objenin seçilmesi.
           Her öncelik değeri toplam öncelik değeri içinde bir ağırlığa sahip oluyor, örneğin objenin önceliği 25 ise ve toplam öncelik değerlerinin toplamı
           150 ise 150 değeri içinde 25 değerlik bir alan kaplıyor. 
        */
        for(int i =0; i< objectsPrefabs.Length;i++)
        { 
            cumulativePriority += objectsPrefabs[i].priority;
            if(randomVal <= cumulativePriority)
            {
                selectedIndex = i;
                break;
            }
        }
        
        //Seçilen nesne.
        InteractableObjects selectedObject = objectsPrefabs[selectedIndex];

        // Seçilen nesnenin yol üzerinde rastgele bir pozisyon'a set edilmesi.
        
        float zValue = Random.Range(-zRadiusForspawn,zRadiusForspawn);
        float xValue = Random.Range(-xRadiusForSpawn,xRadiusForSpawn);
        float yVal = selectedObject.heightOffset;

        Vector3 offsetPos  = new Vector3(xValue,yVal,zValue);

        GameObject poolobj =  ObjectPoolingManager.instance.RequestFromPool(selectedObject.poolObject);
        poolobj.transform.position = road.transform.position + offsetPos;
        poolobj.SetActive(true);
    }
}

[System.Serializable]
public class InteractableObjects
{
    public PoolingObject poolObject;
    public float heightOffset;
    [Tooltip("Higher values are higher priority")]
    public int priority;
}
