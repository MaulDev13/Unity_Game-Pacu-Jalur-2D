using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Jenis movement AI. Pergerakan perahu akan bergantung pada kesempatan acak.
/// </summary>

public class MovementAI2 : Movement
{
    [Header("Movement AI 2")]
    [Tooltip("Rentang nilai maximal dari nilai kesempatan dayung perahu. Nilai minimal adalah 0.")]
    [SerializeField] private int maxRandomRange = 100;
    [Tooltip("Nilai tengah yang digunakan untuk menghitung kesempatan perahu berhasil. Ex: [chanceIndex]/[maxRandomRange] atau [50]/[100]")]
    [SerializeField] private int chanceIndex = 50;

    protected override void Init()
    {
        base.Init();
    }

    protected override void InputCheck()
    {
        DayungAI();
    }

    private void DayungAI()
    {
        var leftDayung = Random.Range(0, maxRandomRange);
        if (leftDayung >= chanceIndex)
        {
            OnDayungLeft();
        }

        var rightDayung = Random.Range(0, maxRandomRange);
        if (rightDayung >= chanceIndex)
        {
            OnDayungRight();
        }
    }
}
