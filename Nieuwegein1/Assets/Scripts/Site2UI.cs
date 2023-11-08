using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Site2UI : MonoBehaviour
{
    public GameObject pipe_image,membrane_image,tank_image;
    public Vector2 pipe_position,membrane_position,tank_position;
    public GameObject findPanel,taskPanel, completedPanel, path2,site2,site2UI;
    public Button confirmButton;
    public TextMeshProUGUI findText, collectedText;


    void Start()
    {
        findPanel.SetActive(false);
        taskPanel.SetActive(false);
        completedPanel.SetActive(false);
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
        membrane_position= new Vector2(membrane_image.transform.GetComponent<RectTransform>().anchorMin.x*w,membrane_image.transform.GetComponent<RectTransform>().anchorMin.y*h);
        tank_position= new Vector2(tank_image.transform.GetComponent<RectTransform>().anchorMin.x*w,tank_image.transform.GetComponent<RectTransform>().anchorMin.y*h);
    }

    public void setImages(int pipe, int membrane, int tank){
        switch(pipe){
            case 1:
                pipe_image.transform.position=pipe_position;
                break;
            case 2:
                pipe_image.transform.position=membrane_position;
                break;
            case 3:
                pipe_image.transform.position=tank_position;
                break;
        }
        switch(membrane){
            case 1:
                membrane_image.transform.position=pipe_position;
                break;
            case 2:
                membrane_image.transform.position=membrane_position;
                break;
            case 3:
                membrane_image.transform.position=tank_position;
                break;
        }
        switch(tank){
            case 1:
                tank_image.transform.position=pipe_position;
                break;
            case 2:
                tank_image.transform.position=membrane_position;
                break;
            case 3:
                tank_image.transform.position=tank_position;
                break;
        }
        pipe_position=pipe_image.transform.position;
        membrane_position=membrane_image.transform.position;
        tank_position=tank_image.transform.position;
    }

    private void confirm(){
        taskPanel.SetActive(false);
        completedPanel.SetActive(true);
        Screen.orientation = ScreenOrientation.AutoRotation;
        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = true;
        site2.GetComponent<Site2>().bool4=true;
        site2.GetComponent<Site2>().timeCongratsPanel = Time.time;
    }

    public void drag_pipe(){
        pipe_image.transform.position=Input.mousePosition;
        pipe_image.transform.SetAsLastSibling();
        if(Vector2.Distance(Input.mousePosition,membrane_position)<50f){
            membrane_image.GetComponent<Image>().color=new Color(0.7529412f,0.4862745f,0.4862745f,1f);
            membrane_image.transform.position=pipe_position;
        }else{
            membrane_image.GetComponent<Image>().color=new Color(1f,1f,1f,1f);
            membrane_image.transform.position=membrane_position;
        }

        if(Vector2.Distance(Input.mousePosition,tank_position)<50f){
            tank_image.GetComponent<Image>().color=new Color(0.7529412f,0.4862745f,0.4862745f,1f);
            tank_image.transform.position=pipe_position;
        }else{
            tank_image.GetComponent<Image>().color=new Color(1f,1f,1f,1f);
            tank_image.transform.position=tank_position;
        }
    }

    public void endDrag_pipe(){
        if(Vector2.Distance(Input.mousePosition,membrane_position)<50f){
            pipe_image.transform.position=membrane_position;
            membrane_image.transform.position=pipe_position;
            Vector3 temp;
            temp=pipe_position;
            pipe_position=membrane_position;
            membrane_position=temp;
            membrane_image.GetComponent<Image>().color=new Color(1f,1f,1f,1f);
        }else if(Vector2.Distance(Input.mousePosition,tank_position)<50f){
            pipe_image.transform.position=tank_position;
            tank_image.transform.position=pipe_position;
            Vector3 temp;
            temp=pipe_position;
            pipe_position=tank_position;
            tank_position=temp;
            tank_image.GetComponent<Image>().color=new Color(1f,1f,1f,1f);
        }else{
            pipe_image.transform.position=pipe_position;
        }
    }

    public void drag_membrane(){
        membrane_image.transform.position=Input.mousePosition;
        membrane_image.transform.SetAsLastSibling();
        if(Vector2.Distance(Input.mousePosition,pipe_position)<50f){
            pipe_image.GetComponent<Image>().color=new Color(0.7529412f,0.4862745f,0.4862745f,1f);
            pipe_image.transform.position=membrane_position;
        }else{
            pipe_image.GetComponent<Image>().color=new Color(1f,1f,1f,1f);
            pipe_image.transform.position=pipe_position;
        }

        if(Vector2.Distance(Input.mousePosition,tank_position)<50f){
            tank_image.GetComponent<Image>().color=new Color(0.7529412f,0.4862745f,0.4862745f,1f);
            tank_image.transform.position=membrane_position;
        }else{
            tank_image.GetComponent<Image>().color=new Color(1f,1f,1f,1f);
            tank_image.transform.position=tank_position;
        }
    }

    public void endDrag_membrane(){
        if(Vector2.Distance(Input.mousePosition,pipe_position)<50f){
            membrane_image.transform.position=pipe_position;
            pipe_image.transform.position=membrane_position;
            Vector3 temp;
            temp=membrane_position;
            membrane_position=pipe_position;
            pipe_position=temp;
            pipe_image.GetComponent<Image>().color=new Color(1f,1f,1f,1f);
        }else if(Vector2.Distance(Input.mousePosition,tank_position)<50f){
            membrane_image.transform.position=tank_position;
            tank_image.transform.position=membrane_position;
            Vector3 temp;
            temp=membrane_position;
            membrane_position=tank_position;
            tank_position=temp;
            tank_image.GetComponent<Image>().color=new Color(1f,1f,1f,1f);
        }else{
            membrane_image.transform.position=membrane_position;
        }
    }

    public void drag_tank(){
        tank_image.transform.position=Input.mousePosition;
        tank_image.transform.SetAsLastSibling();
        if(Vector2.Distance(Input.mousePosition,membrane_position)<50f){
            membrane_image.GetComponent<Image>().color=new Color(0.7529412f,0.4862745f,0.4862745f,1f);
            membrane_image.transform.position=tank_position;
        }else{
            membrane_image.GetComponent<Image>().color=new Color(1f,1f,1f,1f);
            membrane_image.transform.position=membrane_position;
        }

        if(Vector2.Distance(Input.mousePosition,pipe_position)<50f){
            pipe_image.GetComponent<Image>().color=new Color(0.7529412f,0.4862745f,0.4862745f,1f);
            pipe_image.transform.position=tank_position;
        }else{
            pipe_image.GetComponent<Image>().color=new Color(1f,1f,1f,1f);
            pipe_image.transform.position=pipe_position;
        }
    }

    public void endDrag_tank(){
        if(Vector2.Distance(Input.mousePosition,membrane_position)<50f){
            tank_image.transform.position=membrane_position;
            membrane_image.transform.position=tank_position;
            Vector3 temp;
            temp=tank_position;
            tank_position=membrane_position;
            membrane_position=temp;
            membrane_image.GetComponent<Image>().color=new Color(1f,1f,1f,1f);
        }else if(Vector2.Distance(Input.mousePosition,pipe_position)<50f){
            tank_image.transform.position=pipe_position;
            pipe_image.transform.position=tank_position;
            Vector3 temp;
            temp=tank_position;
            tank_position=pipe_position;
            pipe_position=temp;
            pipe_image.GetComponent<Image>().color=new Color(1f,1f,1f,1f);
        }else{
            tank_image.transform.position=tank_position;
        }
    }
}
