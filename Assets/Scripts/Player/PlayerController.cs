using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamageable
{

    public float speed;
    public float jumpForce;

    private Rigidbody2D rb;
    private FixedJoystick joystick;
    private Animator anim;

    [Header("Player state")]
    public float health;
    public bool isDeath;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask groundLayer;

    [Header("States Check")]
    public bool isGround; // 是否在地面，用于避免连续跳跃
    public bool canJump;
    public bool isJump;

    [Header("Jump FX")]
    public GameObject jumpFX;
    public GameObject landFX;

    [Header("Attack Settings")]
    public GameObject bombPrefab;
    public float nextAttack = 0;
    public float attackRate;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        joystick = FindObjectOfType<FixedJoystick>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (isDeath)
            return;
        CheckJump();
    }

    public void FixedUpdate()
    {
        if (isDeath)
        {
            rb.velocity = Vector2.zero;
            return;
        }
        PhysicsCheck();
        Movement();
        Jump();
    }

    void Movement()
    {
        //float horizontalInput = Input.GetAxis("Horizontal"); // -1~1 包括小数
        //float horizontalInput = Input.GetAxisRaw("Horizontal"); // -1 ~ 1 不包括小数

        //rb.velocity = new Vector2(horizontalInput * speed, rb.velocity.y);
        //if (horizontalInput != 0)
        //{
        //    transform.localScale = new Vector3(horizontalInput, 1, 1);
        //}



        // 操作杆
        float horizontalInput = joystick.Horizontal;
        rb.velocity = new Vector2(horizontalInput * speed, rb.velocity.y);
        if (horizontalInput >= 0)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
    }

    void CheckJump()
    {
        // 监测点击跳跃按钮，并且是在地面上的时候
        if (Input.GetButtonDown("Jump") && isGround)
        {
            canJump = true;
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            Attack();
        }
    }

    void Jump()
    {
        if (canJump)
        {
            isJump = true;

            // 跳跃时，执行跳跃动画
            jumpFX.SetActive(true);
            jumpFX.transform.position = transform.position + new Vector3(0, -0.45f, 0);

            rb.gravityScale = 4;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            canJump = false;
        }
    }

    public void ButtonJump()
    {
        canJump = true;
    }

    void PhysicsCheck()
    {
        isGround = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);
        if (isGround)
        {
            rb.gravityScale = 1;
            isJump = false;
        }
    }

    public void LandFX() // Animation Event
    {
        // 在接触到地面时，触发落地动画
        landFX.SetActive(true);
        landFX.transform.position = transform.position + new Vector3(0, -0.75f, 0);
    }

    // 绘制作用范围
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
    }

    public void Attack()
    {
        // 攻击，判断间隔时间，避免重复防止炸弹
        if (Time.time > nextAttack)
        {
            // 创建游戏对象，传入参数分别为：对象，位置，角度
            Instantiate(bombPrefab, transform.position, bombPrefab.transform.rotation);

            nextAttack = Time.time + attackRate;
        }
    }

    public void GetHit(float damage)
    {
        if (!anim.GetCurrentAnimatorStateInfo(1).IsName("player_hit"))
        {
            health -= damage;
            if (health < 1)
            {
                health = 0;
                isDeath = true;
                anim.SetBool("dead", true);
            }
            anim.SetTrigger("hit");

            UIManager.instance.UpdateHealth(health);
        }
    }
}
