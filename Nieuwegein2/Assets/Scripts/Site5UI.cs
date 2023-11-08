using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Site5UI : MonoBehaviour
{
    public GameObject markerHandler, site1, site1UI, site2, site2UI, site3UI, site4UI, site3, app, panelLayoutHandler, gameController, welcomePanel, multichoicePanel;
    public GameObject[] toggles;

    void Start()
    {
        
    }

    public void replay_bttn(){
        gameController.GetComponent<gameController>().restart();
        markerHandler.GetComponent<MarkerHandler>().restart();
        site1.GetComponent<Site1>().restart();
        site1UI.GetComponent<Site1UI>().restart();
        site2.GetComponent<Site2>().restart();
        site2UI.GetComponent<Site2UI>().restart();
        site3UI.GetComponent<Site3UI>().restart();
        site4UI.GetComponent<Site4UI>().restart();
        panelLayoutHandler.GetComponent<PanelLayoutHandler>().restart();
        app.GetComponent<App>().restart();
        welcomePanel.SetActive(true);
        multichoicePanel.SetActive(false);
        // toggles
        foreach(GameObject toggle in toggles)
            toggle.GetComponent<Toggle>().isOn = false;
    }
}
