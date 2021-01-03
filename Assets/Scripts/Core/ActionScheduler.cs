using UnityEngine;

namespace RPG.Core
{
    public class ActionScheduler : MonoBehaviour 
    {
        IAction currentAction;

        // only one action can be running 
        // (e.g. if player moves, then cannot fight and vice versa)
        public void StartAction(IAction action)
        {
            if (currentAction == action) return;

            if (currentAction != null) {
                currentAction.Cancel();
            }

            currentAction = action;
        }

        public void CancelCurrentAction()
        {
            if (currentAction != null) {
                currentAction.Cancel();
            }
            
            currentAction = null;
        }
    }
}