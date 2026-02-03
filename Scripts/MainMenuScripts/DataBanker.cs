using UnityEngine;

// Stores and manages exam and gender related data
public class DataBanker : MonoBehaviour
{
    // Singleton instance of Databanker to ensure only one exists
    public static DataBanker Instance { get; private set; }

    // Private variables to store exam type and gender
    private string exam;
    private string gender;

    private bool FirstCheck = false;
    
    // Call awake when script instance is being loaded
    private void Awake()
    {
        // See if DataBanker exists
        if (Instance == null)
        {
            // Assign the instance as the singleton
            Instance = this;
        }
        gender = "Male"; // Default
    }

    // Set the exam type
    public void SetExamType(string examType)
    {
        Debug.Log("Setting exam type: " + examType);
        exam = examType;
    }

    // Set the gender
    public void SetGender(string genderType)
    {
       gender = genderType;
    }

    // get the exam type
    public string GetExamType()
    {
        return exam;
    }

    // get the gender
    public string GetGender()
    {
        return gender;
    }

    public void setFirstCheck(bool isFirstCheck)
    {
        FirstCheck = isFirstCheck;
    }

    public bool isFirstCheck()
    {
        return FirstCheck;
    }
}