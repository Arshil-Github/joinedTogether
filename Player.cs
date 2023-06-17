using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float runSpeed; //MOvement Speed
    public float holdDelayinMovement; //Add a little hold delay
    public float timeForSwitch;
    public float speedIncreasePer1ms;
    public float maximumSpeed;
    public float jumpSpeed;
    public float attackCoolTime;

    public Rigidbody2D rb; //Rigidbody
    public Animator anim;

    bool checkForInput = false;
    float horizontal;
    bool increaseSpeed = true;
    float originalRunSpeed;
    float xScale;

    bool inAir = false;
    bool jump = false;
    bool allowedToAttack = true;
    // Start is called before the first frame update
    void Start()
    {
        originalRunSpeed = runSpeed;
        xScale = transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxisRaw("Horizontal") != 0 && !checkForInput)
        {
            StartCoroutine(Run());
        }
        if(Input.GetAxisRaw("Horizontal") == 0 && checkForInput) {
            StartCoroutine(Stop());
        }

        if (checkForInput) {
            horizontal = Input.GetAxisRaw("Horizontal");

            if (horizontal > 0)
            {
                transform.localScale = new Vector2(xScale, transform.localScale.y);
            }
            else if(horizontal < 0)
            {
                transform.localScale = new Vector2(-xScale, transform.localScale.y);
            }

            if (increaseSpeed)
            {
                StartCoroutine(speedIncrease());
            }
        }
        if (Input.GetKeyDown(KeyCode.X) && allowedToAttack) {
            Attack();
        }
        if (Input.GetAxisRaw("Fire1") != 0 && !inAir)
        {
            jump = true;
        }
    }
    IEnumerator Run() {

        yield return new WaitForSeconds(holdDelayinMovement);
        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            checkForInput = true;
            anim.SetBool("Run", true);
        }
        StopCoroutine(Run());
    }
   
    IEnumerator Stop()
    {

        yield return new WaitForSeconds(timeForSwitch);
        if (Input.GetAxisRaw("Horizontal") == 0)
        {
            checkForInput = false;
            anim.SetBool("Run", false);
            horizontal = 0;
            increaseSpeed = true;
            runSpeed = originalRunSpeed;
        }
        StopCoroutine(Stop());
    }
    IEnumerator speedIncrease()
    {
        increaseSpeed = false;
        while (runSpeed <= maximumSpeed)
        {
            runSpeed += speedIncreasePer1ms * 1.1f;
            yield return new WaitForSeconds(0.5f);
        }

    }
    private void FixedUpdate()
    {
        if (jump)
        {
            rb.velocity = new Vector2(horizontal * runSpeed, jumpSpeed);
            jump = false;
        }
        else
        {
            rb.velocity = new Vector2(horizontal * runSpeed, rb.velocity.y);
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        inAir = false;
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        inAir = true;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("KillOnTouch")) {
            Debug.Log("Death");
        
        }
    }
    public void MakeStatic() {
        rb.bodyType = RigidbodyType2D.Static;
    }
    public void MakeDynamic()
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
    }
    public void Attack() {
        allowedToAttack = false;
        anim.SetTrigger("Attack");
        StartCoroutine(attackCoolDown());
    }
    IEnumerator attackCoolDown() {
        yield return new WaitForSeconds(attackCoolTime);
        allowedToAttack = true;
    }
}
