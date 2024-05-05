using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class TaskWithTime : Task
{

    public float currentTimer;
    public float MaxTimer;
    // Use this for initialization
    void Update() {
        Timer();
    }
    public float Timer() {
        currentTimer += Time.deltaTime;
        return currentTimer;

    }
    public void AddTimer(float t) {
        currentTimer += t;
    }

}
