using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MyList{

    // Use this for initialization
    [SerializeField]
    public List<Task> listToday;
    public List<TaskWithTime> list2Today;
    public List<Task> listDaily;
    public List<TaskWithTime> list2Daily;
    public List<Task> listWeek;
    public List<TaskWithTime> list2Week;
    public List<Task> listMonth;
    public List<TaskWithTime> list2Month;
    public List<Task> listYear;
    public List<TaskWithTime> list2Year;
    public List<Task> listTODO;
    public List<TaskWithTime> list2TODO;
    public List<Task> listBacklog;
    public List<TaskWithTime> list2Backlog;

}
