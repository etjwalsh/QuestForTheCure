using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuestionTemplate", menuName = "Game/Question")]

public class QuestionTemplate : ScriptableObject
{
    //variables for question content
    [TextArea(3, 10)]
    public string question;
    public string correctAnswer;
    public string incorrectAnswer1;
    public string incorrectAnswer2;
    public string incorrectAnswer3;
    public string questionRole;
}
