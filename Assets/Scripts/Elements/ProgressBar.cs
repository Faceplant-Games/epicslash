using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour {
    public Transform LoadingBar;
    public Transform TextIndicator;
    [SerializeField] public long CurrentExperience;
    [SerializeField] public long ExperienceGoal;

    private const string ExperienceNameString = " xp";

    // Update is called once per frame
    private void Update () {
        TextIndicator.GetComponent<Text>().text = HumanizeXpAmount() + ExperienceNameString;
        LoadingBar.GetComponent<Image>().fillAmount = CurrentExperience / (float)ExperienceGoal;
	}

    private string HumanizeXpAmount() // TODO Smooth it
    {
        string amountString;
        string unit;

        if (CurrentExperience > 2000000)
        {
            amountString = (CurrentExperience / 1000000).ToString();
            unit = "m";
            return amountString + unit;
        }

        if (CurrentExperience > 2000)
        {
            amountString = (CurrentExperience / 1000).ToString();
            unit = "k";
            return amountString + unit;
        }

        return CurrentExperience.ToString();
    }
}
