using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Site3UI : MonoBehaviour
{
    public Button confirmButton, OkCompleted;
    public GameObject findPanel, site3, infoBubble, taskPanel1, completedPanel, valveCanvas;
    public GameObject pipe_image,tap_image, pipe;
    public TextMeshProUGUI findText, congratsText;
    Vector2 pipe_position,tap_position;
    public Camera arCamera;

    void Start()
    {
        OkCompleted.onClick.AddListener(ok_completed);
        confirmButton.onClick.AddListener(confirm);
        findPanel.SetActive(false);
        taskPanel1.SetActive(false);
        completedPanel.SetActive(false);
        findText.gameObject.SetActive(false);
        congratsText.gameObject.SetActive(false);

        float w,h;
        if(Screen.orientation==ScreenOrientation.Portrait){
            w=Screen.width;
            h=Screen.height;
        }else{
            h=Screen.width;
            w=Screen.height;
        }
        pipe_position= new Vector2(pipe_image.transform.GetComponent<RectTransform>().anchorMin.x*w,pipe_image.transform.GetComponent<RectTransform>().anchorMin.y*h);
        tap_position= new Vector2(tap_image.transform.GetComponent<RectTransform>().anchorMin.x*w,tap_image.transform.GetComponent<RectTransform>().anchorMin.y*h);
    }
    private void ok_completed(){
        completedPanel.SetActive(false);
        site3.GetComponent<Site3>().bool4=true;
        valveCanvas.SetActive(true);
        Vector3 dir=arCamera.transform.position-site3.GetComponent<Site3>().pipe.transform.position;
        site3.GetComponent<Site3>().pipe.transform.rotation=Quaternion.LookRotation(new Vector3(dir.x,0f,dir.z), Vector3.up)*Quaternion.Euler(0,-80,0);
    }
    private void confirm(){
        Screen.orientation = ScreenOrientation.AutoRotation;
        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = true;
        completedPanel.SetActive(true);
        taskPanel1.SetActive(false);
    }
    public void drag_pipe(){
        pipe_image.GetComponent<RectTransform>().position=Input.mousePosition;
        pipe_image.transform.SetAsLastSibling();
        if(Vector2.Distance(Input.mousePosition,tap_position)<50f){
            tap_image.GetComponent<Image>().color=new Color(0.7529412f,0.4862745f,0.4862745f,1f);
            tap_image.transform.position=pipe_position;
        }else{
            tap_image.GetComponent<Image>().color=new Color(1f,1f,1f,1f);
            tap_image.transform.position=tap_position;
        }
    }
    public void endDrag_pipe(){
        if(Vector2.Distance(Input.mousePosition,tap_position)<50f){
            pipe_image.GetComponent<RectTransform>().position=tap_position;
            tap_image.GetComponent<RectTransform>().position=pipe_position;
            Vector3 temp;
            temp=pipe_position;
            pipe_position=tap_position;
            tap_position=temp;
            tap_image.GetComponent<Image>().color=new Color(1f,1f,1f,1f);
        }else{
            pipe_image.GetComponent<RectTransform>().position=pipe_position;
        }
    }

    public void drag_tap(){
        tap_image.GetComponent<RectTransform>().position=Input.mousePosition;
        tap_image.transform.SetAsLastSibling();
        if(Vector2.Distance(Input.mousePosition,pipe_position)<50f){
            pipe_image.GetComponent<Image>().color=new Color(0.7529412f,0.4862745f,0.4862745f,1f);
            pipe_image.transform.position=tap_position;
        }else{
            pipe_image.GetComponent<Image>().color=new Color(1f,1f,1f,1f);
            pipe_image.transform.position=pipe_position;
        }
    }
    public void endDrag_tap(){
        if(Vector2.Distance(Input.mousePosition,pipe_position)<50f){
            tap_image.GetComponent<RectTransform>().position=pipe_position;
            pipe_image.GetComponent<RectTransform>().position=tap_position;
            Vector3 temp;
            temp=tap_position;
            tap_position=pipe_position;
            pipe_position=temp;
            pipe_image.GetComponent<Image>().color=new Color(1f,1f,1f,1f);
        }else{
            tap_image.GetComponent<RectTransform>().position=tap_position;
        }
    }
}
