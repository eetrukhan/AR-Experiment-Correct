using UnityEngine;

public class DoneStateController : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Fuse3DButton fuseButton;
        if (animator.transform.parent == null || (fuseButton = animator.transform.parent.GetComponent<Fuse3DButton>()) == null)
            throw new UnityException("DoneStateController couldn't find a parent object or a 'Fuse3DButton' component in it");

        fuseButton.FireButtonPressed();
    }
}
