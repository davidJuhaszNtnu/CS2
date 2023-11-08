using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class Calibration : MonoBehaviour
{
    [SerializeField]
    private Camera arCamera;
    private Vector2 touchPosition = default;
    private static List<ARRaycastHit> hits = new List<ARRaycastHit>();
    private ARRaycastManager arRaycastManager;

    public GameObject app, site1, site1UI, calibrationCanvas,congratsPanel, path1, site2, site2UI, site3, site3UI, path2, site4, site4UI, site5, site5UI, path3, path4, multichoiceCanvas2,pond;
    bool gotFirst,gotSecond, bool1, calibrationOn;
    public TextMeshProUGUI calibrationText;
    Vector3 firstPoint, secondPoint;

    private GameObject aRSessionOrigin;

    void Awake()
    {
        arRaycastManager = GetComponent<ARRaycastManager>();
        gotFirst=false;
        gotSecond=false;
        bool1=true;
        calibrationOn = true;

        firstPoint = new Vector3(0f,0f,0f);
        secondPoint = new Vector3(0f,0f,1f);
        gotFirst=true;
        gotSecond=true;

        aRSessionOrigin = GameObject.Find("AR Session Origin");
    }

    void Update()
    {
        // if(calibrationOn){
            if(Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                touchPosition = touch.position;

                if(touch.phase == TouchPhase.Began)
                {
                    Ray ray = arCamera.ScreenPointToRay(touchPosition);
                    RaycastHit hitObject;

                    if (Physics.Raycast(ray, out hitObject))
                    {
                        //points for the escape room
                            if(hitObject.collider.tag=="point1" && !gotSecond){
                                gotFirst=true;
                                firstPoint=hitObject.point;
                                calibrationText.text="Select the second point";
                            }
                            if(hitObject.collider.tag=="point2" && gotFirst){
                                gotSecond=true;
                                secondPoint=hitObject.point;
                                // mainCanvasUI.GetComponent<MainCanvasUI>().selectText.gameObject.SetActive(false);
                            }
                            // if(hitObject.collider.tag=="well"){
                            //     congratsPanel.SetActive(false);
                            //     site1.SetActive(false);
                            //     site1UI.SetActive(false);
                            //     site2.SetActive(true);
                            //     site2UI.SetActive(true);
                            //     // path1.GetComponent<Path>().arrived=true;
                            //     site2.GetComponent<Site2>().showSite=true;
                            //     // path1.SetActive(false);
                            //     site2.transform.position=new Vector3(hitObject.point.x, 0f, hitObject.point.z);
                            //     // mainCanvasUI.GetComponent<MainCanvasUI>().selectText.gameObject.SetActive(false);
                            // }
                            // if(hitObject.collider.tag=="site3"){
                            //     site2.SetActive(false);
                            //     site2UI.SetActive(false);
                            //     site3.SetActive(true);
                            //     site3UI.SetActive(true);
                            //     site3.GetComponent<Site3>().showSite=true;
                            //     site3.transform.position=new Vector3(hitObject.point.x, 0f, hitObject.point.z);
                            //     // path2.SetActive(false);
                            //     // path2.GetComponent<Path>().arrived=true;
                                
                            //     site3.transform.position=new Vector3(hitObject.point.x, 0f, hitObject.point.z);
                            //     // mainCanvasUI.GetComponent<MainCanvasUI>().selectText.gameObject.SetActive(false);
                            // }
                            // if(hitObject.collider.tag=="site4"){
                            //     multichoiceCanvas2.SetActive(false);
                            //     site3.SetActive(false);
                            //     site3UI.SetActive(false);
                            //     site4.SetActive(true);
                            //     site4UI.SetActive(true);
                            //     site4.GetComponent<Site4>().showSite=true;
                                
                            //     site4.transform.position=new Vector3(hitObject.point.x, 0f, hitObject.point.z);
                            //     // mainCanvasUI.GetComponent<MainCanvasUI>().selectText.gameObject.SetActive(false);
                            // }
                            // if(hitObject.collider.tag=="site5"){
                            //     site4.SetActive(false);
                            //     site4UI.SetActive(false);
                            //     site5.SetActive(true);
                            //     site5UI.SetActive(true);
                            //     // pond.SetActive(true);
                            //     // pond.transform.position=site5.transform.position+(new Vector3(0f,-1f,0.5f));
                            //     site5.GetComponent<Site5>().showSite=true;
                            //     // path4.GetComponent<Path>().arrived=true;
                            //     // path4.SetActive(false);
                                
                            //     site5.transform.position=new Vector3(hitObject.point.x, 0f, hitObject.point.z);
                            //     // mainCanvasUI.GetComponent<MainCanvasUI>().selectText.gameObject.SetActive(false);
                            // }
                    }
                } 
            }

            if(gotFirst && gotSecond && bool1){
                // gameController.GetComponent<Main>().gotPositionDirection=true;
                calibrationOn = false;
                bool1=false;
                var aRScript = aRSessionOrigin.GetComponent<ARSessionOrigin>();
                Vector3 direction=secondPoint-firstPoint;
                // aRScript.MakeContentAppearAt(app.transform, new Vector3(firstPoint.x,0f,firstPoint.z),Quaternion.LookRotation(new Vector3(direction.x,0f,direction.z), Vector3.up));
                // aRScript.MakeContentAppearAt(app.transform, new Vector3(secondPoint.x,0f,secondPoint.z),Quaternion.LookRotation(new Vector3(direction.x,0f,direction.z), Vector3.up));

                site3.SetActive(true);
                site3UI.SetActive(true);
                path2.GetComponent<Path>().arrived=true;
                calibrationCanvas.SetActive(false);
            }  
        // }
    }
}
