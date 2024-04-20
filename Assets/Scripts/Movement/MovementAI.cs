using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Jenis movement AI. Dayung akan bergerak secara konstan.
/// </summary>

public class MovementAI : Movement
{
    [Header("Movement AI 1")]
    [Tooltip("Waktu pengulangan untuk menggerakkan dayung kanan.")]
    [SerializeField] private float dayungRight_repeatingTime = 0.2f;
    [Tooltip("Waktu pengulangan untuk menggerakkan dayung kiri.")]
    [SerializeField] private float dayungLeft_repeatingTime = 0.2f;

    private void OnDisable()
    {
        OnFinishLine-=DisableDayung;
    }

    protected override void Init()
    {
        base.Init();

        InvokeRepeating("OnDayungRight", 1f, dayungRight_repeatingTime);
        InvokeRepeating("OnDayungLeft", 1f, dayungLeft_repeatingTime);

        OnFinishLine += DisableDayung;
    }

    protected override void InputCheck()
    {

    }

    void DisableDayung()
    {
        CancelInvoke("OnDayungRight");
        CancelInvoke("OnDayungLeft");
    }
}