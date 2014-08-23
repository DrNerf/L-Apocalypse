using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MecanimController : MonoBehaviour 
{
    public List<Animator> MecanimAnimator;
    public PlayerController PlayerInfo;

    private float Speed;
    private bool IsSwinging = false;
    public bool IsShooting = false;
	
	// Update is called once per frame
	void Update () 
    {
        if (Input.GetKey("w"))
        {
            Speed = 1;
        }
        else    if (Input.GetKey("s"))
        {
            Speed = -1;
        }
        else    if (Input.GetKey("a"))
        {
        }
        else    if (Input.GetKey("d"))
        {
        }
        else
        {
            Speed = 0;
        }

        if (Input.GetMouseButtonDown(0))
        {
            IsSwinging = true;
            IsShooting = true;
        }
        if(Input.GetMouseButtonUp(0))
        {
            IsSwinging = false;
            IsShooting = false;
        }

        SendDataToAnimator();
	}

    void SendDataToAnimator()
    {
        if (PlayerInfo.Weapon == 0 && MecanimAnimator[0].gameObject.activeSelf)
        {
            MecanimAnimator[0].SetFloat("Speed", Speed);
            MecanimAnimator[0].SetBool("IsSwinging", IsSwinging);
        }
        if (PlayerInfo.Weapon == 1 && MecanimAnimator[1].gameObject.activeSelf)
        {
            MecanimAnimator[1].SetBool("IsShooting", IsShooting);
        }
        if (PlayerInfo.Weapon == 2 && MecanimAnimator[2].gameObject.activeSelf)
        {
            MecanimAnimator[2].SetBool("IsShooting", IsShooting);
        }
    }
}
