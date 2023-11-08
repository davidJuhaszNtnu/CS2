using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Site4UI : MonoBehaviour
{
    public GameObject canvas, task2Panel, site4UI, site4;
    public Button confirmButton;
    public GameObject pipe_image, wwtp_image;
    Vector2 pipe_position, wwtp_position;
    public Camera arCamera;

    // Start is called before the first frame update
    void Start()
    {
        canvas.SetActive(false);
        task2Panel.SetActive(false);
        confirmButton.onClick.AddListener(confirm);

        float w,h;
        if(Screen.orientation==ScreenOrientation.Portrait){
            w=Screen.width;
            h=Screen.height;
        }else{
            h=Screen.width;
            w=Screen.height;
        }
        pipe_position= new Vector2(pipe_image.transform.GetComponent<RectTransform>().anchorMin.x*w,pipe_image.transform.GetComponent<RectTransform>().anchorMin.y*h);
        wwtp_position= new Vector2(wwtp_image.transform.GetComponent<RectTransform>().anchorMin.x*w,wwtp_image.transform.GetComponent<RectTransform>().anchorMin.y*h);
    }

    private void confirm(){
        task2Panel.SetActive(false);
        site4UI.SetActive(false);
        site4.GetComponent<Site4>().completedPanel2.SetActive(true);
        Vector3 dir=arCamera.transform.position-site4.GetComponent<Site4>().canvas.GetComponent<RectTransform>().position;
        site4.GetComponent<Site4>().canvas.GetComponent<RectTransform>().rotation=Quaternion.LookRotation(new Vector3(dir.x,0f,dir.z), Vector3.up)*Quaternion.Euler(0,180,0);
        Screen.orientation = ScreenOrientation.AutoRotation;
        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = true;
    }

    public void drag_pipe(){
        pipe_image.GetComponent<RectTransform>().position=Input.mousePosition;
        pipe_image.transform.SetAsLastSibling();
        if(Vector2.Distance(Input.mousePosition,wwtp_position)<50f){
            wwtp_image.GetComponent<Image>().color=new Color(0.7529412f,0.4862745f,0.4862745f,1f);
            wwtp_image.transform.position=pipe_position;
        }else{
            wwtp_image.GetComponent<Image>().color=new Color(1f,1f,1f,1f);
            wwtp_image.transform.position=wwtp_position;
        }
    }

    public void drag_wwtp(){
        wwtp_image.GetComponent<RectTransform>().position=Input.mousePosition;
        wwtp_image.transform.SetAsLastSibling();
        if(Vector2.Distance(Input.mousePosition,pipe_position)<50f){
            pipe_image.GetComponent<Image>().color=new Color(0.7529412f,0.4862745f,0.4862745f,1f);
            pipe_image.transform.position=wwtp_position;
        }else{
            pipe_image.GetComponent<Image>().color=new Color(1f,1f,1f,1f);
            pipe_image.transform.position=pipe_position;
        }
    }

    public void endDrag_pipe(){
        if(Vector2.Distance(Input.mousePosition,wwtp_position)<50f){
            pipe_image.GetComponent<RectTransform>().position=wwtp_position;
            wwtp_image.GetComponent<RectTransform>().position=pipe_position;
            Vector3 temp;
            temp=pipe_position;
            pipe_position=wwtp_position;
            wwtp_position=temp;
            wwtp_image.GetComponent<Image>().color=new Color(1f,1f,1f,1f);
        }else{
            pipe_image.GetComponent<RectTransform>().position=pipe_position;
        }
    }

    public void endDrag_wwtp(){
        if(Vector2.Distance(Input.mousePosition,pipe_position)<50f){
            wwtp_image.GetComponent<RectTransform>().position=pipe_position;
            pipe_image.GetComponent<RectTransform>().position=wwtp_position;
            Vector3 temp;
            temp=wwtp_position;
            wwtp_position=pipe_position;
            pipe_position=temp;
            pipe_image.GetComponent<Image>().color=new Color(1f,1f,1f,1f);
        }else{
            wwtp_image.GetComponent<RectTransform>().position=wwtp_position;
        }
    }
}
