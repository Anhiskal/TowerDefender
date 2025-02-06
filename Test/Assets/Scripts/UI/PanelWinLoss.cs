using UnityEngine;

public class PanelWinLoss : MonoBehaviour
{
    [SerializeField]
    GameObject popupLoss, popupWin;

    public void onEblePanelLoss() 
    {
        popupLoss.SetActive(true);
    }
     
    public void onEnablePanelWin() 
    {
        popupWin.SetActive(true);
    }

    public void disableAllPanels() 
    {
        popupLoss.SetActive(false);
        popupWin.SetActive(false);
    }
}
