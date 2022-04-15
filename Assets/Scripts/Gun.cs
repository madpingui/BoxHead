using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Gun : MonoBehaviour
{

    public enum GunType { Semi, Burst, Auto, Bazooka, Sniper };

    public LayerMask collisionMask;
    public float gunID;
    public GunType gunType;
    public float rpm;
    public float damage = 1;

    private int maxAmmo;
    public int ammoPerMag = 60;

    public int count = 8;
    public int DamageAmount = 5;
    public float TargetDistance;
    public float AllowedRange = 100;
    public Transform Effect;
    public float scaleLimit = 2.0f;
    public float z = 10f;

    private LineRenderer tracer;
    public Transform spawn;
    public Transform shellEjectionPoint;
    public Rigidbody shell;
    private AudioSource audio;

    [HideInInspector]
    public GameGUI gui;

    private float secondsBetweenShots;
    private float nextPossibleShootTime;

    public static Gun Instance { set; get; }

    private void Start()
    {
        maxAmmo = ammoPerMag;
        Instance = this;
        secondsBetweenShots = 60 / rpm;
        if (GetComponent<LineRenderer>())
        {
            tracer = GetComponent<LineRenderer>();
        }
        audio = GetComponent<AudioSource>();

        gui.SetAmmoInfo(ammoPerMag);
    }

    public void Shoot()
    {
        if (CanShoot())
        {
            if (gunType == GunType.Bazooka)
            {
                Ray ray = new Ray(spawn.position, spawn.forward);
                RaycastHit hit;

                float shotDistance = 20;

                if (Physics.Raycast(ray, out hit, shotDistance))
                {
                    shotDistance = hit.distance;
                }

                Collider[] objs;

                objs = Physics.OverlapSphere(hit.point, 3);
                foreach (Collider c in objs)
                {
                    if (c.GetComponent<Entity>())
                    {
                        c.GetComponent<Entity>().TakeDamage(damage);
                    }
                }

                if (tracer)
                {
                    StartCoroutine("RenderTracer", ray.direction * shotDistance);
                }

            }
            else
            {
                Ray ray = new Ray(spawn.position, spawn.forward);
                RaycastHit hit;

                float shotDistance = 20;

                if (Physics.Raycast(ray, out hit, shotDistance, collisionMask))
                {
                    shotDistance = hit.distance;

                    if (hit.collider.GetComponent<Entity>())
                    {
                        hit.collider.GetComponent<Entity>().TakeDamage(damage);
                    }
                }

                if (tracer)
                {
                    StartCoroutine("RenderTracer", ray.direction * shotDistance);
                }
            }

            nextPossibleShootTime = Time.time + secondsBetweenShots;

            ammoPerMag--;

            gui.SetAmmoInfo(ammoPerMag);

            audio.Play();

            Rigidbody newShell = Instantiate(shell, shellEjectionPoint.position, Quaternion.identity) as Rigidbody;
            newShell.AddForce(shellEjectionPoint.forward * Random.Range(80f, 130f) + spawn.forward * Random.Range(-10f, 10));
        }
    }

    public void ShootAuto()
    {
        if (gunType == GunType.Auto)
        {
            Shoot();
        }
    }

    private bool CanShoot()
    {
        bool canShoot = true;

        if (Time.time < nextPossibleShootTime)
        {
            canShoot = false;
        }

        if (ammoPerMag <= 0)
        {
            canShoot = false;
        }

        return canShoot;
    }

    IEnumerator RenderTracer(Vector3 hitPoint)
    {
        tracer.enabled = true;
        tracer.SetPosition(0, spawn.position);
        tracer.SetPosition(1, spawn.position + hitPoint);

        yield return null;
        tracer.enabled = false;
    }

}
