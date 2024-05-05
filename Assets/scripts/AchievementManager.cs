using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AchievementManager : MonoBehaviour {

    // Use this for initialization
    public List<Achievements> achievmentsList = new List<Achievements>();
    public ToDoListManager tdlm;
	void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
        //descobrir se tem jeito melhor de checar se condicao foi atingida sem usar update ver sobre get and set;
        foreach (Achievements a in achievmentsList)
        {
            if (a.t == Achievements.Type.OverXVariable)
            {
               
                        a.OverTime();

            }
            if (a.achieved)
            {
                achievmentsList.Remove(a);
                break;
            }
        }
    }
   public void PopulateScriptableObjects() {
        foreach (Achievements a in achievmentsList)
        {
            if (a.t == Achievements.Type.OverXVariable)
            {
                foreach (TaskWithTime t in tdlm.twt)
                {
                    if (t.name.Contains(a.Key))
                    {
                        a.taskAssociated = t;
                        a.OverTime();

                    }
                }

            }
        }
    }
    public void CreateMyAsset()
    {
        Achievements asset = ScriptableObject.CreateInstance<Achievements>();

        Instantiate(asset);
        achievmentsList.Add(asset);
    }
}
