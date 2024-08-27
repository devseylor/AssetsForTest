using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpHeight = 2f;
    public float gravity = -9.81f;
    public float attackRange = 1.5f;
    public float attackCooldown = 0.5f;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private bool canAttack = true;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        Move();
        Attack();
        ApplyGravity();
    }

    void Move()
    {
        // Get input for movement
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        controller.Move(move * moveSpeed * Time.deltaTime);

        // Handle Jumping
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    void Attack()
    {
        if (Input.GetButtonDown("Fire1") && canAttack)
        {
            StartCoroutine(PerformAttack());
        }
    }

    IEnumerator PerformAttack()
    {
        canAttack = false;

        // Perform the attack animation
        // Animator should be attached and properly set up with animations
        Animator animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }

        // Detect enemies in range
        RaycastHit hit;
        if (Physics.SphereCast(transform.position, attackRange, transform.forward, out hit, attackRange))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                // Deal damage to enemy
                // Assuming the enemy has a script with a TakeDamage method
                //hit.collider.GetComponent<Enemy>().TakeDamage(10); // Replace with your damage logic
            }
        }

        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    void ApplyGravity()
    {
        isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void OnDrawGizmosSelected()
    {
        // Draw the attack range in the editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + transform.forward * attackRange, attackRange);
    }
}
