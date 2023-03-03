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

    public UnityEvent onHit, onDeath, onHeal, onAddScore;


    public int Health { get => health; set => health = value; }
    public int MaxHealth { get => maxHealth; }
    public float Speed { get => speed; set => speed = value; }
    public int Score { get => score; set => score = value; }
    public bool IsDead { get =>isDead ;  }

    void Start()
    {
        health = maxHealth;
        onHit.AddListener(messageHit);
        onDeath.AddListener(messageDie);
        onHeal.AddListener(messageHealth);
        onAddScore.AddListener(messagePoint);
       
        Debug.Log("la vida del compa "+ Health +"la velocidad "+ Speed+" puntaje "+ 0);
    }
    public void AddScore()
    {
        Score += 1;
        Debug.Log("score" +score);
        onAddScore.Invoke();
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
        if (isDead)
        {
            health -= Damage;
            onHit.Invoke();
        }
        else Debug.Log("Anda Muerto");
        

    }
    public void GetHeal(int Health)
    {
        if (isDead)
        {
            health += Health;
            onHeal.Invoke();
        }
    }
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


}
