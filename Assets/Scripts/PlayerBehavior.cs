using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBehavior : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float maxSpeed;
    [SerializeField] private LayerMask ground;

    private Rigidbody2D myRigidbody2D;


    private Animator myAnimator;
    private SpriteRenderer myRenderer;

    private Vector2 stickDirection;
    private bool isOnGround = false;
    private bool isFacingLeft = true;

    private void OnEnable()
    {
        var playerController = new PlayerController();
        playerController.Enable();
        playerController.Main.Move.performed += MoveOnPerformed;
        playerController.Main.Move.canceled += MoveOnCanceled;

    }

    private void MoveOnPerformed(InputAction.CallbackContext obj)
    {
        stickDirection = obj.ReadValue<Vector2>();
    }

    private void MoveOnCanceled(InputAction.CallbackContext obj)
    {
        stickDirection = Vector2.zero;
    }


    void Start()
    {
        myRigidbody2D = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myRenderer = GetComponent<SpriteRenderer>();
    }


    void FixedUpdate()
    {
        var direction = new Vector2 { x = stickDirection.x, y = 0 };

        if (myRigidbody2D.velocity.sqrMagnitude < maxSpeed)
        {
            myRigidbody2D.AddForce(direction * speed);
        }

        var isWalking = isOnGround && Mathf.Abs(myRigidbody2D.velocity.x) > 0.1f;
        myAnimator.SetBool("isWalking", isWalking);

        Flip();

    }

    private void Flip()
    {
        if (stickDirection.x < -0.1f)
        {
            isFacingLeft = true;
        }

        if (stickDirection.x > 0.1f)
        {
            isFacingLeft = false;
        }
        myRenderer.flipX = isFacingLeft;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        var touchingGround = ground == (ground | (1 << other.gameObject.layer));

        if (touchingGround)
        {
            isOnGround = true;
        }
    }
}
