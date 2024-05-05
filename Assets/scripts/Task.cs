using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Task {
    [SerializeField]
    public string name;
    [SerializeField]
    public bool done;
    [SerializeField]
    public bool SubTasks;
    public bool FromSubTasks;
    public List<Task> subtasksList = new List<Task>();
    public enum Type {today,daily,week,month,year,subtask,TODO,backlog};
    public Type tipo;
    public enum Flag {red,blue,yellow}
    public Flag priority;
    public DateTime DelieveryDay;
    public bool taskWithTime;
    public List<string> tags=new List<string>();
}
