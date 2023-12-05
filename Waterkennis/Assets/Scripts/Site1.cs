using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using Mapbox.Examples;
using TMPro;

public class Site1 : MonoBehaviour
{
    public GameObject site1, site1UI, welcomePanel, welcomePanel2, multichoicePanel, scoreUpdatePanel, answerPanel, site2, site2UI, app;
    public Camera arCamera, mapCamera;
    public TMP_Dropdown dropdown;

    public Sprite markerSprite;

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
        welcomePanel2.SetActive(false);
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
        app.GetComponent<App>().mapInstructionPanel.SetActive(true);
        app.GetComponent<App>().mapInstructionTitle_L.gameObject.SetActive(false);
        app.GetComponent<App>().mapInstructionTitle_P.gameObject.SetActive(false);
        if(dropdown.value == 0){
            //english
            app.GetComponent<App>().mapInstructionText_L.text = "Use the map view to navigate your way to the to the source of the water\n(site 1).\nAt the site, find the sign with the following image:";
            app.GetComponent<App>().mapInstructionText_P.text = "Use the map view to navigate your way to the to the source of the water (site 1).\nAt the site, find the sign with the following image:";
        }else{
            //dutch
            app.GetComponent<App>().mapInstructionText_L.text = "Use the map view to navigate your way to the to the source of the water\n(site 1).\nAt the site, find the sign with the following image (dutch):";
            app.GetComponent<App>().mapInstructionText_P.text = "Use the map view to navigate your way to the to the source of the water (site 1).\nAt the site, find the sign with the following image (dutch):";
        }
        app.GetComponent<App>().markerImage_L.sprite = markerSprite;
        app.GetComponent<App>().markerImage_P.sprite = markerSprite;

        app.GetComponent<App>().help_button.gameObject.SetActive(true);
        app.GetComponent<App>().player.SetActive(true);
        app.GetComponent<App>().sitePathSpawner.GetComponent<SpawnOnMap>().currentSite = 0;
        app.GetComponent<App>().nextSite_index = 2;
        app.GetComponent<App>().siteOn = false;
        app.GetComponent<App>().sitePathSpawner.GetComponent<SpawnOnMap>().showSitePath();
        app.GetComponent<App>().back_button.gameObject.SetActive(true);
        app.GetComponent<App>().showMap_button.gameObject.SetActive(false);
    }
}
