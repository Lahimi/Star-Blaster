using System.Collections;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    [Header("Base Variables")]
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] float projectileSpeed = 10f;
    [SerializeField] float projectileLifetime = 5f;
    [SerializeField] float baseFireRate = 0.2f;
    
    // NEW - Animation variables
    [Header("Animation")]
    [SerializeField] Animator animator;
    [SerializeField] string firingBoolName = "isFiring"; // Must match Animator parameter

    [Header("AI Variables")]
    [SerializeField] bool useAI;
    [SerializeField] float fireRateVariance = 0f;
    [SerializeField] float minimumFireRate = 0.2f;

    // Public field - your PlayerController sets this directly
    [HideInInspector] public bool isFiring;
    
    Coroutine fireCoroutine;

    void Start()
    {
        // Get animator if not assigned
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
        
        if (useAI)
        {
            isFiring = true;
        }
    }

    void Update()
    {
        // Update animator with current firing state EVERY FRAME
        if (animator != null)
        {
            animator.SetBool(firingBoolName, isFiring);
        }
        
        HandleFiring();
    }

    void HandleFiring()
    {
        if (isFiring && fireCoroutine == null)
        {
            fireCoroutine = StartCoroutine(FireContinuously());
        }
        else if (!isFiring && fireCoroutine != null)
        {
            StopCoroutine(fireCoroutine);
            fireCoroutine = null;
        }
    }

    IEnumerator FireContinuously()
    {
        while (true)
        {
            // Spawn projectile
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            projectile.transform.rotation = transform.rotation;
            
            Rigidbody2D projectileRB = projectile.GetComponent<Rigidbody2D>();
            if (projectileRB != null)
            {
                projectileRB.linearVelocity = transform.up * projectileSpeed;
            }
            
            Destroy(projectile, projectileLifetime);

            // Random fire rate
            float waitTime = Random.Range(baseFireRate - fireRateVariance, baseFireRate + fireRateVariance);
            waitTime = Mathf.Clamp(waitTime, minimumFireRate, float.MaxValue);

            yield return new WaitForSeconds(waitTime);
        }
    }
    
    // ===== NEW PUBLIC METHOD FOR BOSS =====
    public void FireSingleProjectile()
    {
        if (projectilePrefab != null)
        {
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            projectile.transform.rotation = transform.rotation;
            
            Rigidbody2D projectileRB = projectile.GetComponent<Rigidbody2D>();
            if (projectileRB != null)
            {
                projectileRB.linearVelocity = transform.up * projectileSpeed;
            }
            
            Destroy(projectile, projectileLifetime);
            
            // Optional: Add debug to confirm it's firing
            // Debug.Log($"{gameObject.name} fired single projectile");
        }
    }
}