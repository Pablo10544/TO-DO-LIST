using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour {
    bool startTimer;
    public ToDoListManager TDLM;
    public DateTime currentTimer;
    bool changetime;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        foreach (TabManager tbmanger in TDLM.tabMngr)
        {
            if ((!tbmanger.gameObject.activeInHierarchy && tbmanger.currrentTimer.Count != 0))
            {
                foreach (GameObject G in tbmanger.currrentTimer)
                {
                    TaskWithTime temp = (TaskWithTime)G.GetComponent<TaskChangeState>().t;
                    temp.Timer();///
                    if (changetime && currentTimer.Year!=0) {
                        temp.currentTimer+= (float)(DateTime.Now - currentTimer).TotalSeconds;
                        currentTimer = DateTime.Now;
                    }
                    startTimer = true;///
                }
                
            }
            
        }
        
          
        
    }
    private void OnApplicationPause(bool pause)
    {///
        if (pause) {
            currentTimer = DateTime.Now;
            changetime = true;
        }
        
    }
    private void OnApplicationFocus(bool focus)
    {
        if (focus&&changetime) {
            foreach (TabManager tbmanger in TDLM.tabMngr)
            {              
                    foreach (GameObject G in tbmanger.currrentTimer)
                    {
                        TaskWithTime temp = (TaskWithTime)G.GetComponent<TaskChangeState>().t;                        
                        if (changetime && currentTimer.Year != 0)
                        {
                            temp.AddTimer((float)(DateTime.Now - currentTimer).TotalSeconds);
                            currentTimer = new DateTime(0,0,0,0,0,0);
                        }
                        startTimer = true;///
                    }                

            }

            changetime = false;
        }
    }
    public void KeepTime()
    {
        startTimer = true;
    }
}
