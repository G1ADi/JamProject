using UnityEngine;
using System.Collections;

// A very simplistic car driving on the x-z plane.

public class PlaceHolderCharControl : MonoBehaviour
{

    public GameObject bananaPrefab;
    public int bananas = 0;
    public float speed = 10.0f;
    public float rotationSpeed = 100.0f;
    public float jumpForce = 10.0f;
    public bool isOnGround = true;
    private Animator playerAnim;
    private Rigidbody playerRb;

    void Start() 
    {   
        playerRb = GetComponent<Rigidbody>();
        playerAnim = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O)) 
        {
            bananas = 0;
            Debug.Log($"Bananas set to 0");
        }  

        if (Input.GetKeyDown(KeyCode.Space) && isOnGround) 
        {
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isOnGround = false;
            playerAnim.SetBool("isOnGround", false);
            playerAnim.SetTrigger("Jump_trig");
        }
        // Get the horizontal and vertical axis.
        // By default they are mapped to the arrow keys.
        // The value is in the range -1 to 1
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        // - speed because the 3D model is backwards

        Vector3 movementDirection = new Vector3(horizontalInput, 0, verticalInput);
        movementDirection.Normalize();

        transform.Translate(movementDirection * Time.deltaTime * -speed, Space.World);

        if (movementDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }

        if (horizontalInput > 0.01f || horizontalInput < -0.01 || verticalInput > 0.01f || verticalInput < -0.01)
        {
            playerAnim.SetBool("isWalking", true);
        } else {
            playerAnim.SetBool("isWalking", false);
        }
    }


    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
            playerAnim.SetBool("isOnGround", true);
        }

        if (other.gameObject.CompareTag("Banana"))
        {
            bananas += 1;
            Debug.Log($"I got a frickin banana");
            Destroy(other.gameObject);               
        }
    }


    private void LateUpdate() 
    {
        if (bananas == 10) {
            transform.localScale += new Vector3(1, 1, 1);
        }
    }

}