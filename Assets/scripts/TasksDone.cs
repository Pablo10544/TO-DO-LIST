using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TasksDone : MonoBehaviour
{
    public GameObject TasksDoneObject;
    public GameObject TextPrefab;
    public float waitTime;
    public GameObject inst;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void LoadTasksDone(List<Task> t) {
        string textreturn = ""; int cont = 0;
        int charcount=0;
        try
        {
            for (int i = t.Count - 1; i >= 0; i--)
            {
                // print(i);
                cont++;
                if (cont >= 17 || charcount >= 208)
                {
                    break;
                }
                textreturn += "● " + t[i].name + "\n";
                charcount = textreturn.Length;
            }
            inst = Instantiate(TextPrefab, TasksDoneObject.transform);
            inst.transform.localPosition = new Vector3(0, 0, 0);
            inst.GetComponent<TMP_Text>().text = textreturn;
            print(charcount);
            print(textreturn);
        }
        catch (Exception ex) {
            print(ex);
            inst = Instantiate(TextPrefab, TasksDoneObject.transform);
            inst.transform.localPosition = new Vector3(0, 0, 0);
            inst.GetComponent<TMP_Text>().text = ex.ToString();
        }


    }
    public IEnumerator AnimateText(TMP_Text tex) {
        tex.ForceMeshUpdate();
        tex.text.Replace("<s>","");
        tex.text=tex.text.Insert(0,"<s>");
        for (int i =3; i < tex.textInfo.characterCount; i+=3)    
        {
            tex.text=tex.text.Insert(i, "</s>");
            //print(i);
            yield return new WaitForSeconds(waitTime);
           tex.text= tex.text.Replace("</s>","");

        }
        
    }
}
