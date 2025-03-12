using System.Collections;
using UnityEngine;
using Unity.Mathematics;

public class MonsterScript : MonoBehaviour
{
    private Rigidbody2D rb;
    public GameObject player;
    public GameObject babyMonster;
    public float monsterSpeed = 1f;
    public bool hasSpawned = false; // Prevent multiple spawns

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void FixedUpdate()
    {
        rb.linearVelocityX = monsterSpeed;
        StartCoroutine(InstantiateChildMonster()); // Start spawning only once

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            player.GetComponent<PlayerScript>().characterCurrentHealth = 0;
        }
    }

    IEnumerator InstantiateChildMonster()
    {
        if (!hasSpawned) // Ensure only one spawn at a time
        {
            hasSpawned = true;
            yield return new WaitForSeconds(4f);
            Instantiate(babyMonster,new Vector3(rb.transform.position.x+0.7f,rb.transform.position.y-0.5f,rb.transform.position.z), quaternion.identity);
            hasSpawned = false; // Allow spawning again
        }
    }
}