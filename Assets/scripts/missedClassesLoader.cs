using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class missedClassesLoader : MonoBehaviour
{
    public MissedClasses msc;
    public Text t;
    public Text tName;
    public ToDoListManager tdlm;
    // Start is called before the first frame update
    void Start()
    {
        //MissedClasses msc = new MissedClasses();
        if (!tdlm.MCW.missedClassesList.Contains(msc))
            tdlm.MCW.missedClassesList.Add(msc);
        msc.name = tName.text;
        if (msc.count == 0) {
            //make button not interactive
        }
        
    }
    public void Add() {
        msc.count++;
        tdlm.MissedClassesSave();
        refreashUI();
    }
    public void Subtract(){
        if (msc.count > 0)
        {
            msc.count--;
            tdlm.MissedClassesSave();
            refreashUI();
        }
    }
    public void refreashUI() {
        t.text = msc.count.ToString();
        
    }
    // Update is called once per frame

}
