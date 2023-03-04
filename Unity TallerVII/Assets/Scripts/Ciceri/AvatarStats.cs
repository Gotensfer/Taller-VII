using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UnityEngine.Events;

public class AvatarStats : NetworkBehaviour
{
    [SerializeField]
    private int health;

    [SerializeField]
    private int maxHealth;

    [SerializeField]
    private float speed;

    [SerializeField]
    private int score;

    [SerializeField]
    private bool isDead;


    private UnityEvent onHit = new UnityEvent() ; public UnityEvent OnHit => OnHit;
    private UnityEvent onDeath = new UnityEvent(); public UnityEvent OnDeath => onDeath;
    private UnityEvent onHeal = new UnityEvent(); public UnityEvent OnHeal => onHeal;
    private UnityEvent onAddScore = new UnityEvent(); public UnityEvent OnAddScore => OnAddScore;

        //, onDeath, onHeal, onAddScore;

    public int Health { get => health;
        set 
        {
            if (Health <=maxHealth)
            {
                health = value;
            }
            else
            {
                health = maxHealth;
            }          
        }
    }
    public int MaxHealth { get => maxHealth; }
    public float Speed { get => speed; set => speed = value; }
    public int Score { get => score; set => score = value; }
    public bool IsDead { get =>isDead ;  }
    //pruebas de networked

    void Start()
    {
        health = maxHealth;
        onHit.AddListener(messageHit);
        onDeath.AddListener(messageDie);
        onHeal.AddListener(messageHealth);
        onAddScore.AddListener(messagePoint);
       
        Debug.Log("la vida del compa "+ Health +" la velocidad "+ Speed+" puntaje "+ 0);
    }
    public void AddScore()
    {
        Score += 1;
        Debug.Log("score" +score);
        //onAddScore.Invoke();
    }
    public void Die()
    {
        if (health <= 0)
        {
            isDead = true;
            onDeath.Invoke();

        }
    }
    public void GetHit(int Damage)
    {
        if (!isDead)
        {
            health -= Mathf.Abs(Damage);
            onHit.Invoke();

            if (health <= 0)
            {
                 Die();
            }
           
        }
        else Debug.Log("Anda Muerto");
        

    }
    public void GetHeal(int Heal)
    {
        if (!isDead)
        {
                        
            if (Health == maxHealth && (Health+Heal) >=maxHealth)
            {
                Health = maxHealth;
            }
            else
            {
                Health += Heal;
                onHeal.Invoke();
            }
        }
    }

    #region eventos temporales 
    //eventos para sucribirse para practicas
    private void messageHit()
    {
        Debug.Log("te golpiaron y feo ");
    }
    private void messageDie()
    {
        Debug.Log("Moriste ");
    }
    private void messageHealth()
    {
        Debug.Log("Se curo ");
    }
    private void messagePoint()
    {
        Debug.Log("Ganaste puntos ");
    }

    #endregion
}
