using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Sebuah Item. Ketika bersentuhan akan mengembalikan kesempatan boost kepada perahu.
/// </summary>

public class IBoost : Item
{
    public override void Acive(Movement collision)
    {
        coll2d.enabled = false;
        spriteRenderer.enabled = false;

        collision.GetBoost();
    }
}
