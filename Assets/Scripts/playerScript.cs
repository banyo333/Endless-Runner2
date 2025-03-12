using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Random = UnityEngine.Random;


public class PlayerScript : MonoBehaviour
{

    public Text scoreText;
    public GameObject EnemyBarrel;
    float currentMapPosition=5.19f;
    float mapLength = 12.1f;
    public LayerMask enemyLayer;
    public Transform attackPoint;
    public float attackRange = 1f;
    public EnemyBarrelScript enemyBarrelScript;
    public GameManagerScript gameManagerScript;
    public GameObject GameOverSceneUI;
    public bool PlayerGetsHit;
    public int playerScore = 0;
    private bool canAttack = true;
    private bool canJump = true;

    public GameObject MapPrefab;
    private bool isFacingRight;
    public int characterCurrentHealth = 100;
    public int characterMaxHealth = 100;
    public float JumpForce = 1.7f;
    public GameObject monster;
    public GameObject babymonster;
    

    // map length = 12.39
    //character 8.67 olunca spawnlansn
    Rigidbody2D rb;
    Vector2 moveInput;
    SpriteRenderer rbSprite;

    public bool playerIsDead = false;
    int attackType ;

      public bool isAttacking { get; private set; }

    public Animator playerAnimator;
    
    float walkSpeed = 2f;
    public bool isMoving { get; private set; }

    private void Start()
    {
        monster = GameObject.FindGameObjectWithTag("Monster");
        babymonster = GameObject.FindGameObjectWithTag("BabyMonster");
    }


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rbSprite = GetComponent<SpriteRenderer>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        
            currentMapPosition = currentMapPosition + mapLength;
            EndlessMap();
        
    }

    


    private void FixedUpdate()
    {
        if (playerIsDead == false)
        {
            rb.AddForce(new Vector2(moveInput.x * walkSpeed, 0f), ForceMode2D.Impulse);
        rb.linearVelocity = new Vector2(Mathf.Clamp(rb.linearVelocity.x, -walkSpeed, walkSpeed), rb.linearVelocity.y);

        }

        scoreText.text = playerScore.ToString();
    }


    public void UpdateEnemySpeed()
    {
        if (playerScore % 200 == 0 && playerScore > 0) // Only update every 200 points
        {
            float increaseFactor = 1.3f; // Increase speed by 5% each time

            // Scale the speed gradually instead of adding a fixed amount
            MonsterScript monsterScript = monster.GetComponent<MonsterScript>();
            ChildMonster childMonsterScript = babymonster.GetComponent<ChildMonster>();

            monsterScript.monsterSpeed *= increaseFactor;
            childMonsterScript.babyMonsterSpeed *= increaseFactor;

            // Prevent monsters from getting uncontrollably fast
            monsterScript.monsterSpeed = Mathf.Clamp(monsterScript.monsterSpeed, 1f, 3f);
            childMonsterScript.babyMonsterSpeed = Mathf.Clamp(childMonsterScript.babyMonsterSpeed, 3f, 5f);

          
        }
    }

        void Update()
    {

       
        
        
        
        if(playerIsDead == false)
        {   

      
            
            if(moveInput.x > 0)
        {
            isFacingRight = true;
            if(canAttack)
            {
                rbSprite.flipX= false;
                             attackPoint.position = new Vector2(rb.position.x + (isFacingRight ? attackRange : -attackRange), rb.position.y);

            }
           
        }
        if(moveInput.x < 0)
        {
            isFacingRight = false;
            if(canAttack)
            {
                rbSprite.flipX = true;
                            attackPoint.position = new Vector2(rb.position.x + (isFacingRight ? attackRange : -attackRange), rb.position.y);

            }
           
        }
    

        if (characterCurrentHealth<=0 )
        {
            playerIsDead = true;
            characterDeath();
        }

        if (characterCurrentHealth > 100)
            characterCurrentHealth = 100;

        }
    }
   public void OnEscape(InputAction.CallbackContext context)
    {
        if(GameOverSceneUI.active)
        {
            GameOverSceneUI.SetActive(false);
        }
       else
            GameOverSceneUI.SetActive(true);

    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (canJump)
        {
            canJump = false;
            playerAnimator.SetTrigger("Jump");
            rb.AddForce(new Vector2(moveInput.x * walkSpeed, JumpForce), ForceMode2D.Impulse);
            StartCoroutine(JumpCooldown()); // Start cooldown
        }
    }


    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started && canAttack && !playerIsDead)
        {
            
            attackType = Random.Range(0, 2);

            if (attackType == 0)
            {
                playerAnimator.SetBool("attackAnim1", true);
            }
            else if (attackType == 1)
            {
                playerAnimator.SetBool("attackAnim2", true);

            }
            
            
            
            
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, 0.5f, enemyLayer);
            
            
            foreach (Collider2D enemy in hitEnemies)
            {
                enemy.GetComponent<EnemyBarrelScript>().health -= 50;
           
                
            }

            isAttacking = true;
            playerAnimator.SetBool("isAttack", true);


            Invoke(nameof(ResetAttack), 0.1f); // Set animation back to false after 0.2s
            StartCoroutine(AttackCooldown()); // Start cooldown
        }
    }

    //BU KISIMA BAK REN!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    //BU KISIMA BAK REN!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

    private void ResetAttack()
    {
        isAttacking = false;
        playerAnimator.SetBool("isAttack", false);
        playerAnimator.SetBool("attackAnim1", false);
        playerAnimator.SetBool("attackAnim2", false);


    }

    private IEnumerator AttackCooldown()
    {
        canAttack = false; // Prevent further attacks
        yield return new WaitForSeconds(0.5f); // Wait for 0.5 seconds
        canAttack = true; // Allow attacking again
    }
    //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    private IEnumerator JumpCooldown()
    {
        yield return new WaitForSeconds(1.3f); // Wait for 0.5 seconds
        canJump = true;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            moveInput = context.ReadValue<Vector2>();
            playerAnimator.SetBool("isMoving", true);
            isMoving = true;
        }

        if (context.canceled)
        {
            moveInput = Vector2.zero;
            playerAnimator.SetBool("isMoving", false);

            isMoving = false;
        }
    }
    public void characterDeath()
    {
        playerAnimator.SetBool("Death",true);
        gameManagerScript.gameOver();
        StartCoroutine(dieTime());

    }

    IEnumerator dieTime()
    {
        yield return new WaitForSeconds(0.5f); // Wait for 0.5 seconds
        Time.timeScale = 0f;

    }
    public void EndlessMap()
    {

        Instantiate(MapPrefab, new Vector3(currentMapPosition, 0.9663233f, 1.548481f), Quaternion.identity);
        Instantiate(EnemyBarrel, new Vector3(currentMapPosition-3 , 0.9663233f,1.548481f), Quaternion.Euler(0,180,0));
        Instantiate(EnemyBarrel, new Vector3(currentMapPosition+3 , 0.9663233f,1.548481f), Quaternion.Euler(0,180,0));

        
    }

    private void OnDrawGizmos()
    {
        if (attackPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, 1f);
        }
    }
}