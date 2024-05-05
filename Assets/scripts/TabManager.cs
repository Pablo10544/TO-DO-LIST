using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabManager : MonoBehaviour {

    public List<Task> taskList = new List<Task>();
    public List<TaskWithTime> taskListTime = new List<TaskWithTime>();
    public Vector3 LasPos;
    public List<GameObject> currrentTimer = new List<GameObject>();

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
