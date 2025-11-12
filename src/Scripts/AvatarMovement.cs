using UnityEngine;
using UnityEngine.AI;

public class AvatarMovement : MonoBehaviour
{
    public Transform destination;

    private NavMeshAgent agent;
    private Animator animator;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        if (agent != null && destination != null)
        {
            agent.SetDestination(destination.position);
        }
    }

    void Update()
    {
        if (agent != null)
        {
            // Check if the agent is moving
            bool isWalking = agent.velocity.magnitude > 0.1f;
            animator.SetBool("IsWalking", isWalking);

            // Stop the agent when it reaches the destination
            if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
            {
                agent.isStopped = true;
                animator.SetBool("IsWalking", false); // Stop walking animation
            }
        }
    }
}