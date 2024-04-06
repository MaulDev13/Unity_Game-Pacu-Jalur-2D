using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementAI2 : Movement
{
    protected override void Init()
    {
        base.Init();
    }

    protected override void CheckInput()
    {
        DayungAI();
    }

    private void DayungAI()
    {
        var leftDayung = Random.Range(0, 2);
        if (leftDayung >= 1)
        {
            OnDayungLeft();
        }

        var rightDayung = Random.Range(0, 2);
        if (rightDayung >= 1)
        {
            OnDayungRight();
        }
    }
}
