using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed;
    public float jumpForce;
    private Rigidbody rb;
    private bool isGrounded = false;
    public Animator anim;
    public Transform pivot;
    public float rotateSpeed;
    public GameObject model;
    private float jumpTimeCounter;
    public float jumpTime;
    private bool isJumping;

    void Start()
    {
       rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        //rb.velocity = new Vector3(horizontal, rb.velocity.y, vertical);

        float yStore = rb.velocity.y;
        Vector3 velocity = (transform.forward * vertical) + (transform.right * horizontal);
        velocity = velocity.normalized * moveSpeed;
        velocity.y = yStore;
        rb.velocity = velocity;
            
        // Move the player in different directions based on camera look direction
        if(horizontal != 0 || vertical != 0) {
            transform.rotation = Quaternion.Euler(0f, pivot.rotation.eulerAngles.y, 0f);
            Quaternion newRotation = Quaternion.LookRotation(new Vector3(rb.velocity.x, 0f, rb.velocity.z));
            model.transform.rotation = Quaternion.Slerp(model.transform.rotation, newRotation, rotateSpeed * Time.deltaTime);
        }

        anim.SetBool("isGrounded", isGrounded);
        anim.SetFloat("speed", Mathf.Abs(vertical) + Mathf.Abs(horizontal));
    }
    
    void Update() {
        Debug.Log(isGrounded);

         if(Input.GetButtonDown("Jump") && isGrounded) {
            anim.SetTrigger("jump");
            isJumping = true;
            jumpTimeCounter = jumpTime;
            rb.velocity = Vector3.up * jumpForce;
        }

        if(Input.GetButton("Jump") && isJumping) {
            if(jumpTimeCounter > 0) {
                rb.velocity = Vector3.up * jumpForce;
                jumpTimeCounter -= Time.deltaTime;
            } else isJumping = false;
        }

        if(Input.GetButtonUp("Jump")) {
            isJumping = false;
        }
    }

    void OnCollisionEnter(Collision other) {
        if(other.gameObject.CompareTag("Ground")) {
            isGrounded = true;           
        }
    }

    void OnCollisionExit(Collision other) {
         if(other.gameObject.CompareTag("Ground")) {
            isGrounded = false;           
        }
    }
}
