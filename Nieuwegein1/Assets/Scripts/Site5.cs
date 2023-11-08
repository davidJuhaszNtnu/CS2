using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Site5 : MonoBehaviour
{
    public GameObject pond, site5, path4;
    bool bool1;
    public bool showSite;
    // Start is called before the first frame update
    void Start()
    {
        bool1=true;
        showSite=false;
        pond.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // if(showSite && bool1){
        if(path4.GetComponent<Path>().arrived && bool1){
            bool1=false;
            pond.SetActive(true);
            pond.transform.position=site5.transform.position+(new Vector3(0f,-1f,0.5f));
            path4.GetComponent<Path>().Arrow.SetActive(false);
        }
    }
}
