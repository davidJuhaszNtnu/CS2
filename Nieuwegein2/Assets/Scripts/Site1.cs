using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using Mapbox.Examples;

public class Site1 : MonoBehaviour
{
    public GameObject site1, site1UI, welcomePanel, multichoicePanel, scoreUpdatePanel, answerPanel, site2, site2UI, app;
    public Camera arCamera, mapCamera;

    // private GameObject aRSessionOrigin;

    void Start()
    {
        restart();

        // welcomePanel.SetActive(false);
        // scoreUpdatePanel.SetActive(true);
        // multichoiceCanvas.gameObject.transform.position = new Vector3(0f,0f,1f);
    }

    public void restart(){
        welcomePanel.SetActive(true);
        multichoicePanel.SetActive(false);
        scoreUpdatePanel.SetActive(false);
        answerPanel.SetActive(false);

        for (int i = 0; i < site1UI.GetComponent<Site1UI>().answered.Length; i++)
            site1UI.GetComponent<Site1UI>().answered[i] = false;
    }

    public void nextSite(){
        site1.SetActive(false);
        site1UI.SetActive(false);
        arCamera.enabled = false;
        mapCamera.enabled = true;
        app.GetComponent<App>().map.SetActive(true);
        // app.GetComponent<App>().mapPanel.SetActive(true);
        app.GetComponent<App>().player.SetActive(true);
        app.GetComponent<App>().sitePathSpawner.GetComponent<SpawnOnMap>().currentSite = 0;
        app.GetComponent<App>().nextSite_index = 2;
        app.GetComponent<App>().siteOn = false;
        app.GetComponent<App>().sitePathSpawner.GetComponent<SpawnOnMap>().showSitePath();
        app.GetComponent<App>().back_button.gameObject.SetActive(true);
        app.GetComponent<App>().showMap_button.gameObject.SetActive(false);
    }
}
