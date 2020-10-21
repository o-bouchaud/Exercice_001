using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBehavior : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float maxSpeed;

    private Rigidbody2D myRigidbody2D;

    private Vector2 stickDirection;


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
    }

    void FixedUpdate()
    {
        var direction = new Vector2 { x = stickDirection.x, y = 0 };

        if (myRigidbody2D.velocity.sqrMagnitude < maxSpeed)
        {
            myRigidbody2D.AddForce(direction * speed);
        }

    }
}
