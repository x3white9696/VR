using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using VRStandardAssets.Utils;

public class Mover : MonoBehaviour
{

    [SerializeField]
    private VRInteractiveItem m_InteractiveItem;       // Reference to the VRInteractiveItem to determine when to lock on.
    [SerializeField]
    private GameObject m_Camera;                       // PlayerReference

    public Vector3 DestinationPosition;
    public Vector3 M_camera;
    public Vector3 InitialPosition;
    public bool Moving;
    public bool inside;
    public float Cooldown;
    public float Speed;
    public bool Activation;

    void Awake()
    {
        Activation = true;
    }

    // Use this for initialization
    void Start()
    {
        inside = false;
        Moving = false;
        Cooldown = 1.0f;
        InitialPosition = m_InteractiveItem.transform.position;
        M_camera = m_Camera.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Activation == true)
        {
            if (Cooldown > 0.0f)
                Cooldown -= Time.deltaTime;

            if (m_Camera.transform.position.x > m_InteractiveItem.transform.position.x - m_InteractiveItem.transform.localScale.x * 0.25 && m_Camera.transform.position.x < m_InteractiveItem.transform.position.x + m_InteractiveItem.transform.localScale.x * 0.25 &&
                m_Camera.transform.position.z > m_InteractiveItem.transform.position.z - m_InteractiveItem.transform.localScale.z * 0.25 && m_Camera.transform.position.z < m_InteractiveItem.transform.position.z + m_InteractiveItem.transform.localScale.z * 0.25 &&
                m_Camera.transform.position.y > m_InteractiveItem.transform.position.y + m_InteractiveItem.transform.localScale.y * 0.5 && m_Camera.transform.position.y < m_InteractiveItem.transform.position.y + m_InteractiveItem.transform.localScale.y * 1.5)
            {
                inside = true;
                if (inside == true && Moving == false && Cooldown <= 0.0f)
                {
                    Moving = true;
                    m_Camera.transform.parent = m_InteractiveItem.transform;
                }
                else
                {
                    Moving = false;
                }
            }
            else
            {
                if (m_Camera.transform.position.x < m_InteractiveItem.transform.position.x - m_InteractiveItem.transform.localScale.x * 0.5 || m_Camera.transform.position.x > m_InteractiveItem.transform.position.x + m_InteractiveItem.transform.localScale.x * 0.5 ||
                    m_Camera.transform.position.z < m_InteractiveItem.transform.position.z - m_InteractiveItem.transform.localScale.z * 0.5 || m_Camera.transform.position.z > m_InteractiveItem.transform.position.z + m_InteractiveItem.transform.localScale.z * 0.5 ||
                    m_Camera.transform.position.y > m_InteractiveItem.transform.position.y + m_InteractiveItem.transform.localScale.y * 0.5 && m_Camera.transform.position.y < m_InteractiveItem.transform.position.y + m_InteractiveItem.transform.localScale.y)
                {
                    inside = false;
                }
            }

            if (m_InteractiveItem.transform.position == DestinationPosition)
            {
                if (inside == true)
                {
                    Cooldown = 1.0f;
                }
                if (inside == false)
                {
                    Vector3 holder = DestinationPosition;
                    DestinationPosition = InitialPosition;
                    InitialPosition = holder;
                }

                m_Camera.transform.parent = null;
                Moving = false;
            }



            if (Moving == true)
            {
                m_Camera.GetComponent<PlayerToBlocks>().PlayerReset();
                m_InteractiveItem.transform.position = Vector3.MoveTowards(m_InteractiveItem.transform.position, DestinationPosition, Speed * Time.deltaTime);
            }
        }
    }
}
