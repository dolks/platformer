using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    Vector2 moveInput;
    Rigidbody2D playerRigidbody2D;
    [SerializeField] float runSpeed = 10;
    [SerializeField] float jumpSpeed = 25;
    [SerializeField] float climbSpeed = 25;
    [SerializeField] float bulletSpeed = 7f;
    [SerializeField] GameObject magicBulletPrefab;
    [SerializeField] GameObject gun;
    Animator playerAnimator;
    Collider2D playerCollider2D;
    Collider2D feetCollider2D;
    float playerGravityAtStart;
    bool isAlive = true;
    const string IS_RUNNING_ANIMATION_PARAM = "isRunning";
    const string IS_CLIMBING_ANIMATION_PARAM = "isClimbing";
    const string DEAD_ANIMATION_PARAM = "Dead";
    const string GROUND_LAYER_NAME = "Ground";
    const string LADDER_LAYER_NAME = "Ladder";
    const string ENEMY_LAYER_NAME = "Enemy";
    const string HAZARD_LAYER_NAME = "Hazard";
    // Start is called before the first frame update
    void Start()
    {
        playerRigidbody2D = GetComponent<Rigidbody2D>();
        playerGravityAtStart = playerRigidbody2D.gravityScale;
        playerAnimator = GetComponent<Animator>();
        playerCollider2D = GetComponent<CapsuleCollider2D>();
        feetCollider2D = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isAlive)
        {
            Run();
            FlipSprite();
            Climb();
        }
    }

    private void Run()
    {
        playerRigidbody2D.velocity = new Vector2(
            moveInput.x * runSpeed,
            playerRigidbody2D.velocity.y
        );
        playerAnimator.SetBool(IS_RUNNING_ANIMATION_PARAM, true);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (playerCollider2D.IsTouchingLayers(
            LayerMask.GetMask(
                ENEMY_LAYER_NAME,
                HAZARD_LAYER_NAME)
                )
            )
        {
            Die();
        }
    }

    private void Die()
    {
        isAlive = false;
        playerAnimator.SetTrigger(DEAD_ANIMATION_PARAM);
        playerRigidbody2D.velocity = new Vector2(0, 0);
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        if (!feetCollider2D.IsTouchingLayers(LayerMask.GetMask(GROUND_LAYER_NAME))
            || !isAlive)
        {
            return;
        }
        if (value.isPressed)
        {
            playerRigidbody2D.velocity += new Vector2(0, jumpSpeed);
        }
    }

    void OnFire(InputValue value)
    {
        if (value.isPressed && isAlive)
        {
            GameObject bullet = Instantiate(magicBulletPrefab, gun.transform.position, transform.rotation);
            bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(bulletSpeed, 0f);
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

    void Climb()
    {
        if (!playerCollider2D.IsTouchingLayers(LayerMask.GetMask(LADDER_LAYER_NAME))) {
            playerAnimator.SetBool(IS_CLIMBING_ANIMATION_PARAM, false);
            playerRigidbody2D.gravityScale = playerGravityAtStart;
            return;
        }
        playerRigidbody2D.velocity = new Vector2(
            playerRigidbody2D.velocity.x,
            moveInput.y * climbSpeed
        );
        playerRigidbody2D.gravityScale = 0;
        bool hasVerticalMovement = Mathf.Abs(playerRigidbody2D.velocity.y) > Mathf.Epsilon;
        playerAnimator.SetBool(IS_CLIMBING_ANIMATION_PARAM, hasVerticalMovement);
    }
}
