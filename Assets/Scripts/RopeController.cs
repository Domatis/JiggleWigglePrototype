using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeController : MonoBehaviour
{
    
    [SerializeField] private Rigidbody startPoint;     //Başlangıç noktası, gemi için.
    [SerializeField] private Rigidbody endPoint;       //Bitiş noktası, oyuncunun tuttuğu yer.
    [SerializeField] private int numberOfNodes = 7;
    [SerializeField] private float nodeColliderRadius = .5f;

    [SerializeField] private LineRenderer l_renderer;

    private List<Rigidbody> nodes = new List<Rigidbody>();

    private bool nodesPosUpdateOn = true;

    private void Start()
    {
        GameplayManager.instance.GameLoseAction += OnGameLoseAction;
        l_renderer.transform.position = startPoint.transform.position;  //Renderer objesini başlangıç pozisyonuna sabitlenmesi.
        l_renderer.positionCount = numberOfNodes;
        CreateNodes();
        UpdateLineRenderer();
    }

    private void Update() 
    {
        SetVelocityOfNodes();
        UpdateLineRenderer();   //Sürekli olarak hinge nesnelerinin
    }

    private void SetVelocityOfNodes()
    {
        if(!nodesPosUpdateOn) return;
        startPoint.velocity = new Vector3(startPoint.velocity.x,startPoint.velocity.y,ShipController.instance.shipSpeed);

        for(int i = 0; i < nodes.Count;i++)
        {
            nodes[i].velocity = new Vector3(nodes[i].velocity.x,nodes[i].velocity.y,ShipController.instance.shipSpeed);
        }
    }

    //Verilen değerlere göre başlangıç ve bitiş noktaları arasında node'ların oluşturulması.
    private void CreateNodes()
    {
        Vector3 totalDistance = endPoint.transform.position - startPoint.transform.position;
        Vector3 offset = totalDistance/(numberOfNodes-1);   //Aralık değerinin belirlenmesi.

        startPoint.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY;

        for(int i =0; i < numberOfNodes-2;i++)
        {
            //Node nesnelerinin oluşturulup rigidbody ve hingejoint bileşenlerinin eklenmesi.
            GameObject newNode = new GameObject("Node" + i);
            Rigidbody rbnewNode = newNode.AddComponent<Rigidbody>();
            CapsuleCollider cldr = newNode.AddComponent<CapsuleCollider>();     //TODO sonra cylindere dönüştürebilir.

            cldr.radius = nodeColliderRadius;
            cldr.isTrigger = true;
            cldr.height = offset.y;


            HingeJoint hinge = newNode.AddComponent<HingeJoint>();

            hinge.transform.position = ((i+1) * offset) + startPoint.transform.position;    //Pozisyonun başlangıç değeri ve offset ile belirlenmesi.

            // HingeJoint bileşen ayarları.
            if(i == 0)      //İlk nesnenin connected body'si start point olacağından ayrı kontrol ediliyor.
            {
                hinge.connectedBody = startPoint;
                hinge.anchor = startPoint.transform.position - hinge.transform.position;
            }
            else
            {   
                //Diğer node'ların connected bodyleri bir önceki obje olacak.
                hinge.connectedBody = nodes[i-1];
                hinge.anchor = nodes[i-1].transform.position - hinge.transform.position;
            }

            //Bütün node'lar için hinge ayarları.
            hinge.axis = Vector3.forward;
            hinge.useSpring = true;
            JointSpring spring = hinge.spring;
            spring.spring = 1000;
            spring.damper = 100;
            hinge.spring = spring;

            nodes.Add(rbnewNode);
        }

        //Bitiş noktasının connected body'sinin oluşturulan son node olması.
        HingeJoint endHinge = endPoint.GetComponent<HingeJoint>();
        endHinge.connectedBody = nodes[nodes.Count-1];
        endHinge.anchor = nodes[nodes.Count-1].transform.position - endHinge.transform.position;


        //Başlangıç noktasının collider ayarları.
        CapsuleCollider startCld = startPoint.GetComponent<CapsuleCollider>();
        startCld.isTrigger = true;
        startCld.radius = nodeColliderRadius;
        startCld.height = offset.y;

        //Bitiş noktasının collider ayarları.
        CapsuleCollider endCld = endPoint.GetComponent<CapsuleCollider>();
        endCld.isTrigger = true;
        endCld.radius = nodeColliderRadius;
        endCld.height = offset.y;

    }

    //Node pozisyonlarına göre renderer noktalarının güncellenmesi.
    private void UpdateLineRenderer()
    {
        l_renderer.transform.position = startPoint.transform.position;

        for(int i = 0; i < nodes.Count;i++)
        {
            Vector3 pos = nodes[i].transform.position - startPoint.transform.position;
            l_renderer.SetPosition(i+1,pos);
        }
        
        // Son nokta dizi içinde değil ayrı hesaplanıyor.
        Vector3 pos2 = endPoint.transform.position - startPoint.transform.position;
        l_renderer.SetPosition(numberOfNodes-1,pos2);
    }

    // Oyuncu öldüGameLoscak fonksiyon.
    public void OnGameLoseAction()
    {
        nodesPosUpdateOn = false;   //Noktaları ilerletilmesinin durdurulması.


        //Bütün noktaların collider ve rigidbody ayarlarının yapılması, yere düzgün düşmesi için.
        startPoint.GetComponent<CapsuleCollider>().isTrigger = false;
        startPoint.velocity = Vector3.zero;
        startPoint.useGravity = true;
        startPoint.isKinematic = false;
        startPoint.constraints = RigidbodyConstraints.None;

        endPoint.GetComponent<CapsuleCollider>().isTrigger = false;
        
        endPoint.velocity = Vector3.zero;
        endPoint.useGravity = true;
        endPoint.isKinematic = false;

        for(int i = 0; i < nodes.Count;i++)
        {
            nodes[i].GetComponent<CapsuleCollider>().isTrigger = false;
            nodes[i].velocity = Vector3.zero;
        }

    }
}
