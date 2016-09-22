using UnityEngine;
using System.Collections;

public class AttackScript : MonoBehaviour {

	public GameObject PlayerObject;
    public int ID;
    
    public float StandbySpeed;
    public float AttackSpeed;

    public float CoolDown;
    public float AttackTime;
    public float step;

	public bool TakePlayerPositionOnce;
    public bool OrderFromController;

    public Vector3 initialPosition;
    public Vector3 PlayerDirection;

   
    enum AI_STATE {
        IDLE_STATE = 0,
        STANDBY_STATE,
        ATTACK_STATE,
        AOE_ATTACK_STATE,
        DEAD_STATE
    }

    AI_STATE AttackWeapon_State;
	// Use this for initialization
	void Start () {
        CoolDown = 1.0f;
        initialPosition = transform.position;
        OrderFromController = false;
		TakePlayerPositionOnce = false;
        StandbySpeed = 5.0f;
        AttackSpeed = 50.0f;
		AttackTime = 0.0f;
        AttackWeapon_State = AI_STATE.IDLE_STATE;
	}
	
	// Update is called once per frame
	void Update () {
        switch(AttackWeapon_State)
        {
            case AI_STATE.IDLE_STATE:
                {
                    AttackWeapon_State = AI_STATE.STANDBY_STATE;
                    break;
                }
            case AI_STATE.STANDBY_STATE:
                {
                    if (TakePlayerPositionOnce == true)
                        AttackWeapon_State = AI_STATE.ATTACK_STATE;
                    break;
                }
            case AI_STATE.ATTACK_STATE:
                {
                    if (transform.position.y <= PlayerDirection.y)
                        AttackWeapon_State = AI_STATE.AOE_ATTACK_STATE;
                    break;
                }
            case AI_STATE.AOE_ATTACK_STATE:
                {
                    if (CoolDown <= 0.0f)
                        AttackWeapon_State = AI_STATE.DEAD_STATE;
                    break;
                }
            case AI_STATE.DEAD_STATE:
                {
                    if (transform.position == initialPosition)
                        AttackWeapon_State = AI_STATE.IDLE_STATE;
                    break;
                }
        }
        UpdateAIState();
	}

    void UpdateAIState()
    {
        switch (AttackWeapon_State)
        {
            case AI_STATE.IDLE_STATE:
                {
                    CoolDown = 1.0f;
                    OrderFromController = false;
                    TakePlayerPositionOnce = false;
                    AttackTime = 0.0f;
                    break;
                }
            case AI_STATE.STANDBY_STATE:
                {
                    step = StandbySpeed * Time.deltaTime;
                    Vector3 Direction = PlayerObject.transform.position - transform.position;
                    Vector3 NewDirection = Vector3.RotateTowards(transform.forward, Direction, step, 0.0f);
                    transform.rotation = Quaternion.LookRotation(NewDirection);
                    if (OrderFromController == true)
                    {
                        if(AttackTime > 1.0f)
                        {
                            PlayerDirection = PlayerObject.transform.position;
                            PlayerDirection.y = PlayerDirection.y - 1.0f;
                            TakePlayerPositionOnce = true;
                        }
                        else
                            AttackTime += Time.deltaTime;
                    }
                        
                    break;
                }
            case AI_STATE.ATTACK_STATE:
                {
                    step = AttackSpeed * Time.deltaTime;
                    //Vector3 Dire = PlayerDirection - transform.position;
                    transform.position = Vector3.MoveTowards(transform.position, PlayerDirection, step);
                    //transform.position += Dire * step;
                    break;
                }
            case AI_STATE.AOE_ATTACK_STATE:
                {
                    CoolDown -= Time.deltaTime;
                    break;
                }
            case AI_STATE.DEAD_STATE:
                {
                    step = StandbySpeed * Time.deltaTime;
                    transform.position = Vector3.MoveTowards(transform.position, initialPosition, step);
                    break;
                }
        }
    }
}
