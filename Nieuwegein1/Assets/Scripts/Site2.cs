using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Random=UnityEngine.Random;
using UnityEngine.UI;

public class Site2 : MonoBehaviour
{
    public GameObject well,pipe,membrane,tank;
    public GameObject site2, site2UI, path1, welcomeCanvas, findPanel, completedPanel, taskPanel, path2, site3, site3UI;
    public Button Ok;
    public Camera arCamera;
    public TextMeshProUGUI findText;
    public TextMeshProUGUI collectedText;
    public TextMeshProUGUI congratsText;
    public GameObject pipeImage;
    public GameObject membraneImage;
    public GameObject tankImage;
    Vector2 first_position,second_position,third_position, first_anchorMin, first_anchorMax, second_anchorMin, second_anchorMax, third_anchorMin, third_anchorMax;
    int order, pipe_order, membrane_order, tank_order;
    bool bool1, bool3, pipeCollected, membraneCollected, tankCollected, allCollected;
    public bool bool2, bool4;
    public float timeFindText, timeCongratsPanel;
    float timeCongratsText;
    public bool showSite;

    Ray ray;
    RaycastHit hit;

    void Start()
    {
        bool1=true;
        showSite=false;
        bool2=false;
        bool3=true;
        bool4=false;
        pipeCollected=false;
        membraneCollected=false;
        tankCollected=false;
        allCollected=false;
        well.SetActive(false);
        pipe.SetActive(false);
        membrane.SetActive(false);
        tank.SetActive(false);
        findText.gameObject.SetActive(false);
        collectedText.gameObject.SetActive(false);
        congratsText.gameObject.SetActive(false);
        pipeImage.SetActive(false);
        membraneImage.SetActive(false);
        tankImage.SetActive(false);
        order=0;
        Ok.onClick.AddListener(ok);
    }

    private void ok(){
        welcomeCanvas.SetActive(false);
        findPanel.SetActive(true);
        timeFindText=Time.time;
        bool2=true;
        findText.gameObject.SetActive(true);
        collectedText.gameObject.SetActive(false);
        first_position=pipeImage.transform.GetComponent<RectTransform>().anchoredPosition;
        first_anchorMin=pipeImage.transform.GetComponent<RectTransform>().anchorMin;
        first_anchorMax=pipeImage.transform.GetComponent<RectTransform>().anchorMax;
        second_position=membraneImage.transform.GetComponent<RectTransform>().anchoredPosition;
        second_anchorMin=membraneImage.transform.GetComponent<RectTransform>().anchorMin;
        second_anchorMax=membraneImage.transform.GetComponent<RectTransform>().anchorMax;
        third_position=tankImage.transform.GetComponent<RectTransform>().anchoredPosition;
        third_anchorMin=tankImage.transform.GetComponent<RectTransform>().anchorMin;
        third_anchorMax=tankImage.transform.GetComponent<RectTransform>().anchorMax;
    }

    // Update is called once per frame
    void Update()
    {
        // if(showSite && bool1){
        if(path1.GetComponent<Path>().arrived && bool1){
            bool1=false;
            path1.GetComponent<Path>().Arrow.SetActive(false);
            welcomeCanvas.SetActive(true);
            well.SetActive(true);
            //well.transform.parent=site2.transform;
            well.transform.position=site2.transform.position+(new Vector3(0f,-1f,0.5f));
            Vector3 dir=arCamera.transform.position-well.transform.position;
            well.transform.rotation=Quaternion.LookRotation(new Vector3(dir.x,0f,dir.z), Vector3.up)*Quaternion.Euler(-90,180,0);

            float angle;
            float dist;
            angle=Random.Range(0, 2*(float)Math.PI);
            // dist=Random.Range(0.5f, 2.5f);
            dist=Random.Range(0.5f, 1f);

            pipe.transform.position=well.transform.position+(new Vector3(dist*(float)Math.Cos(angle),0.5f,dist*(float)Math.Sin(angle)));
            pipe.name="pipe";

            angle=Random.Range(0, 2*(float)Math.PI);
            dist=Random.Range(0.5f, 1f);
    
            membrane.transform.position=well.transform.position+(new Vector3(dist*(float)Math.Cos(angle),0.5f,dist*(float)Math.Sin(angle)));
            membrane.name="membrane";

            angle=Random.Range(0, 2*(float)Math.PI);
            dist=Random.Range(0.5f, 1f);
            tank.transform.position=well.transform.position+(new Vector3(dist*(float)Math.Cos(angle),0.5f,dist*(float)Math.Sin(angle)));
            tank.name="tank";
        }
        if(bool2 && Time.time>timeFindText+3f && !allCollected){
            // allCollected=true;
            findText.gameObject.SetActive(false);
            collectedText.gameObject.SetActive(true);

            float d_pipe=Vector2.Distance(new Vector2(arCamera.transform.position.x,arCamera.transform.position.z),new Vector2(pipe.transform.position.x,pipe.transform.position.z));
            float d_membrane=Vector2.Distance(new Vector2(arCamera.transform.position.x,arCamera.transform.position.z),new Vector2(membrane.transform.position.x,membrane.transform.position.z));
            float d_tank=Vector2.Distance(new Vector2(arCamera.transform.position.x,arCamera.transform.position.z),new Vector2(tank.transform.position.x,tank.transform.position.z));

            if(d_pipe<10.5f && !pipeCollected){
                pipe.SetActive(true);
                ray = arCamera.ScreenPointToRay(Input.mousePosition);
                if(Input.GetMouseButtonDown(0)){
                    if(Physics.Raycast(ray, out hit)){
                        if(hit.collider.name == "pipe"){
                            pipeCollected=true;
                            pipe.SetActive(false);
                            pipeImage.SetActive(true);
                            order++;
                            if(order==1){
                                pipeImage.transform.GetComponent<RectTransform>().anchoredPosition=first_position;
                                pipeImage.transform.GetComponent<RectTransform>().anchorMin=first_anchorMin;
                                pipeImage.transform.GetComponent<RectTransform>().anchorMax=first_anchorMax;
                                pipe_order=1;
                            } else if(order==2){
                                pipeImage.transform.GetComponent<RectTransform>().anchoredPosition=second_position;
                                pipeImage.transform.GetComponent<RectTransform>().anchorMin=second_anchorMin;
                                pipeImage.transform.GetComponent<RectTransform>().anchorMax=second_anchorMax;
                                pipe_order=2;
                                // site2UI.GetComponent<Site2UI>().pipe_image.transform.position=site2UI.GetComponent<Site2UI>().membrane_position;
                            }else if(order==3){
                                pipeImage.transform.GetComponent<RectTransform>().anchoredPosition=third_position;
                                pipeImage.transform.GetComponent<RectTransform>().anchorMin=third_anchorMin;
                                pipeImage.transform.GetComponent<RectTransform>().anchorMax=third_anchorMax;
                                pipe_order=3;
                                // site2UI.GetComponent<Site2UI>().pipe_image.transform.position=site2UI.GetComponent<Site2UI>().tank_position;
                            }
                        }
                    }
                }
            }else pipe.SetActive(false);

            if(d_membrane<10.5f && !membraneCollected){
                membrane.SetActive(true);
                ray = arCamera.ScreenPointToRay(Input.mousePosition);
                if(Input.GetMouseButtonDown(0)){
                    if(Physics.Raycast(ray, out hit)){
                        if(hit.collider.name == "membrane"){
                            membraneCollected=true;
                            membrane.SetActive(false);
                            membraneImage.SetActive(true);
                            order++;
                            if(order==1){
                                membraneImage.transform.GetComponent<RectTransform>().anchoredPosition=first_position;
                                membraneImage.transform.GetComponent<RectTransform>().anchorMin=first_anchorMin;
                                membraneImage.transform.GetComponent<RectTransform>().anchorMax=first_anchorMax;
                                membrane_order=1;
                                // site2UI.GetComponent<Site2UI>().membrane_image.transform.position=site2UI.GetComponent<Site2UI>().pipe_position;
                            }else if(order==2){
                                membraneImage.transform.GetComponent<RectTransform>().anchoredPosition=second_position;
                                membraneImage.transform.GetComponent<RectTransform>().anchorMin=second_anchorMin;
                                membraneImage.transform.GetComponent<RectTransform>().anchorMax=second_anchorMax;
                                membrane_order=2;
                            }else if(order==3){
                                membraneImage.transform.GetComponent<RectTransform>().anchoredPosition=third_position;
                                membraneImage.transform.GetComponent<RectTransform>().anchorMin=third_anchorMin;
                                membraneImage.transform.GetComponent<RectTransform>().anchorMax=third_anchorMax;
                                membrane_order=3;
                                // site2UI.GetComponent<Site2UI>().membrane_image.transform.position=site2UI.GetComponent<Site2UI>().tank_position;
                            }
                        }
                    }
                }
            }else membrane.SetActive(false);

            if(d_tank<10.5f && !tankCollected){
                tank.SetActive(true);
                ray = arCamera.ScreenPointToRay(Input.mousePosition);
                if(Input.GetMouseButtonDown(0)){
                    if(Physics.Raycast(ray, out hit)){
                        if(hit.collider.name == "tank"){
                            tankCollected=true;
                            tank.SetActive(false);
                            tankImage.SetActive(true);
                            order++;
                            if(order==1){
                                tankImage.transform.GetComponent<RectTransform>().anchoredPosition=first_position;
                                tankImage.transform.GetComponent<RectTransform>().anchorMin=first_anchorMin;
                                tankImage.transform.GetComponent<RectTransform>().anchorMax=first_anchorMax;
                                tank_order=1;
                                // site2UI.GetComponent<Site2UI>().tank_image.transform.position=site2UI.GetComponent<Site2UI>().pipe_position;
                            }else if(order==2){
                                tankImage.transform.GetComponent<RectTransform>().anchoredPosition=second_position;
                                tankImage.transform.GetComponent<RectTransform>().anchorMin=second_anchorMin;
                                tankImage.transform.GetComponent<RectTransform>().anchorMax=second_anchorMax;
                                tank_order=2;
                                // site2UI.GetComponent<Site2UI>().tank_image.transform.position=site2UI.GetComponent<Site2UI>().membrane_position;
                            }else if(order==3){
                                tankImage.transform.GetComponent<RectTransform>().anchoredPosition=third_position;
                                tankImage.transform.GetComponent<RectTransform>().anchorMin=third_anchorMin;
                                tankImage.transform.GetComponent<RectTransform>().anchorMax=third_anchorMax;
                                tank_order=3;
                            }
                        }
                    }
                }
            }else tank.SetActive(false);

            if(pipeCollected && membraneCollected && tankCollected){
                allCollected=true;
                timeCongratsText=Time.time;
                congratsText.gameObject.SetActive(true);
                Screen.orientation = ScreenOrientation.Portrait;
                Screen.autorotateToLandscapeLeft = false;
                Screen.autorotateToLandscapeRight = false;
            }
        }

        if(allCollected && Time.time>timeCongratsText+3f && bool3){
            congratsText.gameObject.SetActive(false);
            collectedText.gameObject.SetActive(false);
            pipeImage.SetActive(false);
            membraneImage.SetActive(false);
            tankImage.SetActive(false);
            findPanel.SetActive(false);
            taskPanel.SetActive(true);

            site2UI.GetComponent<Site2UI>().setImages(pipe_order, membrane_order, tank_order);

            bool3=false;
        }
        if(bool4 && Time.time>timeCongratsPanel+3f){
            bool4=false;
            congratsText.gameObject.SetActive(false);
            site2.SetActive(false);
            site2UI.SetActive(false);
            // site3.SetActive(true);
            // site3UI.SetActive(true);
            
            site3.GetComponent<Site3>().showSite=true;
            
            path2.GetComponent<Path>().t=0f;
            path2.SetActive(true);
            path2.GetComponent<Path>().arrived=false;
            path2.GetComponent<Path>().Arrow.SetActive(true);
            path2.GetComponent<Path>().Arrow.transform.position=path2.GetComponent<Path>().path(0f);
            path2.GetComponent<Path>().Arrow.transform.rotation=Quaternion.LookRotation(path2.GetComponent<Path>().pathDer(0f), Vector3.up);   
        }
    }
}
