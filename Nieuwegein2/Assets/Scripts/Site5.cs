using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Site5 : MonoBehaviour
{
    public GameObject pond_prefab, thankYouPanel, gameController;
    public Camera arCamera;
    public TextMeshProUGUI score_text_L, score_text_P;

    GameObject pond;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void startSite(){
        score_text_L.text = "Your final score is:\n" + gameController.GetComponent<gameController>().score.ToString() + " liters of water\n\nTo replay, please return to your starting location.";
        score_text_P.text = score_text_L.text;
        thankYouPanel.SetActive(true);
        pond = Instantiate(pond_prefab);
        pond.transform.SetParent(transform, true);
        pond.SetActive(true);
        Vector3 dir = arCamera.transform.forward;
        pond.transform.position = transform.position + Vector3.Normalize(new Vector3(dir.x,0f,dir.z))*0.5f + new Vector3(0f, 0f, 0f);
    }
}
