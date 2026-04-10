using UnityEngine;

public class AboutUI : MonoBehaviour
{
    void Awake()
    {
        //check for discovery
        if (GameStateMachine.instance.currentStage == "Discovery" && GameStateMachine.instance.discoveryVisited)
        {
            Destroy(gameObject);
        }
        else if (GameStateMachine.instance.currentStage == "Discovery" && !GameStateMachine.instance.discoveryVisited)
        {
            GameStateMachine.instance.discoveryVisited = true;
        }

        //check for preclinical
        if (GameStateMachine.instance.currentStage == "PreClinical" && GameStateMachine.instance.preClinicalVisited)
        {
            Destroy(gameObject);
        }
        else if (GameStateMachine.instance.currentStage == "PreClinical" && !GameStateMachine.instance.preClinicalVisited)
        {
            GameStateMachine.instance.preClinicalVisited = true;
        }

        //check for clinical
        if (GameStateMachine.instance.currentStage == "Clinical" && GameStateMachine.instance.clinicalVisited)
        {
            Destroy(gameObject);
        }
        else if (GameStateMachine.instance.currentStage == "Clinical" && !GameStateMachine.instance.clinicalVisited)
        {
            GameStateMachine.instance.clinicalVisited = true;
        }

        //check for approval
        if (GameStateMachine.instance.currentStage == "Approval" && GameStateMachine.instance.approvalVisited)
        {
            Destroy(gameObject);
        }
        else if (GameStateMachine.instance.currentStage == "Approval" && !GameStateMachine.instance.approvalVisited)
        {
            GameStateMachine.instance.approvalVisited = true;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Destroy(gameObject);
        }
    }

    public void OnEnterClicked()
    {
        Destroy(gameObject);
    }
}
