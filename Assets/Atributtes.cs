using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Atributtes:MonoBehaviour
{
    [SerializeField]
    // Start is called before the first frame update
     float strengh;
    [SerializeField]
    float mental;
    [SerializeField]
    float learn;
    [SerializeField]
    float social;
    [SerializeField]
    float special;
    [SerializeField]
    float problemSolving;
    public TextMeshProUGUI textStrenght, textMental, textLearn, textSocial, textSpecial,textProblemSolving;
    public Image ProfileImage;
    public Sprite StrenghSprite;
    public Sprite MentalSprite;
    public Sprite LearnSprite;
    public Sprite SocialhSprite;
    public Sprite Streng2hSprite;
    public Sprite NormalSprite;


    public float Strengh { get => strengh; set => strengh = value; }
    public float Mental { get => mental; set => mental = value; }
    public float Learn { get => learn; set => learn = value; }
    public float Social { get => social; set => social = value; }
    public float Special { get => special; set => special = value; }
    public float ProblemSolving { get => problemSolving; set => problemSolving = value; }

    public void AddStrenght() {
       strengh= PlayerPrefs.GetFloat("forc");
        strengh++;
        textStrenght.text = strengh.ToString();
        PlayerPrefs.SetFloat("forc",strengh);
        ChangeProfile();
    }
    public void AddMental() {
       mental= PlayerPrefs.GetFloat("mental");
        mental++;
        textMental.text = mental.ToString();
        PlayerPrefs.SetFloat("mental", mental);
        ChangeProfile();

    }
    public void AddLearn() {
     
       learn= PlayerPrefs.GetFloat("learn");
        learn++;
        textLearn.text = learn.ToString();
        PlayerPrefs.SetFloat("learn", learn);
        ChangeProfile();

    }
    public void AddSocial() {
       social= PlayerPrefs.GetFloat("social");
        social++;
        textSocial.text = social.ToString();
        PlayerPrefs.SetFloat("social", social);
        ChangeProfile();

    }
    public void AddSpecial() {
       special= PlayerPrefs.GetFloat("special");
        special++;
        textSpecial.text = special.ToString();
        PlayerPrefs.SetFloat("special", special);
        ChangeProfile();

    }
    public void AddProblemSolving() {
        problemSolving = PlayerPrefs.GetFloat("problemSolving");
        problemSolving++;
        textProblemSolving.text = problemSolving.ToString();
        PlayerPrefs.SetFloat("problemSolving",problemSolving);
        ChangeProfile();

    }
    public void ChangeProfile() {
        float temp = strengh;
        ProfileImage.sprite = StrenghSprite;
        if (mental>temp) {
            temp = mental;
            ProfileImage.sprite = MentalSprite;
        }
        if (learn > temp) {
            temp = learn;
            ProfileImage.sprite = LearnSprite;
        }
        if (social > temp) {
            temp = social;
            ProfileImage.sprite = SocialhSprite;
        }
        if (temp == 0) {
            ProfileImage.sprite = NormalSprite;
        }
    }
}
