using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TriviaController : MonoBehaviour
{
    [SerializeField] private TMP_Text questionText;
    [SerializeField] private Button[] answerButtons;
    private string correctAnswer;
    private QuestionTemplate question;
    private List<string> wrongAnswers = new List<string> { };


    // Start is called before the first frame update
    void Start()
    {
        //get the question
        question = GameStateMachine.instance.currentQuestion;
        Debug.Log("question incorrect answer 1 is:");
        Debug.Log(question.incorrectAnswer1);

        //check what type of question it is
        if (question.questionType == "TrueFalse")
        {
            //get the wrong answer
            wrongAnswers.Add(question.incorrectAnswer1);
            LoadTrueFalseQuestion();
        }
        else if (question.questionType == "MultipleChoice")
        {
            Debug.Log("about to print the wrong answer list");
            Debug.Log(wrongAnswers);

            Debug.Log("about to print question.incorrect answer1");
            Debug.Log(question.incorrectAnswer1);

            //get the wrong answers
            wrongAnswers.Add(question.incorrectAnswer1);
            wrongAnswers.Add(question.incorrectAnswer2);
            wrongAnswers.Add(question.incorrectAnswer3);

            //load the question
            LoadMultipleChoiceQuestion();
        }
    }

    private void LoadMultipleChoiceQuestion()
    {
        //display the question
        questionText.text = question.question;

        //put all the answers into a list
        List<string> allAnswers = new List<string>(wrongAnswers);
        allAnswers.Add(question.correctAnswer);

        //shuffle the answers
        for (int i = 0; i < allAnswers.Count; i++)
        {
            int rand = Random.Range(i, allAnswers.Count);
            (allAnswers[i], allAnswers[rand]) = (allAnswers[rand], allAnswers[i]);
        }
        
        //assign answers to different buttons
        for (int i = 0; i < answerButtons.Length; i++)
        {
            string answer = allAnswers[i];
            answerButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = answer;

            // Remove previous listeners
            answerButtons[i].onClick.RemoveAllListeners();

            // Capture variable for closure
            answerButtons[i].onClick.AddListener(() => OnAnswerSelected(answer, question.correctAnswer));
        }
    }

    private void LoadTrueFalseQuestion()
    {

    }

    private void OnAnswerSelected(string chosenAnswer, string correct)
    {
        if (chosenAnswer == correct)
        {
            Debug.Log("✅ Correct!");
        }
        else
        {
            Debug.Log("❌ Wrong!");
        }
    }
}
