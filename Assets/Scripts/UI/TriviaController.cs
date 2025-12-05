using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TriviaController : MonoBehaviour
{
    [SerializeField] private TMP_Text questionText;
    [SerializeField] private Button[] answerButtons;
    [SerializeField] private Button exitButton;
    [SerializeField] private TMP_Text explanation;
    [SerializeField] private TMP_Text rightWrong;
    private QuestionTemplate question;
    private List<string> wrongAnswers = new List<string> { };


    // Start is called before the first frame update
    void Start()
    {
        //get the question
        question = GameStateMachine.instance.currentQuestion;

        //check what type of question it is
        if (question.questionType == "TrueFalse")
        {
            //get the wrong answer
            wrongAnswers.Add(question.incorrectAnswer1);
            LoadTrueFalseQuestion();
        }
        else if (question.questionType == "MultipleChoice")
        {
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
        exitButton.gameObject.SetActive(false);
        explanation.gameObject.SetActive(false);
        rightWrong.gameObject.SetActive(false);

        //display the question
        questionText.text = question.question;

        //put all the answers into a list
        List<string> allAnswers = new List<string>(wrongAnswers);
        allAnswers.Add(question.correctAnswer);

        //shuffle the answers
        Shuffle(allAnswers);

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

    void Shuffle<T>(List<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            (list[k], list[n]) = (list[n], list[k]);
        }
    }

    private void LoadTrueFalseQuestion()
    {
        exitButton.gameObject.SetActive(false);
        explanation.gameObject.SetActive(false);
        rightWrong.gameObject.SetActive(false);
        answerButtons[2].gameObject.SetActive(false);
        answerButtons[3].gameObject.SetActive(false);

        //display the question
        questionText.text = question.question;

        //put all the answers into a list
        List<string> allAnswers = new List<string>(wrongAnswers);
        allAnswers.Add(question.correctAnswer);

        //shuffle the answers
        Shuffle(allAnswers);

        //assign answers to different buttons
        for (int i = 0; i < allAnswers.Count; i++)
        {
            string answer = allAnswers[i];
            answerButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = answer;

            // Remove previous listeners
            answerButtons[i].onClick.RemoveAllListeners();

            // Capture variable for closure
            answerButtons[i].onClick.AddListener(() => OnAnswerSelected(answer, question.correctAnswer));
        }
    }

    private void OnAnswerSelected(string chosenAnswer, string correct)
    {
        //activate the "correct" word
        rightWrong.gameObject.SetActive(true);

        if (chosenAnswer == correct)
        {
            //show the player "correct"
            rightWrong.text = "Correct!";
        }
        else
        {
            //show the player "incorrect"
            rightWrong.text = "Incorrect!";
        }

        //move to explanation screen
        // deactivate the question
        questionText.gameObject.SetActive(false);

        //deactivate the buttons
        for (int i = 0; i < answerButtons.Length; i++)
        {
            answerButtons[i].gameObject.SetActive(false);
        }

        //activate the explanation
        explanation.gameObject.SetActive(true);
        explanation.text = question.explanation;


        //activate the exit button
        exitButton.gameObject.SetActive(true);

    }

    public void OnDoneClicked()
    {
        //change scenes
        StartCoroutine(PlayerManager.instance.LoadPlayerLocations(LevelLoader.instance.previousScene));
    }
}
