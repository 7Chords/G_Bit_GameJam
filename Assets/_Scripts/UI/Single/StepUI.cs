using UnityEngine;
using UnityEngine.UI;

public class StepUI : MonoBehaviour
{
    private Text stepText;
    private StepManager stepManager;

    private void Start()
    {   
        stepManager = FindObjectOfType<StepManager>();

        if (stepManager == null)
        {
            Debug.LogError("StepManagerŒ¥’“µΩ");
        }
        
        stepText = transform.GetChild(0).GetComponent<Text>();
        stepText.text = stepManager.maxSteps.ToString();
        
        EventManager.OnPlayerMove += UpdateStepText;
        EventManager.OnPlayerLoadData += UpdateStepText;
    }

    private void UpdateStepText()
    {
        stepText.text = stepManager.GetRemainingSteps().ToString();
    }
}
