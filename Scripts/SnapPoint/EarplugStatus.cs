using UnityEngine;
/// <summary>
/// The EarplugSwitcher class is responsible for switching between the earplugs in the bag and worn by the patient.
/// When the earplug bag is snapped to a specific point (via the OnSnapped method from the ISnappable interface),
/// it will activate the worn earplug models and deactivate the earplug bag model.
/// </summary>
public class EarplugStatus : MonoBehaviour, CheckerInterface, ReturnedInterface
{
    public GameObject earplugsWorn;
    public Container garbageCan;

    public string getLabel()
    {
        return "Earplugs on patient";
    }
    public string getReturnedLabel()
    {
        return "Earplugs disposal";
    }

    public bool isCorrect()
    {
        return earplugsWorn.activeSelf;
    }
    public bool isReturned()
    {
        return !isCorrect() && garbageCan.Contains(transform);
    }
}
