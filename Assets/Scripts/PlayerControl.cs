using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public Rigidbody2D Rigidbody;
    public Collider2D Collider;
    public Animator animator;
    public Animator backpack;
    public GameObject player;
    public DialogueManager dialogueManager;
    public float accleration;
    public float acclination;
    public float max_speed;

    Quaternion quaternion;
    public float angle;
    private float t;

    public float jump_time;
    public float jump_height;
    public float jump_thershold;
    public float falling_speed;
    public float falling_plunge;
    public float falling_acceleration;
    public float coyote_time;
    private float coyote_counter;
    public float jump_spare;
    public float walljump_strength;

    private int back_state = -1;
    private float jump_try;
    private float time;
    private bool jumpdown;
    private bool jumpheld;
    private bool jump_cancel = false;
    private bool grounded = false;
    private bool r_wall = false;
    private bool l_wall = false;
    private bool jumping = false;
    private bool coyote_usable = false;
    private bool walljumping = false;
    private bool walljump_available;
    private float ungrounded = float.MinValue;

    private bool attacking = false;
    private float attack_time;
    public float attack_speed;
    private Vector2 velocity = Vector2.zero;

    void Start()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if(!dialogueManager.talking)
        {
            Move();
        }
        else
        {
            Rigidbody.velocity = new Vector2(0, Rigidbody.velocity.y);
        }
        
        Checkcollision();
        
        
    }

    private void Update()
    {
        time += Time.deltaTime;

        if (!dialogueManager.talking)
        {
            Jump();
            Walljump();
            attack();
            Backpack();
        }
        Gatherinput();
        Lock_rotation();
        
        if (dialogueManager.talking)
        {
            animator.SetBool("climbing",false);
            animator.SetFloat("speed", 0);
        }
        
        animator.SetFloat("vertical_speed", Rigidbody.velocity.y);
        animator.SetBool("jumping", !grounded);

    }

    private void Move()
    {
        if (Input.GetAxisRaw("Horizontal") != 0 && ((walljumping && Rigidbody.velocity.y < 0) || !walljumping))
        {
            Rigidbody.velocity = Vector2.SmoothDamp(Rigidbody.velocity, new Vector2(Input.GetAxisRaw("Horizontal") * max_speed, Rigidbody.velocity.y), ref velocity, 1 / accleration);
        }
        else if (Input.GetAxisRaw("Horizontal") == 0 && !walljumping)
        {
            Rigidbody.velocity = Vector2.SmoothDamp(Rigidbody.velocity, new Vector2(Input.GetAxisRaw("Horizontal") * max_speed, Rigidbody.velocity.y), ref velocity, 1 / acclination);
        }


        if (Input.GetAxisRaw("Horizontal") > 0 && ((walljumping && Rigidbody.velocity.y < 0) || !walljumping))
        {
            gameObject.transform.localScale = new Vector3(-1, 1, 1);
        }
        if (Input.GetAxisRaw("Horizontal") < 0 && ((walljumping && Rigidbody.velocity.y < 0) || !walljumping))
        {
            gameObject.transform.localScale = new Vector3(1, 1, 1);
        }
    }



    private void Checkcollision()
    {
        Physics2D.queriesStartInColliders = false;
        grounded = Physics2D.BoxCast(Collider.bounds.center, Collider.bounds.extents, 0, Vector2.down, 2f, LayerMask.GetMask("Wall") | LayerMask.GetMask("Floor"));
        r_wall = Physics2D.BoxCast(Collider.bounds.center, Collider.bounds.extents, 0, Vector2.right, 0.7f,LayerMask.GetMask("Wall"));
        l_wall = Physics2D.BoxCast(Collider.bounds.center, Collider.bounds.extents, 0, Vector2.left, 0.7f,LayerMask.GetMask("Wall"));
    }

    private void Jump()
    {
        if ((grounded && jumpdown) || (coyote_usable && jumpdown && !jumping))
        {
            leap();
        }
        if (Rigidbody.velocity.y < 0 && grounded)
        {
            jumping = false;
        }

        if (jumping == true && jumpdown)
        {
            jump_try = time;
        }

        if (grounded && (time - jump_try <= jump_spare))
        {
            leap();
        }
        
        if(grounded)
        {
            coyote_counter = coyote_time;
        }
        else
        {
            coyote_counter -= Time.deltaTime;
        }

        if(coyote_counter >= 0)
        {
            coyote_usable = true;
        }
        else
        {
            coyote_usable = false;
        }

        if (Rigidbody.velocity.y > jump_thershold && !jumpheld)
        {
            jump_cancel = true;
        }
        else
        {
            jump_cancel = false;
        }

        if (jump_cancel == true)
        {
            Rigidbody.velocity = new Vector2(Rigidbody.velocity.x, falling_plunge * -1);
        }

        if (Rigidbody.velocity.y < falling_speed * -1)
        {
            Rigidbody.velocity = new Vector2(Rigidbody.velocity.x, falling_speed * -1);
        }

        if (Rigidbody.velocity.y < -jump_thershold)
        {
            Rigidbody.gravityScale = falling_acceleration;
        }
        else
        {
            Rigidbody.gravityScale = 3f;
        }


    }

    private void Walljump()
    {
        if (r_wall && !grounded && walljump_available)
        {
            gameObject.transform.localScale = new Vector3(-1, 1, 1);
            Rigidbody.gravityScale = 0.3f;
            if (jumpdown)
            {
                animator.SetBool("climbing", false);
                gameObject.transform.localScale = new Vector3(1, 1, 1);
                walljumping = true;
                jumping = true;
                Rigidbody.velocity = new Vector2(-walljump_strength, walljump_strength);
                walljump_available = false;
            }
        }

        if (l_wall && !grounded && walljump_available)
        {
            gameObject.transform.localScale = new Vector3(1, 1, 1);
            Rigidbody.gravityScale = 0.3f;
            if (jumpdown)
            {
                gameObject.transform.localScale = new Vector3(-1, 1, 1);
                walljumping = true;
                jumping = true;
                Rigidbody.velocity = new Vector2(walljump_strength, walljump_strength);
                walljump_available = false;
            }
        }

        if (walljump_available && Rigidbody.velocity.y > 0 && (r_wall || l_wall))
        {
            Rigidbody.velocity = new Vector2(Rigidbody.velocity.x, 0);
        }

        if (grounded)
        {
            walljumping = false;
            walljump_available = false;
            animator.SetBool("climbing", false);
            ungrounded = 0;
        }

        if ((jumping == true && !r_wall && !l_wall) || Rigidbody.velocity.y < 0)
        {
            walljump_available = true;
        }

        if ((r_wall || l_wall) && walljump_available)
        {
            animator.SetBool("climbing", true);
        }
        else
        {
            animator.SetBool("climbing", false);
        }
    }

    private void Lock_rotation()
    {

         if (Input.GetAxisRaw("Horizontal") == 1 && !dialogueManager.talking)
         {
             quaternion = Quaternion.Euler(0, 0, angle*-1);
         }
         else if (Input.GetAxisRaw("Horizontal") == -1 && !dialogueManager.talking)
         {
             quaternion = Quaternion.Euler(0, 0, angle*1);
         }
         else if (Input.GetAxisRaw("Horizontal") == 0 || dialogueManager.talking)
         {
             quaternion = Quaternion.Euler(0, 0, 0);
         }

        player.transform.rotation = Quaternion.Slerp(player.transform.rotation, quaternion,5f*Time.deltaTime);
        gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);


    }

    private void Gatherinput()
    {
        jumpdown = Input.GetKeyDown(KeyCode.Space);
        jumpheld = Input.GetKey(KeyCode.Space);
        animator.SetFloat("speed", Mathf.Abs(Input.GetAxisRaw("Horizontal")));
    }

    private void leap()
    {
        Rigidbody.velocity = new Vector2(Rigidbody.velocity.x, jump_height);
        coyote_usable = false;
        jumping = true;
    }

    private void attack()
    {
        if (Input.GetMouseButtonDown(0) && grounded && !attacking)
        {
            attacking = true;
            animator.speed = attack_speed;
            animator.SetTrigger("attacking");
        }
        
        if(attacking == true)
        {
            attack_time += Time.deltaTime;
            if(attack_time > attack_speed)
            {
                attacking = false;
                animator.speed = 1;
            }
        }
    }

    private void Backpack()
    {
        if(Input.GetKeyDown("e"))
        {
            back_state *= -1;
            backpack.SetInteger("open",back_state);
        }
    }
}
