using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAi : MonoBehaviour
{
    public Transform targetObj; // The player object
    public float moveSpeed = 10f;
    public float rotationSpeed = 5f; // Speed at which the enemy rotates towards the player
    public float surroundRadius = 5f; // Radius to surround the player
    public float surroundSpeed = 3f; // Speed for surrounding movement
    public float chaseThreshold = 1.5f; // Distance threshold to switch to chasing mode

    public int maxHealth = 5; // Default health
    private int currentHealth; // Current health of the enemy

    private static List<EnemyAi> allEnemies = new List<EnemyAi>(); // List to keep track of all enemies

    void Start()
    {
        // Initialize current health
        currentHealth = maxHealth;

        // Add this enemy to the list of all enemies
        if (!allEnemies.Contains(this))
        {
            allEnemies.Add(this);
        }
    }

    void Update()
    {
        // Check if there are 5 or more enemies
        if (allEnemies.Count >= 5)
        {
            SurroundPlayer();
        }
        else
        {
            MoveTowardsPlayer();
        }
    }

    void MoveTowardsPlayer()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetObj.position, moveSpeed * Time.deltaTime);

        Vector3 direction = targetObj.position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    void SurroundPlayer()
    {
        // Calculate the angle step for distributing enemies around the player
        float angleStep = 360f / allEnemies.Count;
        float baseAngle = Time.time * 10f; // Rotate over time to create dynamic movement

        for (int i = 0; i < allEnemies.Count; i++)
        {
            EnemyAi enemy = allEnemies[i];
            if (enemy != this)
            {
                // Calculate target position around the player
                float angle = baseAngle + angleStep * i;
                Vector3 offset = new Vector3(Mathf.Cos(Mathf.Deg2Rad * angle), 0, Mathf.Sin(Mathf.Deg2Rad * angle)) * surroundRadius;
                Vector3 targetPosition = targetObj.position + offset;

                // Move towards the target position while considering the player’s position
                if (Vector3.Distance(enemy.transform.position, targetObj.position) < surroundRadius * 1.5f)
                {
                    // If too close to the player, move towards the surrounding position
                    Vector3 direction = (targetPosition - enemy.transform.position).normalized;
                    enemy.transform.position += direction * surroundSpeed * Time.deltaTime;
                }
                else
                {
                    // If too far, move towards the player
                    Vector3 direction = (targetObj.position - enemy.transform.position).normalized;
                    enemy.transform.position += direction * moveSpeed * Time.deltaTime;
                }

                // Ensure the enemy looks towards the player
                Vector3 playerDirection = targetObj.position - enemy.transform.position;
                Quaternion targetRotation = Quaternion.LookRotation(playerDirection);
                enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Handle enemy death (e.g., play animation, destroy object)
        Debug.Log("Enemy died!");
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        // Remove this enemy from the list when it is destroyed
        if (allEnemies.Contains(this))
        {
            allEnemies.Remove(this);
        }
    }
}
