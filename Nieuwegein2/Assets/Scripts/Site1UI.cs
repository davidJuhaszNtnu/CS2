using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Site1UI : MonoBehaviour
{
    public GameObject welcomePanel, multichoicePanel, scoreUpdatePanel, answerPanel, site1, gameController;
    public bool[] answered;
    bool[] correct_answer;

    public GameObject[] toggles_L;
    public GameObject[] toggles_P;
    public GameObject[] toggles_score_L;
    public GameObject[] toggles_score_P;

    public GameObject[] scratches_L;
    public GameObject[] scratches_P;
    public GameObject[] points_L;
    public GameObject[] points_P;

    public TextMeshProUGUI score_text_L, score_text_P;
    public GameObject tank_L, tank_P, droplet_L, droplet_P;

    bool animate;
    float minLevel_P, minLevel_L, t, dt, oldLevel_P, newLevel_P, currentLevel_P, oldLevel_L, newLevel_L, currentLevel_L;

    void Start()
    {
        answered = new bool[4];
        correct_answer = new bool[4];

        float aspectRatio = (float)Screen.width/(float)Screen.height;
        if(Screen.orientation == ScreenOrientation.Portrait){
            minLevel_P = tank_P.GetComponent<RectTransform>().rect.height;
            minLevel_L = tank_L.GetComponent<RectTransform>().rect.height * aspectRatio;
        }else{
            minLevel_L = tank_L.GetComponent<RectTransform>().rect.height;
            minLevel_P = tank_P.GetComponent<RectTransform>().rect.height * aspectRatio;
        }

        restart();
    }

    public void restart(){
        for (int i = 0; i < answered.Length; i++)
            answered[i] = false;
        correct_answer[0] = false;
        correct_answer[1] = true;
        correct_answer[2] = true;
        correct_answer[3] = false;

        animate = false;        
        droplet_L.SetActive(false);
        droplet_P.SetActive(false);
    }

    void Update(){
        if(animate)
            animateWaterLevel();
    }

    public void ok_welcomePanel_bttn(){
        welcomePanel.SetActive(false);
        multichoicePanel.SetActive(true);
    }

    public void ok_multichoicePanel_bttn(){
        multichoicePanel.SetActive(false);
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

        scoreUpdatePanel.SetActive(true);
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

    public void ok_scoreUpdatePanel_bttn(){
        scoreUpdatePanel.SetActive(false);
        answerPanel.SetActive(true);
    }

    public void ok_answerPanel_bttn(){
        site1.GetComponent<Site1>().nextSite();
    }

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
}
