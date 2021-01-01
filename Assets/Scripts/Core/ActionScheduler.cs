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
                print("Cancelling " + currentAction);
                currentAction.Cancel();
            }

            currentAction = action;
        }
    }
}