using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.IO;
using UnityEngine.UI;
using Random=UnityEngine.Random;

public class Site3 : MonoBehaviour
{
    public GameObject path2, air, site3,site3UI, infoBubble, taskPanel1, pipe, valve, broken_pipe, canvas, welcomeCanvas, site4, site4UI, 
    findPanel, valveCanvas, warningCanvas, leakCanvas, congratsCanvas, multichoiceCanvas1, multichoice1_taskPanel, multichoice1_infoPanel, multichoiceCanvas2,
    multichoice2_taskPanel, multichoice2_infoPanel, path3, mazeInstructionImage;
    public Button OkInfoBubble, OkWelcome, OkWarning, OkLeak, OkCongrats, OkMultichoice1Task, OkMultichoice1Info, OkMultichoice2Task, CloseMazeInstruction, forwardButton;
    public TextMeshProUGUI findText, congratsText;
    public bool bool1, bool4, bool6, bool7;
    bool bool2, bool3, bool5, leakCanvasRotate;
    public float timeFindText, timeMultichoice2Info;
    float timeCongratsText, timeValveRotation;
    public Camera arCamera;
    public bool showSite;
    
    //maze
    int[,] mazePlan;
    public GameObject maze, mazeUI, block;
    public Material brickWallMaterial;
    Vector3 mazeCameraPosition;
    public Vector3 normal,airPosition;
    Quaternion airRotation;
    public bool isPressed, collision, foundLeak;
    public Camera mazeCamera;
    float cubeSide;

    //multiple choice Canvas 1
    

    Ray ray;
    RaycastHit hit;

    void Start()
    {
        bool1=true;
        bool2=false;
        bool3=false;
        bool4=false;
        bool5=false;
        bool6=false;
        showSite=false;
        leakCanvasRotate=false;
        air.SetActive(false);
        infoBubble.SetActive(false);
        OkInfoBubble.onClick.AddListener(ok_info);
        OkWelcome.onClick.AddListener(ok_welcome);
        OkWarning.onClick.AddListener(ok_warning);
        OkLeak.onClick.AddListener(ok_leak);
        OkCongrats.onClick.AddListener(ok_congrats);
        OkMultichoice1Task.onClick.AddListener(ok_multichoice1Task);
        OkMultichoice1Info.onClick.AddListener(ok_multichoice1Info);
        OkMultichoice2Task.onClick.AddListener(ok_multichoice2Task);
        CloseMazeInstruction.onClick.AddListener(closeMazeInstruction);
        pipe.SetActive(false);
        welcomeCanvas.SetActive(false);
        warningCanvas.SetActive(false);
        valveCanvas.SetActive(false);
        leakCanvas.SetActive(false);
        congratsCanvas.SetActive(false);
        multichoiceCanvas1.SetActive(false);
        multichoiceCanvas2.SetActive(false);
        maze.SetActive(false);
        mazeUI.SetActive(false);
        block.SetActive(false);
        isPressed=false;
        collision=false;
        mazeCamera.enabled=false;
        foundLeak = false;
        broken_pipe.SetActive(false);

        // maze.SetActive(true);
        // mazeUI.SetActive(true);
        // mazeInstructionImage.SetActive(true);
        // air.SetActive(true);
        // forwardButton.interactable=false;
        // broken_pipe.SetActive(true);
        // generateMaze();
        // bool6=true;
        // arCamera.enabled=false;
        // mazeCamera.enabled=true;
        // site3UI.SetActive(false);
        // foundLeak=true;
        // generateMazeFile();

        // pipe.SetActive(true);
        // pipe.transform.position=site3.transform.position+(new Vector3(0f,-1f,0f));
        // Vector3 dir=arCamera.transform.position-pipe.transform.position;
        // pipe.transform.rotation=Quaternion.LookRotation(new Vector3(dir.x,0f,dir.z), Vector3.up)*Quaternion.Euler(0,-80,0);
        // congratsCanvas.SetActive(true);

        // multichoiceCanvas2.GetComponent<RectTransform>().position = site3.transform.position;
        // Vector3 dir=arCamera.transform.position-site3.transform.position;
        // multichoiceCanvas2.GetComponent<RectTransform>().rotation=Quaternion.LookRotation(new Vector3(dir.x,0f,dir.z), Vector3.up)*Quaternion.Euler(0,180,0);
        // multichoiceCanvas2.SetActive(true);
        // multichoice2_taskPanel.SetActive(true);
        // multichoice2_infoPanel.SetActive(false);
    }

    private void closeMazeInstruction(){
        mazeInstructionImage.SetActive(false);
        forwardButton.interactable=true;
    }

    private void ok_multichoice1Task(){
        multichoice1_taskPanel.SetActive(false);
        multichoice1_infoPanel.SetActive(true);
    }

    private void ok_multichoice2Task(){
        multichoice2_taskPanel.SetActive(false);
        multichoice2_infoPanel.SetActive(true);
        timeMultichoice2Info=Time.time;
        bool7=true;
    }

    private void ok_multichoice1Info(){
        multichoiceCanvas1.SetActive(false);
        multichoiceCanvas2.SetActive(true);
        multichoice2_taskPanel.SetActive(true);
        multichoice2_infoPanel.SetActive(false);
        multichoiceCanvas2.GetComponent<RectTransform>().position = site3.transform.position+(new Vector3(0f,0f,0.5f));
        Vector3 dir=arCamera.transform.position-site3.transform.position;
        multichoiceCanvas2.GetComponent<RectTransform>().rotation=Quaternion.LookRotation(new Vector3(dir.x,0f,dir.z), Vector3.up)*Quaternion.Euler(0,180,0);
    }

    private void ok_congrats(){
        congratsCanvas.SetActive(false);
        pipe.SetActive(false);
        multichoiceCanvas1.SetActive(true);
        multichoice1_taskPanel.SetActive(true);
        multichoice1_infoPanel.SetActive(false);
        multichoiceCanvas1.GetComponent<RectTransform>().position = site3.transform.position+(new Vector3(0f,0f,0.5f));
        Vector3 dir=arCamera.transform.position-site3.transform.position;
        multichoiceCanvas1.GetComponent<RectTransform>().rotation=Quaternion.LookRotation(new Vector3(dir.x,0f,dir.z), Vector3.up)*Quaternion.Euler(0,180,0);
    }

    private void ok_leak(){
        bool6=false;
        maze.SetActive(false);
        mazeUI.SetActive(false);
        air.SetActive(false);
        broken_pipe.SetActive(false);
        leakCanvas.SetActive(false);
        // site3UI.SetActive(true);
        pipe.SetActive(true);
        valveCanvas.SetActive(true);
        Vector3 dir=arCamera.transform.position-pipe.transform.position;
        pipe.transform.rotation=Quaternion.LookRotation(new Vector3(dir.x,0f,dir.z), Vector3.up)*Quaternion.Euler(0,-80,0);
        bool4=true;

        mazeCamera.enabled=false;
        arCamera.enabled=true;
    }

    private void ok_info(){
        site3UI.SetActive(true);
        // prevent rotating the screen
        Screen.orientation = ScreenOrientation.Portrait;
        Screen.autorotateToLandscapeLeft = false;
        Screen.autorotateToLandscapeRight = false;
        taskPanel1.SetActive(true);
        infoBubble.SetActive(false);
    }
    private void ok_welcome(){
        welcomeCanvas.SetActive(false);
        findPanel.SetActive(true);
        timeFindText=Time.time;
        bool2=true;
        findText.gameObject.SetActive(true);
        site3UI.SetActive(true);
    }
    private void ok_warning(){
        warningCanvas.SetActive(false);
        pipe.SetActive(false);
        maze.SetActive(true);
        mazeUI.SetActive(true);
        mazeInstructionImage.SetActive(true);
        forwardButton.interactable=false;
        air.SetActive(true);
        generateMaze();
        arCamera.enabled=false;
        mazeCamera.enabled=true;
        broken_pipe.SetActive(true);
        site3UI.SetActive(false);
        bool6=true;
    }

    // Update is called once per frame
    void Update()
    {
        if(bool1){
        // if(path2.GetComponent<Path>().arrived && bool1){
            Debug.Log("site3 on");
            bool1=false;
            welcomeCanvas.SetActive(true);
            path2.GetComponent<Path>().Arrow.SetActive(false);
            // welcomeCanvas.transform.rotation = Quaternion.LookRotation(new Vector3(arCamera.transform.position.x,0f,arCamera.transform.position.z))*Quaternion.Euler(0,0,0);

            float angle;
            float dist;
            angle=Random.Range(0, 2*(float)Math.PI);
            dist=Random.Range(0.5f, 1f);
            pipe.SetActive(true);
            pipe.transform.position=site3.transform.position+(new Vector3(0f,-1f,1.5f));
            Vector3 dir=arCamera.transform.position-pipe.transform.position;
            pipe.transform.rotation=Quaternion.LookRotation(new Vector3(dir.x,0f,dir.z), Vector3.up)*Quaternion.Euler(0,-80,0);
            air.transform.position=pipe.transform.position+(new Vector3(dist*(float)Math.Cos(angle),0.5f,dist*(float)Math.Sin(angle)));
            air.SetActive(false);
        }
        if(bool2 && Time.time>timeFindText+3f){
            findText.gameObject.SetActive(false);

            float d=Vector2.Distance(new Vector2(arCamera.transform.position.x,arCamera.transform.position.z),new Vector2(air.transform.position.x,air.transform.position.z));
            if(d<10.7f){
                air.SetActive(true);
                air.transform.Rotate(0f,0f,1f, Space.Self);
                ray = arCamera.ScreenPointToRay(Input.mousePosition);
                if(Input.GetMouseButtonDown(0)){
                    if(Physics.Raycast(ray, out hit)){
                        if(hit.collider.name == "AIR"){
                            bool2=false;
                            bool3=true;
                            timeCongratsText=Time.time;
                            congratsText.gameObject.SetActive(true);
                            Vector3 dir=arCamera.transform.position-air.transform.position;
                            air.transform.rotation=Quaternion.LookRotation(new Vector3(dir.x,0f,dir.z), Vector3.up)*Quaternion.Euler(-90,-15,0);
                            infoBubble.transform.rotation=Quaternion.LookRotation(new Vector3(dir.x,0f,dir.z), Vector3.up);
                        }
                    }
                }
            }else air.SetActive(false);
        }
        if(Time.time>timeCongratsText+3f && bool3){
            congratsText.gameObject.SetActive(false);
            infoBubble.SetActive(true);
            bool3=false;
            site3UI.SetActive(false);
        }
        if(bool4){
            ray = arCamera.ScreenPointToRay(Input.mousePosition);
            if(Input.GetMouseButtonDown(0)){
                if(Physics.Raycast(ray, out hit)){
                    if(hit.collider.name == "valve"){
                        bool4=false;
                        bool5=true;
                        timeValveRotation=Time.time;
                        valveCanvas.SetActive(false);
                    }
                }
            }
        }
        if(bool5 && Time.time<timeValveRotation+3f){
            valve.transform.Rotate(0f,0f,2f);
        }
        if(bool5 && Time.time>timeValveRotation+3f){
        // if(bool5){
            bool5=false;
            // valve.SetActive(true);
            // pipe.SetActive(true);
            if(!foundLeak){
                warningCanvas.SetActive(true);
                Vector3 dir=arCamera.transform.position-pipe.transform.position;
                pipe.transform.rotation=Quaternion.LookRotation(new Vector3(dir.x,0f,dir.z), Vector3.up)*Quaternion.Euler(0,-80,0);
                site3UI.SetActive(false);
            }else{
                congratsCanvas.SetActive(true);
                Vector3 dir=arCamera.transform.position-pipe.transform.position;
                pipe.transform.rotation=Quaternion.LookRotation(new Vector3(dir.x,0f,dir.z), Vector3.up)*Quaternion.Euler(0,-80,0);
            }
        }
        if(bool6){
            if(!foundLeak){
                if(isPressed){
                    Vector3 forward = Vector3.Normalize(mazeCamera.transform.forward);

                    if(!collision){
                        airPosition+=new Vector3(forward.x,0f,forward.z)*(0.05f);
                    }else{
                        Vector3 tangent = Vector3.Cross(Vector3.up, normal).normalized;
                        float dot = Vector3.Dot(tangent,forward);
                        if(dot<0){
                            tangent = tangent*(-1f);
                            dot = Vector3.Dot(tangent,forward);
                        }
                        airPosition += tangent*(0.05f*dot);
                        airPosition+=new Vector3(normal.x,0f,normal.z)*(0.05f);
                        if(isCollision(airPosition + new Vector3(forward.x,0f,forward.z)*(0.2f)) || 
                        isCollision(airPosition + (Vector3.Cross(Vector3.up, new Vector3(forward.x,0f,forward.z)).normalized)*(0.2f)) ||
                        isCollision(airPosition - (Vector3.Cross(Vector3.up, new Vector3(forward.x,0f,forward.z)).normalized)*(0.2f))){
                            airPosition -= tangent*(0.05f*dot);
                            airPosition-=new Vector3(normal.x,0f,normal.z)*(0.05f);
                        }else{
                            airPosition-=new Vector3(normal.x,0f,normal.z)*(0.05f);
                            airPosition+=new Vector3(normal.x,0f,normal.z)*(0.001f);
                        }
                    }
                }
                

                if(collision){
                    airPosition+=new Vector3(normal.x,0f,normal.z)*(0.001f);
                }

                air.transform.position = airPosition;

                airRotation = Quaternion.LookRotation(new Vector3(arCamera.transform.forward.x,0f,arCamera.transform.forward.z), Vector3.up)*Quaternion.Euler(-90,-15,0);
                air.transform.rotation=airRotation;
            }else{
                //found the leak
                leakCanvas.SetActive(true);
                mazeUI.SetActive(false);
                if(!leakCanvasRotate){
                    Vector3 dir=mazeCamera.transform.position-broken_pipe.transform.position;
                    broken_pipe.transform.rotation=Quaternion.LookRotation(new Vector3(dir.x,0f,dir.z), Vector3.up)*Quaternion.Euler(-90,0,0);
                    leakCanvasRotate=true;
                }
            }

            mazeCamera.transform.rotation = Quaternion.LookRotation(arCamera.transform.forward);
            mazeCameraPosition= new Vector3(airPosition.x-mazeCamera.transform.forward.x*0.75f,airPosition.y+0.2f,airPosition.z-mazeCamera.transform.forward.z*0.75f);
            mazeCamera.transform.position = mazeCameraPosition;

            
        }
        if(bool7){
            if(Time.time>timeMultichoice2Info+3f){
                bool7=false;
                multichoiceCanvas2.SetActive(false);
                site3.SetActive(false);
                site3UI.SetActive(false);
                // site4.SetActive(true);
                // site4UI.SetActive(true);
                path3.SetActive(true);
                path3.GetComponent<Path>().arrived=false;
                site4.GetComponent<Site4>().showSite=true;
                
                path3.GetComponent<Path>().t=0f;
                path3.GetComponent<Path>().Arrow.SetActive(true);
                path3.GetComponent<Path>().Arrow.transform.position=path3.GetComponent<Path>().path(0f);
                path3.GetComponent<Path>().Arrow.transform.rotation=Quaternion.LookRotation(path3.GetComponent<Path>().pathDer(0f), Vector3.up);
            }
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
    }

    public void onPointerUp(){
        if(forwardButton.interactable)
            isPressed=false;
    }

    public void generateMaze(){
        // int n=21,m=31;
        int n=7, m=10;
        // mazePlan = new int[,] {{1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
        // {1, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
        // {1, 0, 0, 1, 0, 0, 1, 1, 1, 1, 0, 0, 1, 0, 0, 1, 1, 1, 1, 1, 1, 1, 0, 0, 1, 0, 0, 1, 0, 0, 1 },
        // {1, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 1 },
        // {1, 0, 0, 1, 0, 0, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 1, 1, 1, 0, 0, 1, 0, 0, 1, 0, 0, 1, 1, 1, 1 },
        // {1, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 1 },
        // {1, 0, 0, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 1, 1, 1, 0, 0, 1, 0, 0, 1 },
        // {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1 },
        // {1, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 1, 0, 0, 1, 1, 1, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1 },
        // {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1 },
        // {1, 0, 0, 1, 1, 1, 1, 0, 0, 1, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 0, 0, 1 },
        // {1, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1 },
        // {1, 0, 0, 1, 0, 0, 1, 1, 1, 1, 1, 1, 1, 0, 0, 1, 0, 0, 1, 1, 1, 1, 1, 1, 1, 0, 0, 1, 0, 0, 1 },
        // {1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1 },
        // {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 1, 1, 1, 1, 0, 0, 1, 0, 0, 1 },
        // {1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1 },
        // {1, 0, 0, 1, 0, 0, 1, 1, 1, 1, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1 },
        // {1, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1 },
        // {1, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 1, 1, 1, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1 },
        // {1, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0 },
        // {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0 }};

        mazePlan = new int[,] {{1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
            {1, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
            {1, 1, 1, 1, 0, 0, 1, 0, 0, 1 },
            {1, 0, 0, 0, 0, 0, 1, 0, 0, 1 },
            {1, 0, 0, 1, 1, 1, 1, 1, 1, 1 },
            {1, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
            {1, 1, 1, 1, 1, 1, 1, 1, 0, 0 }};
        
        // //generate maze in the environment
        cubeSide=0.5f;
        GameObject cube;
        for (int i = 0; i < n; i++){
            for (int j = 0; j < m; j++){
                if(mazePlan[i,j]==1){
                    cube = GameObject.Instantiate(block);
                    cube.transform.SetParent(maze.transform, false);
                    cube.GetComponent<MeshRenderer> ().material = brickWallMaterial;
                    cube.transform.localScale = Vector3.one*cubeSide;
                    cube.transform.localPosition = new Vector3(-j*cubeSide,0f,i*cubeSide);
                    cube.SetActive(true);
                }
            }
            
        }    

        Vector3 mazePosition = new Vector3(arCamera.transform.position.x,0f,arCamera.transform.position.z);
        maze.transform.position = mazePosition;

        airPosition = mazePosition + new Vector3(-cubeSide,0f,cubeSide);
        air.transform.position = airPosition;
        air.transform.localScale = air.transform.localScale*0.75f;
        airRotation = Quaternion.LookRotation(new Vector3(arCamera.transform.forward.x,0f,arCamera.transform.forward.z), Vector3.up)*Quaternion.Euler(-90,-15,0);
        air.transform.rotation=airRotation;

        mazeCamera.transform.rotation = Quaternion.LookRotation(arCamera.transform.forward);
        mazeCameraPosition= new Vector3(airPosition.x-mazeCamera.transform.forward.x*0.75f,airPosition.y+0.2f,airPosition.z-mazeCamera.transform.forward.z*0.75f);
        mazeCamera.transform.position = mazeCameraPosition;

        broken_pipe.transform.position = new Vector3(mazePosition.x-(m+0.5f)*cubeSide,0f,mazePosition.z+(n+0.5f)*cubeSide);
        // broken_pipe.transform.position = airPosition+(new Vector3(0f,0f,2f));
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
