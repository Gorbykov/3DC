using UnityEngine;
using UnityEngine.SceneManagement;


public class UpdateStream : MonoBehaviour
{

    public GameObject inputPanel;
    public Animator listCh;
    public bool menuState = true;
    public CameraControl camContr;

    public void New()
    {
        SceneManager.LoadScene(0);
    }

    public void UpdateCharges()
    {
        Debug.Log("Update all");
        GameObject[] charges = GameObject.FindGameObjectsWithTag("isCharge");
        foreach (GameObject charge in charges)
        {
            Charge chs = charge.GetComponent<Charge>();
            chs.needUpdate = true;
        }
        GameObject[] chPanels = GameObject.FindGameObjectsWithTag("chSyn");
        foreach (GameObject chPanel in chPanels)
        {
            ChargeSyns chS = chPanel.GetComponent<ChargeSyns>();
            chS.needUpdateIn();
            Debug.Log("PanelUpdate " + chPanel.name);
        }
    }


    public void CamLock()
    {
        camContr.enabled = false;
    }

    public void CamUnlock()
    {
        camContr.enabled = true;
    }


    public void OpClMenu()
    {
        menuState = !menuState;
        if (menuState)
        {
            listCh.SetBool("isOpen", true);
            CamLock();
            Debug.Log("Cam lock");
        }
        else
        {
            listCh.SetBool("isOpen", false);
            CamUnlock();
            Debug.Log("Cam unlock");
        }
    }

    void Update()
    {
        //отклучение интерфейса
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            inputPanel.SetActive(!inputPanel.activeSelf);
        }
    }
}