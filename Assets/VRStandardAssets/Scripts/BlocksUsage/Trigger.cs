using System;
using UnityEngine;
using System.Collections;
using VRStandardAssets.Utils;

public class Trigger : MonoBehaviour {

    public event Action LockOnActivatedFunctions;                                    // This event is triggered when the bar finishes filling.

    [SerializeField]    private VRInteractiveItem m_InteractiveItem;       // Reference to the VRInteractiveItem to determine when to lock on.
    [SerializeField]    private GameObject m_Camera;                       // PlayerReference
    
    private bool m_GazeOver;      
    private Vector3 DirectionVector;
    public float m_CoolDown;
    public TriggerActivation TriggerActivationScript;

    enum Directions
    {
        North = 0,
        East,
        South,
        West
    }
    Directions TriggerDirections;

    private void OnEnable()
    {
        m_InteractiveItem.OnClick += HandleClick;

        m_InteractiveItem.OnOver += HandleOver;
        m_InteractiveItem.OnOut += HandleOut;
    }

    private void OnDisable()
    {
        m_InteractiveItem.OnClick -= HandleClick;

        m_InteractiveItem.OnOver -= HandleOver;
        m_InteractiveItem.OnOut -= HandleOut;
    }

    private void HandleClick()
    {
        if (m_GazeOver && m_CoolDown <= 0.0f)
        {
            DirectionVector = transform.position;

            switch (TriggerDirections)
            {
                case Directions.North:
                    {
                        TriggerDirections = Directions.East;
                        DirectionVector.x = transform.position.x + 1;
                        break;
                    }
                case Directions.East:
                    {
                        TriggerDirections = Directions.South;
                        DirectionVector.z = transform.position.z - 1;
                        break;
                    }
                case Directions.South:
                    {
                        TriggerDirections = Directions.West;
                        DirectionVector.x = transform.position.x - 1;
                        break;
                    }
                case Directions.West:
                    {
                        TriggerDirections = Directions.North;
                        DirectionVector.z = transform.position.z + 1;
                        break;
                    }
            }
            TriggerActivationScript.DirectionInputTrigger((short)TriggerDirections);
            m_CoolDown = 1.0f;
        }
    }

    void Awake()
    {
        TriggerDirections = Directions.North;
        DirectionVector = transform.position;
        DirectionVector.z = transform.position.z + 1;
        m_CoolDown = 1.0f;
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
            m_CoolDown -= Time.deltaTime;
            float step = 5.0f * Time.deltaTime;
            Vector3 Direction = DirectionVector - transform.position;
            Vector3 NewDirection = Vector3.RotateTowards(transform.forward, Direction, step, 0.0f);
            transform.rotation = Quaternion.LookRotation(NewDirection);
	}

    private void HandleOver()
    {
        // The user is now looking at the bar.
        m_GazeOver = true;

        // Debugging
        Debug.Log("User is looking at Lock On");

    }


    private void HandleOut()
    {
        // The user is no longer looking at the bar.
        m_GazeOver = false;

        // Debugging
        Debug.Log("User have stop looking at Lock On");

    }
}
