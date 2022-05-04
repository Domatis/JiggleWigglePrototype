using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    
    [SerializeField] private CinemachineBrain brain;
    [SerializeField] private CinemachineVirtualCamera mainCam;
    [SerializeField] private CinemachineVirtualCamera gameWinCam;
    private bool camMoveOn = true;

    private void Start() 
    {
        //Gerekli event kayıtları.
        ShipController.instance.shipMovementAction += CameraMovement;
        GameplayManager.instance.GameWinAction += GameWinAction;
        GameplayManager.instance.GameLoseAction += () => {camMoveOn = false;};
    }

    //Bütün kameraların ship ile hareketinin sağlandığı fonksiyon.
    public void CameraMovement(Vector3 deltamov)
    {
        if(!camMoveOn) return;
        mainCam.transform.position  = mainCam.transform.position + deltamov;
        gameWinCam.transform.position = gameWinCam.transform.position + deltamov;
    }

   
     // Oyun kazanılma durumunda gamewin kamerasına geçilmesi.
    public void GameWinAction()
    {
        mainCam.Priority = 0;
        gameWinCam.Priority = 1;
    }

}
