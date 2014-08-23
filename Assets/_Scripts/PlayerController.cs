using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour 
{
    public int Health = 100;
    public int Weapon = 0;
    public int Ak47Ammo = 30;
    public int RevolverAmmo = 6;

    public GameObject MeleeTrigger;
    public MeshRenderer GunFlash;
    public MeshRenderer MachineGunFlash;
    public GameObject Sparks;

    public Texture HealthBar;
    public Texture AmmoBar;
    public Texture Crosshair;
    public GUIStyle HUDStyle;

    public List<GameObject> Weapons;

    private bool AlreadySwinging = false;

    void Start()
    {
        Screen.lockCursor = true;

        SetRendererRecursive(gameObject, false);
        SetRendererRecursive(Weapons[Weapon], true);
        GunFlash.enabled = false;
        MachineGunFlash.enabled = false;
    }

    void OnGUI()
    {
        GUI.DrawTexture(new Rect(0, Screen.height - 50, 150, 50), HealthBar);
        GUI.DrawTexture(new Rect(Screen.width - 150, Screen.height - 50, 150, 50), AmmoBar);

        GUI.Label(new Rect(70, Screen.height - 47, 150, 50), Health.ToString(), HUDStyle);
        if (Weapon == 1)
        {
            GUI.Label(new Rect(Screen.width - 65, Screen.height - 47, 150, 50), RevolverAmmo.ToString(), HUDStyle);
        }
        if(Weapon == 2)
        {
            GUI.Label(new Rect(Screen.width - 65, Screen.height - 47, 150, 50), Ak47Ammo.ToString(), HUDStyle);
        }

        GUI.DrawTexture(new Rect(Screen.width / 2 - 25, Screen.height / 2 - 25, 50, 50), Crosshair);
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Weapon == 0)
            {
                if (!AlreadySwinging)
                {
                    StartCoroutine("SwingAxes");
                    AlreadySwinging = true;
                }
            }

            if (Weapon == 1)
            {
                if (RevolverAmmo > 0)
                {
                    StartCoroutine(TakeShot());
                }
            }

            if (Weapon == 2)
            {
                if (Ak47Ammo > 0)
                {
                    StartCoroutine("AutomaticShooting");
                }
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (Weapon == 0)
            {
                StopCoroutine("SwingAxes");
                AlreadySwinging = false;
                MeleeTrigger.SetActive(false);
            }

            if (Weapon == 2)
            {
                StopCoroutine("AutomaticShooting");
                MachineGunFlash.enabled = false;
                MachineGunFlash.GetComponent<Light>().enabled = false;
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Weapons[Weapon].GetComponent<Animator>().SetBool("Outtro", true);
            Weapon = 0;
            StartCoroutine("EnableNewWeapon");
            
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Weapons[Weapon].GetComponent<Animator>().SetBool("Outtro", true);
            Weapon = 1;
            StartCoroutine("EnableNewWeapon");
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Weapons[Weapon].GetComponent<Animator>().SetBool("Outtro", true);
            Weapon = 2;
            StartCoroutine("EnableNewWeapon");
        }
    }

    IEnumerator SwingAxes()
    {
        do
        {
            MeleeTrigger.SetActive(false);
            yield return new WaitForSeconds(0.4f);
            MeleeTrigger.SetActive(true);
            yield return new WaitForSeconds(0.2f);
        } while (true);
    }

    IEnumerator EnableNewWeapon()
    {
        yield return new WaitForSeconds(1);

        SetRendererRecursive(gameObject, false);
        foreach (GameObject item in Weapons)
        {
            item.GetComponent<Animator>().SetBool("Intro", true);
        }
        SetRendererRecursive(Weapons[Weapon], true);
        GunFlash.enabled = false;
        MachineGunFlash.enabled = false;

        yield return new WaitForSeconds(0.5f);

        foreach (GameObject item in Weapons)
	    {
            item.GetComponent<Animator>().SetBool("Outtro", false);
            item.GetComponent<Animator>().SetBool("Intro", false);
	    }
    }

    IEnumerator TakeShot()
    {
        //Do raycast
        Transform Cam = Camera.main.transform;
        Ray ray = new Ray(Cam.position, Cam.forward);
        RaycastHit Hit;
        if (Physics.Raycast(ray, out Hit))
        {
            if (Hit.collider.tag == "Enemy")
            {
                Hit.collider.GetComponent<ZombieController>().TakeDamage(Random.Range(25, 60));
            }
            else
            {
                if (Hit.collider.tag == "EnemyHead")
                {
                    try
                    {
                        Hit.transform.parent.GetComponent<ZombieController>().TakeDamage(150);
                        Debug.Log("HEADSHOT");
                    }
                    catch
                    {
                        Debug.LogError("Headshot but cant take damage from enemy!");
                    }
                }
                else 
                {
                    Instantiate(Sparks, Hit.point, Quaternion.identity);
                }

            }
        }
        //
        GunFlash.enabled = true;
        GunFlash.GetComponent<Light>().enabled = true;
        yield return new WaitForSeconds(0.05f);
        GunFlash.enabled = false;
        GunFlash.GetComponent<Light>().enabled = false;
        GetComponent<MecanimController>().IsShooting = false;
        //take out 1 bullet
        RevolverAmmo -= 1;
    }

    IEnumerator AutomaticShooting()
    {
        do
        {
            Transform Cam = Camera.main.transform;
            Ray ray = new Ray(Cam.position, Cam.forward);
            RaycastHit Hit;
            if (Physics.Raycast(ray, out Hit))
            {
                if (Hit.collider.tag == "Enemy")
                {
                    Hit.collider.GetComponent<ZombieController>().TakeDamage(Random.Range(25, 60));
                }
                else
                {
                    if (Hit.collider.tag == "EnemyHead")
                    {
                        try
                        {
                            Hit.transform.parent.GetComponent<ZombieController>().TakeDamage(150);
                            Debug.Log("HEADSHOT");
                        }
                        catch
                        {
                            Debug.LogError("Headshot but cant take damage from enemy!");
                        }
                    }
                    else
                    {
                        Instantiate(Sparks, Hit.point, Quaternion.identity);
                    }
                }
            }
            //
            MachineGunFlash.enabled = true;
            MachineGunFlash.GetComponent<Light>().enabled = true;

            yield return new WaitForSeconds(0.05f);

            MachineGunFlash.enabled = false;
            MachineGunFlash.GetComponent<Light>().enabled = false;
            //take out 1 bullet
            Ak47Ammo -= 1;
            //if we reach 0 bullets we stop shooting
            if (Ak47Ammo < 1)
            {
                StopCoroutine("AutomaticShooting");
            }
            yield return new WaitForSeconds(0.1f);
        } while (true);
    }

    void SetRendererRecursive(GameObject Parent, bool Value)
    {
        Renderer[] rs = Parent.GetComponentsInChildren<Renderer>();
        foreach (Renderer r in rs)
        {
            r.enabled = Value;
        }
    }
}
