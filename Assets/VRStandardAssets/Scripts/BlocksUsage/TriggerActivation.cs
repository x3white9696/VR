using UnityEngine;
using System.Collections;

public class TriggerActivation : MonoBehaviour {

    public short ScriptID;
    public short DirectionFacedForActivation;
	// Use this for initialization
    void Start() {
        switch(ScriptID)
        {
            case 1:
                {
                    transform.GetComponent<Tracker>().Activation = false;
                    break;
                }
            case 2:
                {
                    transform.GetComponent<Teleporter>().Activation = false;
                    break;
                }
            case 3:
                {
                    transform.GetComponent<Mover>().Activation = false;
                    break;
                }
        }
    }

    public void DirectionInputTrigger(short Input)
    {

        bool DirectionInput;

        if (DirectionFacedForActivation == Input)
        {
            DirectionInput = true;
        }
        else
        {
            DirectionInput = false;
        }

        switch (ScriptID)
        {
            case 1:
                {
                    transform.GetComponent<Tracker>().Activation = DirectionInput;
                    break;
                }
            case 2:
                {
                    transform.GetComponent<Teleporter>().Activation = DirectionInput;
                    break;
                }
            case 3:
                {
                    transform.GetComponent<Mover>().Activation = DirectionInput;
                    break;
                }
        }
    }

}
