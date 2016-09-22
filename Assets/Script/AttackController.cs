using UnityEngine;
using System.Collections;

public class AttackController : MonoBehaviour {

	// Use this for initialization
    public int AmountOfAttackPole;
    public float AttackEach;
    public float AttackCooldown;
    public Component[] AttackSript;
    public int PreviousID;

	void Start () {
        AttackEach = 0.0f;
        AmountOfAttackPole = 0;
        AttackCooldown = 5.0f;
        AttackSript = GetComponentsInChildren<AttackScript>();
        PreviousID = -1;
	}
	
	// Update is called once per frame
	void Update () {
        AttackEach += Time.deltaTime;
        if (AmountOfAttackPole < transform.childCount)
        {
            foreach (AttackScript m_AttackScript in AttackSript)
            {
                if (AttackEach > 0.05f && m_AttackScript.OrderFromController == false && PreviousID + 1 == m_AttackScript.ID)
                {
                    m_AttackScript.OrderFromController = true;
                    AttackEach = 0.0f;
                    AmountOfAttackPole++;
                    PreviousID = m_AttackScript.ID;
                    break;
                }
            }
        }
        else
        {
            AttackCooldown -= Time.deltaTime;
            if(AttackCooldown < 0.0f)
            {
                PreviousID = -1;
                AttackEach = 0.0f;
                AttackCooldown = 5.0f;
                AmountOfAttackPole = 0;
            }
        }
        
	}
}
