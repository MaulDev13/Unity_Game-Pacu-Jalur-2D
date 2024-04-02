using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementAI : Movement
{
    protected override void Init()
    {
        base.Init();

        InvokeRepeating("OnDayungRight", 1f, 0.2f);
        InvokeRepeating("OnDayungLeft", 1f, 0.2f);
    }

    protected override void CheckInput()
    {
        //base.CheckInput();
    }
}
