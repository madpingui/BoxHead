using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity {

    private int level;
    private float currentLevelExperience = 1;
    private float experienceToLevel;

    public GameGUI gui;

    void Start()
    {
        LevelUp();
    }

    public void AddExperiencie(float exp)
    {
        currentLevelExperience += exp;
        if(currentLevelExperience >= experienceToLevel)
        {
            currentLevelExperience -= experienceToLevel;
            LevelUp();
        }

        gui.SetPlayerExperience(currentLevelExperience / experienceToLevel, level);
    }

    private void LevelUp()
    {
        level++;
        experienceToLevel = level * 50 + Mathf.Pow(level * 2, 2);
        if (Gun.Instance)
        {
            Gun.Instance.ammoPerMag += 10;
        }
        AddExperiencie(0);
    }

}
