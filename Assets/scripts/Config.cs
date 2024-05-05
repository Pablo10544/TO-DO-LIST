using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Config : MonoBehaviour {
    [SerializeField]
    private int fontSize;
    public GameObject TextFont1, TextFont2;
    public Dropdown dp;
    public Slider sld;
    [SerializeField]
    private int sliderValue;
    public ToDoListManager tdlm;
    public GameObject config;
    [SerializeField]
    public int FontSizeIndex;
    public DateTime DateClearCompleted;
    public InputField ifStrikeThrough;
    private float strikeThrough;
    public InputField ifSwipeRange;
    public SwipeDetector sd;
    private float swipeRange;
    public InputField ifTapRange;
    private float tapRange;
    float backlogDistance;
    public float BacklogDistance { get { return backlogDistance; } set { backlogDistance = value; tdlm.verticalSpacingBacklog = BacklogDistance; } }
    public InputField BacklogDistanceIF;
    public float TapRange {
        get {
            return tapRange;
        }
        set {
            tapRange = value;
            sd.tapRange = tapRange;
        }
    }
    public float SwipeRange {
        get {
            return swipeRange;
        }
        set {
            swipeRange = value;
            sd.swipeRange = SwipeRange;
            
        }
    }
    public float StrikeThrough {
        get {
            return strikeThrough;
        }
        set {
            strikeThrough = value;
            tdlm.td.waitTime = StrikeThrough;
        }

    }
    public int FontSize {
        get { return fontSize;
        }
        set {
            fontSize = value;
            TextFont1.GetComponent<Text>().fontSize = FontSize;
            TextFont2.GetComponent<Text>().fontSize = FontSize;
        }
    }
    public int SliderValue {
        get {
            return sliderValue;
        }
        set {
            sliderValue = value;
            tdlm.spacing = -sliderValue;
        }
    }
    // Use this for initialization
    [System.Serializable]
    public class ConfigBuild{
       public int SliderValueStore;
        public int FontSizeStore;
        public int FontSizeIndex;
        public DateTime DateClearCompleted;
        public float TimerStrikeThrough;
        public float SwipeRange;
        public float TapRange;
        public float BaclogDistance;
}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void ChangeSettings() {//normal font size:20 spacing:57
        FontSize = int.Parse(dp.captionText.text);
        SliderValue = Mathf.RoundToInt(sld.value);
        FontSizeIndex = dp.value;
        StrikeThrough = float.Parse(ifStrikeThrough.text);
        SwipeRange = float.Parse(ifSwipeRange.text);
        TapRange = float.Parse(ifTapRange.text);
        BacklogDistance = float.Parse(BacklogDistanceIF.text);
        SavingConfig();
        
    }
    public void ShowConfig() {
        sld.value = SliderValue;
        dp.value= FontSizeIndex;
        config.SetActive(!config.activeInHierarchy);
        ifStrikeThrough.text = StrikeThrough.ToString();
        ifSwipeRange.text = SwipeRange.ToString();
        ifTapRange.text = tapRange.ToString();
    }
    public void SavingConfig() {
        BinaryFormatter formatter = new BinaryFormatter();
        //string path = @"C:\Users\Windows\Documents\projetos\TO DO LIST\config";
        //string path = @"C:\Users\Windows\Documents\projetos\TO DO LIST\config";
        string path = Application.persistentDataPath + @"/config";
        //New Card File
        FileStream stream = new FileStream(path, FileMode.Create);
        ConfigBuild cf = new ConfigBuild();
        cf.DateClearCompleted = DateClearCompleted;
        cf.FontSizeStore = fontSize;
        cf.SliderValueStore = sliderValue;
        cf.FontSizeIndex = FontSizeIndex;
        if(ifStrikeThrough.text!="")
        cf.TimerStrikeThrough = float.Parse(ifStrikeThrough.text);
        if (ifSwipeRange.text != "")
            cf.SwipeRange = float.Parse(ifSwipeRange.text);
        if (ifTapRange.text != "")
            cf.TapRange = float.Parse(ifTapRange.text);
        //Insert data into file
        if(BacklogDistanceIF.text!="")
        cf.BaclogDistance = float.Parse(BacklogDistanceIF.text);
        formatter.Serialize(stream, cf);
        stream.Close();
    }
    public void OpeningConfig() {
        //string path = @"C:\Users\Windows\Documents\projetos\TO DO LIST\config";
        string path = Application.persistentDataPath + @"/config";
        if (File.Exists(path))
        {
            print(path);
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            //Convert from Binary to Strings
            ConfigBuild cf = formatter.Deserialize(stream) as ConfigBuild;
            stream.Close();
            //Final Return
            SliderValue = cf.SliderValueStore;
            FontSize = cf.FontSizeStore;
            FontSizeIndex = cf.FontSizeIndex;
            DateClearCompleted = cf.DateClearCompleted;
            StrikeThrough = cf.TimerStrikeThrough;
            SwipeRange = cf.SwipeRange;
            TapRange = cf.TapRange;
            BacklogDistance = cf.BaclogDistance;
        }
        else
        {
            Debug.LogError("Saved card not found in " + path);
        }
    }
}
