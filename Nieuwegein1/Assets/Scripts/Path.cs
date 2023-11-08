using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour
{
    public GameObject Arrow;
    public GameObject this_path, nextSite, nextSiteUI;
    public Camera arCamera;
    public Vector3[] points;
    float[] t_i;
    public GameObject Line;
    private LineRenderer lr;
    public float t;
    float L;
    public bool arrived;

    void Start()
    {
        lr = Line.GetComponent<LineRenderer>();
        points=new Vector3[lr.positionCount];
        t_i=new float[lr.positionCount];
        lr.GetPositions(points);
        L=0f;
        t=0f;
        t_i[0]=0f;
        for (int i = 0; i < points.Length-1; i++){
            L+=Vector3.Distance(points[i],points[i+1]);
            t_i[i+1]=t_i[i]+Vector3.Distance(points[i],points[i+1]);
        }
        arrived=false;
        Arrow.transform.position=path(0f);
    }

    // Update is called once per frame
    void Update()
    {   
        float d=Vector2.Distance(new Vector2(arCamera.transform.position.x,arCamera.transform.position.z),new Vector2(Arrow.transform.position.x,Arrow.transform.position.z));
        if(d<0.5f){
            if(t<=L){
                Arrow.transform.position=path(t);
                Arrow.transform.rotation=Quaternion.LookRotation(pathDer(t), Vector3.up);
                t+=0.02f;
            }else if(t>L){
                arrived=true;
                nextSite.SetActive(true);
                nextSiteUI.SetActive(true);
                this_path.SetActive(false);
            }
        }
    }

    public Vector3 path(float s){
        for (int i = 0; i < lr.positionCount-1; i++){
            if(t_i[i]<=s && s<=t_i[i+1]){
                return (points[i+1]-points[i])/(t_i[i+1]-t_i[i])*(s-t_i[i])+points[i];
            }
        }
        return Vector3.zero;
    } 

    public Vector3 pathDer(float s){
        for (int i = 0; i < lr.positionCount-1; i++){
            if(t_i[i]<=s && s<=t_i[i+1]){
                return (points[i+1]-points[i])/(t_i[i+1]-t_i[i]);
            }
        }
        return Vector3.zero;
    } 
}
