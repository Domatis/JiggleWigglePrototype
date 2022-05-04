using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayUIManager : MonoBehaviour
{

    public static GameplayUIManager instance;


    [SerializeField] private GameObject controlButtons;
    [SerializeField] private GameObject controlStick;
    [SerializeField] private GameObject gameEndPanel;
    [SerializeField] private float maxDistanceControlStick;

    private bool delayOn;
    private int currentDirVal;

    private float delayTime,delayTimer;

    private void Awake() 
    {
        instance = this;
    }

    private void Start() 
    {
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
        controlButtons.transform.position = pos;
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

        float calcDeltaVal = Mathf.Min(maxDistanceControlStick,Mathf.Abs(xdeltaMovement));  //Verilen değerin maksimum değerler içinde kalmasının sağlanması.
        Vector3 deltaVec = Vector3.right * calcDeltaVal * signVal;      //Toplam offset vektörünün hesaplanması.

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
