using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TouchScreenController : MonoBehaviour,IPointerDownHandler,IPointerUpHandler,IPointerMoveHandler
{
    
    private Vector2 downPos;
    private bool pointerDown = false;

    // Ekrana basılmasının başlangıcı.
    public void OnPointerDown(PointerEventData data)
    {
        pointerDown = true;
        downPos = data.position;
        GameplayUIManager.instance.ActivateControls(downPos);
    }

    public void OnPointerMove(PointerEventData data)
    {
        if(!pointerDown) return;
        // Basılma durumundayken ilk basma noktasıyla, şimdi ki nokta arasında ki farkın bulunup uimanager'a gönderilmesi.
        Vector3 deltaPos = data.position - downPos;
        GameplayUIManager.instance.MovementControlStick(deltaPos.x);
    }


    // Ekrandan parmağın çekilmesi.
    public void OnPointerUp(PointerEventData data)
    {
        GameplayUIManager.instance.DeActivateControls();
        pointerDown = false;
    }

}
