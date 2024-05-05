using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Achievement")]
public class Achievements : ScriptableObject {
    public string name;
    public string description;
    public bool achieved;
    public Image icon;
    public TaskWithTime taskAssociated;
    public string Key;
    public enum Type {
        OverXVariable
    }
    public Type t;
    public float Surpass;
    


    public void OverTime() {
        
        if (taskAssociated.currentTimer>Surpass)
        {
            achieved = true;

        }
    }
   
}
