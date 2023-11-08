using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.IO;
using UnityEngine.UI;
using Random=UnityEngine.Random;
using Mapbox.Examples;
using UnityEngine.XR.ARFoundation;

public class Site3 : MonoBehaviour
{
    public Camera arCamera, mapCamera, mazeCamera, followCamera;

    public GameObject app, pipe_prefab, air, welcomePanel, taskPanel, taskCompletedPanel, warningPanel, foundBrokenPipePanel,
    pipeOpenedPanel, multichoice1Panel, multichoice2Panel, infoToMultichoice1Panel, infoToMultichoice2Panel, multichoicesCompletedPanel,
    distanceWarningPanel, site3, site3UI, broken_pipe_prefab, gameController, tapOnObjectPanel, infoBubblePanel;

    private float d_air, d_pipe;
    public float showDistance, maxDistance;

    public bool alreadyInstantiated;
    
    GameObject pipe, broken_pipe;
    GameObject valve, infoBubbleCanvas, clickToOpenCanvas;
    bool start_search, waitForValveClick, valveRotating;
    public bool mazeInteractionOn;
    float timeValveRotation;
    
    //maze
    int[,] mazePlan;
    public GameObject maze, block, mazePanel, miniMapPanel;
    public Material floorMaterial, night, day;
    public Button forwardButton;
    public Vector3 normal;
    public bool  collision, foundLeak;

    Vector3 mazeCameraPosition, airPosition;
    GameObject aRSessionOrigin;
    Quaternion airRotation;
    bool isPressed;
    float cubeSide;

    // score update 1 panel
    bool[] answered;
    bool[] correct_answer;

    public GameObject[] toggles_L;
    public GameObject[] toggles_P;
    public GameObject[] toggles_score_L;
    public GameObject[] toggles_score_P;

    public GameObject[] scratches_L;
    public GameObject[] scratches_P;
    public GameObject[] points_L;
    public GameObject[] points_P;

    public TextMeshProUGUI score_text_L, score_text_P, scoreText_multichoiceCompletedPanel_L, scoreText_multichoiceCompletedPanel_P;
    public GameObject tank_L, tank_P, droplet_L, droplet_P, scoreUpdate1Panel, tank_multichoiceCompletedPanel_L, tank_multichoiceCompletedPanel_P;

    bool animate;
    float minLevel_P, minLevel_L, t, dt, oldLevel_P, newLevel_P, currentLevel_P, oldLevel_L, newLevel_L, currentLevel_L, minLevel_multichoiceCompletedPanel_L, minLevel_multichoiceCompletedPanel_P;

    // score update 2 panel
    bool[] answered2;
    bool[] correct_answer2;

    public GameObject[] toggles_L2;
    public GameObject[] toggles_P2;
    public GameObject[] toggles_score_L2;
    public GameObject[] toggles_score_P2;

    public GameObject[] scratches_L2;
    public GameObject[] scratches_P2;
    public GameObject[] points_L2;
    public GameObject[] points_P2;

    public TextMeshProUGUI score_text_L2, score_text_P2;
    public GameObject tank_L2, tank_P2, droplet_L2, droplet_P2, scoreUpdate2Panel;
    bool animate2;

    Ray ray;
    RaycastHit hit;

    void Start()
    {
        //multichoice
        float aspectRatio = (float)Screen.width/(float)Screen.height;
        if(Screen.orientation == ScreenOrientation.Portrait){
            minLevel_P = tank_P.GetComponent<RectTransform>().rect.height;
            minLevel_L = tank_L.GetComponent<RectTransform>().rect.height * aspectRatio;

            minLevel_multichoiceCompletedPanel_P = tank_multichoiceCompletedPanel_P.GetComponent<RectTransform>().rect.height;
            minLevel_multichoiceCompletedPanel_L = tank_multichoiceCompletedPanel_L.GetComponent<RectTransform>().rect.height * aspectRatio;
        }else{
            minLevel_L = tank_L.GetComponent<RectTransform>().rect.height;
            minLevel_P = tank_P.GetComponent<RectTransform>().rect.height * aspectRatio;

            minLevel_multichoiceCompletedPanel_L = tank_multichoiceCompletedPanel_L.GetComponent<RectTransform>().rect.height;
            minLevel_multichoiceCompletedPanel_P = tank_multichoiceCompletedPanel_P.GetComponent<RectTransform>().rect.height * aspectRatio;
        }

        alreadyInstantiated = false;
    }

    public void startSite(){
        arCamera.enabled = true;
        mapCamera.enabled = false;
        mazeCamera.enabled = false;
        followCamera.enabled = false;
        pipe = Instantiate(pipe_prefab);
        pipe.transform.SetParent(transform, true);
        welcomePanel.SetActive(true);
        taskPanel.SetActive(false);
        taskCompletedPanel.SetActive(false);
        warningPanel.SetActive(false);
        mazePanel.SetActive(false);
        miniMapPanel.SetActive(false);
        forwardButton.gameObject.SetActive(false);
        foundBrokenPipePanel.SetActive(false);
        pipeOpenedPanel.SetActive(false);
        multichoice1Panel.SetActive(false);
        multichoice2Panel.SetActive(false);
        infoToMultichoice1Panel.SetActive(false);
        infoToMultichoice2Panel.SetActive(false);
        multichoicesCompletedPanel.SetActive(false);
        pipe.SetActive(true);
        air.SetActive(false);
        //click to open canvas
        pipe.transform.GetChild(0).gameObject.SetActive(false);
        valve = pipe.transform.GetChild(1).gameObject;
        clickToOpenCanvas = pipe.transform.GetChild(0).gameObject;
        clickToOpenCanvas.SetActive(false);
        infoBubbleCanvas = air.transform.GetChild(0).GetChild(0).gameObject;
        infoBubbleCanvas.SetActive(false);
        start_search = false;
        waitForValveClick = false;
        valveRotating = false;
        mazeInteractionOn = false;
        foundLeak = false;
        collision = false;
        isPressed = false;

        float angle;
        float dist;
        angle = Random.Range(0, 2*(float)Math.PI);
        dist = Random.Range(0.5f, maxDistance);

        Vector3 dir = arCamera.transform.forward;
        pipe.transform.position = transform.position + Vector3.Normalize(new Vector3(dir.x,0f,dir.z))*1f + new Vector3(-0.12f, -0.75f, 0f);

        dir = arCamera.transform.position - pipe.transform.position;
        pipe.transform.rotation = Quaternion.LookRotation(new Vector3(dir.x,0f,dir.z), Vector3.up)*Quaternion.Euler(0f, -90f, 0f);
        
        air.transform.position = pipe.transform.position + (new Vector3(dist*(float)Math.Cos(angle),0f,dist*(float)Math.Sin(angle)));
        // air.transform.position = pipe.transform.position;
        if(alreadyInstantiated)
            air.transform.localScale = air.transform.localScale/0.4f;

        // score update 1
        answered = new bool[4];
        correct_answer = new bool[4];

        for (int i = 0; i < answered.Length; i++)
            answered[i] = false;
        correct_answer[0] = true;
        correct_answer[1] = true;
        correct_answer[2] = false;
        correct_answer[3] = false;

        animate = false;
        droplet_L.SetActive(false);
        droplet_P.SetActive(false);

        // score update 2
        answered2 = new bool[4];
        correct_answer2 = new bool[4];

        for (int i = 0; i < answered2.Length; i++)
            answered2[i] = false;
        correct_answer2[0] = false;
        correct_answer2[1] = true;
        correct_answer2[2] = true;
        correct_answer2[3] = false;

        droplet_L2.SetActive(false);
        droplet_P2.SetActive(false);

        animate2 = false;
    }

    public void ok_welcomePanel_bttn(){
        welcomePanel.SetActive(false);
        start_search = true;
    }

    public void ok_infoBubbleCanvas_bttn(){
        // gained components
        gameController.GetComponent<gameController>().updateStatus(0, true);

        infoBubbleCanvas.SetActive(false);
        infoBubblePanel.SetActive(false);
        taskPanel.SetActive(true);

        string orientation;
        if(Screen.orientation == ScreenOrientation.Portrait)
            orientation = "portrait";
        else orientation = "landscape";
        site3UI.GetComponent<Site3UI>().setImages(2, 1, 3, orientation);
    }

    public void switchInfoBubblesWorld_bttn(){
        infoBubbleCanvas.SetActive(false);
        infoBubblePanel.SetActive(true);
    }

    public void switchInfoBubblesScreen_bttn(){
        infoBubbleCanvas.SetActive(true);
        infoBubblePanel.SetActive(false);
    }

    public void ok_warningPanel_bttn(){
        warningPanel.SetActive(false);
        pipe.SetActive(false);
        maze.SetActive(true);
        mazePanel.SetActive(true);
        miniMapPanel.SetActive(true);
        forwardButton.gameObject.SetActive(true);
        forwardButton.interactable = false;
        // forwardButton_P.interactable = true;
        mazeInteractionOn = true;
        air.SetActive(true);
        RenderSettings.skybox = night;
        arCamera.enabled = false;
        mapCamera.enabled = false;
        mazeCamera.enabled = true;
        followCamera.enabled = true;
        broken_pipe = Instantiate(broken_pipe_prefab);
        broken_pipe.transform.SetParent(transform, true);
        broken_pipe.SetActive(true);
        generateMaze();
    }

    public void ok_mazePanel_bttn(){
        mazePanel.SetActive(false);
        forwardButton.interactable = true;
        // forwardButton_P.interactable = true;
    }

    public void taskCompleted(){
        air.SetActive(false);
        Vector3 dir = arCamera.transform.position - pipe.transform.position;
        clickToOpenCanvas.transform.rotation = Quaternion.LookRotation(new Vector3(dir.x,0f,dir.z), Vector3.up)*Quaternion.Euler(0f, 180f, 0f);
        clickToOpenCanvas.SetActive(true);
        waitForValveClick = true;
    }

    public void ok_foundBrokenPipePanel_bttn(){
        maze.SetActive(false);
        air.SetActive(false);
        broken_pipe.transform.GetChild(0).gameObject.SetActive(true);
        broken_pipe.SetActive(false);
        foundBrokenPipePanel.SetActive(false);
        pipe.SetActive(true);
        Vector3 dir = arCamera.transform.position - pipe.transform.position;
        clickToOpenCanvas.transform.rotation = Quaternion.LookRotation(new Vector3(dir.x,0f,dir.z), Vector3.up)*Quaternion.Euler(0f, 180f, 0f);
        clickToOpenCanvas.SetActive(true);
        waitForValveClick = true;

        mazeCamera.enabled=false;
        followCamera.enabled = false;
        mapCamera.enabled=false;
        arCamera.enabled=true;
    }

    public void ok_pipeOpenedPanel_bttn(){
        pipeOpenedPanel.SetActive(false);
        multichoice1Panel.SetActive(true);
    }

    //multichoice 1
    public void toggle1(Toggle toggle){
        answered[0] = toggle.isOn;
        toggles_L[0].GetComponent<Toggle>().isOn = toggle.isOn;
        toggles_P[0].GetComponent<Toggle>().isOn = toggle.isOn;
        toggles_score_L[0].GetComponent<Toggle>().isOn = toggle.isOn;
        toggles_score_P[0].GetComponent<Toggle>().isOn = toggle.isOn;
    }

    public void toggle2(Toggle toggle){
        answered[1] = toggle.isOn;
        toggles_L[1].GetComponent<Toggle>().isOn = toggle.isOn;
        toggles_P[1].GetComponent<Toggle>().isOn = toggle.isOn;
        toggles_score_L[1].GetComponent<Toggle>().isOn = toggle.isOn;
        toggles_score_P[1].GetComponent<Toggle>().isOn = toggle.isOn;
    }

    public void toggle3(Toggle toggle){
        answered[2] = toggle.isOn;
        toggles_L[2].GetComponent<Toggle>().isOn = toggle.isOn;
        toggles_P[2].GetComponent<Toggle>().isOn = toggle.isOn;
        toggles_score_L[2].GetComponent<Toggle>().isOn = toggle.isOn;
        toggles_score_P[2].GetComponent<Toggle>().isOn = toggle.isOn;
    }

    public void toggle4(Toggle toggle){
        answered[3] = toggle.isOn;
        toggles_L[3].GetComponent<Toggle>().isOn = toggle.isOn;
        toggles_P[3].GetComponent<Toggle>().isOn = toggle.isOn;
        toggles_score_L[3].GetComponent<Toggle>().isOn = toggle.isOn;
        toggles_score_P[3].GetComponent<Toggle>().isOn = toggle.isOn;
    }

    //multichoice 2
    public void toggle12(Toggle toggle){
        answered2[0] = toggle.isOn;
        toggles_L2[0].GetComponent<Toggle>().isOn = toggle.isOn;
        toggles_P2[0].GetComponent<Toggle>().isOn = toggle.isOn;
        toggles_score_L2[0].GetComponent<Toggle>().isOn = toggle.isOn;
        toggles_score_P2[0].GetComponent<Toggle>().isOn = toggle.isOn;
    }

    public void toggle22(Toggle toggle){
        answered2[1] = toggle.isOn;
        toggles_L2[1].GetComponent<Toggle>().isOn = toggle.isOn;
        toggles_P2[1].GetComponent<Toggle>().isOn = toggle.isOn;
        toggles_score_L2[1].GetComponent<Toggle>().isOn = toggle.isOn;
        toggles_score_P2[1].GetComponent<Toggle>().isOn = toggle.isOn;
    }

    public void toggle32(Toggle toggle){
        answered2[2] = toggle.isOn;
        toggles_L2[2].GetComponent<Toggle>().isOn = toggle.isOn;
        toggles_P2[2].GetComponent<Toggle>().isOn = toggle.isOn;
        toggles_score_L2[2].GetComponent<Toggle>().isOn = toggle.isOn;
        toggles_score_P2[2].GetComponent<Toggle>().isOn = toggle.isOn;
    }

    public void toggle42(Toggle toggle){
        answered2[3] = toggle.isOn;
        toggles_L2[3].GetComponent<Toggle>().isOn = toggle.isOn;
        toggles_P2[3].GetComponent<Toggle>().isOn = toggle.isOn;
        toggles_score_L2[3].GetComponent<Toggle>().isOn = toggle.isOn;
        toggles_score_P2[3].GetComponent<Toggle>().isOn = toggle.isOn;
    }

    public void ok_multichoice1Panel_bttn(){
        multichoice1Panel.SetActive(false);
        float oldScore = (float)gameController.GetComponent<gameController>().score;
        for (int i = 0; i < answered.Length; i++){
            if(answered[i] != correct_answer[i]){
                scratches_P[i].SetActive(true);
                points_P[i].GetComponent<TextMeshProUGUI>().text = "-20";
                points_P[i].transform.GetChild(0).gameObject.SetActive(false);
                scratches_L[i].SetActive(true);
                points_L[i].GetComponent<TextMeshProUGUI>().text = "-20";
                points_L[i].transform.GetChild(0).gameObject.SetActive(false);
                gameController.GetComponent<gameController>().score -= 20;
                gameController.GetComponent<gameController>().updateStatus(20, false);
            }else{
                scratches_P[i].SetActive(false);
                points_P[i].GetComponent<TextMeshProUGUI>().text = "";
                points_P[i].transform.GetChild(0).gameObject.SetActive(true);
                scratches_L[i].SetActive(false);
                points_L[i].GetComponent<TextMeshProUGUI>().text = "";
                points_L[i].transform.GetChild(0).gameObject.SetActive(true);
            }
        }
        score_text_L.text = "Your new score: " + gameController.GetComponent<gameController>().score.ToString() + " liters";
        score_text_P.text = "Your new score: " + gameController.GetComponent<gameController>().score.ToString() + " liters";

        //update water level in the tank
        setupAnimationWaterLevel((float)gameController.GetComponent<gameController>().score, oldScore);

        scoreUpdate1Panel.SetActive(true);
    }

    private void setupAnimationWaterLevel(float newScore, float oldScore){
        oldLevel_P = -(minLevel_P-oldScore/(float)gameController.GetComponent<gameController>().maxScore*minLevel_P);
        newLevel_P = -(minLevel_P-newScore/(float)gameController.GetComponent<gameController>().maxScore*minLevel_P);
        oldLevel_L = -(minLevel_L-oldScore/(float)gameController.GetComponent<gameController>().maxScore*minLevel_L);
        newLevel_L = -(minLevel_L-newScore/(float)gameController.GetComponent<gameController>().maxScore*minLevel_L);
        t = 0f;
        dt = 0.01f;
        if(oldScore != newScore){
            animate = true;
            droplet_L.SetActive(true);
            droplet_P.SetActive(true);
        }else{
            tank_P.GetComponent<RectTransform>().offsetMax = new Vector2(0f, oldLevel_P);
            tank_P.GetComponent<RectTransform>().offsetMin = new Vector2(0f, oldLevel_P);
            tank_L.GetComponent<RectTransform>().offsetMax = new Vector2(0f, oldLevel_L);
            tank_L.GetComponent<RectTransform>().offsetMin = new Vector2(0f, oldLevel_L);
        }
    }

    private void animateWaterLevel(){
        if(t<1f){
            currentLevel_P = oldLevel_P + (newLevel_P - oldLevel_P)*t;
            currentLevel_L = oldLevel_L + (newLevel_L - oldLevel_L)*t;
            tank_P.GetComponent<RectTransform>().offsetMax = new Vector2(0f, currentLevel_P);
            tank_P.GetComponent<RectTransform>().offsetMin = new Vector2(0f, currentLevel_P);
            tank_L.GetComponent<RectTransform>().offsetMax = new Vector2(0f, currentLevel_L);
            tank_L.GetComponent<RectTransform>().offsetMin = new Vector2(0f, currentLevel_L);
            t += dt;
        }else{
            animate = false;
            droplet_L.SetActive(false);
            droplet_P.SetActive(false);
        }
    }

    public void ok_scoreUpdate1Panel_bttn(){
        scoreUpdate1Panel.SetActive(false);
        infoToMultichoice1Panel.SetActive(true);
    }

    public void ok_scoreUpdate2Panel_bttn(){
        scoreUpdate2Panel.SetActive(false);
        //update tank level on multichoiceCompletedPanel
        scoreText_multichoiceCompletedPanel_L.text = "You have completed this site.\nYour score is " + gameController.GetComponent<gameController>().score.ToString() + " liters.";
        scoreText_multichoiceCompletedPanel_P.text = scoreText_multichoiceCompletedPanel_L.text;
        float level_P = -(minLevel_multichoiceCompletedPanel_P-(float)gameController.GetComponent<gameController>().score/(float)gameController.GetComponent<gameController>().maxScore*minLevel_multichoiceCompletedPanel_P);
        float level_L = -(minLevel_multichoiceCompletedPanel_L-(float)gameController.GetComponent<gameController>().score/(float)gameController.GetComponent<gameController>().maxScore*minLevel_multichoiceCompletedPanel_L);
        tank_multichoiceCompletedPanel_P.GetComponent<RectTransform>().offsetMax = new Vector2(0f, level_P);
        tank_multichoiceCompletedPanel_P.GetComponent<RectTransform>().offsetMin = new Vector2(0f, level_P);
        tank_multichoiceCompletedPanel_L.GetComponent<RectTransform>().offsetMax = new Vector2(0f, level_L);
        tank_multichoiceCompletedPanel_L.GetComponent<RectTransform>().offsetMin = new Vector2(0f, level_L);

        multichoicesCompletedPanel.SetActive(true);
    }

    public void ok_multichoice2Panel_bttn(){
        multichoice2Panel.SetActive(false);
        float oldScore = (float)gameController.GetComponent<gameController>().score;
        for (int i = 0; i < answered2.Length; i++){
            if(answered2[i] != correct_answer2[i]){
                scratches_P2[i].SetActive(true);
                points_P2[i].GetComponent<TextMeshProUGUI>().text = "-20";
                points_P2[i].transform.GetChild(0).gameObject.SetActive(false);
                scratches_L2[i].SetActive(true);
                points_L2[i].GetComponent<TextMeshProUGUI>().text = "-20";
                points_L2[i].transform.GetChild(0).gameObject.SetActive(false);
                gameController.GetComponent<gameController>().score -= 20;
                gameController.GetComponent<gameController>().updateStatus(20, false);
            }else{
                scratches_P2[i].SetActive(false);
                points_P2[i].GetComponent<TextMeshProUGUI>().text = "";
                points_P2[i].transform.GetChild(0).gameObject.SetActive(true);
                scratches_L2[i].SetActive(false);
                points_L2[i].GetComponent<TextMeshProUGUI>().text = "";
                points_L2[i].transform.GetChild(0).gameObject.SetActive(true);
            }
        }
        score_text_L2.text = "Your new score: " + gameController.GetComponent<gameController>().score.ToString() + " liters";
        score_text_P2.text = "Your new score: " + gameController.GetComponent<gameController>().score.ToString() + " liters";

        //update water level in the tank
        setupAnimationWaterLevel2((float)gameController.GetComponent<gameController>().score, oldScore);

        scoreUpdate2Panel.SetActive(true);
    }

    private void setupAnimationWaterLevel2(float newScore, float oldScore){
        oldLevel_P = -(minLevel_P-oldScore/(float)gameController.GetComponent<gameController>().maxScore*minLevel_P);
        newLevel_P = -(minLevel_P-newScore/(float)gameController.GetComponent<gameController>().maxScore*minLevel_P);
        oldLevel_L = -(minLevel_L-oldScore/(float)gameController.GetComponent<gameController>().maxScore*minLevel_L);
        newLevel_L = -(minLevel_L-newScore/(float)gameController.GetComponent<gameController>().maxScore*minLevel_L);
        t = 0f;
        dt = 0.01f;
        if(oldScore != newScore){
            animate2 = true;
            droplet_L2.SetActive(true);
            droplet_P2.SetActive(true);
        }else{
            tank_P2.GetComponent<RectTransform>().offsetMax = new Vector2(0f, oldLevel_P);
            tank_P2.GetComponent<RectTransform>().offsetMin = new Vector2(0f, oldLevel_P);
            tank_L2.GetComponent<RectTransform>().offsetMax = new Vector2(0f, oldLevel_L);
            tank_L2.GetComponent<RectTransform>().offsetMin = new Vector2(0f, oldLevel_L);
        }
    }

    private void animateWaterLevel2(){
        if(t<1f){
            currentLevel_P = oldLevel_P + (newLevel_P - oldLevel_P)*t;
            currentLevel_L = oldLevel_L + (newLevel_L - oldLevel_L)*t;
            tank_P2.GetComponent<RectTransform>().offsetMax = new Vector2(0f, currentLevel_P);
            tank_P2.GetComponent<RectTransform>().offsetMin = new Vector2(0f, currentLevel_P);
            tank_L2.GetComponent<RectTransform>().offsetMax = new Vector2(0f, currentLevel_L);
            tank_L2.GetComponent<RectTransform>().offsetMin = new Vector2(0f, currentLevel_L);
            t += dt;
        }else{
            animate2 = false;
            droplet_L2.SetActive(false);
            droplet_P2.SetActive(false);
        }
    }

    public void ok_infoToMultichoice1Panel_bttn(){
        infoToMultichoice1Panel.SetActive(false);
        infoToMultichoice2Panel.SetActive(true);
    }

    public void ok_infoToMultichoice2Panel_bttn(){
        infoToMultichoice2Panel.SetActive(false);
        multichoice2Panel.SetActive(true);
    }

    public void ok_multichoicesCompletedPanel_bttn(){
        Destroy(pipe);
        Destroy(broken_pipe);
        alreadyInstantiated = true;
        arCamera.enabled = false;
        mapCamera.enabled = true;
        app.GetComponent<App>().map.SetActive(true);
        app.GetComponent<App>().player.SetActive(true);
        app.GetComponent<App>().sitePathSpawner.GetComponent<SpawnOnMap>().currentSite = 2;
        app.GetComponent<App>().nextSite_index = 4;
        app.GetComponent<App>().siteOn = false;
        app.GetComponent<App>().sitePathSpawner.GetComponent<SpawnOnMap>().showSitePath();
        app.GetComponent<App>().back_button.gameObject.SetActive(true);
        app.GetComponent<App>().showMap_button.gameObject.SetActive(false);
        site3.SetActive(false);
        site3UI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(animate)
            animateWaterLevel();
        if(animate2)
            animateWaterLevel2();

        if(start_search){
            d_air = Vector2.Distance(new Vector2(arCamera.transform.position.x,arCamera.transform.position.z),new Vector2(air.transform.position.x,air.transform.position.z));
            d_pipe = Vector2.Distance(new Vector2(arCamera.transform.position.x,arCamera.transform.position.z),new Vector2(pipe.transform.position.x,pipe.transform.position.z));
            
            if(d_pipe>maxDistance)
                distanceWarningPanel.SetActive(true);
            else distanceWarningPanel.SetActive(false);

            if(d_air < showDistance){
                air.SetActive(true);
                tapOnObjectPanel.SetActive(true);
                // air.transform.Rotate(0f,0f,1f, Space.Self);
                ray = arCamera.ScreenPointToRay(Input.mousePosition);
                if(Input.GetMouseButtonDown(0)){
                    if(Physics.Raycast(ray, out hit)){
                        if(hit.collider.tag == "air"){
                            start_search = false;
                            distanceWarningPanel.SetActive(false);
                            tapOnObjectPanel.SetActive(false);
                            air.GetComponent<Animator>().SetTrigger("dance");
                            infoBubbleCanvas.SetActive(true);
                            Vector3 dir = arCamera.transform.position - air.transform.position;
                            // air.transform.rotation = Quaternion.LookRotation(new Vector3(dir.x,0f,dir.z), Vector3.up)*Quaternion.Euler(0,-16f,0);
                            air.transform.rotation = Quaternion.LookRotation(new Vector3(dir.x,0f,dir.z), Vector3.up);
                        }
                    }
                }
            }else{
                air.SetActive(false);
                tapOnObjectPanel.SetActive(false);
            }
        }
        if(waitForValveClick){
            ray = arCamera.ScreenPointToRay(Input.mousePosition);
            if(Input.GetMouseButtonDown(0)){
                // ray = arCamera.ScreenPointToRay(Input.mousePosition);
                //  Debug.DrawRay(ray.origin, ray.direction*100, Color.green, 10);
                if(Physics.Raycast(ray, out hit)){
                    if(hit.collider.name == "valve"){
                        waitForValveClick = false;
                        valveRotating = true;
                        valve.GetComponent<Animator>().Play("valve_animation", 0, 0f);
                    }
                }
            }
        }

        if(valveRotating && valve.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("valve_animation") && valve.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f){
            valveRotating = false;
            if(!foundLeak)
                warningPanel.SetActive(true);
            else pipeOpenedPanel.SetActive(true);
            clickToOpenCanvas.SetActive(false);
        }
        
        if(mazeInteractionOn){         
            if(!foundLeak){
                if(isPressed){
                    Vector3 forward = Vector3.Normalize(mazeCamera.transform.forward);

                    if(!collision){
                        airPosition += new Vector3(forward.x,0f,forward.z)*(0.05f);
                    }else{
                        Vector3 tangent = Vector3.Cross(Vector3.up, normal).normalized;
                        float dot = Vector3.Dot(tangent,forward);
                        if(dot < 0){
                            tangent = tangent*(-1f);
                            dot = Vector3.Dot(tangent,forward);
                        }
                        airPosition += tangent*(0.05f*dot);
                        airPosition += new Vector3(normal.x,0f,normal.z)*(0.05f);
                        if(isCollision(airPosition + new Vector3(forward.x,0f,forward.z)*(0.1f)) || 
                        isCollision(airPosition + (Vector3.Cross(Vector3.up, new Vector3(forward.x,0f,forward.z)).normalized)*(0.1f)) ||
                        isCollision(airPosition - (Vector3.Cross(Vector3.up, new Vector3(forward.x,0f,forward.z)).normalized)*(0.1f))){
                            airPosition -= tangent*(0.05f*dot);
                            airPosition -= new Vector3(normal.x,0f,normal.z)*(0.05f);
                        }else{
                            airPosition -= new Vector3(normal.x,0f,normal.z)*(0.05f);
                            airPosition += new Vector3(normal.x,0f,normal.z)*(0.001f);
                        }
                    }
                }
                

                if(collision){
                    airPosition += new Vector3(normal.x,0f,normal.z)*(0.001f);
                }

                // air.transform.position = new Vector3(airPosition.x, -0.105f, airPosition.z);
                air.transform.position = new Vector3(airPosition.x, -0.235f, airPosition.z);

                // airRotation = Quaternion.LookRotation(new Vector3(arCamera.transform.forward.x,0f,arCamera.transform.forward.z), Vector3.up)*Quaternion.Euler(0,-16f,0);
                airRotation = Quaternion.LookRotation(new Vector3(arCamera.transform.forward.x,0f,arCamera.transform.forward.z), Vector3.up);
                air.transform.rotation = airRotation;
            }else{
                //found the leak
                mazeInteractionOn = false;
                broken_pipe.transform.GetChild(0).gameObject.SetActive(false);
                foundBrokenPipePanel.SetActive(true);
                air.GetComponent<Animator>().SetBool("walk", false);
                mazePanel.SetActive(false);
                miniMapPanel.SetActive(false);
                forwardButton.gameObject.SetActive(false);
                RenderSettings.skybox = day;
                Vector3 dir= -mazeCamera.transform.forward;
                broken_pipe.transform.rotation = Quaternion.LookRotation(new Vector3(dir.x,0f,dir.z), Vector3.up)*Quaternion.Euler(-90,0,0);
            }

            mazeCamera.transform.rotation = Quaternion.LookRotation(arCamera.transform.forward);
            mazeCameraPosition = new Vector3(airPosition.x-mazeCamera.transform.forward.x*0.75f,airPosition.y+0.4f,airPosition.z-mazeCamera.transform.forward.z*0.75f);
            mazeCamera.transform.position = mazeCameraPosition;
            followCamera.transform.position = airPosition + Vector3.up;
        }
    }
    
    public bool isCollision(Vector3 position){
        Vector3 relativePos = position - maze.transform.position;
        Vector3 newPosition = (relativePos + (new Vector3(-cubeSide/2f,0f,cubeSide/2f)))/cubeSide;
        int j= (int)Mathf.Floor(-newPosition.x);
        int i= (int)Mathf.Floor(newPosition.z);

        if(mazePlan[i,j] == 1){
            return true;
        }else return false;
    }

    public void onPointerDown(){
        if(forwardButton.interactable)
            isPressed=true;

        // if(forwardButton_P.interactable)
        //     isPressed=true;
        air.GetComponent<Animator>().SetBool("walk", true);
    }

    public void onPointerUp(){
        if(forwardButton.interactable)
            isPressed=false;

        // if(forwardButton_P.interactable)
        //     isPressed=false;
        air.GetComponent<Animator>().SetBool("walk", false);
    }

    public void generateMaze(){
        int n=21,m=31;
        // int n=7, m=10;
        mazePlan = new int[,] {{1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
        {1, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
        {1, 0, 0, 1, 0, 0, 1, 1, 1, 1, 0, 0, 1, 0, 0, 1, 1, 1, 1, 1, 1, 1, 0, 0, 1, 0, 0, 1, 0, 0, 1 },
        {1, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 1 },
        {1, 0, 0, 1, 0, 0, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 1, 1, 1, 0, 0, 1, 0, 0, 1, 0, 0, 1, 1, 1, 1 },
        {1, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 1 },
        {1, 0, 0, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 1, 1, 1, 0, 0, 1, 0, 0, 1 },
        {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1 },
        {1, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 1, 0, 0, 1, 1, 1, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1 },
        {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1 },
        {1, 0, 0, 1, 1, 1, 1, 0, 0, 1, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 0, 0, 1 },
        {1, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1 },
        {1, 0, 0, 1, 0, 0, 1, 1, 1, 1, 1, 1, 1, 0, 0, 1, 0, 0, 1, 1, 1, 1, 1, 1, 1, 0, 0, 1, 0, 0, 1 },
        {1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1 },
        {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 1, 1, 1, 1, 0, 0, 1, 0, 0, 1 },
        {1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1 },
        {1, 0, 0, 1, 0, 0, 1, 1, 1, 1, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1 },
        {1, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1 },
        {1, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 1, 1, 1, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1 },
        {1, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0 },
        {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0 }};

        // mazePlan = new int[,] {{1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
        //     {1, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
        //     {1, 1, 1, 1, 0, 0, 1, 0, 0, 1 },
        //     {1, 0, 0, 0, 0, 0, 1, 0, 0, 1 },
        //     {1, 0, 0, 1, 1, 1, 1, 1, 1, 1 },
        //     {1, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
        //     {1, 1, 1, 1, 1, 1, 1, 1, 0, 0 }};
        
        // //generate maze in the environment
        cubeSide = 0.5f;
        if(!alreadyInstantiated){
            GameObject cube, floor;
            for (int i = 0; i < n; i++){
                for (int j = 0; j < m; j++){
                    floor = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    floor.transform.SetParent(maze.transform, false);
                    floor.transform.localScale = new Vector3(cubeSide, 0.01f, cubeSide);
                    floor.transform.localPosition = new Vector3(-j*cubeSide, -cubeSide/2, i*cubeSide);
                    floor.GetComponent<MeshRenderer>().material = floorMaterial;
                    floor.GetComponent<BoxCollider>().enabled = false;

                    if(mazePlan[i,j]==1){
                        cube = Instantiate(block);
                        cube.transform.SetParent(maze.transform, false);
                        cube.transform.localScale = Vector3.one*cubeSide;
                        cube.transform.localPosition = new Vector3(-j*cubeSide,0f,i*cubeSide);
                        cube.SetActive(true);
                    }
                }   
            }
        }   

        Vector3 mazePosition = new Vector3(arCamera.transform.position.x,0f,arCamera.transform.position.z);
        maze.transform.position = mazePosition;

        // airPosition = mazePosition + new Vector3(-1.5f*cubeSide, -0.105f, cubeSide);
        airPosition = mazePosition + new Vector3(-1.5f*cubeSide, -0.235f, cubeSide);
        air.transform.position = airPosition;
        air.transform.localScale = air.transform.localScale*0.4f;
        // airRotation = Quaternion.LookRotation(new Vector3(arCamera.transform.forward.x,0f,arCamera.transform.forward.z), Vector3.up)*Quaternion.Euler(0,-16f,0);
        airRotation = Quaternion.LookRotation(new Vector3(arCamera.transform.forward.x,0f,arCamera.transform.forward.z), Vector3.up);
        air.transform.rotation=airRotation;

        mazeCamera.transform.rotation = Quaternion.LookRotation(arCamera.transform.forward);
        mazeCameraPosition= new Vector3(airPosition.x-mazeCamera.transform.forward.x*0.75f,airPosition.y+0.4f,airPosition.z-mazeCamera.transform.forward.z*0.75f);
        mazeCamera.transform.position = mazeCameraPosition;

        // broken_pipe.transform.position = new Vector3(mazePosition.x-(m-1+0.5f)*cubeSide,0f,mazePosition.z+(n-2+0.5f)*cubeSide);
        broken_pipe.transform.position = airPosition + (new Vector3(0f,0f,2.5f));

        // var aRScript = aRSessionOrigin.GetComponent<ARSessionOrigin>();
        // aRScript.MakeContentAppearAt(maze.transform, Quaternion.LookRotation(new Vector3(arCamera.transform.forward.x, 0f, arCamera.transform.forward.z), Vector3.up));
    }

    public void generateMazeFile(){
        StreamReader input = new StreamReader("Assets/Scripts/mazePlanSmall.txt");
        StreamWriter output = new StreamWriter("Assets/Scripts/mazePlanSmallToRead.txt");

        // input.ReadLine();
        output.Write("{{");
        while (input.Peek() >= 0){
            string current = ((char)input.Read()).ToString();
           
            if(input.Peek()!=10){
                output.Write(current+", ");
                 
            }else {
                output.Write("},\n{");
                 input.Read();
            }
        }

        output.Write("}}");

        input.Close();
        output.Close();
    }
}
