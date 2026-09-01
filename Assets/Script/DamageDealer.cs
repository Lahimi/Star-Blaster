using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    [SerializeField] int damage = 10;
    [SerializeField] bool isProjectile = true; // Check this for Bullets, Uncheck for Enemies

    public int GetDamage()
    {
        return damage;
    }

    public void Hit()
    {
        // Only bullets should destroy themselves immediately on impact
        if (isProjectile)
        {
            Destroy(gameObject);
        }
    }
}