using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1f;
    Rigidbody2D enemyRigidBody2D;
    Collider2D enemyBoxCollider2D;
    // Start is called before the first frame update
    void Start()
    {
        enemyRigidBody2D = GetComponent<Rigidbody2D>();
        enemyBoxCollider2D = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        enemyRigidBody2D.velocity = new Vector2(moveSpeed, 0f);
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        moveSpeed = -moveSpeed;
        transform.localScale = new Vector2(-Mathf.Sign(enemyRigidBody2D.velocity.x), 1f);
    }
}
