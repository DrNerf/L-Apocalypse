using UnityEngine;
using System.Collections;

public class BulletTrailController : MonoBehaviour 
{
    public float Speed = 300;
	
	// Update is called once per frame
	void Update () 
    {
        transform.Translate(transform.forward * Time.deltaTime * Speed);
	}
}
