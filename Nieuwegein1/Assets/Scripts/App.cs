using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class App : MonoBehaviour
{
    public GameObject calibrationCanvas, site1, site1UI, site2, site2UI, path1, path2, path3, path4, site3,
    site3UI, site4, site4UI, site5, site5UI, Arrow;
    public Button SavePoint;
    public Camera arCamera;
    int count;
    public GameObject savePointCanvas;

    void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        // calibrationCanvas.SetActive(true);

        Arrow.SetActive(false);
        site1.SetActive(false);
        site1UI.SetActive(false);
        site2.SetActive(false);
        site2UI.SetActive(false);
        site3.SetActive(false);
        site3UI.SetActive(false);
        site4.SetActive(false);
        site4UI.SetActive(false);
        site5.SetActive(false);
        site5UI.SetActive(false);
        path1.SetActive(false);
        path2.SetActive(false);
        path3.SetActive(false);
        path4.SetActive(false);
        SavePoint.onClick.AddListener(savePoint);
        savePointCanvas.SetActive(false);

        count=0;

        site1.SetActive(false);
        site1UI.SetActive(false);
        site3.SetActive(true);
        site3UI.SetActive(true);
        path2.GetComponent<Path>().arrived=true;
    }

    private void savePoint(){
        Debug.Log(count+". "+arCamera.transform.position);
        count++;
    }
}
