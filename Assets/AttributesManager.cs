using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttributesManager : MonoBehaviour
{
    // Start is called before the first frame update
    public Atributtes att;
    public Image forca, mental, learn, social, special,problemSolving;
    public bool attstarted;
    void Start()
    {
        attstarted = true;
        att.ProblemSolving = PlayerPrefs.GetFloat("problemSolving");
       att.Strengh=PlayerPrefs.GetFloat("forc");
        att.Mental = PlayerPrefs.GetFloat("mental");
        att.Learn = PlayerPrefs.GetFloat("learn");
        att.Social = PlayerPrefs.GetFloat("social");
        att.Special = PlayerPrefs.GetFloat("special");
        att.textProblemSolving.text = att.ProblemSolving.ToString();
        att.textStrenght.text = att.Strengh.ToString();
        att.textMental.text = att.Mental.ToString();
        att.textLearn.text = att.Learn.ToString();
        att.textSocial.text = att.Social.ToString();
        att.textSpecial.text = att.Special.ToString();
        problemSolving.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,att.ProblemSolving);
        forca.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, att.Strengh);
        mental.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, att.Mental);
        learn.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, att.Learn);
        social.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, att.Social);
        special.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, att.Special);
        att.ChangeProfile();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
