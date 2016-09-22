using System;
using UnityEngine;
using System.Collections;
using VRStandardAssets.Utils;


public class Block : MonoBehaviour {

    public event Action LockOnActivatedFunctions;                                    // This event is triggered when the bar finishes filling.

    //[SerializeField] private float m_Duration = 2f;                     // The length of time it takes for the bar to fill.
    [SerializeField] private Collider m_Collider;                       // Optional reference to the Collider used to detect the user's gaze, turned off when the UIFader is not visible.
    [SerializeField] private VRInteractiveItem m_InteractiveItem;       // Reference to the VRInteractiveItem to determine when to lock on.
    [SerializeField] private GameObject m_Camera;                       // PlayerReference
    [SerializeField] private PlayerToBlocks m_Player;
    [SerializeField] private VRInput m_VRInput;                         // Reference to the VRInput to detect button presses.
    //private bool m_Activation;                                          // Whether the bar is currently filled.
    private bool m_GazeOver;                                            // Whether the user is currently looking at the bar.
    private float m_LockOnTimer;                                        // Used to determine how much of the bar should be filled.
    private Coroutine m_LockOnRoutine;                                 // Reference to the coroutine that controls the bar filling up, used to stop it if required.
    private Vector3 ObjectPos;
    private bool m_Holding;

    public enum BlockType : short {
        Tracker = 0,
        Teleporter,
        Holder
    };
    
    private Teleporter m_TeleporterScript;
    public short BlockTypeID;

    private void OnEnable()
    {
        m_VRInput.OnDown += HandleDown;
        m_VRInput.OnUp += HandleUp;
        m_InteractiveItem.OnClick += HandleClick;
        m_VRInput.OnSwipe += CheckSwipe;

        m_InteractiveItem.OnOver += HandleOver;
        m_InteractiveItem.OnOut += HandleOut;
    }

    private void OnDisable()
    {
        m_VRInput.OnDown -= HandleDown;
        m_VRInput.OnUp -= HandleUp;
        m_InteractiveItem.OnClick -= HandleClick;
        m_VRInput.OnSwipe -= CheckSwipe;

        m_InteractiveItem.OnOver -= HandleOver;
        m_InteractiveItem.OnOut -= HandleOut;
    }


    private void CheckSwipe(VRInput.SwipeDirection SwipeDir)
    {
        if ((BlockType)BlockTypeID == BlockType.Holder)
        {
            if (m_Holding == true)
            {
                switch (SwipeDir)
                {
                    case VRInput.SwipeDirection.LEFT:
                        {
                            m_InteractiveItem.transform.Translate(-transform.forward);
                            break;
                        }
                    case VRInput.SwipeDirection.RIGHT:
                        {
                            m_InteractiveItem.transform.Translate(transform.forward);
                            break;
                        }
                }
            }
        }
    }

    private void HandleClick()
    {
        if (m_GazeOver && (BlockType)BlockTypeID == BlockType.Holder)
        {
            m_Holding = !m_Holding;
           // m_InteractiveItem.GetComponent<Holder>().Activation();
        }
        else if ((BlockType)BlockTypeID == BlockType.Holder)
        {
            m_Holding = !m_Holding;
            //m_InteractiveItem.GetComponent<Holder>().Activation();
        }
        if (m_Holding == true)
        {
            m_InteractiveItem.transform.parent = m_VRInput.transform;
            m_InteractiveItem.GetComponent<Collider>().attachedRigidbody.useGravity = false;
            m_InteractiveItem.GetComponent<Collider>().attachedRigidbody.drag = Mathf.Infinity;
            m_InteractiveItem.GetComponent<Collider>().attachedRigidbody.angularDrag = Mathf.Infinity;
        }
        else
        {
            m_InteractiveItem.transform.parent = null;
            m_InteractiveItem.GetComponent<Collider>().attachedRigidbody.useGravity = true;
            m_InteractiveItem.GetComponent<Collider>().attachedRigidbody.drag = 0.0f;
            m_InteractiveItem.GetComponent<Collider>().attachedRigidbody.angularDrag = 0.05f;
        }
    }

    void Awake()
    {
        m_Player = m_Camera.GetComponent<PlayerToBlocks>();
        switch((BlockType)BlockTypeID)
        {
            case BlockType.Teleporter:
                {
                    m_TeleporterScript = m_InteractiveItem.GetComponent<Teleporter>();
                    break;
                }
            default:
                {
                    m_TeleporterScript = null;
                    break;
                }
        }
        m_Holding = false;
        float PosX = m_InteractiveItem.transform.GetComponentInParent<Transform>().position.x;
        float PosY = m_InteractiveItem.transform.position.y;
        float PosZ = m_InteractiveItem.transform.GetComponentInParent<Transform>().position.z;
        ObjectPos = Vector3.zero;
        ObjectPos.Set(PosX, PosY, PosZ);
    }

	// Use this for initialization
	void Start () {
        m_Collider.enabled = true;
        
	}
	
	// Update is called once per frame
	void Update () {
        if(m_Holding == true)
            m_InteractiveItem.transform.Rotate(Vector3.up * 90 * Time.deltaTime);

        //if (m_Player.m_PlayerLockOn == false && (BlockType)BlockTypeID == BlockType.Holder)
        //    m_InteractiveItem.transform.parent = null;
        if (m_GazeOver && m_Player.m_PlayerLockOn == true)
        {
            if ((BlockType)BlockTypeID == BlockType.Holder)
            {
            //    m_InteractiveItem.transform.parent = m_VRInput.transform;
                
            }
            else
            {
                if ((BlockType)BlockTypeID == BlockType.Teleporter &&
            m_Camera.transform.position.x > ObjectPos.x - m_InteractiveItem.transform.lossyScale.x && m_Camera.transform.position.x < ObjectPos.x + m_InteractiveItem.transform.lossyScale.x &&
            m_Camera.transform.position.z > ObjectPos.z - m_InteractiveItem.transform.lossyScale.z && m_Camera.transform.position.z < ObjectPos.z + m_InteractiveItem.transform.lossyScale.z)
                {
                    //m_TeleporterScript.TeleportPlayer(m_Camera);
                    m_Player.PlayerReset();
                }
                Vector3 DirectionMove = ObjectPos - m_VRInput.transform.position;
                //Vector3 DirectionMove = m_InteractiveItem.transform.position - m_VRInput.transform.position;
                DirectionMove.Normalize();
                m_Camera.transform.Translate(DirectionMove * Time.deltaTime * 10);
                //m_Camera.transform.position = m_InteractiveItem.transform.position;
            }
            }
            
        
	}

    private IEnumerator LockOn()
    {
        // When the bar starts to fill, reset the timer.
        m_LockOnTimer = 0f;

        // Until the timer is greater than the fill time...
        while (m_LockOnTimer < m_Player.m_PlayerLockOnDelay)
        {
            // ... add to the timer the difference between frames.
            m_LockOnTimer += Time.deltaTime;

            // Wait until next frame.
            yield return null;

            // If the user is still looking at the bar, go on to the next iteration of the loop.
            if (m_GazeOver)
                continue;

            // If the user is no longer looking at the bar, reset the timer and bar and leave the function.
            m_LockOnTimer = 0f;
            yield break;
        }
        Debug.Log("done");

        // If the loop has finished the bar is now full.
        m_Player.m_PlayerLockOn = true;
        //m_Activation = true;

        // If anything has subscribed to OnBarFilled call it now.
        if (LockOnActivatedFunctions != null)
            LockOnActivatedFunctions();
    }

    private void HandleDown()
    {
        // Debugging
        Debug.Log("User looking");
    }


    private void HandleUp()
    {
        // Debugging
        Debug.Log("user stop");
    }


    private void HandleOver()
    {
        // The user is now looking at the bar.
        m_GazeOver = true;

        // Start the LockOn if User is looking
        if (m_GazeOver)
            m_LockOnRoutine = StartCoroutine(LockOn());

        // Debugging
        Debug.Log("User is looking at Lock On");

        // Do something else
    }


    private void HandleOut()
    {
        // The user is no longer looking at the bar.
        m_GazeOver = false;

        // If the LockOn coroutine has been started, stop it.
        if (m_LockOnRoutine != null)
            StopCoroutine(m_LockOnRoutine);

        // Debugging
        Debug.Log("User have stop looking at Lock On");

        // Reset the timer and bar values.
        m_LockOnTimer = 0f;
    }
}
