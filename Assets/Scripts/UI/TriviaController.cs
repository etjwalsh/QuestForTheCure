using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TriviaController : MonoBehaviour
{
    [SerializeField] private TMP_Text a1;
    [SerializeField] private TMP_Text a2;
    [SerializeField] private TMP_Text a3;
    [SerializeField] private TMP_Text a4;
    [SerializeField] private TMP_Text questionText;


    // Start is called before the first frame update
    void Start()
    {
        //check what type of question it is
        if (GameStateMachine.instance.currentQuestion.questionType == "TrueFalse")
        {
            LoadTrueFalseQuestion();
        }
        else if (GameStateMachine.instance.currentQuestion.questionType == "MultipleChoice")
        {
            LoadMultipleChoiceQuestion();
        }
    }

    private void LoadMultipleChoiceQuestion()
    {
        Debug.Log("current question is: " + GameStateMachine.instance.currentQuestion);
        a1.text = GameStateMachine.instance.currentQuestion.incorrectAnswer1;
        a2.text = GameStateMachine.instance.currentQuestion.incorrectAnswer2;
        a3.text = GameStateMachine.instance.currentQuestion.incorrectAnswer3;
        a4.text = GameStateMachine.instance.currentQuestion.correctAnswer;
        questionText.text = GameStateMachine.instance.currentQuestion.question;
    }

    private void LoadTrueFalseQuestion()
    {
        Debug.Log("current question is: " + GameStateMachine.instance.currentQuestion);
        a1.text = GameStateMachine.instance.currentQuestion.correctAnswer;
        a2.text = GameStateMachine.instance.currentQuestion.incorrectAnswer1;
    }
}
