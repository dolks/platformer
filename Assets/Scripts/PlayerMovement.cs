using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    Vector2 moveInput;
    Rigidbody2D playerRigidbody2D;
    [SerializeField] float runSpeed = 10f;
    // Start is called before the first frame update
    void Start()
    {
        playerRigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Run();
    }

    private void Run()
    {
        playerRigidbody2D.velocity = new Vector2(
            moveInput.x * runSpeed,
            playerRigidbody2D.velocity.y
        );
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }
}
