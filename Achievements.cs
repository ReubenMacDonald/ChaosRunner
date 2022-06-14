using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.IO;
using System.Linq;

public class Achievements : MonoBehaviour
{
    public string leaderboard;
    public string profileName;
    public string achievements;
    private List<string> names = new List<string>();
    [SerializeField] private GameObject nameTxt;
    private List<string> achFile = new List<string>();
    [SerializeField] private GameObject text1;
    [SerializeField] private GameObject text2;
    [SerializeField] private GameObject text3;
    [SerializeField] private GameObject text4;
    [SerializeField] private GameObject text5;
    [SerializeField] private GameObject text6;
    [SerializeField] private GameObject text7;
    [SerializeField] private GameObject text8;
    [SerializeField] private GameObject text9;
    [SerializeField] private GameObject text10;
    [SerializeField] private GameObject text11;
    [SerializeField] private GameObject text12;

    void Start()
    {
        Debug.Log(Application.persistentDataPath);
        leaderboard = Application.persistentDataPath + "/leaderboard.txt";
        achievements = Application.persistentDataPath + "/Achievements.txt";
        profileName = Application.persistentDataPath + "/Profiles.txt";
    }

    public void achieveCheck()
    {
        string nameIn;
        int countText = 1;
        string name = nameTxt.GetComponent<Text>().text;
        Debug.Log(name);
        string[] text = name.Split(' ');
        name = "";
        while (text.Length > countText)
        {
            name = name + text[countText];
            countText++;
            if (text.Length>countText)
            {
                name = name + " ";
            }
        }
        Debug.Log(name);

        using (var sr = new StreamReader(achievements))
        {
            while (sr.Peek() != -1)
            {
                nameIn = sr.ReadLine();
                if (nameIn == name)
                {
                    achFile.Add(nameIn);
                    nameTxt.GetComponent<Text>().text = "Name: " + nameIn;
                    LeaderCheck(name);
                    ProfileCheck(name);
                    for (int i = 0; i < 12; i++)
                    {
                        sr.ReadLine();
                    }
                }
                else
                {
                    achFile.Add(nameIn);
                    for (int i = 0; i < 12; i++)
                    {
                        achFile.Add(sr.ReadLine());
                    }
                }
            }
        }

        for (int i = 0; i < achFile.Count; i++)
        {
            if (achFile[i] == "")
            {
                achFile.RemoveAt(i);
                i--;
            }
        }

        File.WriteAllLines(achievements, achFile);
        achFile = new List<string>();
    }

    public void ProfileCheck(string name)
    {
        int bird;
        int car;
        int drown;
        int gun;
        using (var sr = new StreamReader(profileName))
        {
            while (sr.Peek() != -1)
            {
                if (name == sr.ReadLine())
                {
                    sr.ReadLine();
                    bird = int.Parse(sr.ReadLine());
                    car = int.Parse(sr.ReadLine());
                    drown = int.Parse(sr.ReadLine());
                    gun = int.Parse(sr.ReadLine());
                    if (bird > 20)
                    {
                        achFile.Add("True");
                        text7.GetComponent<Text>().color = new Color32(0, 250, 0, 255);
                    }
                    else
                    {
                        achFile.Add("False");
                        text7.GetComponent<Text>().color = new Color32(250, 0, 0, 255);
                    }

                    if (car > 20)
                    {
                        achFile.Add("True");
                        text8.GetComponent<Text>().color = new Color32(0, 250, 0, 255);
                    }
                    else
                    {
                        achFile.Add("False");
                        text8.GetComponent<Text>().color = new Color32(250, 0, 0, 255);
                    }

                    if (drown > 20)
                    {
                        achFile.Add("True");
                        text9.GetComponent<Text>().color = new Color32(0, 250, 0, 255);
                    }
                    else
                    {
                        achFile.Add("False");
                        text9.GetComponent<Text>().color = new Color32(250, 0, 0, 255);
                    }

                    if (gun > 20)
                    {
                        achFile.Add("True");
                        text10.GetComponent<Text>().color = new Color32(0, 250, 0, 255);
                    }
                    else
                    {
                        achFile.Add("False");
                        text10.GetComponent<Text>().color = new Color32(250, 0, 0, 255);
                    }

                    if (bird + car + drown + gun > 100)
                    {
                        achFile.Add("True");
                        achFile.Add("True");
                        text11.GetComponent<Text>().color = new Color32(0, 250, 0, 255);
                        text12.GetComponent<Text>().color = new Color32(0, 250, 0, 255);
                    }
                    else if (bird+car+drown+gun>50)
                    {
                        
                        achFile.Add("True");
                        achFile.Add("False");
                        text11.GetComponent<Text>().color = new Color32(0, 250, 0, 255);
                        text12.GetComponent<Text>().color = new Color32(250, 0, 0, 255);
                    }
                    else
                    {
                        achFile.Add("False");
                        achFile.Add("False");
                        text11.GetComponent<Text>().color = new Color32(250, 0, 0, 255);
                        text12.GetComponent<Text>().color = new Color32(250, 0, 0, 255);
                    }
                }
            }
        }
    }

    public void LeaderCheck(string name)
    {
        int scoreIn;
        int scoreMax = 0;
        int scoreMaxO = 0;
        using (var sr = new StreamReader(leaderboard))
        {
            while (sr.Peek() != -1)
            {
                if (name == sr.ReadLine())
                {
                    scoreIn = int.Parse(sr.ReadLine());
                    if (scoreIn>scoreMax)
                    {
                        scoreMax = scoreIn;
                    }
                }
                else
                {
                    scoreIn = int.Parse(sr.ReadLine());
                    if (scoreIn>scoreMaxO)
                    {
                        scoreMaxO = scoreIn;
                    }
                }
            }
        }

        if (scoreMax > 100)
        {
            achFile.Add("True");
            achFile.Add("True");
            achFile.Add("True");
            achFile.Add("True");
            achFile.Add("True");
            text1.GetComponent<Text>().color = new Color32(0, 250, 0, 255);
            text2.GetComponent<Text>().color = new Color32(0, 250, 0, 255);
            text3.GetComponent<Text>().color = new Color32(0, 250, 0, 255);
            text4.GetComponent<Text>().color = new Color32(0, 250, 0, 255);
            text5.GetComponent<Text>().color = new Color32(0, 250, 0, 255);
        }
        else if (scoreMax > 75)
        {
            achFile.Add("True");
            achFile.Add("True");
            achFile.Add("True");
            achFile.Add("True");
            achFile.Add("False");
            text1.GetComponent<Text>().color = new Color32(0, 250, 0, 255);
            text2.GetComponent<Text>().color = new Color32(0, 250, 0, 255);
            text3.GetComponent<Text>().color = new Color32(0, 250, 0, 255);
            text4.GetComponent<Text>().color = new Color32(0, 250, 0, 255);
            text5.GetComponent<Text>().color = new Color32(250, 0, 0, 255);
        }
        else if (scoreMax > 50)
        {
            achFile.Add("True");
            achFile.Add("True");
            achFile.Add("True");
            achFile.Add("False");
            achFile.Add("False");
            text1.GetComponent<Text>().color = new Color32(0, 250, 0, 255);
            text2.GetComponent<Text>().color = new Color32(0, 250, 0, 255);
            text3.GetComponent<Text>().color = new Color32(0, 250, 0, 255);
            text4.GetComponent<Text>().color = new Color32(250, 0, 0, 255);
            text5.GetComponent<Text>().color = new Color32(250, 0, 0, 255);
        }
        else if (scoreMax > 25)
        {
            achFile.Add("True");
            achFile.Add("True");
            achFile.Add("False");
            achFile.Add("False");
            achFile.Add("False");
            text1.GetComponent<Text>().color = new Color32(0, 250, 0, 255);
            text2.GetComponent<Text>().color = new Color32(0, 250, 0, 255);
            text3.GetComponent<Text>().color = new Color32(250, 0, 0, 255);
            text4.GetComponent<Text>().color = new Color32(250, 0, 0, 255);
            text5.GetComponent<Text>().color = new Color32(250, 0, 0, 255);
        }
        else if (scoreMax > 10)
        {
            achFile.Add("True");
            achFile.Add("False");
            achFile.Add("False");
            achFile.Add("False");
            achFile.Add("False");
            text1.GetComponent<Text>().color = new Color32(0, 250, 0, 255);
            text2.GetComponent<Text>().color = new Color32(250, 0, 0, 255);
            text3.GetComponent<Text>().color = new Color32(250, 0, 0, 255);
            text4.GetComponent<Text>().color = new Color32(250, 0, 0, 255);
            text5.GetComponent<Text>().color = new Color32(250, 0, 0, 255);
        }
        else
        {
            achFile.Add("False");
            achFile.Add("False");
            achFile.Add("False");
            achFile.Add("False");
            achFile.Add("False");
            text1.GetComponent<Text>().color = new Color32(250, 0, 0, 255);
            text2.GetComponent<Text>().color = new Color32(250, 0, 0, 255);
            text3.GetComponent<Text>().color = new Color32(250, 0, 0, 255);
            text4.GetComponent<Text>().color = new Color32(250, 0, 0, 255);
            text5.GetComponent<Text>().color = new Color32(250, 0, 0, 255);
        }

        if (scoreMax >= scoreMaxO)
        {
            achFile.Add("True");
            text6.GetComponent<Text>().color = new Color32(0, 250, 0, 255);
        }
        else
        {
            achFile.Add("False");
            text6.GetComponent<Text>().color = new Color32(250, 0, 0, 255);
        }
    }

}