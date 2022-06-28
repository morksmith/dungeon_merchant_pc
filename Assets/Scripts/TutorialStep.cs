using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TutorialStep : MonoBehaviour
{
    public List<UnityEvent> StepEvents;
    [TextArea(5, 10)]
    public string StepText;
    public GameObject NextButton;
    public float TextboxPosition = 128;
}
