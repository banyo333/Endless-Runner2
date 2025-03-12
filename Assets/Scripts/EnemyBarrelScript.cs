using UnityEngine;
using System.Collections;
using System;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.SceneTemplate;
using UnityEngine.UI;
public class EnemyBarrelScript : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 0.7f;
    public int enemyBarrelHitDamage = 20;
    public GameObject HealGameObject;
    public Vector2 EnemyBarrelDiePosition;
    private Animator enemyBarrelAnimator;
    private Transform playerTransform;
    private Rigidbody2D rb;
    private GameObject player;
    private SpriteRenderer spriteRenderer;
    public Transform attackPoint;
    public float attackRange = 0.75f;
    public Slider EnemyHealthBar;
    public bool changeDirectionAttack = true;
    private bool wake = false;
    private bool walk = false;
    private bool canAttack = false;
    private bool currentlyAnimating = false;
    public float health = 100;
    public float maxhealth = 100;
    public bool enemybarrelisDead = false;
    private bool isFacingRight = false;
    public LayerMask playerLayer;

    private void Awake()
    {
        enemyBarrelAnimator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        // Find the player using tag
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
        }
    }

    
    private void Update()
    {
        updateEnemyBarrelHealthBar();

        if ( playerTransform != null&& changeDirectionAttack)
        {
            // Calculate direction to player
            Vector2 direction = (playerTransform.position - transform.position).normalized;
            if (walk)
            {
                rb.linearVelocityX = direction.x * moveSpeed;

            }            if (direction.x > 0) isFacingRight = true;
            if(direction.x < 0) isFacingRight = false;
            // Flip sprite based on movement direction using SpriteRenderer
            spriteRenderer.flipX = direction.x > 0;
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }

        if (health <= 0)
        {
            health = 0;
            EnemyBarrelDie();
        }
            attackPoint.position = new Vector2(rb.position.x + (isFacingRight ? attackRange : -attackRange), rb.position.y);

    }

    public void updateEnemyBarrelHealthBar()
    {
        EnemyHealthBar.value = health/maxhealth;
    }

    private void EnemyBarrelDie()
    {
        if(!enemybarrelisDead)
        {
            enemybarrelisDead = true;
        enemyBarrelAnimator.SetBool("Death",true);
        EnemyBarrelDiePosition = rb.position;
        player.GetComponent<PlayerScript>().playerScore += 50; ;

        StartCoroutine( enemyBarrelDieCountdown());
        player.GetComponent<PlayerScript>().UpdateEnemySpeed();
        }

    
    }

    public void OnWakeZoneEnter()
    {
        if (!wake)
        {
            wake = true;
            enemyBarrelAnimator.SetTrigger("Wake");
            StartCoroutine(StartWalkingAfterWake());
        }
    }

    public void OnAttackZoneEnter()
    {
        canAttack = true;
        walk = false;
        enemyBarrelAnimator.SetBool("isWalking", false);
        StartCoroutine(AttackLoop());
    }

    public void OnWakeZoneExit()
    {
        walk = false;
        wake = false;
        canAttack = false;
        currentlyAnimating = false;
        enemyBarrelAnimator.SetBool("isWalking", false);
        enemyBarrelAnimator.SetTrigger("Sleep");
    }

    public void OnAttackZoneExit()
    {
        canAttack = false;
       
            enemyBarrelAnimator.SetBool("isWalking", true);
            walk = true;
  

    }
   


    private IEnumerator StartWalkingAfterWake()
    {
        yield return new WaitForSeconds(0.5f); // Wait for wake animation
        walk = true;
        enemyBarrelAnimator.SetBool("isWalking", true);
    }
    private IEnumerator enemyBarrelDieCountdown()
    {
        yield return new WaitForSeconds(1.3f); // Wait for wake animation
        Destroy(gameObject);
        Instantiate(HealGameObject, new Vector3(EnemyBarrelDiePosition.x,EnemyBarrelDiePosition.y,0f),quaternion.identity) ;

    }

    private IEnumerator AttackLoop()
    {
        while (canAttack && !currentlyAnimating )
        {

                 currentlyAnimating = true;
                enemyBarrelAnimator.SetTrigger("Attack");

            yield return new WaitForSeconds(0.6f);
            changeDirectionAttack = false;

            yield return new WaitForSeconds(1.8f);

            Collider2D hitPlayer = Physics2D.OverlapCircle(attackPoint.position, 0.75f, playerLayer);
            

            if (hitPlayer && hitPlayer.GetComponent<PlayerScript>().characterCurrentHealth > 0)
            {
                hitPlayer.GetComponent<PlayerScript>().playerAnimator.SetTrigger("playerGetsHit");
                hitPlayer.GetComponent<PlayerScript>().characterCurrentHealth -= enemyBarrelHitDamage;

            }
            yield return new WaitForSeconds(0.9f);
            changeDirectionAttack = true;
            yield return new WaitForSeconds(0.6f);

            currentlyAnimating = false;
            
        }
    }
           
}