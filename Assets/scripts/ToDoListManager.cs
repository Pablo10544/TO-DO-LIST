using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using System.Linq;

public class ToDoListManager : MonoBehaviour {
    [SerializeField]
    public List<Task> t = new List<Task>();
    public List<TaskWithTime> twt = new List<TaskWithTime>();
    public List<TabManager> tabMngr = new List<TabManager>();
    public List<Task> completed;
    public List<GameObject> Tabs;
    public GameObject TaskGameobject, TaskGameobjectWithTime;
    public Transform Canvas;
    public InputField If, IfSubtask, ifDate1, ifDate2, ifDate3, IfDateTime1, IfDateTime2;
    public Vector3 LastPos;
    public bool TImer;
    public GameObject btn1, btn2, btn3, drop, btn4, SaveCanvas, Tasksgameobject, subtaskif, subtaskifParent, DateIfParent, toggleDate;//ver variaveis inuteis
    MyList m;
    public AchievementManager am;
    public Toggle SubtasksToggle;
    public int spacing;
    public int verticalSpacing;
    public float verticalSpacingBacklog;
    public Config c;
    public Text Day, time;
    public Dropdown dpdaily;
    public Dropdown dpPriorityEdit;
    public GameObject currentTab;
    public GameObject Spawn;
    public GameObject currentTabSelected;
    public TimeManager tm;
    public Dropdown dropdownPriority;
    public Dropdown dropAMPM;
    public bool editing;
    public Task TaskBeingEdited;
    public Notification notification;
    public Text Header;
    public Image Background;
    public Text phrase;
    public InputField IfEditTask;
    public Toggle TimetaskEdit, SubtaskEdit, DateEdit;
    public GameObject TaskEdit;
    public GameObject DropEditTime, ifDateEdit, toggleDateEdit, subtaskifParentEdit, subtaskifedit;
    public InputField IfDateEdit1, IfDateEdit2, IfDateEdit3, IfTimeEdit1, IftimeEdit2, IfSubtaskEdit;
    public Dropdown tipoEdit;
    public MissedClassesWrapper MCW;
    public List<missedClassesLoader> listmcl;
    public GameObject ToggleTag;
    public GameObject InputFieldTag;
    public InputField IfDate1, IfDate2, IfDate3, IfTime1, Iftime2;
    public SeeAll seeAll;
    public GameObject SeeAllg;
    public Button SeeAllButton;
    public GameObject EditButton;
    public TasksDone td;
    public Atributtes att;
    public Image DayOfTheWeek;
    // public Text test;
    void Start() {
        StartCoroutine("startweektext");
        GameObject temp = GameObject.FindGameObjectWithTag("SeeAll");
        if (temp != null && temp != SeeAllg)
        {
            seeAll = temp.GetComponent<SeeAll>();
            Destroy(SeeAllg);
        }
        // showAll = true;      
        m = new MyList();
        MissedClassesFill();
        spacing = -35;
        c.OpeningConfig();
        Opening();       
        SortList();      
        ShowTasksLoad();
        BegginingFadeText();
        OpeningTasksDone();
        td.LoadTasksDone(completed);
        ClearTaskDone(false);
        

        am.PopulateScriptableObjects();
        if (seeAll.showAll == true) {
            SeeAllButton.image.color = new Color32(247, 143, 179, 255);
            print(SeeAllButton.gameObject.GetComponentInChildren<Image>().name);
            SeeAllButton.gameObject.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color32(247, 143, 179, 255);
        }
        if (currentTabSelected == null)
        {
            currentTabSelected = tabMngr[0].gameObject;
        }


    }

    void Update() {
        //  test.text = "width: "+Screen.width +"height: "+ Screen.height;
        if (Input.GetKey(KeyCode.R) && Input.GetKey(KeyCode.LeftShift))
        {
            RestartScene();
        }
    }

    public void ShowTasksLoad()
    {
        foreach (TabManager t in tabMngr)
        {
            foreach (TaskWithTime taswtime in t.taskListTime)
            {
                //task with time
                ShowTaskWithTime(taswtime);

            }
            foreach (Task tas in t.taskList)
            {
                ShowNormalTask(tas);
            }
        }
    }
    public void ShowNormalTask(Task tt) {
        GameObject ob = Instantiate(TaskGameobject);
        SwitchType(tt, ob);
        if (tt.SubTasks && tt.DelieveryDay.Date != System.DateTime.Now.Date)
        { 
         
            Vector3 LasPosSubTask = LastPos + new Vector3(0, spacing + 10, 0);
            foreach (Task tas in tt.subtasksList)
            {
                if (tt.DelieveryDay != System.DateTime.Now.Date && tt.done && tt.tipo == Task.Type.daily)
                {
                    tas.done = false;

                }

                GameObject temp = Instantiate(TaskGameobject, ob.transform);
                temp.transform.position = LasPosSubTask + new Vector3(10, spacing + 10, 0);
                LasPosSubTask = new Vector3(LastPos.x, temp.transform.position.y, LastPos.z);
                LastPos = new Vector3(LastPos.x, temp.transform.position.y, LastPos.z);
                currentTab.GetComponent<TabManager>().LasPos = LastPos;
                temp.GetComponentInChildren<Text>().text = tas.name;
                temp.GetComponentInChildren<Text>().fontSize = 30;
                temp.GetComponentInChildren<TaskChangeState>().t = tas;
                if (tas.done)
                {
                    temp.GetComponentInChildren<Toggle>().isOn = true;
                }

            }
            if (tt.DelieveryDay != System.DateTime.Now.Date && tt.done)
            {
                tt.done = false;
            }
        }

        //Mudar collors
        switch (tt.priority)
        {
            case Task.Flag.red:
                ob.GetComponent<TaskChangeState>().FlagBackground.color = Color.red;
                break;
            case Task.Flag.blue:
                ob.GetComponent<TaskChangeState>().FlagBackground.color = Color.blue;

                break;
            case Task.Flag.yellow:
                ob.GetComponent<TaskChangeState>().FlagBackground.color = Color.yellow;

                break;
            default:
                break;
        }
        ob.GetComponentInChildren<Text>().text = tt.name;
        ob.GetComponentInChildren<TaskChangeState>().t = tt;
        print(ob.GetComponentInChildren<TaskChangeState>().t.DelieveryDay.ToString());
        if (ob.GetComponentInChildren<TaskChangeState>().t.DelieveryDay.ToString() == "01/01/0001 00:00:00"&& tt.tipo == Task.Type.backlog)
        {                    
                ob.GetComponentInChildren<TaskChangeState>().DateAndTime.text = "";
            
        }
        else if (tt.tipo == Task.Type.daily)
        {
            ob.GetComponentInChildren<TaskChangeState>().DateAndTime.text = tt.DelieveryDay.TimeOfDay.ToString();
            ob.GetComponentInChildren<TaskChangeState>().ImageTime.SetActive(true);
        }
        else if (ob.GetComponentInChildren<TaskChangeState>().t.DelieveryDay.TimeOfDay.ToString() == "00:00:00")
        {
            ob.GetComponentInChildren<TaskChangeState>().DateAndTime.text = tt.DelieveryDay.Date.ToString("dd/MM/yyyy");
        }
        else if (ob.GetComponentInChildren<TaskChangeState>().t.DelieveryDay.ToString() != "1/1/0001 12:00:00 AM")
        {
            ob.GetComponentInChildren<TaskChangeState>().DateAndTime.text = tt.DelieveryDay.Date.ToString("dd/MM/yyyy ") + tt.DelieveryDay.TimeOfDay.ToString();
            print(tt.DelieveryDay);
            ob.GetComponentInChildren<TaskChangeState>().ImageTime.SetActive(true);
        }
        print((tt.DelieveryDay.Date - System.DateTime.Now.Date).TotalDays < 0);
        if (tt.tipo == Task.Type.daily)
        {
            if (tt.DelieveryDay.Date.ToString("dd/MM/yyyy") != System.DateTime.Now.Date.ToString("dd/MM/yyyy"))
            {
                TabButtonBehaviour t1 = Tabs[1].GetComponent<TabButtonBehaviour>();
                t1.WarnCount++;
                t1.textButton.text = (t1.WarnCount).ToString();
                t1.imageButtonWarn.gameObject.SetActive(true);
            }
        } else if (tt.DelieveryDay.Date.ToString("dd/MM/yyyy") == System.DateTime.Now.Date.ToString("dd/MM/yyyy") || (tt.DelieveryDay.Date - System.DateTime.Now.Date).TotalDays < 0)
        {
            ob.GetComponent<TaskChangeState>().DateAndTime.color = new Color32(225, 95, 65, 255);
            switch (tt.tipo)
            {
                case Task.Type.today:
                    TabButtonBehaviour t = Tabs[0].GetComponent<TabButtonBehaviour>();
                    t.WarnCount++;
                    t.textButton.text = (t.WarnCount).ToString();
                    t.imageButtonWarn.gameObject.SetActive(true);
                    break;
                case Task.Type.daily:

                    break;
                case Task.Type.week:
                    TabButtonBehaviour t2 = Tabs[2].GetComponent<TabButtonBehaviour>();
                    t2.WarnCount++;
                    t2.textButton.text = (t2.WarnCount).ToString();
                    t2.imageButtonWarn.gameObject.SetActive(true);
                    break;
                case Task.Type.month:
                    TabButtonBehaviour t3 = Tabs[3].GetComponent<TabButtonBehaviour>();
                    t3.WarnCount++;
                    t3.textButton.text = (t3.WarnCount).ToString();
                    t3.imageButtonWarn.gameObject.SetActive(true);
                    break;
                case Task.Type.year:
                    TabButtonBehaviour t4 = Tabs[4].GetComponent<TabButtonBehaviour>();
                    t4.WarnCount++;
                    t4.textButton.text = (t4.WarnCount).ToString();
                    t4.imageButtonWarn.gameObject.SetActive(true);
                    break;
                case Task.Type.subtask:
                    break;
                case Task.Type.TODO:
                    TabButtonBehaviour t5 = Tabs[5].GetComponent<TabButtonBehaviour>();
                    t5.WarnCount++;
                    t5.textButton.text = (t5.WarnCount).ToString();
                    t5.imageButtonWarn.gameObject.SetActive(true);
                    break;
               default:
                    break;
            }
        }
        if(tt.tipo==Task.Type.backlog)
            Destroy(ob.GetComponentInChildren<Toggle>().gameObject);

        m.listToday = t;

    }
    public void ShowTaskWithTime(Task tt) {

        GameObject ob = Instantiate(TaskGameobjectWithTime);
        SwitchType(tt, ob);
        ob.GetComponent<Text>().text = tt.name;
        ob.GetComponent<TaskChangeState>().t = tt;
        if (tt.tipo == Task.Type.daily)
        {
            ob.GetComponentInChildren<TaskChangeState>().DateAndTime.text = tt.DelieveryDay.TimeOfDay.ToString();
            ob.GetComponentInChildren<TaskChangeState>().ImageTime.SetActive(true);
        }
        else if (ob.GetComponentInChildren<TaskChangeState>().t.DelieveryDay.ToString() != "1/1/0001 12:00:00 AM")
        {
            ob.GetComponentInChildren<TaskChangeState>().DateAndTime.text = tt.DelieveryDay.ToString();
            ob.GetComponentInChildren<TaskChangeState>().ImageTime.SetActive(true);
        }

        switch (tt.priority)
        {
            case Task.Flag.red:
                ob.GetComponent<TaskChangeState>().FlagBackground.color = Color.red;
                break;
            case Task.Flag.blue:
                ob.GetComponent<TaskChangeState>().FlagBackground.color = Color.blue;

                break;
            case Task.Flag.yellow:
                ob.GetComponent<TaskChangeState>().FlagBackground.color = Color.yellow;

                break;
            default:
                break;
        }
        m.list2Today = twt;



    }
    public void BegginingFadeText() {
        var cult = new System.Globalization.CultureInfo("pt-BR");
        time.text = System.DateTime.Now.TimeOfDay.Hours + ":" + System.DateTime.Now.TimeOfDay.Minutes;
        string dayUpper = cult.DateTimeFormat.GetDayName(System.DateTime.Now.DayOfWeek).Replace("-feira", "");
        dayUpper= char.ToUpper(dayUpper[0])+dayUpper.Substring(1);
        Day.text = dayUpper;
        phrase.text = Phrases.phrases[UnityEngine.Random.Range(0,Phrases.phrases.Count-1)];
    }
    public void ShowNoTime() {
        btn1.SetActive(false);
        SaveCanvas.SetActive(true);

    }
    public void SubTasks() {
        IfSubtask.gameObject.SetActive(!IfSubtask.gameObject.activeInHierarchy);
    }
    public void SubTasksEdit() {
        IfSubtaskEdit.gameObject.SetActive(!IfSubtaskEdit.gameObject.activeInHierarchy);

    }
    public void ShowSubTasksEdit(InputField ifTask) {

        if (int.Parse(ifTask.text) > 0)
        {
            subtaskifParentEdit.SetActive(true);

        }

        for (int i = 0; i < 8; i++)
        {
            if (int.Parse(ifTask.text) >= i + 1)
            {
                subtaskifedit.gameObject.transform.GetChild(i).gameObject.SetActive(true);

            }
            else
            {
                subtaskifedit.gameObject.transform.GetChild(i).gameObject.SetActive(false);

            }
        }
    }
    public void ShowSubTasks(InputField ifTask) {
        subtaskifParent.SetActive(true);

        for (int i = 0; i < 8; i++)
        {
            if (int.Parse(ifTask.text) >= i + 1)
            {
                subtaskif.gameObject.transform.GetChild(i).gameObject.SetActive(true);

            }
            else {
                subtaskif.gameObject.transform.GetChild(i).gameObject.SetActive(false);

            }
        }
    }
    public void UnshowTask() {
        btn1.SetActive(true);
        SaveCanvas.SetActive(false);
    }
    public void UnshowTaskEdit()
    {
        TaskEdit.SetActive(false);
    }
    public void ShowWithTimeEdit() {
        DropEditTime.SetActive(!DropEditTime.activeInHierarchy);
    }
    public void ShowWithTime() {
        drop.SetActive(!drop.activeInHierarchy);
        TImer = !TImer;
    }
    public void CreateTask()
    {
        if ((If.text != "" && !(InputFieldTag.GetComponent<InputField>().text.Contains("otd"))) || ((InputFieldTag.GetComponent<InputField>().text.Contains("otd") && !toggleDate.activeInHierarchy && If.text != "")))
        {
            // if (!InputFieldTag.GetComponent<InputField>().text.Contains("rpt1w"))
            // {
            if (TImer)
            {
                TaskWithTime newTask = new TaskWithTime();
                newTask.name = If.text;
                newTask.taskWithTime = true;
                newTask.MaxTimer = int.Parse(drop.GetComponent<Dropdown>().captionText.text);

                if (toggleDate.activeInHierarchy == false)
                {
                    newTask.DelieveryDay = new DateTime(Int32.Parse(ifDate1.text), Int32.Parse(ifDate2.text), Int32.Parse(ifDate3.text), Int32.Parse(IfDateTime1.text), Int32.Parse(IfDateTime2.text), 0);
                }
                notification.sendNotifications(newTask.name, newTask.name, newTask.DelieveryDay);

                //
                switch (dropdownPriority.value)
                {
                    case 0:
                        newTask.priority = Task.Flag.red;
                        break;
                    case 1:
                        newTask.priority = Task.Flag.blue;
                        break;
                    case 2:
                        newTask.priority = Task.Flag.yellow;
                        break;

                }
                switch (dpdaily.value)
                {
                    case 0:
                        newTask.tipo = Task.Type.today;
                        twt.Add(newTask);
                        break;
                    case 1:
                        newTask.tipo = Task.Type.daily;
                        tabMngr[1].taskListTime.Add(newTask);
                        break;
                    case 2:
                        newTask.tipo = Task.Type.week;
                        tabMngr[2].taskListTime.Add(newTask);
                        break;
                    case 3:
                        tabMngr[3].taskListTime.Add(newTask);
                        newTask.tipo = Task.Type.month;
                        break;
                    case 4:
                        tabMngr[4].taskListTime.Add(newTask);
                        newTask.tipo = Task.Type.year;
                        break;
                    case 5:
                        tabMngr[5].taskListTime.Add(newTask);
                        newTask.tipo = Task.Type.TODO;
                        break;
                   
                }
                if (ToggleTag.GetComponent<Toggle>().isOn)
                {
                    newTask.tags.Add(InputFieldTag.GetComponent<InputField>().text);
                    newTask.name += " [" + newTask.tags[0] + "]";
                }
                ShowTaskWithTime(newTask);
                Saving();
            }
            else
            {
                Task newTask = new Task();
                newTask.name = If.text;

                //
                if (toggleDate.activeInHierarchy == false)
                {
                    newTask.DelieveryDay = new DateTime(Int32.Parse(ifDate1.text), Int32.Parse(ifDate2.text), Int32.Parse(ifDate3.text), Int32.Parse(IfDateTime1.text), Int32.Parse(IfDateTime2.text), 0);
                }
                notification.sendNotifications(newTask.name, newTask.name, newTask.DelieveryDay);

                //
                switch (dropdownPriority.value)
                {
                    case 0:
                        newTask.priority = Task.Flag.red;
                        break;
                    case 1:
                        newTask.priority = Task.Flag.blue;
                        break;
                    case 2:
                        newTask.priority = Task.Flag.yellow;
                        break;

                }
                switch (dpdaily.value)
                {
                    case 0:
                        newTask.tipo = Task.Type.today;
                        tabMngr[0].taskList.Add(newTask);
                        break;
                    case 1:
                        newTask.tipo = Task.Type.daily;
                        tabMngr[1].taskList.Add(newTask);
                        break;
                    case 2:
                        tabMngr[2].taskList.Add(newTask);
                        newTask.tipo = Task.Type.week;
                        break;
                    case 3:
                        tabMngr[3].taskList.Add(newTask);
                        newTask.tipo = Task.Type.month;
                        break;
                    case 4:
                        tabMngr[4].taskList.Add(newTask);
                        newTask.tipo = Task.Type.year;
                        break;
                    case 5:
                        tabMngr[5].taskList.Add(newTask);
                        newTask.tipo = Task.Type.TODO;
                        break;
                    case 6:
                        tabMngr[6].taskList.Add(newTask);
                        newTask.tipo = Task.Type.backlog;
                        break;
                }
                if (SubtasksToggle.isOn)
                {
                    newTask.SubTasks = true;
                    foreach (Transform t in subtaskif.gameObject.transform)
                    {
                        if (t.gameObject.activeInHierarchy)
                        {
                            Task newSubtask = new Task();
                            newSubtask.tipo = Task.Type.subtask;
                            newSubtask.name = t.GetComponentInChildren<InputField>().text;
                            newSubtask.FromSubTasks = true;
                            newTask.subtasksList.Add(newSubtask);
                        }
                    }
                }
                if (InputFieldTag.activeInHierarchy)
                {
                    newTask.tags.Add(InputFieldTag.GetComponent<InputField>().text);
                    newTask.name += " [" + newTask.tags[0] + "]";
                }
                
                ShowNormalTask(newTask);
                Saving();
            }


            // }
        }
    }
    public void LoadTabContent(GameObject tab) {
        if (currentTabSelected == null)
        {
            currentTabSelected = tabMngr[0].gameObject;
        }
        currentTabSelected.SetActive(false);
        tab.SetActive(true);
        currentTabSelected = tab;
        //change this to a UI script
        if (currentTabSelected == tabMngr[3].gameObject)
        {
            Header.text = "month";
            Header.fontSize = 21;
            Background.color = new Color32(196, 69, 105, 255);
            for (int i = 0; i < Tabs.Count; i++)
            {
                Tabs[i].GetComponent<Image>().color = new Color32(196, 69, 105, 255);
            }

        }
        else if (currentTabSelected == tabMngr[1].gameObject)
        {
            Header.text = "Daily";
            Header.fontSize = 21;
            Background.color = new Color32(99, 205, 218, 255);
            for (int i = 0; i < Tabs.Count; i++)
            {
                Tabs[i].GetComponent<Image>().color = new Color32(99, 205, 218, 255);
            }
        }
        else if (currentTabSelected == tabMngr[2].gameObject)
        {
            Header.text = "Week";
            Header.fontSize = 21;
            Background.color = new Color32(207, 106, 135, 255);
            for (int i = 0; i < Tabs.Count; i++)
            {
                Tabs[i].GetComponent<Image>().color = new Color32(207, 106, 135, 255);
            }

        }
        else if (currentTabSelected == tabMngr[4].gameObject)
        {
            Header.text = "Year";
            Header.fontSize = 21;
            Background.color = new Color32(120, 111, 116, 255);
            for (int i = 0; i < Tabs.Count; i++)
            {
                Tabs[i].GetComponent<Image>().color = new Color32(120, 111, 116, 255);
            }
        }
        else if (currentTabSelected == tabMngr[0].gameObject)
        {
            Header.text = "Today";
            Header.fontSize = 21;
            Background.color = new Color32(247, 215, 148, 255);
            for (int i = 0; i < Tabs.Count; i++)
            {
                Tabs[i].GetComponent<Image>().color = new Color32(247, 215, 148, 255);
            }

        }
        else if (currentTabSelected == tabMngr[5].gameObject)
        {
            Header.text = "Faltas";
            Header.fontSize = 21;
            Background.color = new Color32(248, 165, 194, 255);
            for (int i = 0; i < Tabs.Count; i++)
            {
                Tabs[i].GetComponent<Image>().color = new Color32(248, 165, 194, 255);
            }


        }
        else if (currentTabSelected == tabMngr[6].gameObject) {
            Header.text = "Backlog";
            Header.fontSize = 21;

        }
        else
        {
            Header.text = "Skills";
            Header.fontSize = 21;
            Background.color = new Color32(247, 215, 148, 255);
            for (int i = 0; i < Tabs.Count; i++)
            {
                Tabs[i].GetComponent<Image>().color = new Color32(247, 215, 148, 255);
            }
        }

    }
    public void Saving() {
        //something here is making it go slow, maybe set it after the usual tasks
        BinaryFormatter formatter = new BinaryFormatter();
        // string path = @"C:\Users\Windows\Documents\projetos\TO DO LIST\todos";
        //  string path = @"C:\Users\Windows\Documents\projetos\TO DO LIST\todostest";
        string path = Application.persistentDataPath + @"/todos";
        //New Card File
        FileStream stream = new FileStream(path, FileMode.Create);
        MyList data = new MyList();
        data.listToday = tabMngr[0].taskList;
        data.list2Today = tabMngr[0].taskListTime;
        data.listDaily = tabMngr[1].taskList;
        data.list2Daily = tabMngr[1].taskListTime;
        data.listWeek = tabMngr[2].taskList;
        data.list2Week = tabMngr[2].taskListTime;
        data.listMonth = tabMngr[3].taskList;
        data.list2Month = tabMngr[3].taskListTime;
        data.listYear = tabMngr[4].taskList;
        data.list2Year = tabMngr[4].taskListTime;
        data.listTODO = tabMngr[5].taskList;
        data.list2TODO = tabMngr[5].taskListTime;
        data.list2Backlog = tabMngr[6].taskListTime;
        data.listBacklog = tabMngr[6].taskList;
        //Insert data into file
        formatter.Serialize(stream, data);
        stream.Close();
    }
    public void SavingTasksDone() {
        BinaryFormatter formatter = new BinaryFormatter();

        //string path = @"C:\Users\Windows\Documents\projetos\TO DO LIST\TasksDone";
        string path = Application.persistentDataPath + @"/TasksDone";

        FileStream stream = new FileStream(path, FileMode.Create);
        formatter.Serialize(stream, completed);
        stream.Close();
    }
    public void ClearTaskDone(bool t)
    {
        if (c.DateClearCompleted.Year == 0) {
            c.DateClearCompleted = DateTime.Now;
            c.SavingConfig();
        }
        if ((DateTime.Now - c.DateClearCompleted).TotalDays >= 7 || t) {
            completed.Clear();
            SavingTasksDone();
            c.DateClearCompleted = DateTime.Now;
            c.SavingConfig();
        }

    }
    public void OpeningTasksDone() {
        // string path = @"C:\Users\Windows\Documents\projetos\TO DO LIST\TasksDone";
        string path = Application.persistentDataPath + @"/TasksDone";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            completed = formatter.Deserialize(stream) as List<Task>;
            stream.Close();
        }
    }
    public void Opening() {
        //string path = @"C:\Users\Windows\Documents\projetos\TO DO LIST\todos";
        //  string path = @"C:\Users\Windows\Documents\projetos\TO DO LIST\todostest";
        string path = Application.persistentDataPath + @"/todos";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            //Convert from Binary to Strings
            MyList data = formatter.Deserialize(stream) as MyList;
            stream.Close();
            //Final Return
            t = data.listToday;
            twt = data.list2Today;
            tabMngr[0].taskList = data.listToday;
            tabMngr[0].taskListTime = data.list2Today;
            tabMngr[1].taskList = data.listDaily;
            tabMngr[1].taskListTime = data.list2Daily;
            tabMngr[2].taskList = data.listWeek;
            tabMngr[2].taskListTime = data.list2Week;
            tabMngr[3].taskList = data.listMonth;
            tabMngr[3].taskListTime = data.list2Month;
            tabMngr[4].taskList = data.listYear;
            tabMngr[4].taskListTime = data.list2Year;
            tabMngr[5].taskList = data.listTODO;
            tabMngr[5].taskListTime = data.list2TODO;
            tabMngr[6].taskList = data.listBacklog;
            tabMngr[6].taskListTime = data.list2Backlog;
        }
        else
        {
            Debug.LogError("Saved card not found in " + path);
        }
    }
    public void SwitchType(Task tt, GameObject ob) {
        switch (tt.tipo)
        {
            case Task.Type.today:
                if (tt.tags != null && !seeAll.showAll)
                    if (tt.tags.Contains("otd") && tt.DelieveryDay.Date != System.DateTime.Now.Date && (tt.DelieveryDay.Date - System.DateTime.Now.Date).TotalDays > 0)
                    {//change this to task tags
                        Destroy(ob);
                        return;
                    }
                if (tt.tags.Contains("otd")) {
                    ob.GetComponent<Text>().color = new Color(255, 255, 255);
                }
                ob.transform.parent = tabMngr[0].transform;
                LastPos = tabMngr[0].LasPos;
                currentTab = tabMngr[0].gameObject;
                break;
            case Task.Type.daily:
                if (tt.DelieveryDay.Date == System.DateTime.Now.Date)
                {
                    Destroy(ob);
                    return;
                }
                ob.transform.parent = tabMngr[1].transform;
                LastPos = tabMngr[1].LasPos; currentTab = tabMngr[1].gameObject;
                break;
            case Task.Type.week:
                if (tt.tags != null && !seeAll.showAll)
                    if (tt.tags.Contains("rpt1w") && tt.DelieveryDay.Date != System.DateTime.Now.Date && (tt.DelieveryDay.Date - System.DateTime.Now.Date).TotalDays > 0) {
                        Destroy(ob);
                        return;
                    }
                ob.transform.parent = tabMngr[2].transform;
                LastPos = tabMngr[2].LasPos;
                currentTab = tabMngr[2].gameObject;

                break;
            case Task.Type.month:
                ob.transform.parent = tabMngr[3].transform;
                LastPos = tabMngr[3].LasPos;
                currentTab = tabMngr[3].gameObject;

                break;
            case Task.Type.year:
                ob.transform.parent = tabMngr[4].transform;
                LastPos = tabMngr[4].LasPos;
                currentTab = tabMngr[4].gameObject;

                break;
            case Task.Type.subtask:
                ///  ob.transform.parent = tabMngr[5].transform;
                /// LastPos = tabMngr[5].LasPos;
                ///  currentTab = tabMngr[5].gameObject;

                break;
            case Task.Type.TODO:
                ob.transform.parent = tabMngr[5].transform;
                LastPos = tabMngr[5].LasPos;
                currentTab = tabMngr[5].gameObject;
                break;
            case Task.Type.backlog:
                ob.transform.parent = tabMngr[6].transform;
                LastPos = tabMngr[6].LasPos;
                currentTab = tabMngr[6].gameObject;
                break;
        }
        ob.transform.localScale = new Vector3(0.571226f, 0.571226f, 0.571226f);
        //talvez dentro desse if
        if (LastPos == new Vector3(0, 0, 0))
        {
            verticalSpacing = tt.tipo == Task.Type.backlog ? (int)verticalSpacingBacklog : 0;
            LastPos = Spawn.transform.position - new Vector3(verticalSpacing, spacing, 0);
        }
        //talvez aqui

        
        ob.transform.position = LastPos + new Vector3(0, spacing, 0);

        LastPos = ob.transform.position;




        currentTab.GetComponent<TabManager>().LasPos = LastPos;
    }
    public void ShowDate() {
        DateIfParent.SetActive(!DateIfParent.activeInHierarchy);
        toggleDate.SetActive(false);
        IfDate1.text = System.DateTime.Now.Day.ToString();
        IfDate2.text = System.DateTime.Now.Month.ToString();
        IfDate3.text = System.DateTime.Now.Year.ToString();
        IfTime1.text = "00";
        Iftime2.text = "00";

    }
    public void ShowTags() {
        InputFieldTag.SetActive(!InputFieldTag.activeInHierarchy);
        ToggleTag.SetActive(false);
    }
    public void ShowDateEdit() {
        ifDateEdit.SetActive(!ifDateEdit.activeInHierarchy);
        toggleDateEdit.SetActive(false);
    }
    public void DestroyTaskEdit() {

        switch (TaskBeingEdited.tipo)
        {
            case Task.Type.today:
                if (TaskBeingEdited is TaskWithTime)
                {
                    tabMngr[0].taskListTime.Remove((TaskWithTime)TaskBeingEdited);
                }
                else
                {
                    tabMngr[0].taskList.Remove(TaskBeingEdited);
                }
                break;
            case Task.Type.daily:
                if (TaskBeingEdited is TaskWithTime)
                {
                    tabMngr[1].taskListTime.Remove((TaskWithTime)TaskBeingEdited);
                }
                else
                {
                    tabMngr[1].taskList.Remove(TaskBeingEdited);
                }
                break;
            case Task.Type.week:
                if (TaskBeingEdited is TaskWithTime)
                {
                    tabMngr[2].taskListTime.Remove((TaskWithTime)TaskBeingEdited);
                }
                else
                {
                    tabMngr[2].taskList.Remove(TaskBeingEdited);
                }
                break;
            case Task.Type.month:
                if (TaskBeingEdited is TaskWithTime)
                {
                    tabMngr[3].taskListTime.Remove((TaskWithTime)TaskBeingEdited);
                }
                else
                {
                    tabMngr[3].taskList.Remove(TaskBeingEdited);
                }
                break;
            case Task.Type.year:
                if (TaskBeingEdited is TaskWithTime)
                {
                    tabMngr[4].taskListTime.Remove((TaskWithTime)TaskBeingEdited);
                }
                else
                {
                    tabMngr[4].taskList.Remove(TaskBeingEdited);
                }
                break;
            case Task.Type.subtask:
                break;
            case Task.Type.TODO:
                if (TaskBeingEdited is TaskWithTime)
                {
                    tabMngr[5].taskListTime.Remove((TaskWithTime)TaskBeingEdited);
                }
                else
                {
                    tabMngr[5].taskList.Remove(TaskBeingEdited);
                }
                break;
            case Task.Type.backlog:
                if (TaskBeingEdited is TaskWithTime)
                {
                    tabMngr[6].taskListTime.Remove((TaskWithTime)TaskBeingEdited);
                }
                else {
                    tabMngr[6].taskList.Remove(TaskBeingEdited);
                }
                break;
            default:
                break;
        }

        Saving();
        RestartScene();
    }
    void ChangeTaskTypeToTime(Task.Type t) {
        TaskBeingEdited = new TaskWithTime();
        TaskBeingEdited.tipo = t;
        TaskBeingEdited.taskWithTime = TimetaskEdit.isOn;
    }
    void ChangeTaskTypeToTask(Task.Type t) {
        TaskBeingEdited = new Task();
        TaskBeingEdited.tipo = t;
        TaskBeingEdited.taskWithTime = TimetaskEdit.isOn;
    }
    public void EditTask() {
        if (TaskBeingEdited == null)
        {
            return;
        }
        print(TaskBeingEdited.taskWithTime);
        TaskBeingEdited.taskWithTime = TimetaskEdit.isOn;
        if (TaskBeingEdited.tipo.ToString() != tipoEdit.options[tipoEdit.value].text)
        {
            if (TaskBeingEdited is TaskWithTime)
            {


                switch (TaskBeingEdited.tipo)
                {
                    case Task.Type.today:
                        tabMngr[0].taskListTime.Remove((TaskWithTime)TaskBeingEdited);
                        break;
                    case Task.Type.daily:
                        tabMngr[1].taskListTime.Remove((TaskWithTime)TaskBeingEdited);
                        break;
                    case Task.Type.week:
                        tabMngr[2].taskListTime.Remove((TaskWithTime)TaskBeingEdited);
                        break;
                    case Task.Type.month:
                        tabMngr[3].taskListTime.Remove((TaskWithTime)TaskBeingEdited);
                        break;
                    case Task.Type.year:
                        tabMngr[4].taskListTime.Remove((TaskWithTime)TaskBeingEdited);
                        break;
                    case Task.Type.subtask:
                        break;
                    case Task.Type.TODO:
                        tabMngr[5].taskListTime.Remove((TaskWithTime)TaskBeingEdited);
                        break;
                    case Task.Type.backlog:
                        tabMngr[6].taskListTime.Remove((TaskWithTime)TaskBeingEdited);
                        break;
                    default:
                        break;
                }
                switch (tipoEdit.options[tipoEdit.value].text)
                {
                    case "today":
                        tabMngr[0].taskListTime.Add((TaskWithTime)TaskBeingEdited);
                        TaskBeingEdited.tipo = Task.Type.today;
                        break;
                    case "daily":
                        tabMngr[1].taskListTime.Add((TaskWithTime)TaskBeingEdited);
                        TaskBeingEdited.tipo = Task.Type.daily;
                        break;
                    case "week":
                        tabMngr[2].taskListTime.Add((TaskWithTime)TaskBeingEdited);
                        TaskBeingEdited.tipo = Task.Type.week;
                        break;
                    case "month":
                        tabMngr[3].taskListTime.Add((TaskWithTime)TaskBeingEdited);
                        TaskBeingEdited.tipo = Task.Type.month;
                        break;
                    case "year":
                        tabMngr[4].taskListTime.Add((TaskWithTime)TaskBeingEdited);
                        TaskBeingEdited.tipo = Task.Type.year;

                        break;
                    case "TODO":
                        tabMngr[5].taskListTime.Add((TaskWithTime)TaskBeingEdited);
                        TaskBeingEdited.tipo = Task.Type.TODO;

                        break;
                }
            }
            else {
                switch (TaskBeingEdited.tipo)
                {
                    case Task.Type.today:
                        tabMngr[0].taskList.Remove(TaskBeingEdited);
                        break;
                    case Task.Type.daily:
                        tabMngr[1].taskList.Remove(TaskBeingEdited);
                        break;
                    case Task.Type.week:
                        tabMngr[2].taskList.Remove(TaskBeingEdited);
                        break;
                    case Task.Type.month:
                        tabMngr[3].taskList.Remove(TaskBeingEdited);
                        break;
                    case Task.Type.year:
                        tabMngr[4].taskList.Remove(TaskBeingEdited);
                        break;
                    case Task.Type.subtask:
                        break;
                    case Task.Type.TODO:
                        tabMngr[5].taskList.Remove(TaskBeingEdited);
                        break;
                    case Task.Type.backlog:
                        tabMngr[6].taskList.Remove(TaskBeingEdited);
                        break;
                    default:
                        break;
                }
                switch (tipoEdit.value)
                {
                    case 0:
                        tabMngr[0].taskList.Add(TaskBeingEdited);
                        TaskBeingEdited.tipo = Task.Type.today;
                        break;
                    case 1:
                        tabMngr[1].taskList.Add(TaskBeingEdited);
                        TaskBeingEdited.tipo = Task.Type.daily;
                        break;
                    case 2:
                        tabMngr[2].taskList.Add(TaskBeingEdited);
                        TaskBeingEdited.tipo = Task.Type.week;

                        break;
                    case 3:
                        tabMngr[3].taskList.Add(TaskBeingEdited);
                        TaskBeingEdited.tipo = Task.Type.month;

                        break;
                    case 4:
                        tabMngr[4].taskList.Add(TaskBeingEdited);
                        TaskBeingEdited.tipo = Task.Type.year;

                        break;
                    case 5:
                        tabMngr[5].taskList.Add(TaskBeingEdited);
                        TaskBeingEdited.tipo = Task.Type.TODO;
                        break;
                }
                switch (dpPriorityEdit.value) {
                    case 0:
                        TaskBeingEdited.priority = Task.Flag.red;
                        break;
                    case 1:
                        TaskBeingEdited.priority = Task.Flag.blue;
                        break;
                    case 2:
                        TaskBeingEdited.priority = Task.Flag.yellow;

                        break;

                }

            }
        }
        if (TaskBeingEdited.taskWithTime)
        {
            if (!(TaskBeingEdited is TaskWithTime))
            {
                switch (TaskBeingEdited.tipo)
                {
                    case Task.Type.today:
                        tabMngr[0].taskList.Remove(TaskBeingEdited);
                        ChangeTaskTypeToTime(Task.Type.today);
                        tabMngr[0].taskListTime.Add((TaskWithTime)TaskBeingEdited);
                        break;
                    case Task.Type.daily:
                        tabMngr[1].taskList.Remove(TaskBeingEdited);
                        ChangeTaskTypeToTime(Task.Type.daily);
                        tabMngr[1].taskListTime.Add((TaskWithTime)TaskBeingEdited);
                        break;
                    case Task.Type.week:
                        tabMngr[2].taskList.Remove(TaskBeingEdited);
                        ChangeTaskTypeToTime(Task.Type.week);
                        tabMngr[2].taskListTime.Add((TaskWithTime)TaskBeingEdited);
                        break;
                    case Task.Type.month:
                        tabMngr[3].taskList.Remove(TaskBeingEdited);
                        ChangeTaskTypeToTime(Task.Type.month);
                        tabMngr[3].taskListTime.Add((TaskWithTime)TaskBeingEdited);
                        break;
                    case Task.Type.year:
                        tabMngr[4].taskList.Remove(TaskBeingEdited);
                        ChangeTaskTypeToTime(Task.Type.year);
                        tabMngr[4].taskListTime.Add((TaskWithTime)TaskBeingEdited);
                        break;
                    case Task.Type.subtask:
                        break;
                    case Task.Type.TODO:
                        tabMngr[5].taskList.Remove(TaskBeingEdited);
                        ChangeTaskTypeToTime(Task.Type.TODO);
                        tabMngr[5].taskListTime.Add((TaskWithTime)TaskBeingEdited);
                        break;
                    default:
                        break;
                }


            }
            TaskWithTime temp2 = (TaskWithTime)TaskBeingEdited;

            Dropdown temp = DropEditTime.GetComponent<Dropdown>();
            temp2.MaxTimer = int.Parse(temp.captionText.text);
        }
        else if (TaskBeingEdited is TaskWithTime)
        {
            switch (TaskBeingEdited.tipo)
            {
                case Task.Type.today:
                    tabMngr[0].taskListTime.Remove((TaskWithTime)TaskBeingEdited);
                    ChangeTaskTypeToTask(Task.Type.today);
                    tabMngr[0].taskList.Add(TaskBeingEdited);
                    break;
                case Task.Type.daily:
                    tabMngr[1].taskListTime.Remove((TaskWithTime)TaskBeingEdited);
                    ChangeTaskTypeToTask(Task.Type.daily);
                    tabMngr[1].taskList.Add(TaskBeingEdited);
                    break;
                case Task.Type.week:
                    tabMngr[2].taskListTime.Remove((TaskWithTime)TaskBeingEdited);
                    ChangeTaskTypeToTask(Task.Type.week);
                    tabMngr[2].taskList.Add(TaskBeingEdited);
                    break;
                case Task.Type.month:
                    tabMngr[3].taskListTime.Remove((TaskWithTime)TaskBeingEdited);
                    ChangeTaskTypeToTask(Task.Type.month);
                    tabMngr[3].taskList.Add(TaskBeingEdited);
                    break;
                case Task.Type.year:
                    tabMngr[4].taskListTime.Remove((TaskWithTime)TaskBeingEdited);
                    ChangeTaskTypeToTask(Task.Type.year);
                    tabMngr[4].taskList.Add(TaskBeingEdited);
                    break;
                case Task.Type.subtask:
                    break;
                case Task.Type.TODO:
                    tabMngr[5].taskListTime.Remove((TaskWithTime)TaskBeingEdited);
                    ChangeTaskTypeToTask(Task.Type.TODO);
                    tabMngr[5].taskList.Add(TaskBeingEdited);
                    break;
                default:
                    break;
            }


        }
        TaskBeingEdited.name = IfEditTask.text;
        //caso va de time task to normal task with date it doesnt work probably here
        if (TaskBeingEdited.DelieveryDay.ToString() != "1/1/0001 12:00:00 AM")
        {
            DateTime temp = new DateTime(Int32.Parse(IfDateEdit3.text), Int32.Parse(IfDateEdit2.text), Int32.Parse(IfDateEdit1.text), Int32.Parse(IfTimeEdit1.text), Int32.Parse(IftimeEdit2.text), 0);

            TaskBeingEdited.DelieveryDay = temp;
        }
        TaskBeingEdited.SubTasks = SubtaskEdit.isOn;
        if (TaskBeingEdited.SubTasks)
        {
            if (TaskBeingEdited.subtasksList.Count == Int32.Parse(IfSubtaskEdit.text))
            {
                for (int i = 0; i < 8; i++)
                {
                    if (TaskBeingEdited.subtasksList.Count >= i + 1)
                    {
                        TaskBeingEdited.subtasksList[i].name = subtaskifedit.gameObject.transform.GetChild(i).GetComponent<InputField>().text;

                    }
                    else
                    {
                        subtaskifedit.gameObject.transform.GetChild(i).gameObject.SetActive(false);

                    }
                }
            }
            else if (TaskBeingEdited.subtasksList.Count < Int32.Parse(IfSubtaskEdit.text))
            {
                //criar subtasks
                for (int i = 0; i < 8; i++)
                {
                    if (Int32.Parse(IfSubtaskEdit.text) >= i + 1)
                    {
                        if (i + 1 > TaskBeingEdited.subtasksList.Count)
                        {
                            Task newSubtask = new Task();
                            newSubtask.tipo = Task.Type.subtask;
                            newSubtask.name = subtaskifedit.transform.GetChild(i).GetComponent<InputField>().text;
                            newSubtask.FromSubTasks = true;
                            TaskBeingEdited.subtasksList.Add(newSubtask);
                        }
                        else
                            TaskBeingEdited.subtasksList[i].name = subtaskifedit.gameObject.transform.GetChild(i).GetComponent<InputField>().text;

                    }
                    else
                    {
                        subtaskifedit.gameObject.transform.GetChild(i).gameObject.SetActive(false);

                    }
                }
            }
            else
            {

                //remover subtasks
                for (int i = 0; i < 8; i++)
                {
                    if (Int32.Parse(IfSubtaskEdit.text) >= i + 1)
                    {
                        TaskBeingEdited.subtasksList[i].name = subtaskifedit.gameObject.transform.GetChild(i).GetComponent<InputField>().text;

                    }
                    else if (TaskBeingEdited.subtasksList.Count > i)
                    {
                        print(i);
                        TaskBeingEdited.subtasksList.RemoveAt(i);
                        subtaskifedit.gameObject.transform.GetChild(i).gameObject.SetActive(false);

                    }
                }
            }
        }
        else {
            TaskBeingEdited.subtasksList.RemoveRange(0, TaskBeingEdited.subtasksList.Count);
        }

        editing = false;
        EditButton.GetComponent<Image>().color = new Color32(119, 139, 235, 255);
        notification.sendNotifications(TaskBeingEdited.name, TaskBeingEdited.name + TaskBeingEdited.DelieveryDay, TaskBeingEdited.DelieveryDay);

        TaskBeingEdited = null;
        Saving();
        RestartScene();
    }
    public void PopulatingTaskEdit(Task t) {
        //populate box of tipo
        TaskBeingEdited = t;
        IfEditTask.text = t.name;
        TimetaskEdit.isOn = t.taskWithTime;

        DropEditTime.SetActive(TimetaskEdit.isOn);
        if (t.taskWithTime)
        {
            TaskWithTime temp2 = (TaskWithTime)t;
            Dropdown temp = DropEditTime.GetComponent<Dropdown>();
            temp.value = temp.options.FindIndex(option => option.text.IndexOf(temp2.MaxTimer.ToString()) >= 0);
        }
        tipoEdit.value = tipoEdit.options.FindIndex(option => option.text.IndexOf(TaskBeingEdited.tipo.ToString()) >= 0);
        dpPriorityEdit.value = dpPriorityEdit.options.FindIndex(option => option.text.IndexOf(t.priority.ToString()) >= 0);
        if (t.DelieveryDay.ToString() != "1/1/0001 12:00:00 AM")
        {
            DateEdit.isOn = true;
            IfDateEdit1.text = t.DelieveryDay.Day.ToString();
            IfDateEdit2.text = t.DelieveryDay.Month.ToString();
            IfDateEdit3.text = t.DelieveryDay.Year.ToString();
            IfTimeEdit1.text = t.DelieveryDay.Hour.ToString();
            IftimeEdit2.text = t.DelieveryDay.Minute.ToString();
        }
        else {
            DateEdit.isOn = false;
        }
        IfSubtaskEdit.text = t.subtasksList.Count.ToString();
        SubtaskEdit.isOn = t.SubTasks;
        if (t.SubTasks)
        {
            subtaskifParentEdit.SetActive(true);

            for (int i = 0; i < 8; i++)
            {
                if (t.subtasksList.Count >= i + 1)
                {
                    subtaskifedit.gameObject.transform.GetChild(i).gameObject.SetActive(true);
                    subtaskifedit.gameObject.transform.GetChild(i).GetComponent<InputField>().text = t.subtasksList[i].name;

                }
                else
                {
                    subtaskifedit.gameObject.transform.GetChild(i).gameObject.SetActive(false);

                }
            }
        }

    }
    public void RestartScene() {
        //make it go to the last tab
        SceneManager.LoadScene(0);
    }
    public void EditingState() {
        editing = !editing;

        if (editing == true)
        {
            EditButton.GetComponent<Image>().color = new Color32(247, 143, 179, 255);
        }
        else {
            EditButton.GetComponent<Image>().color = new Color32(119, 139, 235, 255);
        }

    }
    public void MissedClassesFill()
    {
        string path = Application.persistentDataPath + @"/MissedClasses";
        print(path);
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            //Convert from Binary to Strings
            MissedClassesWrapper data = formatter.Deserialize(stream) as MissedClassesWrapper;
            stream.Close();
            if (data.missedClassesList != null)
                MCW.missedClassesList = data.missedClassesList;
            for (int i = 0; i < MCW.missedClassesList.Count; i++)
            {
                print("filing" + listmcl[i].t.text);
                listmcl[i].msc = data.missedClassesList[i];
                MCW.missedClassesList[i].count = data.missedClassesList[i].count;
                listmcl[i].refreashUI();


            }
        }
    }
    public void MissedClassesSave()
    {
        print("saving");
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + @"/MissedClasses";
        FileStream stream = new FileStream(path, FileMode.Create);
        MissedClassesWrapper data = new MissedClassesWrapper();
        data.missedClassesList = MCW.missedClassesList;
        for (int i = 0; i < MCW.missedClassesList.Count; i++)
        {
            data.missedClassesList[i] = listmcl[i].msc;
            data.missedClassesList[i].count = MCW.missedClassesList[i].count;
        }
        //Insert data into file
        formatter.Serialize(stream, data);
        stream.Close();


    }
    public void DeleteMissedClasses() {
        string path = Application.persistentDataPath + @"/MissedClasses";
        File.Delete(path);

    }
    public void ShowAllTasksOtd() {
        seeAll.showAll = !seeAll.showAll;
        RestartScene();

    }
    public IEnumerator startweektext() {

        yield return new WaitForSeconds(0.5f);
       DayOfTheWeek.DOColor(new Color32(48, 57, 82, 0), 0.5f);
        yield return new WaitForSeconds(0.5f);
        DayOfTheWeek.transform.gameObject.SetActive(false);
    }
    public void SortList() {
        foreach (TabManager t in tabMngr)
        {
           t.taskListTime= t.taskListTime.OrderBy(o=>o.DelieveryDay).ToList();
           t.taskList= t.taskList.OrderBy(o=>o.DelieveryDay).ToList();
            }
    } }
