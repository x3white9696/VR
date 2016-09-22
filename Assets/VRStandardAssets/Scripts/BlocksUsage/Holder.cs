using UnityEngine;
using System.Collections;
using VRStandardAssets.Utils;

public class Holder : MonoBehaviour {

    [SerializeField]    private VRInteractiveItem m_InteractiveItem;       // Reference to the VRInteractiveItem to determine when to lock on.
    [SerializeField]    private GameObject m_Camera;                       // PlayerReference
    [SerializeField]    private VRInput m_VRInput;                         // Reference to the VRInput to detect button presses.
    private bool m_GazeOver;                                            // Whether the user is currently looking at the bar.
    private bool m_Holding;


    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "9")
        {
            Debug.Log("Floor");

            m_InteractiveItem.transform.parent = null;
            m_InteractiveItem.GetComponent<Collider>().attachedRigidbody.useGravity = true;
            m_InteractiveItem.GetComponent<Collider>().attachedRigidbody.drag = 0.0f;
            m_InteractiveItem.GetComponent<Collider>().attachedRigidbody.angularDrag = 0.05f;
        }
    }
    

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
        if (m_GazeOver == true)
            m_Holding = !m_Holding;
        else
            m_Holding = false;

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
        m_VRInput = m_Camera.GetComponentInChildren<VRInput>();
    }

	// Use this for initialization
	void Start () {
        m_Holding = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private void HandleOver()
    {
        // The user is now looking at the bar.
        m_GazeOver = true;


        // Debugging
        Debug.Log("User is looking at Lock On");

        // Do something else
    }


    private void HandleOut()
    {
        // The user is no longer looking at the bar.
        m_GazeOver = false;


        // Debugging
        Debug.Log("User have stop looking at Lock On");

    }
}
