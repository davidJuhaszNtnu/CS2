using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Site4UI : MonoBehaviour
{
    public GameObject disposal_pipe_image_L, wwtp_image_L, sewer_pipe_image_L, lake_image_L, disposal_pipe_image_P, wwtp_image_P, sewer_pipe_image_P, lake_image_P;
    public GameObject disposal_pipe_image, wwtp_image, sewer_pipe_image, lake_image, site4UI, site4, taskPanel, taskPanel_L, taskPanel_P, afterTaskPanel, taskCompletedPanel;
    public GameObject anchors_portrait, anchors_landscape;
    public Camera arCamera;

    int[] order;
    int[] correct_order;
    string orientation;
    GameObject parent;
    float swap_distance;

    bool animate;
    public GameObject tank_L, tank_P, droplet_L, droplet_P, gameController, incorrectOrderPanel, tank_afterTaskPanel_L, tank_afterTaskPanel_P;
    float minLevel_P, minLevel_L, t, dt, oldLevel_P, newLevel_P, currentLevel_P, oldLevel_L, newLevel_L, currentLevel_L, minLevel_afterTaskPanel_L, minLevel_afterTaskPanel_P;
    public TextMeshProUGUI scoreText_L, scoreText_P, scoreText_afterTaskPanel_L, scoreText_afterTaskPanel_P;
    int attempts;

    // Start is called before the first frame update
    void Start()
    {
        order = new int[4];
        correct_order = new int[4];
        swap_distance = 100f;

        animate = false;
        //ordering
        float aspectRatio = (float)Screen.width/(float)Screen.height;
        if(Screen.orientation == ScreenOrientation.Portrait){
            minLevel_P = tank_P.GetComponent<RectTransform>().rect.height;
            minLevel_L = tank_L.GetComponent<RectTransform>().rect.height * aspectRatio;

            minLevel_afterTaskPanel_P = tank_afterTaskPanel_P.GetComponent<RectTransform>().rect.height;
            minLevel_afterTaskPanel_L = tank_afterTaskPanel_L.GetComponent<RectTransform>().rect.height * aspectRatio;
        }else{
            minLevel_L = tank_L.GetComponent<RectTransform>().rect.height;
            minLevel_P = tank_P.GetComponent<RectTransform>().rect.height * aspectRatio;

            minLevel_afterTaskPanel_L = tank_afterTaskPanel_L.GetComponent<RectTransform>().rect.height;
            minLevel_afterTaskPanel_P = tank_afterTaskPanel_P.GetComponent<RectTransform>().rect.height * aspectRatio;
        }
        droplet_L.SetActive(false);
        droplet_P.SetActive(false);
        attempts = 0;
    }

    public void restart(){
        animate = false;
        attempts = 0;
        droplet_L.SetActive(false);
        droplet_P.SetActive(false);
    }

    void Update(){
        if(animate)
            animateWaterLevel();
    }

    public void setImages(int disposal_pipe, int wwtp, int sewer_pipe, int lake, string screen){
        // disposal pipe 0, wwtp 1, sewer pipe 2, lake 3
        order[0] = disposal_pipe;
        order[1] = wwtp;
        order[2] = sewer_pipe;
        order[3] = lake;
        
        correct_order[0] = 1;
        correct_order[1] = 3;
        correct_order[2] = 2;
        correct_order[3] = 4;

        Screen.autorotateToLandscapeLeft = false;
        Screen.autorotateToLandscapeRight = false;
        Screen.autorotateToPortrait = false;

        orientation = screen;
        if(orientation == "portrait"){
            taskPanel_P.SetActive(true);
            taskPanel_L.SetActive(false);
            parent = anchors_portrait;
            disposal_pipe_image = disposal_pipe_image_P;
            wwtp_image = wwtp_image_P;
            sewer_pipe_image = sewer_pipe_image_P;
            lake_image = lake_image_P;
        }else{
            taskPanel_P.SetActive(false);
            taskPanel_L.SetActive(true);
            parent = anchors_landscape;
            disposal_pipe_image = disposal_pipe_image_L;
            wwtp_image = wwtp_image_L;
            sewer_pipe_image = sewer_pipe_image_L;
            lake_image = lake_image_L;
        }

        disposal_pipe_image.transform.SetParent(parent.transform.GetChild(order[0] - 1).transform, true);
        wwtp_image.transform.SetParent(parent.transform.GetChild(order[1] - 1).transform, true);
        sewer_pipe_image.transform.SetParent(parent.transform.GetChild(order[2] - 1).transform, true);
        lake_image.transform.SetParent(parent.transform.GetChild(order[3] - 1).transform, true);

        disposal_pipe_image.GetComponent<RectTransform>().offsetMin = Vector2.zero;
        disposal_pipe_image.GetComponent<RectTransform>().offsetMax = Vector2.zero;
        wwtp_image.GetComponent<RectTransform>().offsetMin = Vector2.zero;
        wwtp_image.GetComponent<RectTransform>().offsetMax = Vector2.zero;
        sewer_pipe_image.GetComponent<RectTransform>().offsetMin = Vector2.zero;
        sewer_pipe_image.GetComponent<RectTransform>().offsetMax = Vector2.zero;
        lake_image.GetComponent<RectTransform>().offsetMin = Vector2.zero;
        lake_image.GetComponent<RectTransform>().offsetMax = Vector2.zero;
    }

    public void ok_taskPanel_bttn(){
        if(order[0] == correct_order[0] && order[1] == correct_order[1] && order[2] == correct_order[2] && order[3] == correct_order[3]){
            //after task panel
            // if(attempts == 0){
            //     scoreText_afterTaskPanel_L.text = "The order was pipe, membrane, tank. You made it on the first attempt, so you are not losing any water.";
            //     scoreText_afterTaskPanel_P.text = scoreText_afterTaskPanel_L.text;
            // }else{
            //     scoreText_afterTaskPanel_L.text = "The order was pipe, sewer pipe, water treatment plant and lake. You had "+ attempts.ToString() + " attempts, so you lost " + attempts.ToString() + 
            //     "x10=" +  (attempts*10).ToString() + " liters of water.\n\n\n" + "Your new score: " + gameController.GetComponent<gameController>().score.ToString() + 
            //     " liters";
            //     scoreText_afterTaskPanel_P.text = scoreText_afterTaskPanel_L.text;
            // }
            scoreText_afterTaskPanel_L.text = "The order was pipe, sewer pipe, water treatment plant and lake.\n\nYour current score: " + gameController.GetComponent<gameController>().score.ToString() + " liters.";
            scoreText_afterTaskPanel_P.text = scoreText_afterTaskPanel_L.text;
            float level_P = -(minLevel_afterTaskPanel_P-(float)gameController.GetComponent<gameController>().score/(float)gameController.GetComponent<gameController>().maxScore*minLevel_afterTaskPanel_P);
            float level_L = -(minLevel_afterTaskPanel_L-(float)gameController.GetComponent<gameController>().score/(float)gameController.GetComponent<gameController>().maxScore*minLevel_afterTaskPanel_L);
            tank_afterTaskPanel_P.GetComponent<RectTransform>().offsetMax = new Vector2(0f, level_P);
            tank_afterTaskPanel_P.GetComponent<RectTransform>().offsetMin = new Vector2(0f, level_P);
            tank_afterTaskPanel_L.GetComponent<RectTransform>().offsetMax = new Vector2(0f, level_L);
            tank_afterTaskPanel_L.GetComponent<RectTransform>().offsetMin = new Vector2(0f, level_L);

            taskPanel.SetActive(false);
            afterTaskPanel.SetActive(true);
            Screen.autorotateToLandscapeLeft = true;
            Screen.autorotateToLandscapeRight = true;
            Screen.autorotateToPortrait = true;
        }else{
            attempts ++;
            taskPanel.SetActive(false);
            incorrectOrderPanel.SetActive(true);
            //update water level in the tank
            float oldScore = (float)gameController.GetComponent<gameController>().score;
            gameController.GetComponent<gameController>().score -= 10;
            gameController.GetComponent<gameController>().updateStatus(10, false);
            scoreText_L.text = "You have lost 10 liters of water.\n Your current score is:\n" + gameController.GetComponent<gameController>().score.ToString() + " liters."
            + "\n Try again!";
            scoreText_P.text = "You have lost 10 liters of water.\n Your current score is:\n" + gameController.GetComponent<gameController>().score.ToString() + " liters."
            + "\n Try again!";
            setupAnimationWaterLevel((float)gameController.GetComponent<gameController>().score, oldScore);
        }
    }
    
    private void setupAnimationWaterLevel(float newScore, float oldScore){
        oldLevel_P = -(minLevel_P-oldScore/(float)gameController.GetComponent<gameController>().maxScore*minLevel_P);
        newLevel_P = -(minLevel_P-newScore/(float)gameController.GetComponent<gameController>().maxScore*minLevel_P);
        oldLevel_L = -(minLevel_L-oldScore/(float)gameController.GetComponent<gameController>().maxScore*minLevel_L);
        newLevel_L = -(minLevel_L-newScore/(float)gameController.GetComponent<gameController>().maxScore*minLevel_L);
        t = 0f;
        dt = 0.01f;
        animate = true;
        droplet_L.SetActive(true);
        droplet_P.SetActive(true);
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

    public void ok_incorrectOrderPanel_bttn(){
        taskPanel.SetActive(true);
        incorrectOrderPanel.SetActive(false);
    }

    public void ok_afterTaskPanel_bttn(){
        afterTaskPanel.SetActive(false);
        taskCompletedPanel.SetActive(true);
    }

    void relocate_picture(GameObject picture, Transform parent){
        picture.transform.SetParent(parent.transform, true);
        picture.transform.SetParent(parent.transform, true);
        picture.transform.SetAsLastSibling();
        
        picture.GetComponent<RectTransform>().offsetMin = Vector2.zero;
        picture.GetComponent<RectTransform>().offsetMax = Vector2.zero;
    }

    public void drag_disposal_pipe(){
        disposal_pipe_image.transform.position = Input.mousePosition;
        float distance;
        int this_index = 0;

        for (int i = 0; i < order.Length; i++){
            if(i != this_index){
                distance = Vector2.Distance(Input.mousePosition, parent.transform.GetChild(order[i] - 1).transform.position);
                if(distance < swap_distance){
                    if(parent.transform.GetChild(order[i] - 1).transform.childCount > 0)
                        relocate_picture(parent.transform.GetChild(order[i] - 1).transform.GetChild(0).gameObject, parent.transform.GetChild(order[this_index] - 1));
                }else if(parent.transform.GetChild(order[i] - 1).transform.childCount == 0)
                    relocate_picture(parent.transform.GetChild(order[this_index] - 1).transform.GetChild(1).gameObject, parent.transform.GetChild(order[i] - 1));
            }
        }
    }

    public void endDrag_disposal_pipe(){
        int this_index = 0;
        bool swaped = false;
        for (int i = 0; i < order.Length; i++)
            if(parent.transform.GetChild(order[i] - 1).transform.childCount == 0){
                swaped = true;
                relocate_picture(disposal_pipe_image, parent.transform.GetChild(order[i] - 1));
                int temp = order[i];
                order[i] = order[this_index];
                order[this_index] = temp;
            }
        if(!swaped){
            disposal_pipe_image.GetComponent<RectTransform>().offsetMin = Vector2.zero;
            disposal_pipe_image.GetComponent<RectTransform>().offsetMax = Vector2.zero;
        }
    }

    public void drag_wwtp(){
        wwtp_image.transform.position = Input.mousePosition;
        float distance;
        int this_index = 1;

        for (int i = 0; i < order.Length; i++){
            if(i != this_index){
                distance = Vector2.Distance(Input.mousePosition, parent.transform.GetChild(order[i] - 1).transform.position);
                if(distance < swap_distance){
                    if(parent.transform.GetChild(order[i] - 1).transform.childCount > 0)
                        relocate_picture(parent.transform.GetChild(order[i] - 1).transform.GetChild(0).gameObject, parent.transform.GetChild(order[this_index] - 1));
                }else if(parent.transform.GetChild(order[i] - 1).transform.childCount == 0)
                    relocate_picture(parent.transform.GetChild(order[this_index] - 1).transform.GetChild(1).gameObject, parent.transform.GetChild(order[i] - 1));
            }
        }
    }

    public void endDrag_wwtp(){
        int this_index = 1;
        bool swaped = false;
        for (int i = 0; i < order.Length; i++)
            if(parent.transform.GetChild(order[i] - 1).transform.childCount == 0){
                swaped = true;
                relocate_picture(wwtp_image, parent.transform.GetChild(order[i] - 1));
                int temp = order[i];
                order[i] = order[this_index];
                order[this_index] = temp;
            }
        if(!swaped){
            wwtp_image.GetComponent<RectTransform>().offsetMin = Vector2.zero;
            wwtp_image.GetComponent<RectTransform>().offsetMax = Vector2.zero;
        }
    }

    public void drag_sewer_pipe(){
        sewer_pipe_image.transform.position = Input.mousePosition;
        float distance;
        int this_index = 2;

        for (int i = 0; i < order.Length; i++){
            if(i != this_index){
                distance = Vector2.Distance(Input.mousePosition, parent.transform.GetChild(order[i] - 1).transform.position);
                if(distance < swap_distance){
                    if(parent.transform.GetChild(order[i] - 1).transform.childCount > 0)
                        relocate_picture(parent.transform.GetChild(order[i] - 1).transform.GetChild(0).gameObject, parent.transform.GetChild(order[this_index] - 1));
                }else if(parent.transform.GetChild(order[i] - 1).transform.childCount == 0)
                    relocate_picture(parent.transform.GetChild(order[this_index] - 1).transform.GetChild(1).gameObject, parent.transform.GetChild(order[i] - 1));
            }
        }
    }

    public void endDrag_sewer_pipe(){
        int this_index = 2;
        bool swaped = false;

        for (int i = 0; i < order.Length; i++)
            if(parent.transform.GetChild(order[i] - 1).transform.childCount == 0){
                swaped = true;
                relocate_picture(sewer_pipe_image, parent.transform.GetChild(order[i] - 1));
                int temp = order[i];
                order[i] = order[this_index];
                order[this_index] = temp;
            }
        if(!swaped){
            sewer_pipe_image.GetComponent<RectTransform>().offsetMin = Vector2.zero;
            sewer_pipe_image.GetComponent<RectTransform>().offsetMax = Vector2.zero;
        }
    }

    public void drag_lake(){
        lake_image.transform.position = Input.mousePosition;
        float distance;
        int this_index = 3;

        for (int i = 0; i < order.Length; i++){
            if(i != this_index){
                distance = Vector2.Distance(Input.mousePosition, parent.transform.GetChild(order[i] - 1).transform.position);
                if(distance < swap_distance){
                    if(parent.transform.GetChild(order[i] - 1).transform.childCount > 0)
                        relocate_picture(parent.transform.GetChild(order[i] - 1).transform.GetChild(0).gameObject, parent.transform.GetChild(order[this_index] - 1));
                }else if(parent.transform.GetChild(order[i] - 1).transform.childCount == 0)
                    relocate_picture(parent.transform.GetChild(order[this_index] - 1).transform.GetChild(1).gameObject, parent.transform.GetChild(order[i] - 1));
            }
        }
    }

    public void endDrag_lake(){
        int this_index = 3;
        bool swaped = false;

        for (int i = 0; i < order.Length; i++)
            if(parent.transform.GetChild(order[i] - 1).transform.childCount == 0){
                swaped = true;
                relocate_picture(lake_image, parent.transform.GetChild(order[i] - 1));
                int temp = order[i];
                order[i] = order[this_index];
                order[this_index] = temp;
            }
        if(!swaped){
            lake_image.GetComponent<RectTransform>().offsetMin = Vector2.zero;
            lake_image.GetComponent<RectTransform>().offsetMax = Vector2.zero;
        }
    }
}
