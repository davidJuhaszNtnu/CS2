using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Site2UI : MonoBehaviour
{
    public GameObject pipe_image_L, membrane_image_L, tank_image_L, pipe_image_P, membrane_image_P, tank_image_P;
    GameObject pipe_image, membrane_image, tank_image;
    public GameObject anchors_portrait_taskPanel, anchors_landscape_taskPanel;
    public GameObject findPanel, taskPanel, taskPanel_L, taskPanel_P, incorrectOrderPanel, taskCompletedPanel, site2, site2UI;
    public GameObject tank_taskCompletedPanel_L, tank_taskCompletedPanel_P;

    int[] order;
    int[] correct_order;
    string orientation;
    GameObject parent;
    float swap_distance;

    bool animate;
    public GameObject tank_L, tank_P, droplet_L, droplet_P, gameController;
    float minLevel_P, minLevel_L, t, dt, oldLevel_P, newLevel_P, currentLevel_P, oldLevel_L, newLevel_L, currentLevel_L, minLevel_taskCompletedPanel_L, minLevel_taskCompletedPanel_P;
    public TextMeshProUGUI scoreText_L, scoreText_P, scoreText_taskCompletedPanel_L, scoreText_taskCompletedPanel_P;
    int attempts;

    void Start()
    {
        float aspectRatio = (float)Screen.width/(float)Screen.height;
        if(Screen.orientation == ScreenOrientation.Portrait){
            minLevel_P = tank_P.GetComponent<RectTransform>().rect.height;
            minLevel_L = tank_L.GetComponent<RectTransform>().rect.height * aspectRatio;

            minLevel_taskCompletedPanel_P = tank_taskCompletedPanel_P.GetComponent<RectTransform>().rect.height;
            minLevel_taskCompletedPanel_L = tank_taskCompletedPanel_L.GetComponent<RectTransform>().rect.height * aspectRatio;
        }else{
            minLevel_L = tank_L.GetComponent<RectTransform>().rect.height;
            minLevel_P = tank_P.GetComponent<RectTransform>().rect.height * aspectRatio;

            minLevel_taskCompletedPanel_L = tank_taskCompletedPanel_L.GetComponent<RectTransform>().rect.height;
            minLevel_taskCompletedPanel_P = tank_taskCompletedPanel_P.GetComponent<RectTransform>().rect.height * aspectRatio;
        }
        // Debug.Log(minLevel_P + ", " + minLevel_L);

        restart();
    }

    public void restart(){
        // pipe 0, membrane 1, tank 2
        order = new int[3];
        correct_order = new int[3];
        swap_distance = 100f;
        // setImages(1, 2, 3, "portrait");

        animate = false;
        //ordering
        droplet_L.SetActive(false);
        droplet_P.SetActive(false);
        attempts = 0;
    }

    void Update(){
        if(animate)
            animateWaterLevel();
    }

    public void setImages(int pipe, int membrane, int tank, string screen){
        // pipe 0, membrane 1, tank 2
        order[0] = pipe;
        order[1] = membrane;
        order[2] = tank;

        correct_order[0] = 1;
        correct_order[1] = 2;
        correct_order[2] = 3;

        Screen.autorotateToLandscapeLeft = false;
        Screen.autorotateToLandscapeRight = false;
        Screen.autorotateToPortrait = false;

        orientation = screen;
        if(orientation == "portrait"){
            taskPanel_P.SetActive(true);
            taskPanel_L.SetActive(false);
            parent = anchors_portrait_taskPanel;
            pipe_image = pipe_image_P;
            membrane_image = membrane_image_P;
            tank_image = tank_image_P;
        }else{
            taskPanel_P.SetActive(false);
            taskPanel_L.SetActive(true);
            parent = anchors_landscape_taskPanel;
            pipe_image = pipe_image_L;
            membrane_image = membrane_image_L;
            tank_image = tank_image_L;
        }

        pipe_image.transform.SetParent(parent.transform.GetChild(order[0] - 1).transform, true);
        membrane_image.transform.SetParent(parent.transform.GetChild(order[1] - 1).transform, true);
        tank_image.transform.SetParent(parent.transform.GetChild(order[2] - 1).transform, true);

        pipe_image.GetComponent<RectTransform>().offsetMin = Vector2.zero;
        pipe_image.GetComponent<RectTransform>().offsetMax = Vector2.zero;
        membrane_image.GetComponent<RectTransform>().offsetMin = Vector2.zero;
        membrane_image.GetComponent<RectTransform>().offsetMax = Vector2.zero;
        tank_image.GetComponent<RectTransform>().offsetMin = Vector2.zero;
        tank_image.GetComponent<RectTransform>().offsetMax = Vector2.zero;
    }

    public void ok_taskPanel_bttn(){
        if(order[0] == correct_order[0] && order[1] == correct_order[1] && order[2] == correct_order[2]){
            //task completed panel
            // if(attempts == 0){
            //     scoreText_taskCompletedPanel_L.text = "The order was pipe, membrane, tank. You made it on the first attempt, so you are not losing any water.";
            //     scoreText_taskCompletedPanel_P.text = scoreText_taskCompletedPanel_L.text;
            // }else{
            //     scoreText_taskCompletedPanel_L.text = "The order was pipe, membrane, tank. You had "+ attempts.ToString() + " attempts, so you lost " + attempts.ToString() + 
            //     "x10=" +  (attempts*10).ToString() + " liters of water.\n\n" + "Your new score: " + gameController.GetComponent<gameController>().score.ToString() + 
            //     " liters";
            //     scoreText_taskCompletedPanel_P.text = scoreText_taskCompletedPanel_L.text;
            // }
            //update tank level on taskCompletedPanel
            scoreText_taskCompletedPanel_L.text = "The order was pipe, membrane, tank.\nYour current score: " + gameController.GetComponent<gameController>().score.ToString() + " liters.";
            scoreText_taskCompletedPanel_P.text = scoreText_taskCompletedPanel_L.text;
            float level_P = -(minLevel_taskCompletedPanel_P-(float)gameController.GetComponent<gameController>().score/(float)gameController.GetComponent<gameController>().maxScore*minLevel_taskCompletedPanel_P);
            float level_L = -(minLevel_taskCompletedPanel_L-(float)gameController.GetComponent<gameController>().score/(float)gameController.GetComponent<gameController>().maxScore*minLevel_taskCompletedPanel_L);
            tank_taskCompletedPanel_P.GetComponent<RectTransform>().offsetMax = new Vector2(0f, level_P);
            tank_taskCompletedPanel_P.GetComponent<RectTransform>().offsetMin = new Vector2(0f, level_P);
            tank_taskCompletedPanel_L.GetComponent<RectTransform>().offsetMax = new Vector2(0f, level_L);
            tank_taskCompletedPanel_L.GetComponent<RectTransform>().offsetMin = new Vector2(0f, level_L);

            taskPanel.SetActive(false);
            taskCompletedPanel.SetActive(true);
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
            scoreText_P.text = scoreText_L.text;
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

    public void ok_taskCompletedPanel_bttn(){
        site2.GetComponent<Site2>().nextSite();
    }

    void relocate_picture(GameObject picture, Transform parent){
        picture.transform.SetParent(parent.transform, true);
        picture.transform.SetAsLastSibling();
        
        picture.GetComponent<RectTransform>().offsetMin = Vector2.zero;
        picture.GetComponent<RectTransform>().offsetMax = Vector2.zero;
    }

    public void drag_pipe(){
        pipe_image.transform.position = Input.mousePosition;
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

    public void endDrag_pipe(){
        bool swaped = false;
        int this_index = 0;

        for (int i = 0; i < order.Length; i++)
            if(parent.transform.GetChild(order[i] - 1).transform.childCount == 0){
                swaped = true;
                relocate_picture(pipe_image, parent.transform.GetChild(order[i] - 1));
                int temp = order[i];
                order[i] = order[this_index];
                order[this_index] = temp;
            }
        if(!swaped){
            pipe_image.GetComponent<RectTransform>().offsetMin = Vector2.zero;
            pipe_image.GetComponent<RectTransform>().offsetMax = Vector2.zero;
        }
    }

    public void drag_membrane(){
        membrane_image.transform.position = Input.mousePosition;
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

    public void endDrag_membrane(){
        bool swaped = false;
        int this_index = 1;

        for (int i = 0; i < order.Length; i++)
            if(parent.transform.GetChild(order[i] - 1).transform.childCount == 0){
                swaped = true;
                relocate_picture(membrane_image, parent.transform.GetChild(order[i] - 1));
                int temp = order[i];
                order[i] = order[this_index];
                order[this_index] = temp;
            }
        if(!swaped){
            membrane_image.GetComponent<RectTransform>().offsetMin = Vector2.zero;
            membrane_image.GetComponent<RectTransform>().offsetMax = Vector2.zero;
        }
    }

    public void drag_tank(){
        tank_image.transform.position = Input.mousePosition;
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

    public void endDrag_tank(){
        bool swaped = false;
        int this_index = 2;

        for (int i = 0; i < order.Length; i++)
            if(parent.transform.GetChild(order[i] - 1).transform.childCount == 0){
                swaped = true;
                relocate_picture(tank_image, parent.transform.GetChild(order[i] - 1));
                int temp = order[i];
                order[i] = order[this_index];
                order[this_index] = temp;
            }
        if(!swaped){
            tank_image.GetComponent<RectTransform>().offsetMin = Vector2.zero;
            tank_image.GetComponent<RectTransform>().offsetMax = Vector2.zero;
        }
    }
}
