using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TriviaController : MonoBehaviour
{
    [SerializeField] private TMP_Text q1;
    [SerializeField] private TMP_Text q2;
    [SerializeField] private TMP_Text q3;
    [SerializeField] private TMP_Text q4;
    [SerializeField] private TMP_Text questionText;


    // Start is called before the first frame update
    void Start()
    {
        LoadMultipleChoiceQuestion();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void LoadMultipleChoiceQuestion()
    {
        q1.text = GameStateMachine.instance.currentQuestion.incorrectAnswer1;
        q2.text = GameStateMachine.instance.currentQuestion.incorrectAnswer2;
        q3.text = GameStateMachine.instance.currentQuestion.incorrectAnswer3;
        q4.text = GameStateMachine.instance.currentQuestion.correctAnswer;
        questionText.text = GameStateMachine.instance.currentQuestion.question;
    }
}
