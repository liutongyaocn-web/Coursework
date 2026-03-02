using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class ZombieController : MonoBehaviour
{
    private NavMeshAgent agent;
    public Transform target;

    public int attackDamage = 10;
    public float attackCooldown = 1f;
    private float lastAttackTime;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        
        if (target == null)
        {
            GameObject playerObj = GameObject.Find("Player");
            if (playerObj != null)
            {
                target = playerObj.transform;
            }
        }
    }

    void Update()
    {
        if (target != null)
        {
            agent.SetDestination(target.position);
        }
    }

    void OnCollisionStay(Collision collision)
    {
        // Try to find the HealthSystem on the object we collided with
        HealthSystem health = collision.gameObject.GetComponent<HealthSystem>();
        
        // If it has a HealthSystem AND enough time has passed since our last attack
        if (health != null && Time.time >= lastAttackTime + attackCooldown)
        {
            // Specifically checking if it's the Player
            if (collision.gameObject.name == "Player")
            {
                // Deal damage and update the cooldown timer
                health.TakeDamage(attackDamage);
                lastAttackTime = Time.time;
            }
        }
    }
}
