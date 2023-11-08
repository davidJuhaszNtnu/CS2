using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mapbox.Examples;

public class Site4 : MonoBehaviour
{
    public GameObject wwtp_prefab, welcomePanel, statementsPanel, scoreUpdatePanel1, afterStatementsPanel, taskPanel, taskCompletedPanel, site4, site4UI, app, gameController;
    public Button true_button_L, false_button_L, next_button_L, true_button_P, false_button_P, next_button_P;
    public Camera arCamera, mapCamera;
    string[] statements, info_on_correct, info_on_false;
    bool[] correct_answers;
    int statementCount, incorrentAnsweresCount;
    GameObject question_L, question_P, statementsPanel_L, statementsPanel_P, wwtp;
    TextMeshProUGUI resultText_L, answerText_L, resultText_P, answerText_P;

    bool animate;
    public GameObject tank_L, tank_P, droplet_L, droplet_P;
    float minLevel_P, minLevel_L, t, dt, oldLevel_P, newLevel_P, currentLevel_P, oldLevel_L, newLevel_L, currentLevel_L;
    public TextMeshProUGUI scoreText_L, scoreText_P;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update(){
        if(animate)
            animateWaterLevel();
    }

    public void startSite(){
        wwtp = Instantiate(wwtp_prefab);
        wwtp.transform.SetParent(transform, true);
        wwtp.SetActive(true);
        Vector3 dir = arCamera.transform.forward;
        wwtp.transform.position = transform.position + Vector3.Normalize(new Vector3(dir.x,0f,dir.z))*0.5f + new Vector3(0f, 0f, 0f);

        welcomePanel.SetActive(true);
        statementsPanel.SetActive(false);
        scoreUpdatePanel1.SetActive(false);
        statementsPanel_L = statementsPanel.transform.GetChild(0).gameObject;
        statementsPanel_P = statementsPanel.transform.GetChild(1).gameObject;
        afterStatementsPanel.SetActive(false);
        taskPanel.SetActive(false);
        taskCompletedPanel.SetActive(false);
        question_L = statementsPanel_L.transform.GetChild(0).gameObject;
        question_P = statementsPanel_P.transform.GetChild(0).gameObject;
        resultText_L = statementsPanel_L.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        answerText_L = statementsPanel_L.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        resultText_P = statementsPanel_P.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        answerText_P = statementsPanel_P.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        statementCount = 0;
        incorrentAnsweresCount = 0;

        statements = new string[6];
        statements[0]="Water from treated wastewater can be reused for drinking water.";
        statements[1]="Water from treated wastewater can be reused for irrigation.";
        statements[2]="Wastewater contains energy that can be reclaimed.";
        statements[3]="Wastewater contains heat that can be recovered.";
        statements[4]="Wastewater contains high levels of precious metals that can be recovered.";
        statements[5]="Making drinking water produces waste.";

        info_on_correct = new string[6];
        info_on_correct[0]="The law does not allow the reuse of treated wastewater for drinking water in the Netherlands.";
        info_on_correct[1]="Treated wastewater can be used to water parks and for farming.";
        info_on_correct[2]="Waste captured from the treatment of wastewater can be used to generate energy.";
        info_on_correct[3]="It is possible to recover heat from wastewater using spesific technologies, which can be used for heating.";
        info_on_correct[4]="Wastewater may contain low levels og precious metals.";
        info_on_correct[5]="Some waste streams from the production of drinking water can be re-used (e.g. lime pellets (which contain calcium carbonate) are removed  during the treatment process and sold to fertilizer, steel and construction industries.";

        info_on_false = new string[6];
        info_on_false[0]="The law does not allow the reuse of treated wastewater for drinking water in the Netherlands.";
        info_on_false[1]="Treated wastewater can be used to water parks and for farming.";
        info_on_false[2]="Waste captured from the treatment of wastewater can be used to generate energy.";
        info_on_false[3]="It is possible to recover heat from wastewater using spesific technologies, which can be used for heating.";
        info_on_false[4]="Wastewater may contain low levels og precious metals.";
        info_on_false[5]="Some waste streams from the production of drinking water can be re-used (e.g. lime pellets (which contain calcium carbonate) are removed  during the treatment process and sold to fertilizer, steel and construction industries.";

        correct_answers = new bool[6];
        correct_answers[0] = false;
        correct_answers[1] = true;
        correct_answers[2] = true;
        correct_answers[3] = true;
        correct_answers[4] = false;
        correct_answers[5] = false;

        question_L.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = (statementCount + 1).ToString() + "/6";
        question_L.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = statements[statementCount];
        question_P.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = (statementCount + 1).ToString() + "/6";
        question_P.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = statements[statementCount];
        next_button_L.gameObject.SetActive(false);
        true_button_L.gameObject.SetActive(true);
        false_button_L.gameObject.SetActive(true);
        resultText_L.gameObject.SetActive(false);
        answerText_L.gameObject.SetActive(false);
        next_button_P.gameObject.SetActive(false);
        true_button_P.gameObject.SetActive(true);
        false_button_P.gameObject.SetActive(true);
        resultText_P.gameObject.SetActive(false);
        answerText_P.gameObject.SetActive(false);

        animate = false;
        minLevel_P = 290f;
        minLevel_L = 220f;
        droplet_L.SetActive(false);
        droplet_P.SetActive(false);
    }

    public void ok_welcomePanel_bttn(){
        welcomePanel.SetActive(false);
        statementsPanel.SetActive(true);
    }

    public void true_bttn(){
        resultText_L.gameObject.SetActive(true);
        answerText_L.gameObject.SetActive(true);

        resultText_P.gameObject.SetActive(true);
        answerText_P.gameObject.SetActive(true);

        if(correct_answers[statementCount]){
            resultText_L.text = "Correct!";
            answerText_L.text = info_on_correct[statementCount];

            resultText_P.text = "Correct!";
            answerText_P.text = info_on_correct[statementCount];
        }else{
            incorrentAnsweresCount++;
            resultText_L.text = "Incorrect!";
            answerText_L.text = info_on_false[statementCount];

            resultText_P.text = "Incorrect!";
            answerText_P.text = info_on_false[statementCount];
        }

        true_button_L.gameObject.SetActive(false);
        false_button_L.gameObject.SetActive(false);
        next_button_L.gameObject.SetActive(true);

        true_button_P.gameObject.SetActive(false);
        false_button_P.gameObject.SetActive(false);
        next_button_P.gameObject.SetActive(true);

        statementCount++;
    }

    public void false_bttn(){
        resultText_L.gameObject.SetActive(true);
        answerText_L.gameObject.SetActive(true);

        resultText_P.gameObject.SetActive(true);
        answerText_P.gameObject.SetActive(true);

        if(!correct_answers[statementCount]){
            resultText_L.text = "Correct!";
            answerText_L.text = info_on_correct[statementCount];

            resultText_P.text = "Correct!";
            answerText_P.text = info_on_correct[statementCount];
        }else{
            incorrentAnsweresCount++;
            resultText_L.text = "Incorrect!";
            answerText_L.text = info_on_false[statementCount];

            resultText_P.text = "Incorrect!";
            answerText_P.text = info_on_false[statementCount];
        }

        true_button_L.gameObject.SetActive(false);
        false_button_L.gameObject.SetActive(false);
        next_button_L.gameObject.SetActive(true);

        true_button_P.gameObject.SetActive(false);
        false_button_P.gameObject.SetActive(false);
        next_button_P.gameObject.SetActive(true);

        statementCount++;
    }

    public void next_bttn(){
        resultText_L.gameObject.SetActive(false);
        answerText_L.gameObject.SetActive(false);
        true_button_L.gameObject.SetActive(true);
        false_button_L.gameObject.SetActive(true);
        next_button_L.gameObject.SetActive(false);

        resultText_P.gameObject.SetActive(false);
        answerText_P.gameObject.SetActive(false);
        true_button_P.gameObject.SetActive(true);
        false_button_P.gameObject.SetActive(true);
        next_button_P.gameObject.SetActive(false);
        if(statementCount<6){
            question_L.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = (statementCount + 1).ToString() + "/6";
            question_L.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = statements[statementCount];

            question_P.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = (statementCount + 1).ToString() + "/6";
            question_P.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = statements[statementCount];
        }else{
            // it was the last statement
            statementsPanel.SetActive(false);
            scoreUpdatePanel1.SetActive(true);
            if(incorrentAnsweresCount == 0){
                scoreText_L.text = "Congratulations! All your answers were correct.\n Your score is " + gameController.GetComponent<gameController>().score.ToString() + "liters.";
                scoreText_P.text = scoreText_L.text;
            }else{
                float oldScore = (float)gameController.GetComponent<gameController>().score;
                gameController.GetComponent<gameController>().score -= incorrentAnsweresCount*10;
                gameController.GetComponent<gameController>().updateStatus(incorrentAnsweresCount*10, false);
                setupAnimationWaterLevel((float)gameController.GetComponent<gameController>().score, oldScore);
                scoreText_L.text = "You answered " + incorrentAnsweresCount.ToString() + " out of 6 incorrect. You lost " + 
                incorrentAnsweresCount.ToString() + "x10=" + (incorrentAnsweresCount*10).ToString() + " liters of water.\n\n" +
                "Your new score is " + gameController.GetComponent<gameController>().score.ToString() + " liters.";
                scoreText_P.text = scoreText_L.text;
            }
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

    public void ok_scoreUpdatePanel1_bttn(){
        scoreUpdatePanel1.SetActive(false);
        afterStatementsPanel.SetActive(true);
        //components gained
        gameController.GetComponent<gameController>().updateStatus(0, true);
    }

    public void ok_afterStatementsPanel_bttn(){
        afterStatementsPanel.SetActive(false);
        taskPanel.SetActive(true);
        string orientation;
        if(Screen.orientation == ScreenOrientation.Portrait)
            orientation = "portrait";
        else orientation = "landscape";
        site4UI.GetComponent<Site4UI>().setImages(1, 2, 3, 4, orientation);
    }

    public void ok_taskCompletedPanel_bttn(){
        site4.SetActive(false);
        site4UI.SetActive(false);
        arCamera.enabled = false;
        mapCamera.enabled = true;
        app.GetComponent<App>().map.SetActive(true);
        app.GetComponent<App>().player.SetActive(true);
        app.GetComponent<App>().sitePathSpawner.GetComponent<SpawnOnMap>().currentSite = 3;
        app.GetComponent<App>().nextSite_index = 5;
        app.GetComponent<App>().siteOn = false;
        app.GetComponent<App>().sitePathSpawner.GetComponent<SpawnOnMap>().showSitePath();
        app.GetComponent<App>().back_button.gameObject.SetActive(true);
        app.GetComponent<App>().showMap_button.gameObject.SetActive(false);
    }
}
