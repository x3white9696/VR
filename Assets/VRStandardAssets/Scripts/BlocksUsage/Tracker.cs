using System;
using UnityEngine;
using System.Collections;
using VRStandardAssets.Utils;

public class Tracker : MonoBehaviour
{

    public event Action LockOnActivatedFunctions;                                    // This event is triggered when the bar finishes filling.

    [SerializeField]    private VRInteractiveItem m_InteractiveItem;       // Reference to the VRInteractiveItem to determine when to lock on.
    [SerializeField]    private GameObject m_Camera;                       // PlayerReference
    [SerializeField]    private PlayerToBlocks m_Player;
    [SerializeField]    private VRInput m_VRInput;                         // Reference to the VRInput to detect button presses.

    private bool m_GazeOver;                                            // Whether the user is currently looking at the bar.
    public bool m_Selected;

    private float m_LockOnTimer;                                        // Used to determine how much of the bar should be filled.
    private Coroutine m_LockOnRoutine;                                 // Reference to the coroutine that controls the bar filling up, used to stop it if required.
    public Vector3 Position;

    public bool Activation;

    private void OnEnable()
    {
        m_InteractiveItem.OnOver += HandleOver;
        m_InteractiveItem.OnOut += HandleOut;
    }

    private void OnDisable()
    {
        m_InteractiveItem.OnOver -= HandleOver;
        m_InteractiveItem.OnOut -= HandleOut;
    }

    void Awake()
    {
        Activation = true;
        m_Player = m_Camera.GetComponent<PlayerToBlocks>();
        m_VRInput = m_Camera.GetComponentInChildren<VRInput>();
        Position = m_VRInput.transform.position;
        m_Selected = false;
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Activation == true)
        {
            if (m_Player.m_PlayerLockOnPosition == m_InteractiveItem.transform.position)
                m_Selected = true;
            else
                m_Selected = false;

            if (m_Player.m_PlayerLockOn == true && m_Selected == true)
            {

                //Vector3 DirectionMove = m_Player.m_PlayerLockOnPosition - m_VRInput.transform.position;
                //DirectionMove.y = 0.0f;
                ////Vector3 DirectionMove = m_InteractiveItem.transform.position - m_VRInput.transform.position;
                //DirectionMove.Normalize();
                //Debug.Log(DirectionMove);
                //m_Camera.transform.Translate(DirectionMove * Time.deltaTime * 8.0f);


                float Herp = 5.0f;
                Vector3 Crap = m_Player.m_PlayerLockOnPosition;
                Crap.y = m_Player.m_PlayerLockOnPosition.y + Herp;


                float Distance = Vector3.Distance(m_VRInput.transform.position, Crap);

                if (m_Camera.transform.position.x == Crap.x && m_Camera.transform.position.z == Crap.z)
                {
                    if (Distance < 0.5f)
                        Crap.y = m_Camera.transform.position.y;

                    m_Camera.transform.position = Vector3.MoveTowards(m_Camera.transform.position, Crap, Time.deltaTime * 8.0f);
                }
                else
                    m_Camera.transform.position = Vector3.MoveTowards(m_Camera.transform.position, Crap, Time.deltaTime * 8.0f);


                // Vector3 Size = Vector3.zero;
                // Size.x = m_InteractiveItem.transform.localScale.x * 0.5f;
                // Size.y = m_InteractiveItem.transform.localScale.y * 0.5f;
                // Size.z = m_InteractiveItem.transform.localScale.z * 0.5f;

                // Derp = Vector3.Distance(m_Player.m_PlayerLockOnPosition + Size, m_VRInput.transform.position);
                // if (Derp < 10.0f)
                //     m_InteractiveItem.GetComponent<Collider>().enabled = false;
                //m_Camera.transform.position = m_InteractiveItem.transform.position;
            }
            //else
            //  m_InteractiveItem.GetComponent<Collider>().enabled = true;
        }
    }

    private void HandleOver()
    {
        // The user is now looking at the bar.
        m_GazeOver = true;

        // Start the LockOn if User is looking
        if (m_GazeOver)
            m_LockOnRoutine = StartCoroutine(LockOn());

        if (Vector3.Distance(m_Camera.transform.position, m_InteractiveItem.transform.position) < 50.0f)
        m_Player.m_PlayerLockOnPosition = m_InteractiveItem.transform.position;

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
}
