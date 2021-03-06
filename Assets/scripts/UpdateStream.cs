﻿using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UpdateStream : MonoBehaviour
{

    public GameObject canvas;
    public Animator listCh;
    public bool menuState = true;
    public CameraControl camContr;
    public GameObject XYZ;
    public delegate void UpdateAction(ref GameObject[] charges);
    public static event UpdateAction OnUpdateCharges;
    public Transform addButton;
    GameObject[] charges = new GameObject[0];
    public GameObject waitPanel;
    public GameObject chListGo;

    public void New()
    {
        SceneManager.LoadScene(0);
    }

    public void UpdateCharges()
    {
        charges = new GameObject[0];
        charges = GameObject.FindGameObjectsWithTag("isCharge");
        if (OnUpdateCharges!=null)
            OnUpdateCharges(ref charges);
    }

    /*IEnumerator UpdateChargesEnum()
    {
        Debug.Log("Update all");
        yield return null;
        GameObject[] charges = GameObject.FindGameObjectsWithTag("isCharge");
        foreach (GameObject charge in charges)
        {
            Charge chs = charge.GetComponent<Charge>();
            chs.needUpdate = true;
            Debug.Log(Time.time.ToString());
            if (Time.time > 0.015f)
                yield return null;
        }
        GameObject[] chPanels = GameObject.FindGameObjectsWithTag("chSyn");
        foreach (GameObject chPanel in chPanels)
        {
            ChargeSyns chS = chPanel.GetComponent<ChargeSyns>();
            chS.needUpdateIn();
            Debug.Log("PanelUpdate " + chPanel.name);
        }
        yield break;
    }*/


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
        addButton.SetAsLastSibling();
        //отключение интерфейса
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            chListGo.SetActive(false);
            canvas.SetActive(!canvas.activeSelf);
            chListGo.SetActive(true); //дикий костыль
        }
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray, out hit, 100))
            {
                Debug.Log("No clicks");
                XYZ.transform.SetParent(transform.parent);
                XYZ.SetActive(false);
            }
        }
    }
}