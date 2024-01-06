using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationCondition : PlayerMovements
{
    [SerializeField] private Animator animator;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        animator.SetBool("IsGrounded", isGrounded);
        float Speed = rb.velocity.magnitude;
        animator.SetFloat("Speed", Speed);

        if (isGrounded )
        {
            Debug.Log("ausollanim");
        }
    }
}
