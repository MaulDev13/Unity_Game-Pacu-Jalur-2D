using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IObstacle : Item
{
    [SerializeField] float time = 1f;

    private Movement boat;

    public override void Acive(Movement collision)
    {
        coll2d.enabled = false;
        spriteRenderer.enabled = false;

        boat = collision;
        boat.GetStunned(time);
    }
}
