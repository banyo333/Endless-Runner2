using System;
using UnityEngine;

public class ChildMonster : MonoBehaviour
{
    
    private Rigidbody2D rb;
    public GameObject player;
    public float babyMonsterSpeed = 3f;


    private void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {
        rb.linearVelocityX = babyMonsterSpeed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            player.GetComponent<PlayerScript>().characterCurrentHealth -=50;
            Destroy(gameObject);

        }
    }
}