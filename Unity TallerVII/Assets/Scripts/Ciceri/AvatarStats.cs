using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UnityEngine.Events;

public class AvatarStats : NetworkBehaviour
{
    //variables normales 

    [SerializeField] private int maxHealth;

    [SerializeField] private float speed;

    //variables  Networked
    [Networked] public int Health { get; private set; }

    [Networked] public int Score { get; private set; }

    [Networked] public bool IsDead  { get; private set; }

    // Events 
    #region UnityEvents   
    private UnityEvent onHit = new UnityEvent(); public UnityEvent OnHit => OnHit;
    private UnityEvent onDeath = new UnityEvent(); public UnityEvent OnDeath => onDeath;
    private UnityEvent onHeal = new UnityEvent(); public UnityEvent OnHeal => onHeal;
    private UnityEvent onAddScore = new UnityEvent(); public UnityEvent OnAddScore => onAddScore;

    #endregion
    //, onDeath, onHeal, onAddScore;

    // 
    public int MaxHealth { get => maxHealth; }
    public float Speed { get => speed; set => speed = value; }

    public override void Spawned()
    {
        Health = MaxHealth;
        onHit.AddListener(messageHit);
        onDeath.AddListener(messageDie);
        onHeal.AddListener(messageHealth);
        onAddScore.AddListener(messagePoint);
       
        Debug.Log("la vida del compa "+ Health + " la velocidad "+ Speed+" puntaje "+ 0);
    }
    public void AddScore()
    {
        Score += 1;
        Debug.Log("score" +Score);
        onAddScore.Invoke();
    }
    public void Die()
    {
        if (Health <= 0)
        {
            IsDead = true;
            onDeath.Invoke();

        }
    }
    public void GetHit(int Damage)
    {
        if (!IsDead)
        {
            Health -= Mathf.Abs(Damage);
            onHit.Invoke();

            if (Health <= 0)
            {
                 Die();
            }
           
        }
        else Debug.Log("Anda Muerto");
        

    }
    public void GetHeal(int Heal)
    {
        if (!IsDead)
        {
                        
            if (Health == maxHealth && (Health + Heal) >=maxHealth)
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
    //funcion temporal para Respawn
    public void Respawn()
    {
        IsDead = false;
        Health = 200;
    }

    #region eventos temporales 
    //eventos para sucribirse para practicas
    private void messageHit()
    {
        Debug.Log("te golpiaron y feo ");
    }
    private void messageDie()
    {
        Score = 0;
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
