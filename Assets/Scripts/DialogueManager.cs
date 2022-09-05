using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public List<float> ProfitSteps;
    public List<Dialogue> Dialogues;
    public DialogueBox TextBox;
    public ScrollingWindow BottomContent;
    public GameObject DialoguePanel;
    public int CurrentDialogue = 0;
    public GameObject StoryButton;

    private void Start()
    {
        CurrentDialogue = PlayerPrefs.GetInt("Dialogue Step");
    }

    public void StartDialogue ()
    {
        DialoguePanel.SetActive(true);
        if(Dialogues[CurrentDialogue] != null)
        {
            TextBox.StartDialogue(Dialogues[CurrentDialogue]);
        }
    }

    public void NewDialogue()
    {
        BottomContent.NewHeroIcon();
        StoryButton.SetActive(true);
        PlayerPrefs.SetInt("Dialogue Step", CurrentDialogue);

    }

    public void IterateDialogue(float p)
    {
        var previousDialogue = CurrentDialogue;
        for (var i = 0;i < ProfitSteps.Count; i++)
        {
            if(p > ProfitSteps[i])
            {
                
                CurrentDialogue = i;
                if(i > CurrentDialogue)
                {
                    NewDialogue();
                }
            }
        }
    }
    
    
}
