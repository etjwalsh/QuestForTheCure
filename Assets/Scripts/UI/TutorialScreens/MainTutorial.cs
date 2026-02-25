using UnityEngine;

public class MainTutorial : MonoBehaviour
{
    public void OnButtonClicked()
    {
        GameStateMachine.instance.currentState = GameStateMachine.GameState.CharSelect;
    }
}
