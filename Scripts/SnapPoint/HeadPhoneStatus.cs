using UnityEngine;
/// <summary>
/// The HeadPhoneSwitcher class is responsible for switching between the open and closed states of the headphones.
/// When the headphones are snapped to a specific point (via the OnSnapped method from the ISnappable interface),
/// it will activate the open headphone model and deactivate the closed one.
/// </summary>
public class HeadPhoneStatus : MonoBehaviour, CheckerInterface, ReturnedInterface
{
    public GameObject headphoneOpen;

    public string getLabel()
    {
        return "Headphones on patient";
    }

    public string getReturnedLabel()
    {
        return "Headphones removal";
    }

    public bool isCorrect()
    {
        return headphoneOpen.activeSelf;
    }
    public bool isReturned()
    {
        return !isCorrect();
    }
}
