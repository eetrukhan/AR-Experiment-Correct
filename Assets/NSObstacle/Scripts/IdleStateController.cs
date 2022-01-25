using UnityEngine;

public class IdleStateController : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Transform bar = animator.transform.Find("Bar");
        if (bar == null)
            throw new UnityException("IdleStateController couldn't find a child object named 'Bar'");

        bar.gameObject.SetActive(false);
    }
}
