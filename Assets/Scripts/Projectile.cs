using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int damage = 3; // Damage dealt by the projectile

    void OnCollisionEnter(Collision collision)
    {
        /*
        // Check if the projectile hits an enemy
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Assuming the enemy has a script with a method to handle taking damage
            EnemyAi enemy = collision.gameObject.GetComponent<EnemyAi>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }
        */
        // Destroy the projectile on collision
        //Destroy(gameObject);
    }
}