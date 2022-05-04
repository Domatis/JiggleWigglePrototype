using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EdgeWallsController : MonoBehaviour
{
    [SerializeField] private float wallDistance = 3f;
    [SerializeField] private GameObject leftWall;
    [SerializeField] private GameObject rightWall;


    private void Start() 
    {
        ShipController.instance.shipMovementAction += WallsMovement;    //Duvarlarında gemi ile beraber hareket etmesi için event kaydı.
        // Duvarların pozisyonlarını oyuncuya göre ayarla.
        leftWall.transform.position = PlayerController.instance.transform.position + (Vector3.left * wallDistance);
        rightWall.transform.position = PlayerController.instance.transform.position + (Vector3.right * wallDistance);
    }

    public void WallsMovement(Vector3 deltamov)
    {
        transform.position = transform.position + deltamov;
    }

}
