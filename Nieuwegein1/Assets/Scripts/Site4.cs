using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Site4 : MonoBehaviour
{
    public GameObject path3, canvas, welcomePanel, task1Panel, completedPanel1, completedPanel2, site4, site4UI, path4, site5, site5UI;
    string[] statements, answers;
    int statementCount;
    public Button OkWelcome, TrueButton, FalseButton, NextButton, OkCongrats1, OkCongrats2;
    public TextMeshProUGUI statementText, resultText, answerText;
    public Camera arCamera;
    bool bool1;
    public bool showSite;

    // Start is called before the first frame update
    void Start()
    {
        bool1 = true;
        canvas.SetActive(true);
        welcomePanel.SetActive(false);
        task1Panel.SetActive(false);
        completedPanel1.SetActive(false);
        completedPanel2.SetActive(false);
        OkWelcome.onClick.AddListener(ok_welcome);
        TrueButton.onClick.AddListener(trueButton);
        FalseButton.onClick.AddListener(falseButton);
        NextButton.onClick.AddListener(nextButton);
        OkCongrats1.onClick.AddListener(ok_congrats1);
        OkCongrats2.onClick.AddListener(ok_congrats2);
            
        statements = new string[6];
        statements[0]="1."+"\n\n"+"Water from treated wastewater can be reused for drinking water.";
        statements[1]="2."+"\n\n"+"Water from treated wastewater can be reused for irrigation.";
        statements[2]="3."+"\n\n"+"Wastewater contains energy that can be reclaimed.";
        statements[3]="4."+"\n\n"+"Wastewater contains heat that can be recovered.";
        statements[4]="5."+"\n\n"+"Wastewater contains high levels of precious metals that can be recovered.";
        statements[5]="6."+"\n\n"+"Making drinking water produces waste.";

        answers = new string[6];
        answers[0]="The law does not allow it.";
        answers[1]="In parks and farming for example.";
        answers[2]="Indirectly - all the waste captured at the WWTP is turned into biogas.";
        answers[3]="Wastewater is warm and the warmth can be extracted e.g. to heat houses.";
        answers[4]="But why?";
        answers[5]="Some waste streams can be reused - e.g. lime which is removed at the treatment plant is sold to industry.";

        statementCount = 0;
        showSite=false;
        

        // site4UI.SetActive(true);
        // site4UI.GetComponent<Site4UI>().canvas.SetActive(true);
        // site4UI.GetComponent<Site4UI>().task2Panel.SetActive(true);
        // Screen.orientation = ScreenOrientation.Portrait;
        // Screen.autorotateToLandscapeLeft = false;
        // Screen.autorotateToLandscapeRight = false;
    }

    private void ok_congrats1(){
        completedPanel1.SetActive(false);
        site4UI.SetActive(true);
        site4UI.GetComponent<Site4UI>().canvas.SetActive(true);
        site4UI.GetComponent<Site4UI>().task2Panel.SetActive(true);
        Screen.orientation = ScreenOrientation.Portrait;
        Screen.autorotateToLandscapeLeft = false;
        Screen.autorotateToLandscapeRight = false;
    }

    private void ok_congrats2(){
        completedPanel2.SetActive(false);
        site4.SetActive(false);
        site4UI.SetActive(false);
        // site5.SetActive(true);
        // site5UI.SetActive(true);
        path4.SetActive(true);
        path4.GetComponent<Path>().arrived=false;
        site5.GetComponent<Site5>().showSite=true;
        
        path4.GetComponent<Path>().t=0f;
        path4.GetComponent<Path>().Arrow.SetActive(true);
        path4.GetComponent<Path>().Arrow.transform.position=path4.GetComponent<Path>().path(0f);
        path4.GetComponent<Path>().Arrow.transform.rotation=Quaternion.LookRotation(path4.GetComponent<Path>().pathDer(0f), Vector3.up);
    }

    private void trueButton(){
        resultText.gameObject.SetActive(true);
        answerText.gameObject.SetActive(true);
        answerText.text = answers[statementCount];

        if(statementCount == 1 || statementCount == 2 || statementCount == 3){
            resultText.text = "Correct!";
            resultText.GetComponent<TextMeshProUGUI>().color = Color.green;
            answerText.GetComponent<TextMeshProUGUI>().color = Color.green;
        }
        if(statementCount == 0 || statementCount == 4 || statementCount == 5){
            resultText.text = "Incorrect";
            resultText.GetComponent<TextMeshProUGUI>().color = Color.red;
            answerText.GetComponent<TextMeshProUGUI>().color = Color.red;
        }

        TrueButton.gameObject.SetActive(false);
        FalseButton.gameObject.SetActive(false);
        NextButton.gameObject.SetActive(true);
        statementCount++;
    }
    private void falseButton(){
        resultText.gameObject.SetActive(true);
        answerText.gameObject.SetActive(true);
        answerText.text = answers[statementCount];

        if(statementCount == 1 || statementCount == 2 || statementCount == 3){
            resultText.text = "Incorrect!";
            resultText.GetComponent<TextMeshProUGUI>().color = Color.red;
            answerText.GetComponent<TextMeshProUGUI>().color = Color.red;
        }
        if(statementCount == 0 || statementCount == 4 || statementCount == 5){
            resultText.text = "Correct";
            resultText.GetComponent<TextMeshProUGUI>().color = Color.green;
            answerText.GetComponent<TextMeshProUGUI>().color = Color.green;
        }

        TrueButton.gameObject.SetActive(false);
        FalseButton.gameObject.SetActive(false);
        NextButton.gameObject.SetActive(true);
        statementCount++;
    }
    private void nextButton(){
        resultText.gameObject.SetActive(false);
        answerText.gameObject.SetActive(false);
        TrueButton.gameObject.SetActive(true);
        FalseButton.gameObject.SetActive(true);
        NextButton.gameObject.SetActive(false);
        if(statementCount<6)
            statementText.text = statements[statementCount];
        else{
            task1Panel.SetActive(false);
            completedPanel1.SetActive(true);
            Vector3 dir=arCamera.transform.position-canvas.GetComponent<RectTransform>().position;
            canvas.GetComponent<RectTransform>().rotation=Quaternion.LookRotation(new Vector3(dir.x,0f,dir.z), Vector3.up)*Quaternion.Euler(0,180,0);
        }
    }

    private void ok_welcome(){
        welcomePanel.SetActive(false);
        task1Panel.SetActive(true);
        site4UI.GetComponent<Site4UI>().canvas.SetActive(false);
        TrueButton.gameObject.SetActive(true);
        FalseButton.gameObject.SetActive(true);
        NextButton.gameObject.SetActive(false);
        resultText.gameObject.SetActive(false);
        answerText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // if(showSite && bool1){
        if(path3.GetComponent<Path>().arrived && bool1){
            bool1=false;
            // path3.SetActive(false);
            path3.GetComponent<Path>().Arrow.SetActive(false);

            canvas.GetComponent<RectTransform>().position = site4.transform.position+(new Vector3(0f,0f,1f));
            Vector3 dir=arCamera.transform.position-canvas.GetComponent<RectTransform>().position;
            canvas.GetComponent<RectTransform>().rotation=Quaternion.LookRotation(new Vector3(dir.x,0f,dir.z), Vector3.up)*Quaternion.Euler(0,180,0);
            welcomePanel.SetActive(true);
        }
    }
}
