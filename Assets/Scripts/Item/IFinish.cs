using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IFinish : Item
{
    Movement boat;

    public override void Acive(Movement collision)
    {
        boat = collision;
        boat.GetEnd();

        Debug.Log($"{boat.name} is finsih");
    }
}
