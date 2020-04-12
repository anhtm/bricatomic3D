using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] string[] instructions;
    [SerializeField] GameObject instructionPanel;
    [SerializeField] float panelTimeout = 3f;
    private int currentInstructionIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        PopulateInstructions();
        StartCoroutine(ActivateInstruction(currentInstructionIndex));
    }

    private void PopulateInstructions()
    {
        for (int i = 0; i < instructions.Length; i++)
        {
            GameObject instruction = instructionPanel.transform.GetChild(i).gameObject;
            Text txt = instruction.GetComponent<Text>();
            txt.text = instructions[i];
        }
    }

    // Update is called once per frame
    void Update()
    {
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
