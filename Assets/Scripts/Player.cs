using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed;
    public float jumpForce;
    private Rigidbody rb;
    private bool isGrounded;
    public Animator anim;

    public Transform pivot;
    public float rotateSpeed;
    
    public GameObject model;

    void Start()
    {
       rb = GetComponent<Rigidbody>();
    }

    void Update()
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

        if(Input.GetButtonDown("Jump") && rb.velocity.y == 0) {
            anim.SetTrigger("jump");
            rb.velocity = Vector3.up * jumpForce;
            //StartCoroutine(StartJumping());
        }

        anim.SetBool("isGrounded", isGrounded);
        anim.SetFloat("speed", Mathf.Abs(vertical) + Mathf.Abs(horizontal));
    }

    IEnumerator StartJumping() {
        anim.SetTrigger("jump");
        yield return new WaitForSeconds(.5f);
        rb.velocity = Vector3.up * jumpForce;
    }

    void OnCollisionStay(Collision other) {
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
