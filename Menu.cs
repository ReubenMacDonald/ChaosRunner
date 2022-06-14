using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public GameObject text1;
    public GameObject text2;
    public GameObject text3;
    public GameObject text4;
    public GameObject text5;
    [SerializeField] private GameObject nameTxt;
    [SerializeField] private GameObject nameTxt2;
    [SerializeField] private GameObject heroTxt;
    [SerializeField] private GameObject heroImage;
    [SerializeField] private GameObject profile1;
    [SerializeField] private GameObject profile2;
    [SerializeField] private GameObject profile3;
    [SerializeField] private GameObject profile4;
    public GameObject score1;
    public GameObject score2;
    public GameObject score3;
    public GameObject score4;
    public GameObject score5;
    [SerializeField] private Sprite human;
    [SerializeField] private Sprite mole;
    [SerializeField] private Sprite treeEnt;
    private List<string> names = new List<string>();
    private List<int> scores = new List<int>();
    public string leaderBName;
    public string profileName;
    public string achievements;

    public void PlayGame()
    {
        SceneManager.LoadScene("Game");
    }

    void Start()
    {
        leaderBName = Application.persistentDataPath + "/leaderboard.txt";
        profileName = Application.persistentDataPath + "/Profiles.txt";
        achievements = Application.persistentDataPath + "/Achievements.txt";
        if (!File.Exists(leaderBName))
        {
            File.WriteAllText(leaderBName, "");
        }

        if (!File.Exists(profileName))
        {
            File.WriteAllText(profileName, "");
        }

        if (!File.Exists(achievements))
        {
            File.WriteAllText(achievements, "");
        }
    }

    public void Profiles()
    {
        string nameIn;
        string profile;
        string car;
        string gun;
        string drown;
        string bird;

        using (var sr = new StreamReader(profileName))
        {
            nameIn = sr.ReadLine();
            profile = sr.ReadLine();
            bird = sr.ReadLine();
            car = sr.ReadLine();
            drown = sr.ReadLine();
            gun = sr.ReadLine();
            nameTxt.GetComponent<Text>().text = "Name: " + nameIn;
            nameTxt2.GetComponent<Text>().text = "Name: " + nameIn;
            heroTxt.GetComponent<Text>().text = "Hero: " + profile;
            profile1.GetComponent<Text>().text = "Bird: " + bird;
            profile2.GetComponent<Text>().text = "Car: " + car;
            profile3.GetComponent<Text>().text = "Drown: " + drown;
            profile4.GetComponent<Text>().text = "Gun: " + gun;
            if (profile == "Human")
            {
                heroImage.GetComponent<Image>().sprite = human;
            }
            else if (profile == "Mole")
            {
                heroImage.GetComponent<Image>().sprite = mole;
            }
            else if (profile == "Treant")
            {
                heroImage.GetComponent<Image>().sprite = treeEnt;
            }
        }
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void LeaderBoard()
    {
        string nameIn;
        string scoreIn = "";
        bool found = false;
        int index;
        using (var sr = new StreamReader(leaderBName))
        {
            while (sr.Peek() != -1)
            {
                found = false;
                nameIn = sr.ReadLine();
                scoreIn = sr.ReadLine();
                for (int i = 0; i < names.Count; i++)
                {
                    if (nameIn == names.ToArray()[i])
                    {
                        found = true;
                        if (int.Parse(scoreIn) > scores.ToArray()[i])
                        {
                            scores.RemoveAt(i);
                            names.RemoveAt(i);
                            index = ScoreCheck(int.Parse(scoreIn));
                            if (index > names.Count)
                            {
                                names.Add(nameIn);
                                scores.Add(int.Parse(scoreIn));
                            }
                            else
                            {
                                names.Insert(index, nameIn);
                                scores.Insert(index, int.Parse(scoreIn));
                            }
                        }
                    }
                }

                if (names.Count == 0 || !found)
                {
                    index = ScoreCheck(int.Parse(scoreIn));
                    if (index > names.Count)
                    {
                        names.Add(nameIn);
                        scores.Add(int.Parse(scoreIn));
                    }
                    else
                    {
                        names.Insert(index, nameIn);
                        scores.Insert(index, int.Parse(scoreIn));
                    }

                    found = true;
                }
            }

            for (int totalCount = 0; totalCount < names.Count; totalCount++)
            {
                switchText(totalCount + 1, names[totalCount], 1);
                switchText(totalCount + 1, scores[totalCount].ToString(), 0);
            }
        }

        for (int i = names.Count + 1; i <= 5; i++)
        {
            switchText(i, "", 0);
            switchText(i, "", 1);
        }
    }

    public void emptyPic()
    {
        nameTxt.GetComponent<Text>().text = "Name: ";
        heroTxt.GetComponent<Text>().text = "Hero: Human";
        heroImage.GetComponent<Image>().sprite = human;
    }

    public int ScoreCheck(int score)
    {
        int i;
        for (i = 0; i < scores.Count; i++)
        {
            if (score > scores[i])
            {
                return i;
            }
        }

        return i;
    }

    public void switchText(int index, string textI, int count)
    {
        switch (index)
        {
            case 1:
                if (count == 1)
                {
                    text1.GetComponent<Text>().text = textI;
                }
                else
                {
                    score1.GetComponent<Text>().text = textI;
                }

                break;
            case 2:
                if (count == 1)
                {
                    text2.GetComponent<Text>().text = textI;
                }
                else
                {
                    score2.GetComponent<Text>().text = textI;
                }

                break;
            case 3:
                if (count == 1)
                {
                    text3.GetComponent<Text>().text = textI;
                }
                else
                {
                    score3.GetComponent<Text>().text = textI;
                }

                break;
            case 4:
                if (count == 1)
                {
                    text4.GetComponent<Text>().text = textI;
                }
                else
                {
                    score4.GetComponent<Text>().text = textI;
                }

                break;
            case 5:
                if (count == 1)
                {
                    text5.GetComponent<Text>().text = textI;
                }
                else
                {
                    score5.GetComponent<Text>().text = textI;
                }

                break;
        }
    }
}