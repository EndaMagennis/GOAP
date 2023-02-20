using System.Collections.Generic;
using UnityEngine;
using System.Linq;

// Class to represent a node in a graph, used for constructing a plan.
public class Node
{
    // Parent node of the current node.
    public Node parent;
    // Cost of reaching this node.
    public float cost;
    // Current state represented by this node.
    public Dictionary<string, int> state;
    // Action performed to reach this node.
    public GAction action;

    // Constructor for the node, taking in a parent node, cost, current state and the action performed to reach this node.
    public Node(Node parent, float cost, Dictionary<string, int> allStates, GAction action)
    {
        // Set the parent node.
        this.parent = parent;
        // Set the cost.
        this.cost = cost;
        // Initialize the state.
        this.state = new Dictionary<string, int>(allStates);
        // Set the action.
        this.action = action;
    }

    // Overloaded constructor for the node, taking in a parent node, cost, current state, belief state, and the action performed to reach this node.
    public Node(Node parent, float cost, Dictionary<string, int> allStates, Dictionary<string, int> beliefStates, GAction action)
    {
        // Set the parent node.
        this.parent = parent;
        // Set the cost.
        this.cost = cost;
        // Initialize the state.
        this.state = new Dictionary<string, int>(allStates);
        // Add the belief state to the current state.
        foreach (KeyValuePair<string, int> b in beliefStates)
        {
            // Check if the current state already contains the belief state key.
            if (!this.state.ContainsKey(b.Key))
                // If not, add the belief state key-value pair to the current state.
                this.state.Add(b.Key, b.Value);
        }
        // Set the action.
        this.action = action;
    }
}

// Class to represent a planner that can find a plan based on a set of possible actions and a goal state.
public class GPlanner
{
    // Method to return a plan based on a list of possible actions, a goal state, and belief state.
    public Queue<GAction> plan(List<GAction> actions, Dictionary<string, int> goal, WorldStates beliefStates)
    {
        // List to hold the achievable actions.
        List<GAction> usableActions = new List<GAction>();
        // Loop through all actions and add the achievable actions to the list.
        foreach (GAction a in actions)
        {
            if (a.IsAchievable())
                usableActions.Add(a);
        }

        // List to hold the leaves of the graph (i.e., the nodes that have achieved the goal state).
        List<Node> leaves = new List<Node>();
        // Initialize the starting node, with the current world state, belief state, and no action performed yet.
        Node start = new Node(null, 0, GWorld.Instance.GetWorld().GetStates(), beliefStates.GetStates(), null);

        // Build the graph to find a plan.
        bool success = BuildGraph(start, leaves, usableActions, goal);

        if (!success)
        {
            Debug.Log("NO PLAN");
            return null;
        }

        Node cheapest = null;
        foreach (Node leaf in leaves)
        {
            if (cheapest == null)
                cheapest = leaf;
            else if (leaf.cost < cheapest.cost)
                cheapest = leaf;
        }
        List<GAction> result = new List<GAction>();
        Node n = cheapest;
        while(n != null)
        {
            if(n.action != null)
            {
                result.Insert(0, n.action);
            }
            n = n.parent;
        }
        Queue<GAction> queue = new Queue<GAction>();
        foreach( GAction a in result)
        {
            queue.Enqueue(a);
        }
        Debug.Log("The plan is: ");
        foreach(GAction a in queue)
        {
            Debug.Log("Q: " + a.actionName);
        }
        return queue;
    }

    private bool BuildGraph(Node parent, List<Node> leaves, List<GAction> usuableActions, Dictionary<string, int> goal)
    {
        bool foundPath = false;
        foreach(GAction action in usuableActions)
        {
            if (action.IsAchievableGiven(parent.state))
            {
                Dictionary<string, int> currentState = new Dictionary<string, int>(parent.state);
                foreach(KeyValuePair<string, int> eff in action.effects)
                {
                    if(!currentState.ContainsKey(eff.Key))
                        currentState.Add(eff.Key, eff.Value);
                }

                Node node = new Node(parent, parent.cost + action.cost, currentState, action);

                if(GoalAchieved(goal, currentState))
                {
                    leaves.Add(node);
                    foundPath = true;
                }
                else
                {
                    List<GAction> subset = ActionSubset(usuableActions, action);
                    bool found = BuildGraph(node, leaves, subset, goal);
                    if (found)
                        foundPath = true;
                }
            }
        }
        return foundPath;
    }

    private bool GoalAchieved(Dictionary<string, int> goal, Dictionary<string, int> state)
    {
        foreach(KeyValuePair<string, int> g in goal)
        {
            if (!state.ContainsKey(g.Key))
                return false;
        }
        return true;
    }
    
    private List<GAction> ActionSubset(List<GAction> actions, GAction removeMe)
    {
        List<GAction> subset = new List<GAction>();
        foreach(GAction a in actions)
        {
            if(!a.Equals(removeMe))
                subset.Add(a);
        }
        return subset;
    }
}
