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
        Dialogues[CurrentDialogue].HasPlayed = true;

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
        Dialogues[CurrentDialogue].HasPlayed = true;
    }

    public void IterateDialogue(float currentProfit)
    {
        
        for (var i = 0; i < ProfitSteps.Count; i++)
        {
            if(currentProfit < ProfitSteps[i])
            {
                //Before we do this, check if i-1 exists.
                var previousIndex = i - 1;
                if(previousIndex < 0)
                {
                    //We've not hit any breakpoint yet. Break;
                    break;
                }


                if(currentProfit > ProfitSteps[previousIndex])
                {
                    //Great, we've hit the closest breakpoint over the amount: i -1
                    CurrentDialogue = previousIndex;

                    if (!Dialogues[previousIndex].HasPlayed)
                    {
                        NewDialogue();
                        break;
                    }
                }
            }

            
        }
    }
    
    
}
