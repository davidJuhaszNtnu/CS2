using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class Site1 : MonoBehaviour
{
    public GameObject site1, site1UI, path1, welcomePanel, multichoicePanel, congratsPanel, canvasWorld, site2, site2UI;
    public Button OkWelcome, OkMultichoice;
    float timeCongratsPanel;
    bool bool1;
    public Camera arCamera;

    // private GameObject aRSessionOrigin;

    void Start()
    {
        bool1=false;
        canvasWorld.SetActive(true);
        canvasWorld.GetComponent<RectTransform>().position = new Vector3(0f,0f,1f);
        welcomePanel.SetActive(true);
        multichoicePanel.SetActive(false);
        congratsPanel.SetActive(false);
        OkWelcome.onClick.AddListener(ok_welcome);
        OkMultichoice.onClick.AddListener(ok_multichoice);

        // welcomePanel.SetActive(false);
        // multichoiceCanvas.SetActive(true);
        // multichoiceCanvas.gameObject.transform.position = new Vector3(0f,0f,1f);
    }

    private void ok_welcome(){
        welcomePanel.SetActive(false);
        canvasWorld.SetActive(true);
        multichoicePanel.SetActive(true);
        Vector3 dir=arCamera.transform.position-canvasWorld.GetComponent<RectTransform>().position;
        canvasWorld.GetComponent<RectTransform>().rotation=Quaternion.LookRotation(new Vector3(dir.x,0f,dir.z), Vector3.up)*Quaternion.Euler(0,180,0);
    }

    private void ok_multichoice(){
        multichoicePanel.SetActive(false);
        congratsPanel.SetActive(true);
        timeCongratsPanel = Time.time;
        bool1=true;
    }

    void Update(){
        if(bool1 && Time.time>timeCongratsPanel+5f){
            bool1=false;
            congratsPanel.SetActive(false);
            site1.SetActive(false);
            site1UI.SetActive(false);
            // site2.SetActive(true);
            // site2UI.SetActive(true);
            path1.GetComponent<Path>().arrived=false;
            site2.GetComponent<Site2>().showSite=true;
            path1.SetActive(true);
            path1.GetComponent<Path>().t=0f;
            path1.GetComponent<Path>().Arrow.SetActive(true);
            path1.GetComponent<Path>().Arrow.transform.position=path1.GetComponent<Path>().path(0f);
            path1.GetComponent<Path>().Arrow.transform.rotation=Quaternion.LookRotation(path1.GetComponent<Path>().pathDer(0f), Vector3.up);
        }
    }
}
