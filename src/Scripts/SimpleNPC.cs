using UnityEngine;
using UnityEngine.AI;

public class SimpleNPC : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Keep walking animation synced with movement
        animator.SetBool("isWalking", agent.velocity.magnitude > 0.1f);
    }

    public void MoveTo(Transform target)
    {
        agent.isStopped = false;
        agent.SetDestination(target.position);
    }

    public void TriggerAnim(string triggerName)
    {
        animator.SetTrigger(triggerName);
    }

    public bool IsMoving()
    {
        if (agent.pathPending) return true;
        return agent.remainingDistance > agent.stoppingDistance;
    }
}