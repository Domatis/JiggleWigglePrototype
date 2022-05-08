using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayUIManager : MonoBehaviour
{

    public static GameplayUIManager instance;

    [Header("Control Button Part")]
    [SerializeField] private GameObject controlButtons;
    [SerializeField] private GameObject controlButtonRightBorder;
    [SerializeField] private GameObject controlButtonLeftBorder;
    [SerializeField] private GameObject controlButtonTopBorder;
    [SerializeField] private GameObject controlButtonBottomBorder;
    [Header("Control Stick Part")]
    [SerializeField] private GameObject controlStick;
    [SerializeField] private GameObject controlStickRightBorder;
    [SerializeField] private GameObject controlStickLeftBorder;
    [SerializeField] private GameObject gameEndPanel;

    private bool delayOn;
    private int currentDirVal;

    private float delayTime,delayTimer;
    private float controlStickRightOffset,controlStickLeftOffset;
    private float controlButtonRightOffset,controlButtonLeftOffset,controlButtonBottomOffset,controlButtonTopOffset;

    private void Awake() 
    {
        instance = this;
    }

    private void Start() 
    {
        controlStickRightOffset = controlStickRightBorder.transform.position.x - transform.position.x;
        controlStickLeftOffset = controlStickLeftBorder.transform.position.x - transform.position.x;

        controlButtons.SetActive(false);
        gameEndPanel.SetActive(false);
    }

    private void Update() 
    {
        // Oyun bitiş panelinin gecikme ile açılması durumunun sağlanması.
        if(delayOn) 
        {
            delayTimer += Time.deltaTime;
            if(delayTimer >= delayTime)
            {
                gameEndPanel.SetActive(true);
                delayOn = false;
            }
        }

        PlayerController.instance.SetControlDirection(currentDirVal);   //Sürekli olarak pozisyon bilgisinin gönderilmesi, off durumunda iken 0 gönderilecek.
    }


    // Control butonlarını aktif edilmesi.
    public void ActivateControls(Vector3 pos)
    {
        float xClamped = Mathf.Clamp(pos.x,controlButtonLeftBorder.transform.position.x,controlButtonRightBorder.transform.position.x);
        float yClamped = Mathf.Clamp(pos.y,controlButtonBottomBorder.transform.position.y,controlButtonTopBorder.transform.position.y);

        Vector3 newPos = new Vector3(xClamped,yClamped,0);

        controlButtons.transform.position = newPos;
        controlButtons.SetActive(true);
    }

    // Control butonlarını inaktif edilmesi.
    public void DeActivateControls()
    {
        controlButtons.SetActive(false);
        controlStick.transform.position = controlButtons.transform.position;
        currentDirVal = 0;
    }

    
    public void MovementControlStick(float xdeltaMovement)
    {
        float signVal = Mathf.Sign(xdeltaMovement);     // Oyuncunun ilk ana basış noktasından x ekseni için ne kadar uzaklaştığı ve pozitif, negatif bilgisinin alınması.
        currentDirVal = (int)signVal;       // Sağ taraf ise +1 sol ise -1 bilgisinin alınması.

        //Verilen değerin minimum,maksimum değerler içinde kalmasının sağlanması.
        float calcDeltaVal = Mathf.Clamp(xdeltaMovement,controlStickLeftOffset,controlStickRightOffset);

        Vector3 deltaVec = Vector3.right * calcDeltaVal;      //Toplam offset vektörünün hesaplanması.

        controlStick.transform.position = controlButtons.transform.position + deltaVec; // Pozisyon güncellenmesi.
    }

    // Button objesinden çağırılan fonksiyon.
    public void PlayAgainButtonAction()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Gecikme olmadan oyun bitiş panelinin açılması.
    public void OpenGameEndPanel()
    {
        if(delayOn) return;
        gameEndPanel.SetActive(true);
    }

    // Gecikme ile oyun bitiş panelinin açılması.
    public void OpenGameEndPanel(float delay)
    {
        if(delayOn) return;
        delayOn = true;
        delayTime = delay;
        delayTimer = 0;
    }

}
