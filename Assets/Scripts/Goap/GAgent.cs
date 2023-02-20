using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SubGoal
{
    public Dictionary<string, int> sGoals;
    public bool remove; 

    public SubGoal(string s, int i, bool r)
    {
        sGoals = new Dictionary<string, int>();
        sGoals.Add(s, i);
        remove = r;
    }

}

public class GAgent : MonoBehaviour
{
    public List<GAction> actions = new List<GAction>();
    public Dictionary<SubGoal, int> goals = new Dictionary<SubGoal, int> ();
    public GInventory inventory = new GInventory ();
    public WorldStates beliefs = new WorldStates ();// to be used by individual agents, rather than ass a world state

    GPlanner planner;
    Queue<GAction> actionQueue;
    public GAction currentAction;
    SubGoal currenGoal;

    protected virtual void Start()
    {
        GAction[] acts = this.GetComponents<GAction> ();
        foreach (GAction a in acts)
            actions.Add(a);
    }

    bool invoked = false;
    void CompleteAction()
    {
        currentAction.running = false;
        currentAction.PostPerform();
        invoked = false;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(currentAction != null && currentAction.running)
        {
            float distanceToTarget = Vector3.Distance(currentAction.target.transform.position, this.transform.position);
            if(currentAction.agent.hasPath && distanceToTarget < 2f)// currentAction.agent.remainingDistance <1.0f;
            {
                if (!invoked)
                {
                    Invoke("CompleteAction", currentAction.duration);
                    invoked = true;
                }
            }
            return;
        }
        //checking if a planner or queue exists for agent
        if (planner == null || actionQueue == null)
        {
            //if not, create new planner
            planner = new GPlanner();
            // sorts goals based on cost
            var sortedGoals = from entry in goals orderby entry.Value descending select entry;

            foreach(KeyValuePair<SubGoal, int> sg in sortedGoals)
            {
                actionQueue = planner.plan(actions, sg.Key.sGoals, beliefs);
                if(actionQueue != null)
                {
                    currenGoal = sg.Key;
                    break;
                }
            }
        }

        if(actionQueue != null && actionQueue.Count == 0)
        {
            if (currenGoal.remove)
            {
                goals.Remove(currenGoal);
            }
            planner = null;
        }

        if(actionQueue != null && actionQueue.Count > 0)
        {
            currentAction = actionQueue.Dequeue();
            if (currentAction.PrePerform())
            {
                if (currentAction.target == null && currentAction.targetTag != "")
                    currentAction.target = GameObject.FindWithTag(currentAction.targetTag);

                if(currentAction.target != null)
                {
                    currentAction.running = true;
                    currentAction.agent.SetDestination(currentAction.target.transform.position);
                }
            }
            else
            {
                actionQueue = null;
            }
        }
    }
}
