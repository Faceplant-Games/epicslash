using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour {
    public Transform LoadingBar;
    public Transform TextIndicator;
    [SerializeField] public long currentExperience;
    [SerializeField] public long experienceGoal;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        TextIndicator.GetComponent<Text>().text = HumanizeXPAmount() + " XP";
        LoadingBar.GetComponent<Image>().fillAmount = (float)currentExperience / (float)experienceGoal;
	}

    private string HumanizeXPAmount() // TODO Smooth it
    {
        string amountString;
        string unit;

        if (currentExperience > 2000000)
        {
            amountString = (currentExperience / 1000000).ToString();
            unit = "m";
            return amountString + unit;
        }

        if (currentExperience > 2000)
        {
            amountString = (currentExperience / 1000).ToString();
            unit = "k";
            return amountString + unit;
        }

        return currentExperience.ToString();
    }
}
