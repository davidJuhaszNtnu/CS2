using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Random=UnityEngine.Random;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using Mapbox.Examples;

public class Site2 : MonoBehaviour
{
    public GameObject well_prefab, pipe_prefab, membrane_prefab, tank_prefab;
    public GameObject site2, site2UI, welcomePanel, findPanel, itemsFoundPanel, incorrectOrderPanel, taskCompletedPanel, taskPanel, app, 
    distanceWarningPanel, gameController, languagePanel, tapOnObjectPanel;
    public GameObject pipe_image_findPanel_L, membrane_image_findPanel_L, tank_image_findPanel_L;
    public GameObject[] image_anchors_findPanel_L;
    public GameObject pipe_image_findPanel_P, membrane_image_findPanel_P, tank_image_findPanel_P;
    public GameObject[] image_anchors_findPanel_P;
    public Camera arCamera, mapCamera;
    int order, pipe_order, membrane_order, tank_order;
    bool pipeCollected, membraneCollected, tankCollected, allCollected;
    bool start_search;
    private float d_membrane, d_pipe, d_tank, d_well;
    public float showDistance, maxDistance;
    private bool objectShown;

    GameObject well, pipe, membrane, tank;

    Ray ray;
    RaycastHit hit;

    void Start()
    {
        restart();
    }

    public void restart(){
        pipeCollected = false;
        membraneCollected = false;
        tankCollected = false;
        allCollected = false;

        start_search = false;
        order=0;
        welcomePanel.SetActive(true);
        findPanel.SetActive(false);
        itemsFoundPanel.SetActive(false);
        incorrectOrderPanel.SetActive(false);
        taskPanel.SetActive(false);
        taskCompletedPanel.SetActive(false);

        pipe_image_findPanel_L.SetActive(false);
        membrane_image_findPanel_L.SetActive(false);
        tank_image_findPanel_L.SetActive(false);
        pipe_image_findPanel_P.SetActive(false);
        membrane_image_findPanel_P.SetActive(false);
        tank_image_findPanel_P.SetActive(false);
    }

    public void ok_welcomePanel_bbtn(){
        welcomePanel.SetActive(false);
        languagePanel.SetActive(false);
        findPanel.SetActive(true);
        start_search = true;
    }

    public void ok_itemsFoundPanel_bttn(){
        itemsFoundPanel.SetActive(false);
        // findPanel.SetActive(false);
        taskPanel.SetActive(true);

        string orientation;
        if(Screen.orientation == ScreenOrientation.Portrait)
            orientation = "portrait";
        else orientation = "landscape";
        site2UI.GetComponent<Site2UI>().setImages(pipe_order, membrane_order, tank_order, orientation);
    }
    
    public void startSite(){
        welcomePanel.SetActive(true);
        findPanel.SetActive(false);
        itemsFoundPanel.SetActive(false);
        taskPanel.SetActive(false);
        taskCompletedPanel.SetActive(false);

        pipe_image_findPanel_L.SetActive(false);
        membrane_image_findPanel_L.SetActive(false);
        tank_image_findPanel_L.SetActive(false);
        pipe_image_findPanel_P.SetActive(false);
        membrane_image_findPanel_P.SetActive(false);
        tank_image_findPanel_P.SetActive(false);

        well = Instantiate(well_prefab);
        pipe = Instantiate(pipe_prefab);
        membrane = Instantiate(membrane_prefab);
        tank = Instantiate(tank_prefab);
        well.transform.SetParent(transform, true);
        pipe.transform.SetParent(transform, true);
        membrane.transform.SetParent(transform, true);
        tank.transform.SetParent(transform, true);

        Vector3 dir = arCamera.transform.forward;
        well.transform.position = transform.position + Vector3.Normalize(new Vector3(dir.x,0f,dir.z))*1f + new Vector3(0f, -0.5f, 0f);
        well.AddComponent<ARAnchor>();
        // well.transform.rotation = Quaternion.LookRotation(new Vector3(dir.x,0f,dir.z), Vector3.up)*Quaternion.Euler(-90,0,0);

        float angle;
        float dist;
        angle = Random.Range(0, 2*(float)Math.PI);
        dist = Random.Range(0.5f, maxDistance);

        pipe.transform.position = well.transform.position + (new Vector3(dist*(float)Math.Cos(angle),0.5f,dist*(float)Math.Sin(angle))) + new Vector3(0f, -0f, 0f);
        pipe.name = "pipe";
        pipe.AddComponent<ARAnchor>();

        angle = Random.Range(0, 2*(float)Math.PI);
        dist = Random.Range(0.5f, maxDistance);

        membrane.transform.position = well.transform.position + (new Vector3(dist*(float)Math.Cos(angle),0.5f,dist*(float)Math.Sin(angle))) + new Vector3(0f, -0f, 0f);
        membrane.name = "membrane";
        membrane.AddComponent<ARAnchor>();

        angle = Random.Range(0, 2*(float)Math.PI);
        dist = Random.Range(0.5f, maxDistance);
        tank.transform.position = well.transform.position + (new Vector3(dist*(float)Math.Cos(angle),0.5f,dist*(float)Math.Sin(angle))) + new Vector3(0f, -0f, 0f);
        tank.name = "tank";
        tank.AddComponent<ARAnchor>();

        well.SetActive(true);
        pipe.SetActive(false);
        membrane.SetActive(false);
        tank.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        if(start_search && !allCollected){
            // allCollected=true;
            d_pipe=Vector2.Distance(new Vector2(arCamera.transform.position.x,arCamera.transform.position.z),new Vector2(pipe.transform.position.x,pipe.transform.position.z));
            d_membrane=Vector2.Distance(new Vector2(arCamera.transform.position.x,arCamera.transform.position.z),new Vector2(membrane.transform.position.x,membrane.transform.position.z));
            d_tank=Vector2.Distance(new Vector2(arCamera.transform.position.x,arCamera.transform.position.z),new Vector2(tank.transform.position.x,tank.transform.position.z));
            d_well=Vector2.Distance(new Vector2(arCamera.transform.position.x,arCamera.transform.position.z),new Vector2(well.transform.position.x,well.transform.position.z));

            if(d_well>maxDistance)
                distanceWarningPanel.SetActive(true);
            else distanceWarningPanel.SetActive(false);
            
            objectShown = false;
            if(d_pipe<showDistance && !pipeCollected){
                pipe.SetActive(true);
                objectShown = true;
                ray = arCamera.ScreenPointToRay(Input.mousePosition);
                if(Input.GetMouseButtonDown(0)){
                    if(Physics.Raycast(ray, out hit)){
                        if(hit.collider.name == "pipe"){
                            pipeCollected=true;
                            pipe.SetActive(false);
                            pipe_image_findPanel_L.SetActive(true);
                            pipe_image_findPanel_P.SetActive(true);
                            order++;
                            pipe_order = order;
                            pipe_image_findPanel_L.transform.SetParent(image_anchors_findPanel_L[order - 1].transform, true);
                            pipe_image_findPanel_L.GetComponent<RectTransform>().offsetMin = Vector2.zero;
                            pipe_image_findPanel_L.GetComponent<RectTransform>().offsetMax = Vector2.zero;

                            pipe_image_findPanel_P.transform.SetParent(image_anchors_findPanel_P[order - 1].transform, true);
                            pipe_image_findPanel_P.GetComponent<RectTransform>().offsetMin = Vector2.zero;
                            pipe_image_findPanel_P.GetComponent<RectTransform>().offsetMax = Vector2.zero;
                        }
                    }
                }
            }else{
                pipe.SetActive(false);
            }

            if(d_membrane<showDistance && !membraneCollected){
                membrane.SetActive(true);
                objectShown = true;
                ray = arCamera.ScreenPointToRay(Input.mousePosition);
                if(Input.GetMouseButtonDown(0)){
                    if(Physics.Raycast(ray, out hit)){
                        if(hit.collider.name == "membrane"){
                            membraneCollected=true;
                            membrane.SetActive(false);
                            membrane_image_findPanel_L.SetActive(true);
                            membrane_image_findPanel_P.SetActive(true);
                            order++;
                            membrane_order = order;
                            membrane_image_findPanel_L.transform.SetParent(image_anchors_findPanel_L[order - 1].transform, true);
                            membrane_image_findPanel_L.GetComponent<RectTransform>().offsetMin = Vector2.zero;
                            membrane_image_findPanel_L.GetComponent<RectTransform>().offsetMax = Vector2.zero;

                            membrane_image_findPanel_P.transform.SetParent(image_anchors_findPanel_P[order - 1].transform, true);
                            membrane_image_findPanel_P.GetComponent<RectTransform>().offsetMin = Vector2.zero;
                            membrane_image_findPanel_P.GetComponent<RectTransform>().offsetMax = Vector2.zero;
                        }
                    }
                }
            }else{
                membrane.SetActive(false);
            }

            if(d_tank<showDistance && !tankCollected){
                tank.SetActive(true);
                objectShown = true;
                ray = arCamera.ScreenPointToRay(Input.mousePosition);
                if(Input.GetMouseButtonDown(0)){
                    if(Physics.Raycast(ray, out hit)){
                        if(hit.collider.name == "tank"){
                            tankCollected=true;
                            tank.SetActive(false);
                            tank_image_findPanel_L.SetActive(true);
                            tank_image_findPanel_P.SetActive(true);
                            order++;
                            tank_order = order;
                            tank_image_findPanel_L.transform.SetParent(image_anchors_findPanel_L[order - 1].transform, true);
                            tank_image_findPanel_L.GetComponent<RectTransform>().offsetMin = Vector2.zero;
                            tank_image_findPanel_L.GetComponent<RectTransform>().offsetMax = Vector2.zero;

                            tank_image_findPanel_P.transform.SetParent(image_anchors_findPanel_P[order - 1].transform, true);
                            tank_image_findPanel_P.GetComponent<RectTransform>().offsetMin = Vector2.zero;
                            tank_image_findPanel_P.GetComponent<RectTransform>().offsetMax = Vector2.zero;
                        }
                    }
                }
            }else{
                tank.SetActive(false);
            }

            if(objectShown)
                tapOnObjectPanel.SetActive(true);
            else tapOnObjectPanel.SetActive(false);

            if(pipeCollected && membraneCollected && tankCollected){
                allCollected=true;
                gameController.GetComponent<gameController>().updateStatus(0, true);
                distanceWarningPanel.SetActive(false);
                findPanel.SetActive(false);
                tapOnObjectPanel.SetActive(false);
                languagePanel.SetActive(true);
                itemsFoundPanel.SetActive(true);
            }
        }
    }

    public void nextSite(){
        Destroy(well);
        Destroy(pipe);
        Destroy(membrane);
        Destroy(tank);
        arCamera.enabled=false;
        mapCamera.enabled=true;
        app.GetComponent<App>().map.SetActive(true);
        app.GetComponent<App>().player.SetActive(true);
        app.GetComponent<App>().sitePathSpawner.GetComponent<SpawnOnMap>().currentSite = 1;
        app.GetComponent<App>().nextSite_index = 3;
        app.GetComponent<App>().siteOn = false;
        app.GetComponent<App>().sitePathSpawner.GetComponent<SpawnOnMap>().showSitePath();
        app.GetComponent<App>().back_button.gameObject.SetActive(true);
        app.GetComponent<App>().showMap_button.gameObject.SetActive(false);
        site2.SetActive(false);
        site2UI.SetActive(false);
    }
}
