using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using VRStandardAssets.Utils;

public class Selection : MonoBehaviour {

    public event Action OnBarFilled;                                    // This event is triggered when the bar finishes filling.

    [SerializeField] private float m_Duration = 2f;                     // The length of time it takes for the bar to fill.
    [SerializeField] private Collider m_Collider;                       // Optional reference to the Collider used to detect the user's gaze, turned off when the UIFader is not visible.
    [SerializeField] private VRInteractiveItem m_InteractiveItem;       // Reference to the VRInteractiveItem to determine when to fill the bar.
    [SerializeField] private GameObject m_Camera;
    [SerializeField] private VRInput m_VRInput;                         // Reference to the VRInput to detect button presses.
    [SerializeField] private bool m_DisableOnBarFill;                   // Whether the bar should stop reacting once it's been filled (for single use bars).
    [SerializeField] private bool m_DisappearOnBarFill;                 // Whether the bar should disappear instantly once it's been filled.

    private bool m_Activation;                                           // Whether the bar is currently filled.
    private bool m_GazeOver;                                            // Whether the user is currently looking at the bar.
    private float m_Timer;                                              // Used to determine how much of the bar should be filled.
    private Coroutine m_FillBarRoutine;                                 // Reference to the coroutine that controls the bar filling up, used to stop it if required.

    private void OnEnable()
    {
        m_VRInput.OnDown += HandleDown;
        m_VRInput.OnUp += HandleUp;

        m_InteractiveItem.OnOver += HandleOver;
        m_InteractiveItem.OnOut += HandleOut;

    }


    private void OnDisable()
    {
        m_VRInput.OnDown -= HandleDown;
        m_VRInput.OnUp -= HandleUp;

        m_InteractiveItem.OnOver -= HandleOver;
        m_InteractiveItem.OnOut -= HandleOut;
    }

	// Use this for initialization
	void Start () {
        m_Collider.enabled = true;
	}
	
	// Update is called once per frame
	void Update () {
        if (m_GazeOver)
        {
            Vector3 DirectionMove = m_InteractiveItem.transform.position - m_VRInput.transform.position;
            
            //Vector3 DirectionMove = m_InteractiveItem.transform.position - m_VRInput.transform.position;
            DirectionMove.Normalize();
            //m_Camera.transform.Translate(DirectionMove * Time.deltaTime * 10);
            m_Camera.transform.position = m_InteractiveItem.transform.position;
        }
	}

    public IEnumerator WaitForBarToFill()
    {
        // Currently the bar is unfilled.
        m_Activation = false;

        // Reset the timer and set the slider value as such.
        m_Timer = 0f;
        Debug.Log("waiting");
        // Keep coming back each frame until the bar is filled.
        while (!m_Activation)
        {
            yield return null;
        }
    }


    private IEnumerator FillBar()
    {
        // When the bar starts to fill, reset the timer.
        m_Timer = 0f;

       

        // Until the timer is greater than the fill time...
        while (m_Timer < 2.0f)
        {
            // ... add to the timer the difference between frames.
            m_Timer += Time.deltaTime;

            // Wait until next frame.
            yield return null;

            // If the user is still looking at the bar, go on to the next iteration of the loop.
            if (m_GazeOver)
                continue;

            // If the user is no longer looking at the bar, reset the timer and bar and leave the function.
            m_Timer = 0f;
            yield break;
        }
        Debug.Log("done");
        // If the loop has finished the bar is now full.
        m_Activation = true;
        m_InteractiveItem.transform.position.Set(m_InteractiveItem.transform.position.x, 0.5f, m_InteractiveItem.transform.position.z);

        // If anything has subscribed to OnBarFilled call it now.
        if (OnBarFilled != null)
            OnBarFilled();

        // If the bar should be disabled once it is filled, do so now.
        if (m_DisableOnBarFill)
            enabled = false;
    }

    private void HandleDown()
    {

        Debug.Log("userlooking");
        // If the user is looking at the bar start the FillBar coroutine and store a reference to it.
        if (m_GazeOver)
            m_FillBarRoutine = StartCoroutine(FillBar());
    }


    private void HandleUp()
    {
        // If the coroutine has been started (and thus we have a reference to it) stop it.
        if (m_FillBarRoutine != null)
            StopCoroutine(m_FillBarRoutine);
        Debug.Log("user stop");
        // Reset the timer and bar values.
        m_Timer = 0f;
    }


    private void HandleOver()
    {
        // The user is now looking at the bar.
        m_GazeOver = true;
        Debug.Log("userIslooking");
        // Play the clip appropriate for when the user starts looking at the bar.
    }


    private void HandleOut()
    {
        // The user is no longer looking at the bar.
        m_GazeOver = false;

        // If the coroutine has been started (and thus we have a reference to it) stop it.
        if (m_FillBarRoutine != null)
            StopCoroutine(m_FillBarRoutine);
        Debug.Log("userhavestop");
        // Reset the timer and bar values.
        m_Timer = 0f;
    }
}
