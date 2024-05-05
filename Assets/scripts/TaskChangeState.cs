using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TaskChangeState : MonoBehaviour {

    ToDoListManager tdlm;
    public Task t;
    public GameObject gm;
    public float tim;
    public bool timer;
    public Text txt;
    TimeManager tm;
    public Image FlagBackground;
    public Text DateAndTime;
    public GameObject ImageTime;
    public void Remove() {
        gm.transform.DOPunchPosition(new Vector3(1,-0.1f,0)*50,0.4f,10,0f);
       gm.transform.DOPunchScale(new Vector3(0.75f,0.75f,0.75f),0.15f);
        if (t.tags.Contains("str"))
        {
            tdlm.att.AddStrenght();
         }
        if (t.tags.Contains("ment"))
        {
            tdlm.att.AddMental();
        }
        if (t.tags.Contains("lern"))
        {
            tdlm.att.AddLearn();
        }
        if (t.tags.Contains("soc"))
        {
            tdlm.att.AddSocial();
        }
        if (t.tags.Contains("special"))
        {
            tdlm.att.AddSpecial();
        }
        if (t.SubTasks)
        {
            return;
        }
        if (t is TaskWithTime)
        {
            timer = !timer;
           
            switch (t.tipo)
            {
                case Task.Type.today:
                    if (tdlm.tabMngr[0].currrentTimer.Contains(gm))
                    {
                        tdlm.tabMngr[0].currrentTimer.Remove(gm);
                    }else
                    tdlm.tabMngr[0].currrentTimer.Add(gm);
                    break;
                case Task.Type.daily:
                    if (tdlm.tabMngr[1].currrentTimer.Contains(gm))
                    {
                        tdlm.tabMngr[1].currrentTimer.Remove(gm);
                    }
                    else
                        tdlm.tabMngr[1].currrentTimer.Add(gm);
                    break;
                case Task.Type.week:
                    if (tdlm.tabMngr[2].currrentTimer.Contains(gm))
                    {
                        tdlm.tabMngr[2].currrentTimer.Remove(gm);
                    }
                    else
                        tdlm.tabMngr[2].currrentTimer.Add(gm);
                    break;
                case Task.Type.month:
                    if (tdlm.tabMngr[3].currrentTimer.Contains(gm))
                    {
                        tdlm.tabMngr[3].currrentTimer.Remove(gm);
                    }
                    else
                        tdlm.tabMngr[3].currrentTimer.Add(gm);
                    break;
                case Task.Type.year:
                    if (tdlm.tabMngr[4].currrentTimer.Contains(gm))
                    {
                        tdlm.tabMngr[4].currrentTimer.Remove(gm);
                    }
                    else
                        tdlm.tabMngr[4].currrentTimer.Add(gm);
                    break;
                case Task.Type.subtask:
                 /*  if (tdlm.tabMngr[5].currrentTimer.Contains(gm))
                    {
                        tdlm.tabMngr[5].currrentTimer.Remove(gm);
                    }
                    else
                        tdlm.tabMngr[5].currrentTimer.Add(gm);*/
                    break;
                case Task.Type.TODO:
                    if (tdlm.tabMngr[5].currrentTimer.Contains(gm))
                    {
                        tdlm.tabMngr[5].currrentTimer.Remove(gm);
                    }
                    else
                        tdlm.tabMngr[5].currrentTimer.Add(gm);
                    break;
            }
            tdlm.Saving();
           
        }
        else
        {
            if (t.FromSubTasks)
            {
                if (!t.done)
                {
                    tdlm.GetComponent<AudioSource>().Play();
                    //fazer funcionar
                    print(gm.transform.parent.GetComponent<TaskChangeState>().transform.gameObject.name);
                    print(gm.transform.parent.GetComponent<TaskChangeState>().t.tipo);
                    switch (gm.transform.parent.GetComponent<TaskChangeState>().t.tipo)
                    {
                        case Task.Type.today:
                            
                            int i = tdlm.t.FindIndex(x => x.subtasksList.Find(y => y == t) == t);

                            tdlm.t[i].subtasksList.Find(y => y == t).done = true;
                            if (tdlm.t[i].subtasksList.All(c => c.done == true))
                            {
                                gm.transform.parent.GetComponentInChildren<Toggle>().isOn = true;
                                if (!gm.transform.parent.GetComponent<TaskChangeState>().t.name.Contains("test")) {

                                    tdlm.completed.Add(gm.transform.parent.GetComponent<TaskChangeState>().t);
                                    tdlm.SavingTasksDone();

                                }


                                tdlm.t.RemoveAt(i);
                            }
                            break;
                        case Task.Type.daily:
                           
                            int u = tdlm.tabMngr[1].taskList.FindIndex(x => x.subtasksList.Find(y => y == t) == t);

                            tdlm.tabMngr[1].taskList[u].subtasksList.Find(y => y == t).done = true;
                            if (tdlm.tabMngr[1].taskList[u].subtasksList.All(c => c.done == true))
                            {
                                gm.transform.parent.GetComponentInChildren<Toggle>().isOn = true;
                                gm.transform.parent.GetComponent<TaskChangeState>().t.DelieveryDay = new System.DateTime(System.DateTime.Now.Date.Year, System.DateTime.Now.Date.Month, System.DateTime.Now.Date.Day, t.DelieveryDay.Hour, t.DelieveryDay.Minute, 0);
                                tdlm.notification.sendNotifications(t.name, t.name, t.DelieveryDay);

                                /*  if (!gm.transform.parent.GetComponent<TaskChangeState>().t.name.Contains("test"))
                                  {

                                      tdlm.completed.Add(gm.transform.parent.GetComponent<TaskChangeState>().t);
                                      tdlm.SavingTasksDone();

                                  }
                                  tdlm.tabMngr[1].taskList.RemoveAt(u);*/
                                gm.transform.parent.GetComponent<TaskChangeState>().t.done = true;
                            }
                            break;
                        case Task.Type.week:
                            int o = tdlm.tabMngr[2].taskList.FindIndex(x => x.subtasksList.Find(y => y == t) == t);

                            tdlm.tabMngr[2].taskList[o].subtasksList.Find(y => y == t).done = true;
                            if (tdlm.tabMngr[2].taskList[o].subtasksList.All(c => c.done == true))
                            {
                                gm.transform.parent.GetComponentInChildren<Toggle>().isOn = true;
                                if (!gm.transform.parent.GetComponent<TaskChangeState>().t.name.Contains("test"))
                                {

                                    tdlm.completed.Add(gm.transform.parent.GetComponent<TaskChangeState>().t);
                                    tdlm.SavingTasksDone();

                                }
                                tdlm.tabMngr[2].taskList.RemoveAt(o);
                            }
                            break;
                        case Task.Type.month:
                            int p = tdlm.tabMngr[3].taskList.FindIndex(x => x.subtasksList.Find(y => y == t) == t);

                            tdlm.tabMngr[3].taskList[p].subtasksList.Find(y => y == t).done = true;
                            if (tdlm.tabMngr[3].taskList[p].subtasksList.All(c => c.done == true))
                            {
                                gm.transform.parent.GetComponentInChildren<Toggle>().isOn = true;
                                if (!gm.transform.parent.GetComponent<TaskChangeState>().t.name.Contains("test"))
                                {

                                    tdlm.completed.Add(gm.transform.parent.GetComponent<TaskChangeState>().t);
                                    tdlm.SavingTasksDone();

                                }
                                tdlm.tabMngr[3].taskList.RemoveAt(p);
                            }
                            break;
                        case Task.Type.year:
                            int z = tdlm.tabMngr[4].taskList.FindIndex(x => x.subtasksList.Find(y => y == t) == t);

                            tdlm.tabMngr[4].taskList[z].subtasksList.Find(y => y == t).done = true;
                            if (tdlm.tabMngr[4].taskList[z].subtasksList.All(c => c.done == true))
                            {
                                gm.transform.parent.GetComponentInChildren<Toggle>().isOn = true;

                                if (!gm.transform.parent.GetComponent<TaskChangeState>().t.name.Contains("test"))
                                {

                                    tdlm.completed.Add(gm.transform.parent.GetComponent<TaskChangeState>().t);
                                    tdlm.SavingTasksDone();

                                }
                                tdlm.tabMngr[4].taskList.RemoveAt(z);
                            }
                            break;
                        case Task.Type.TODO:
                            int b = tdlm.tabMngr[5].taskList.FindIndex(x => x.subtasksList.Find(y => y == t) == t);

                            tdlm.tabMngr[5].taskList[b].subtasksList.Find(y => y == t).done = true;
                            if (tdlm.tabMngr[5].taskList[b].subtasksList.All(c => c.done == true))
                            {
                                gm.transform.parent.GetComponentInChildren<Toggle>().isOn = true;

                                if (!gm.transform.parent.GetComponent<TaskChangeState>().t.name.Contains("test"))
                                {

                                    tdlm.completed.Add(gm.transform.parent.GetComponent<TaskChangeState>().t);
                                    tdlm.SavingTasksDone();

                                }
                                tdlm.tabMngr[5].taskList.RemoveAt(b);
                            }
                            break;
                        default:
                            break;
                    }
                    
                    tdlm.Saving();
                }

            }
            else
            {
                tdlm.GetComponent<AudioSource>().Play();
                if (!t.name.Contains("test"))
                {
                tdlm.completed.Add(t);
                    tdlm.SavingTasksDone();


                }
                switch (t.tipo)
                {
                    case Task.Type.today:                    
                            tdlm.t.Remove(t);
                            tdlm.tabMngr[0].taskList.Remove(t);
                        
                        break;
                    case Task.Type.daily:
                        t.DelieveryDay = new System.DateTime( System.DateTime.Now.Date.Year, System.DateTime.Now.Date.Month, System.DateTime.Now.Date.Day,t.DelieveryDay.Hour,t.DelieveryDay.Minute,0);
                        tdlm.notification.sendNotifications(t.name, t.name, t.DelieveryDay.AddDays(1));
                        break;
                    case Task.Type.week:
                        if (t.tags.Contains("rpt1w"))
                        {
                            t.DelieveryDay = new System.DateTime(System.DateTime.Now.Date.Year, System.DateTime.Now.Date.Month, System.DateTime.Now.Date.Day, t.DelieveryDay.Hour, t.DelieveryDay.Minute, 0).AddDays(7);
                        }
                        else
                        {
                            tdlm.tabMngr[2].taskList.Remove(t);
                        }
                        break;
                    case Task.Type.month:
                        tdlm.tabMngr[3].taskList.Remove(t);
                        break;
                    case Task.Type.year:
                        tdlm.tabMngr[4].taskList.Remove(t);
                        break;
                    case Task.Type.subtask:
                      //  tdlm.tabMngr[5].taskList.Remove(t);
                        break;
                    case Task.Type.TODO:
                        tdlm.tabMngr[5].taskList.Remove(t);
                        break;

                }
                tdlm.Saving();

            }
            //  tdlm.Saving();
        }
    }
    void Start() {
        tm = GameObject.FindObjectOfType<TimeManager>();
        tdlm = GameObject.FindObjectOfType<ToDoListManager>();
        gm = this.gameObject;
        if (t is TaskWithTime)
        {
            TaskWithTime twt = new TaskWithTime();
            twt = (TaskWithTime)t;
           
            txt.text = twt.currentTimer.ToString("00")+" segundos /"+(twt.MaxTimer/60)+" minutos";

        }
        if (t.SubTasks)
        {
            gm.GetComponentInChildren<Toggle>().interactable = false;
        }
    }
    void Update() {
        if (timer)
        {          
            TaskWithTime twt = new TaskWithTime();
            twt = (TaskWithTime)t;
           tim= twt.currentTimer;
            txt.text = twt.currentTimer.ToString("00") + " segundos /" + (twt.MaxTimer / 60) + " minutos";
            if (twt.MaxTimer<twt.Timer())
            {
                if (!twt.name.Contains("test")&&tdlm.twt.Contains((TaskWithTime)t))
                {
                    tdlm.completed.Add(t);
                    tdlm.SavingTasksDone();

                }
                tdlm = GameObject.FindObjectOfType<ToDoListManager>();
                tdlm.twt.Remove((TaskWithTime)t);
                tdlm.Saving();
            }
        }
    }
    public void clicking()
    {
        if (tdlm.editing)
        {
            tdlm.TaskEdit.SetActive(true);
            tdlm.PopulatingTaskEdit(t);
        }
    }
}
