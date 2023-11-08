using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class gameController : MonoBehaviour
{
    public int score;
    public int maxScore = 120;
    
    public TextMeshProUGUI scoreText_L, scoreText_P;
    public int[] scoreLost;
    public GameObject[] sites_L, sites_P;
    public GameObject app;

    void Start()
    {
        scoreLost = new int[4];
        restart();
        if(app.GetComponent<App>().nextSite_index != 1){
            sites_L[app.GetComponent<App>().nextSite_index - 1].transform.GetChild(2).GetChild(0).gameObject.SetActive(true);
            sites_L[app.GetComponent<App>().nextSite_index - 1].transform.GetChild(2).GetChild(1).gameObject.SetActive(true);
            sites_L[app.GetComponent<App>().nextSite_index - 1].transform.GetChild(2).GetChild(2).gameObject.SetActive(false);

            sites_P[app.GetComponent<App>().nextSite_index - 1].transform.GetChild(2).GetChild(0).gameObject.SetActive(true);
            sites_P[app.GetComponent<App>().nextSite_index - 1].transform.GetChild(2).GetChild(1).gameObject.SetActive(true);
            sites_P[app.GetComponent<App>().nextSite_index - 1].transform.GetChild(2).GetChild(2).gameObject.SetActive(false);
        }else{
            sites_L[app.GetComponent<App>().nextSite_index - 1].transform.GetChild(2).GetChild(0).gameObject.SetActive(true);

            sites_P[app.GetComponent<App>().nextSite_index - 1].transform.GetChild(2).GetChild(0).gameObject.SetActive(true);
        }
    }

    public void restart(){
        score = maxScore;
        for (int i = 0; i < 4; i++){
            sites_L[i].transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Liters lost: 0";
            sites_P[i].transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Liters lost: 0";
            scoreLost[i] = 0;
            if(i > 0){
                sites_L[i].transform.GetChild(2).GetChild(0).gameObject.SetActive(false);
                sites_L[i].transform.GetChild(2).GetChild(1).gameObject.SetActive(false);
                sites_L[i].transform.GetChild(2).GetChild(2).gameObject.SetActive(true);
                sites_L[i].transform.GetChild(2).GetChild(3).gameObject.SetActive(false);

                sites_P[i].transform.GetChild(2).GetChild(0).gameObject.SetActive(false);
                sites_P[i].transform.GetChild(2).GetChild(1).gameObject.SetActive(false);
                sites_P[i].transform.GetChild(2).GetChild(2).gameObject.SetActive(true);
                sites_P[i].transform.GetChild(2).GetChild(3).gameObject.SetActive(false);

                sites_L[i].transform.GetChild(2).GetChild(2).GetComponent<TextMeshProUGUI>().text = "Not visited yet";
                sites_P[i].transform.GetChild(2).GetChild(2).GetComponent<TextMeshProUGUI>().text = "Not visited yet";
            }
        }
        sites_L[0].transform.GetChild(2).GetChild(0).gameObject.SetActive(true);
        sites_P[0].transform.GetChild(2).GetChild(0).gameObject.SetActive(true);
    }

    public void updateStatus(int lost, bool gotComponents){
        int currentSite_index = app.GetComponent<App>().nextSite_index - 1;
        scoreLost[currentSite_index] += lost;
        scoreText_L.text = "Your current score: " + score.ToString() + " liters";
        scoreText_P.text = "Your current score:\n" + score.ToString() + " liters";
        sites_L[currentSite_index].transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Liters lost: " + scoreLost[currentSite_index].ToString();
        sites_P[currentSite_index].transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Liters lost: " + scoreLost[currentSite_index].ToString();

        if(gotComponents){
            sites_L[currentSite_index].transform.GetChild(2).GetChild(3).gameObject.SetActive(true);
            sites_P[currentSite_index].transform.GetChild(2).GetChild(3).gameObject.SetActive(true);
        }
    }

}
