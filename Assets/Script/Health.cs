using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] int health = 100;
    [SerializeField] int scoreValue = 50; 
    [SerializeField] int penaltyValue = 25; 
    [SerializeField] bool isPlayer;
    [SerializeField] bool isBoss;

    [Header("Juice")]
    [SerializeField] ParticleSystem hitEffect;
    [SerializeField] bool applyCameraShake;
    CameraShake cameraShake;

    public Action OnDeath;

    void Awake()
    {
        if (Camera.main != null)
        {
            cameraShake = Camera.main.GetComponent<CameraShake>();
        }
    }

    void Start()
    {
        // INTEGRATION: Sync health with GameSession at the start of the level
        if (isPlayer && GameSession.Instance != null)
        {
            health = GameSession.Instance.GetPersistentHealth();
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        // INTEGRATION: Update the master record in GameSession immediately
        if (isPlayer && GameSession.Instance != null)
        {
            GameSession.Instance.UpdatePersistentHealth(health);
        }

        PlayHitEffect();
        DoCameraShake();

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        OnDeath?.Invoke();

        if (isPlayer)
        {
            SceneManager.LoadScene("GameOver");
        }
        else if (isBoss)
        {
            MusicPlayer.Instance?.StopMusic();
        }

        Destroy(gameObject);
    }

    void PlayHitEffect()
    {
        if (hitEffect != null)
        {
            ParticleSystem instance = Instantiate(hitEffect, transform.position, Quaternion.identity);
            Destroy(instance.gameObject, instance.main.duration + instance.main.startLifetime.constantMax);
        }
    }

    void DoCameraShake()
    {
        if (applyCameraShake && cameraShake != null)
        {
            cameraShake.Play();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageDealer = other.GetComponent<DamageDealer>();
        if (damageDealer == null) return;

        // --- SCORE & PENALTY LOGIC ---
        if (!isPlayer && other.CompareTag("PlayerProjectile"))
        {
            // Enemy hit by player laser -> GAIN POINTS
            GameSession.Instance?.AddToScore(scoreValue);
        }
        else if (isPlayer)
        {
            // Player hit by anything (projectile or body) -> LOSE POINTS
            GameSession.Instance?.PenaltyScore(penaltyValue);
        }

        TakeDamage(damageDealer.GetDamage());
        damageDealer.Hit();
    }
}