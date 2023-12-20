using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : MonoBehaviour
{
    public bool isDead;

    public abstract void GetInputs();
    public abstract void Movement();
    public abstract void Aim();
    public abstract void Fire();
    public abstract void TakeDamage(int pDamage);
    public abstract void Death();

    public virtual void Example()
    {
        Debug.Log("Tsägädägä");
    }
}
