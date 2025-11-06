using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuestionTemplate", menuName = "Game/Question")]

public class QuestionTemplate : ScriptableObject
{
    //variables for question content
    [TextArea(3, 10)]
    public string question;
    [TextArea(3, 10)]
    public string explanation;
    [TextArea(1, 10)]
    public string correctAnswer;
    [TextArea(1, 10)]
    public string incorrectAnswer1;
    [TextArea(1, 10)]
    public string incorrectAnswer2;
    [TextArea(1, 10)]
    public string incorrectAnswer3;
    public string questionRole;
    public string questionType;
    public string questionStage;
}
