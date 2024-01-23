using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    public float horizontal;
    public float vertical;
    public bool wDown;
    public bool jDown;
    public bool isJump;

    public bool isDodge;


    Vector3 moveVec;
    Vector3 dodgeVec;
    Rigidbody rigid;
    Animator anim;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        Move();
        Turn();
        Jump();
        Dodge();
 
    }
    void GetInput()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        wDown = Input.GetButton("Walk");
        jDown = Input.GetButtonDown("Jump");
    }
    void Move()
    {
        moveVec = new Vector3(horizontal, 0, vertical).normalized;

        if (isDodge)
            moveVec = dodgeVec;

        if (wDown)
            transform.position += moveVec * speed * 0.3f * Time.deltaTime;
        else
            transform.position += moveVec * speed * Time.deltaTime;


        anim.SetBool("isRun", moveVec != Vector3.zero);
        anim.SetBool("isWalk", wDown);
    }
    void Turn()
    {
        transform.LookAt(transform.position + moveVec);
    }
    void Jump()
    {
        if (jDown && !isJump && moveVec == Vector3.zero && !isDodge)
        {
            rigid.AddForce(Vector3.up * 10, ForceMode.Impulse);
            anim.SetBool("isJump", true);
            anim.SetTrigger("doJump");
            isJump = true;
        }
    }
    void Dodge()
    {
        if (jDown && !isJump && moveVec != Vector3.zero)
        {
            dodgeVec = moveVec;
            speed *= 2;         
            anim.SetTrigger("doDodge");
            isDodge = true;

            Invoke("DodgeOut", 0.5f);


        }
    }
    void DodgeOut()
    {
        speed *= 0.5f;
        isDodge = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Floor")
        {
            anim.SetBool("isJump", false);
            isJump = false;
        }
    }
}
