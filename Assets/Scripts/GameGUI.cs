using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameGUI : MonoBehaviour {

    public Slider expbar;
    public Text levelText;
    public Text ammoText;

    public void SetPlayerExperience(float percentToLevel, int playerLevel)
    {
        levelText.text = "Level: " + playerLevel;
        expbar.value = percentToLevel;
    }

    public void SetAmmoInfo(int ammoInMag)
    {
        ammoText.text = ammoInMag.ToString();
    }
}
