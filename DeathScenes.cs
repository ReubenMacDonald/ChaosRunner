using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System;
using System.IO;
using System.Linq;

public interface IObserver
{
    public void OnNotify(string death, PlayerManager mPlayerManager, GameObject mImage, Sprite mBird, Sprite mCar,
        Sprite mDrown, Sprite mGun, GameObject deadPanel, GameObject canvas, GameManager gm);
}

public class DeathScenes : IObserver
{
    private GameObject mImage;
    private Sprite mBird;
    private Sprite mCar;
    private Sprite mDrown;
    private Sprite mGun;
    private GameObject deadPanel;
    private GameObject canvas;
    private PlayerManager mPlayerManager;
    private GameManager gm;
    public string profileName = Application.persistentDataPath + "/Profiles.txt";

    void IObserver.OnNotify(string death, PlayerManager mPlayerManager1, GameObject mImage1, Sprite mBird1,
        Sprite mCar1, Sprite mDrown1, Sprite mGun1, GameObject deadPanel1, GameObject canvas1, GameManager gm1)
    {
        mCar = mCar1;
        mDrown = mDrown1;
        mBird = mBird1;
        mGun = mGun1;
        mImage = mImage1;
        deadPanel = deadPanel1;
        canvas = canvas1;
        mPlayerManager = mPlayerManager1;
        gm = gm1;
        switch (death)
        {
            case "Bird":
                AddFile(1);
                Bird();
                break;
            case "Car":
                AddFile(2);
                Car();
                break;
            case "Drown":
                Drown();
                AddFile(3);
                break;
            case "Gun":
                Gun();
                AddFile(4);
                break;
        }

        Debug.Log("This");
        deadPanel.SetActive(true);
        canvas.SetActive(false);
    }

    public void AddFile(int index)
    {
        bool notSame = false;
        List<string> profileFile = new List<string>();
        string name;
        string managerName;
        List<char> nameArray;
        List<char> mNameArray;
        managerName = mPlayerManager.GetName();
        mNameArray = managerName.ToList();
        using (var sr = new StreamReader(profileName))
        {
            while (sr.Peek() != -1)
            {
                name = sr.ReadLine();
                nameArray = name.ToList();
                if (nameArray.Count == mNameArray.Count)
                {
                    for (int i = 0; i < nameArray.Count; i++)
                    {
                        if (nameArray[i] != mNameArray[i])
                        {
                            notSame = true;
                        }
                    }
                }
                else
                {
                    notSame = true;
                }

                profileFile.Add(name);
                if (!notSame)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        if (i == index)
                        {
                            int count = int.Parse(sr.ReadLine());
                            profileFile.Add((count + 1).ToString());
                        }
                        else
                        {
                            profileFile.Add(sr.ReadLine());
                        }
                    }

                }
                else
                {
                    notSame = false;
                    for (int i = 0; i < 5; i++)
                    {
                        profileFile.Add(sr.ReadLine());
                    }
                }
            }
        }

        File.WriteAllLines(profileName, profileFile);
    }

    public void NewGame()
    {
        gm.StartGame();
        gm.StartBuild();
    }

    public void Bird()
    {
        mImage.GetComponent<Image>().sprite = mBird;
    }

    public void Car()
    {
        mImage.GetComponent<Image>().sprite = mCar;
    }

    public void Drown()
    {
        mImage.GetComponent<Image>().color = new Color32(0, 206, 209, 255);
        mImage.GetComponent<Image>().sprite = mDrown;
    }

    public void Gun()
    {
        mImage.GetComponent<Image>().color = new Color32(150, 0, 0, 255);
        mImage.GetComponent<Image>().sprite = mGun;
    }
}