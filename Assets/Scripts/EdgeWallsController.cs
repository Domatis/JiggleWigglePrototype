using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeWallsController : MonoBehaviour
{
    [SerializeField] private float wallDistance = 3f;
    [SerializeField] private Rigidbody leftWall;
    [SerializeField] private Rigidbody rightWall;

    private bool movingOn = true;
    
    private void Start() 
    {
        GameplayManager.instance.GameLoseAction += () =>
        {
            movingOn = false;
            leftWall.velocity = Vector3.zero;
            rightWall.velocity = Vector3.zero;
            
        };


        // Duvarların pozisyonlarını oyuncuya göre ayarla.
        leftWall.transform.position = PlayerController.instance.transform.position + (Vector3.left * wallDistance);
        rightWall.transform.position = PlayerController.instance.transform.position + (Vector3.right * wallDistance);
    }

    private void Update() 
    {
        if(!movingOn) return;
        leftWall.velocity = Vector3.forward * ShipController.instance.shipSpeed;
        rightWall.velocity = Vector3.forward * ShipController.instance.shipSpeed;
    }
}
