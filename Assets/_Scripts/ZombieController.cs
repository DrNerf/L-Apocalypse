using UnityEngine;
using System.Collections;

public class ZombieController : MonoBehaviour 
{
    public int Health = 100;
    public GameObject BloodFX;
    public Transform BloodFXPos;
    public GameObject Ragdoll;

    public Transform Player;
    public NavMeshAgent Agent;
	// Use this for initialization
	void Start () 
    {
        Player = GameObject.Find("Player").transform;
        StartCoroutine(ChasePlayer());
	}

    void Update()
    {
        if (Health <= 0)
        {
            Die();
        }
    }

    IEnumerator ChasePlayer()
    {
        do
        {
            Agent.SetDestination(Player.position);
            yield return new WaitForSeconds(0.5f);
        } while (true);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PlayerWeapon")
        {
            TakeDamage(Random.Range(25, 60));
        }
    }

    public void TakeDamage(int Damage)
    {
        Health -= Damage;
        GameObject TempBloodFX = Instantiate(BloodFX, BloodFXPos.position, Quaternion.identity) as GameObject;
        Destroy(TempBloodFX, 2);
    }

    void Die()
    {
        GameObject TempRagdoll = Instantiate(Ragdoll, transform.position, transform.rotation) as GameObject;
        if (Health <= -30)
        {
            TempRagdoll.rigidbody.AddForce(transform.forward * -6000);
        }
        GameObject.Destroy(gameObject);
    }
}
