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
    public int CurrentDialogueIndexYouBigBaby = 0;
    public GameObject StoryButton;

    private void Start()
    {
        //CurrentDialogue = PlayerPrefs.GetInt("Dialogue Step");
    }

    public void StartDialogue ()
    {
        DialoguePanel.SetActive(true);
        if(Dialogues[CurrentDialogueIndexYouBigBaby] != null)
        {
            TextBox.StartDialogue(Dialogues[CurrentDialogueIndexYouBigBaby]);
        }
    }

    public void NewDialogue()
    {
        BottomContent.NewHeroIcon();
        StoryButton.SetActive(true);
        Dialogues[CurrentDialogueIndexYouBigBaby].HasPlayed = true;
    }

    public void IterateDialogue(float currentProfit)
    {
        //if(p > ProfitSteps[CurrentDialogue])
        //{
        //    NewDialogue();
        //    CurrentDialogue++;
        //    if(p> ProfitSteps[CurrentDialogue])
        //    {

        //    }
        //}
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
                    CurrentDialogueIndexYouBigBaby = previousIndex;

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
