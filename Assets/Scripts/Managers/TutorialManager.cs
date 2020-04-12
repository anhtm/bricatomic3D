using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages the tutorial UI at the beginning of the game
/// </summary>
public class TutorialManager : MonoBehaviour
{
    [SerializeField] string[] instructions;
    [SerializeField] GameObject instructionPanel;
    [SerializeField] float panelTimeout = 3f;
    private int currentInstructionIndex = 0;

    void Start()
    {
        PopulateInstructions();
        StartCoroutine(ActivateInstruction(currentInstructionIndex));
    }

    /// <summary>
    /// Add text to UI according to instructions array
    /// </summary>
    private void PopulateInstructions()
    {
        for (int i = 0; i < instructions.Length; i++)
        {
            GameObject instruction = instructionPanel.transform.GetChild(i).gameObject;
            Text txt = instruction.GetComponent<Text>();
            txt.text = instructions[i];
        }
    }

    void Update()
    {
        // Move to next instruction when currentMode is updated
        if ((int) ModeManager.Instance.currentMode == currentInstructionIndex)
        {
            currentInstructionIndex++;
            StartCoroutine(ActivateInstruction(currentInstructionIndex, 1f));
        }

        // Deactivate instructions panel once tutorial is done
        if (currentInstructionIndex == instructions.Length - 1)
        {
            StartCoroutine(CloseInstructions());
        }
    }

    public IEnumerator ActivateInstruction(int i, float delayTime = 0)
    {
        yield return new WaitForSeconds(delayTime);
        if (i > 0)
        {
            instructionPanel.transform.GetChild(i - 1).gameObject.SetActive(false);
        }
        instructionPanel.transform.GetChild(i).gameObject.SetActive(true);
    }

    public IEnumerator CloseInstructions()
    {
        yield return new WaitForSeconds(panelTimeout);
        instructionPanel.SetActive(false);
    }
}
