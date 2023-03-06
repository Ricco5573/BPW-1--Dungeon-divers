using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{

    public int health;
    public int speed;
    public int damage = 2;
    public int attackSpeed;
    public bool alive;
    public bool reviving;
    public bool canAttack = false;
    public NavMeshAgent agent;
    private bool patrolling;
    private bool chasing;
    private bool investigating;
    private bool attacking;
    private GameObject player;
    [SerializeField]
    private int patrolPoint;
    [SerializeField]
    private Transform[] patrolRoute;

    [SerializeField]
    private Animator anim;
    [SerializeField]
    private AudioSource Walking;
    void Start()
    {
        if(patrolRoute.Length != 0)
        {
            patrolling = true;
            Walking.Play();
                
        }
        else patrolling = false;
    }

    void Update()
    {
        if (alive)
        {
            if (patrolling)
            {
                Walking.Play();
                ChangeRoute();
                anim.SetBool("Patrolling", true);
                anim.SetBool("Chasing", false);

            }
            else if (chasing)
            {
                agent.SetDestination(player.transform.position);
                anim.SetBool("Patrolling", false);
                anim.SetBool("Chasing", true);

                while (!attacking && canAttack && chasing && !agent.pathPending)
                {
                    anim.SetBool("Attacking", true);
                    StartCoroutine(Attack());
                }
                if (agent.remainingDistance < 1)
                {
                    agent.isStopped = true;
                    canAttack = true;
                    Walking.Stop();

                }
                else if (agent.remainingDistance > 2)
                {
                    canAttack = false;
                    Walking.Play();

                }
                if (agent.remainingDistance > 40)
                {
                    patrolling = false;
                    chasing = false;

                }
            }
            else if (!chasing && !patrolling)
            {
                if (agent.remainingDistance > 0.1f)
                {
                    patrolling = true;
                }
            }
            if (!alive && !reviving)
            {
                StartCoroutine(Revive());
            }
        }
    }

        public void Hit(int damageTaken)
    {
        health -= damageTaken;
        if(health <= 0)
        {
            alive = false;
            Walking.Stop();

            anim.SetBool("death", true);
        }
    }
    private IEnumerator Attack()
    {
        attacking = true;
        Debug.Log("Attacking");
        yield return new WaitForSecondsRealtime(1);
        if (canAttack)
        {
            bool parried = player.GetComponent<Player>().Damage(damage);
            if (!parried)
            {
                yield return new WaitForSecondsRealtime(1);
            }
            else if (parried)
            {
                yield return new WaitForSecondsRealtime(2);
                anim.SetBool("parried", true);
            }   
        }
            attacking = false;
        
        yield return null;
        anim.SetBool("Attacking", false);
    }

    private IEnumerator Revive()
    {
        reviving = true;
        yield return new WaitForSecondsRealtime(300);
        alive = true;
        reviving = false;
        anim.SetBool("death", false);
    }

    private void ChangeRoute()
    {
        if (agent.pathPending)
            return;

        if (agent.remainingDistance < .5f)
        {
            patrolPoint += 1;

            if (patrolPoint >= patrolRoute.Length)  
            {
                patrolPoint = 0;
            }
            agent.SetDestination(patrolRoute[patrolPoint].position);

        }
    }
    private void OnTriggerEnter(Collider other)
    {
      
        if(other.gameObject.tag == "Player")
        {
            patrolling = false;
            chasing = true;
            player = other.gameObject;
            canAttack = true;
        }
    }
}
