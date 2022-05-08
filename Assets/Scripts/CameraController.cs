using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera mainCam;
    [SerializeField] private CinemachineVirtualCamera gameWinCam;
    private bool camMoveOn = true;


    private Rigidbody mainCamrb,winCamRb;

    private void Awake() 
    {
        mainCamrb =  mainCam.GetComponent<Rigidbody>();
        winCamRb = gameWinCam.GetComponent<Rigidbody>();
    }

    private void Start() 
    {
        //Gerekli event kayıtları.
        GameplayManager.instance.GameWinAction += GameWinAction;
        GameplayManager.instance.GameLoseAction += GameLoseAction;
    }

    private void Update() 
    {
        if(!camMoveOn) return;
        mainCamrb.velocity = Vector3.forward * ShipController.instance.shipSpeed;
        winCamRb.velocity = Vector3.forward * ShipController.instance.shipSpeed;
    }

     // Oyun kazanılma durumunda gamewin kamerasına geçilmesi.
    public void GameWinAction()
    {
        mainCam.Priority = 0;
        gameWinCam.Priority = 1;
    }

    public void GameLoseAction()
    {
        camMoveOn = false;
        mainCamrb.velocity = Vector3.zero;
        winCamRb.velocity = Vector3.zero;
    }

}
