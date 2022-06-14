using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class Profile : MonoBehaviour
{
    private string text = "";
    private string heroText = "";
    private int profileCounter = 1;
    [SerializeField] private GameObject nameTxt;
    [SerializeField] private GameObject nameTxtA;
    [SerializeField] private GameObject textBox;
    [SerializeField] private GameObject heroTxt;
    [SerializeField] private GameObject heroImage;
    [SerializeField] private GameObject profile1;
    [SerializeField] private GameObject profile2;
    [SerializeField] private GameObject profile3;
    [SerializeField] private GameObject profile4;
    public PlayerManager playerManager;
    public GameManager gameManager;
    [SerializeField] private Sprite human;
    [SerializeField] private Sprite mole;
    [SerializeField] private Sprite treeEnt;
    public string profileName;
    public string achievements;
    public bool newProfile = false;

    void Start()
    {
        profileName = Application.persistentDataPath + "/Profiles.txt";
        achievements = Application.persistentDataPath + "/Achievements.txt";
        gameManager.Profiles();
    }

    public void NewProfile(bool np)
    {
        newProfile = np;
    }

    public void CheckText()
    {
        text = nameTxt.GetComponent<Text>().text;
        string[] name = text.Split(' ');
        text = "";
        int countText = 1;
        while (name.Length > countText)
        {
            text = text + name[countText];
            countText++;
            if (name.Length > 2)
            {
                text = text + " ";
            }
        }

        playerManager.SetName(text);
    }

    public void SaveProfile()
    {
        int index = 0;
        int count = 0;
        int countText = 1;
        string[] name;
        if (nameTxt.GetComponent<Text>().text == "Name: ")
        {
            text = textBox.GetComponent<Text>().text;
        }
        else
        {
            text = nameTxt.GetComponent<Text>().text;
            name = text.Split(' ');
            text = "";
            while (name.Length > countText)
            {
                text = text + name[countText];
                countText++;
            }
        }

        if (text == "")
        {
            text = "Player";
        }
        name = text.Split(' ');
        text = ""; 
        countText = 0;
        while (name.Length > countText)
        {
            text = text + name[countText];
            countText++;
        }
        heroText = heroTxt.GetComponent<Text>().text;
        switch (heroText)
        {
            case "Hero: Mole":
                heroText = "Mole";
                break;
            case "Hero: Human":
                heroText = "Human";
                break;
            case "Hero: Treant":
                heroText = "Treant";
                break;
            case "Hero: ":
                heroText = "Human";
                break;
        }

        using (var sr = new StreamReader(profileName))
        {
            int random = Random.Range(0, 800000);
            while (sr.Peek() != -1)
            {
                count++;
                if (sr.ReadLine() == text)
                {
                    index = count;
                    if (newProfile)
                    {
                        text = text + "." + random.ToString();
                        for (int i = 0; i < 5; i++)
                        {
                            sr.ReadLine();
                        }
                    }
                }
            }
        }

        if (newProfile)
        {
            StreamWriter file = File.AppendText(profileName);
            file.WriteLine(text);
            file.WriteLine(heroText);
            for (int i = 0; i < 4; i++)
            {
                file.WriteLine(0);
            }

            file.Close();
        }
        else
        {
            string[] replaceArray = File.ReadAllLines(profileName);
            replaceArray[index] = heroText;
            File.WriteAllLines(profileName, replaceArray);
        }

        if (newProfile)
        {
            Debug.Log("Y");
            StreamWriter file2 = File.AppendText(achievements);
            file2.WriteLine(text);
            for (int i = 0; i < 12; i++)
            {
                file2.WriteLine("False");
            }

            file2.Close();
        }
    }

    public void changeHero(bool nxbk)
    {
        heroText = heroTxt.GetComponent<Text>().text;
        switch (heroText)
        {
            case "Hero: Mole":
                if (nxbk)
                {
                    heroImage.GetComponent<Image>().sprite = human;
                    heroText = "Hero: Human";
                }
                else
                {
                    heroImage.GetComponent<Image>().sprite = treeEnt;
                    heroText = "Hero: Treant";
                }

                break;
            case "Hero: Human":
                if (nxbk)
                {
                    heroImage.GetComponent<Image>().sprite = treeEnt;
                    heroText = "Hero: Treant";
                }
                else
                {
                    heroImage.GetComponent<Image>().sprite = mole;
                    heroText = "Hero: Mole";
                }

                break;
            case "Hero: Treant":
                if (nxbk)
                {
                    heroImage.GetComponent<Image>().sprite = mole;
                    heroText = "Hero: Mole";
                }
                else
                {
                    heroImage.GetComponent<Image>().sprite = human;
                    heroText = "Hero: Human";
                }

                break;
        }

        heroTxt.GetComponent<Text>().text = heroText;
    }

    public void moveProfile(bool lr)
    {
        int count = 0;
        string nameIn;
        string profile;
        string car;
        string gun;
        string drown;
        string bird;
        if (lr)
        {
            profileCounter++;
        }
        else
        {
            profileCounter--;
        }

        using (var sr = new StreamReader(profileName))
        {
            while (sr.Peek() != -1)
            {
                for (int i = 0; i < 6; i++)
                {
                    sr.ReadLine();
                }

                count++;
            }
        }

        if (profileCounter > count)
        {
            profileCounter = 1;
        }

        if (profileCounter == 0)
        {
            profileCounter = count;
        }

        int readCount = 0;
        using (var sr = new StreamReader(profileName))
        {
            while (sr.Peek() != -1 && profileCounter > readCount)
            {
                readCount++;
                nameIn = sr.ReadLine();
                profile = sr.ReadLine();
                bird = sr.ReadLine();
                car = sr.ReadLine();
                drown = sr.ReadLine();
                gun = sr.ReadLine();
                nameTxt.GetComponent<Text>().text = "Name: " + nameIn;
                nameTxtA.GetComponent<Text>().text = "Name: " + nameIn;
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
    }
}