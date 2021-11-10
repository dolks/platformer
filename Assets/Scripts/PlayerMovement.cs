using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    Vector2 moveInput;
    Rigidbody2D playerRigidbody2D;
    [SerializeField] float runSpeed = 10;
    [SerializeField] float jumpSpeed = 5;
    Animator playerAnimator;
    Collider2D playerCollider2D;
    const string IS_RUNNING_ANIMATION_PARAM = "isRunning";
    const string GROUND_LAYER_NAME = "Ground";
    // Start is called before the first frame update
    void Start()
    {
        playerRigidbody2D = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        playerCollider2D = GetComponent<CapsuleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Run();
        FlipSprite();
    }

    private void Run()
    {
        playerRigidbody2D.velocity = new Vector2(
            moveInput.x * runSpeed,
            playerRigidbody2D.velocity.y
        );
        playerAnimator.SetBool(IS_RUNNING_ANIMATION_PARAM, true);
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        if (value.isPressed && playerCollider2D.IsTouchingLayers(LayerMask.GetMask(GROUND_LAYER_NAME)))
        {
            playerRigidbody2D.velocity += new Vector2(0, jumpSpeed);
        }
    }

    void FlipSprite()
    {
        bool hasHorizontalMovement = Mathf.Abs(playerRigidbody2D.velocity.x) > Mathf.Epsilon;
        if (hasHorizontalMovement)
        {
            transform.localScale = new Vector2(Mathf.Sign(playerRigidbody2D.velocity.x), 1f);

        } else
        {
            playerAnimator.SetBool(IS_RUNNING_ANIMATION_PARAM, false);
        }

    }
}
