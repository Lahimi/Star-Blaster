using System.Collections;
using UnityEngine;

public class BossAttackAnimation : MonoBehaviour
{
    [Header("Animation Settings")]
    [SerializeField] Animator animator;
    [SerializeField] string attackTriggerName = "Attack";
    [SerializeField] float attackCooldown = 2f; // Time between attacks
    
    [Header("Timing Settings")]
    [SerializeField] float preAttackDelay = 0.3f; // Time before projectile fires
    [SerializeField] float animationLength = 1f; // Length of attack animation
    
    Shooter shooter;
    bool canAttack = true;

    void Start()
    {
        // Get the Shooter component
        shooter = GetComponent<Shooter>();
        
        // Get animator if not assigned
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
        
        if (shooter == null)
        {
            Debug.LogError("BossAttackAnimation: No Shooter component found!");
        }
    }

    void Update()
    {
        // Boss attacks on a cooldown, not continuously
        if (shooter != null && shooter.isFiring && canAttack)
        {
            StartCoroutine(PerformAttack());
        }
    }

    IEnumerator PerformAttack()
    {
        canAttack = false;
        
        // Stop normal firing
        shooter.isFiring = false;
        
        // Play attack animation
        if (animator != null)
        {
            animator.SetTrigger(attackTriggerName);
            Debug.Log("Boss attack animation triggered");
        }
        
        // Wait for animation to reach firing point
        yield return new WaitForSeconds(preAttackDelay);
        
        // Fire projectile using the new public method
        if (shooter != null)
        {
            shooter.FireSingleProjectile();
            Debug.Log("Boss fired projectile");
        }
        
        // Wait for the rest of the animation to complete
        yield return new WaitForSeconds(animationLength - preAttackDelay);
        
        // Resume normal firing
        shooter.isFiring = true;
        
        // Wait for cooldown before next attack
        yield return new WaitForSeconds(attackCooldown);
        
        canAttack = true;
    }
    
    // Optional: Visual feedback in Scene view
    void OnDrawGizmosSelected()
    {
        if (shooter != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, 0.5f);
        }
    }
}