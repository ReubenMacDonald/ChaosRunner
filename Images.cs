using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class Images
{
    public string profileName;
    public string profile;
    
    public Images(string name)
    {
        profileName = Application.persistentDataPath + "/Profiles.txt";
        using (var sr = new StreamReader(profileName))
        {
            while (sr.Peek() != -1)
            {
                string nameIn = sr.ReadLine();
                // nameIn = nameIn + " ";
                Debug.Log(name);
                Debug.Log(nameIn);
                if (nameIn != name)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        sr.ReadLine();
                    }
                }
                else
                {
                    profile = sr.ReadLine();
                    Debug.Log(profile);
                    for (int i = 0; i < 4; i++)
                    {
                        sr.ReadLine();
                    }
                }
            }
        }
    }

    public string GetSprite(int direct)
    {
        if (profile == "Mole")
        {
            switch (direct)
            {
                case 0:
                    return "MoleW";
                case 1:
                    return "MoleE";
                case 2:
                    return "MoleN";
                case 3:
                    return "MoleS";
                case 5:
                    return "Mole";
            }
        }
        else if (profile == "Treant")
        {
            switch (direct)
            {
                case 0:
                    return "TreantW";
                case 1:
                    return "TreantE";
                case 2:
                    return "TreantN";
                case 3:
                    return "TreantS";
                case 5:
                    return "Treant";
            }
        }
        else
        {
            switch (direct)
            {
                case 0:
                    return "HumanW";
                case 1:
                    return "HumanE";
                case 2:
                    return "HumanN";
                case 3:
                    return "HumanS";
                case 5:
                    return "Human";
            }
        }

        return "Fail";
    }
}
