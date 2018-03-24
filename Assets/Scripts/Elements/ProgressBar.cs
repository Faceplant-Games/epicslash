using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour {
    public Transform LoadingBar;
    public Transform TextIndicator;
    [SerializeField] public int currentXP = 0;
    [SerializeField] public int XPGoal = 3000000;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        TextIndicator.GetComponent<Text>().text = HumanizeXPAmount() + " XP";
        LoadingBar.GetComponent<Image>().fillAmount = (float)currentXP / (float)XPGoal;
	}

    private string HumanizeXPAmount()
    {
        string amountString;
        string unit;

        if (currentXP > 2000000)
        {
            amountString = (currentXP / 1000000).ToString();
            unit = "m";
            return amountString + unit;
        }

        if (currentXP > 2000)
        {
            amountString = (currentXP / 1000).ToString();
            unit = "k";
            return amountString + unit;
        }

        return currentXP.ToString();
    }
}
