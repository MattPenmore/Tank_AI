using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIPanel : MonoBehaviour
{
    public Text[] states;
    public Text action;
    GameObject plannerTank;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        plannerTank = GameObject.FindGameObjectWithTag("PlannerTank");
        //Get world states of planner tank and print as text
        int i = 0;
        foreach(KeyValuePair<string, int> pair in GWorld.world.states)
        {
            if(states.Length > i)
            {
                states[i].text = pair.Key + " , " + pair.Value.ToString();
                i++;
            }
        }

        //Display planner tanks current action
        if(GameObject.FindGameObjectWithTag("PlannerTank"))
        {
            if(GameObject.FindGameObjectWithTag("PlannerTank").GetComponent<PlannerTank>().currentAction != null)
            {
                action.text = "Action: " + GameObject.FindGameObjectWithTag("PlannerTank").GetComponent<PlannerTank>().currentAction.actionName;
            }
            else
            {
                action.text = "Action: NULL";
            }
        }
        else
        {
            action.text = "Action: NULL";
        }
    }
}
