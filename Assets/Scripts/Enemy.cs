using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity {

    public float expOnDeath;
    private Player player;
    private GameObject wayPoint;
    private Vector3 wayPointPos;
    private float speed = 0.5f;

    private void Start()
    {
        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
            wayPoint = GameObject.FindGameObjectWithTag("Player");
        }
    }

    public override void Die()
    {
        player.AddExperiencie(expOnDeath);
        base.Die();
    }

    public void Update()
    {
        if (player != null)
        {
            wayPointPos = new Vector3(wayPoint.transform.position.x, transform.position.y, wayPoint.transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, wayPointPos, speed * Time.deltaTime);
            transform.LookAt(wayPointPos);
        }
    }
}
