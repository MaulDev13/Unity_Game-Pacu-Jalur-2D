using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementUser : Movement
{
    private float rightPower;
    private float leftPower;

    private bool rightPowerOn = false;
    private bool leftPowerOn = false;

    [Header("Dayung Movement")]
    [Tooltip("Nilai peningkatan kekuatan mendayung setiap dayungan.")]
    [SerializeField] private float dayungPower = 10f;
    [Tooltip("Nilai pengurangan kekuatan mendayung ketika tidak didayung. Perubahan ini terjadi setelah rentang waktu powerOn_Time berakhir.")]
    [SerializeField] private float dayungPowerDegeneration = 1f;

    [Tooltip("Batas minimum dari rentang dayungPower (kekuatan mendayung).")]
    [SerializeField] private float dayungPowerMinLimit = 0f;
    [Tooltip("Batas maximum dari rentang dayungPower (kekuatan mendayung).")]
    [SerializeField] private float dayungPowerMaxLimit = 100f;

    [Tooltip("Rentang waktu untuk dayungPowerDegeneration aktif setelah melakukan dayung.")]
    [SerializeField] private float powerOn_Time = 0.1f;

    [Tooltip("Batas tengah supaya perahu dapat bergerak.")]
    [SerializeField] private float midTreshold = 20f;
    [Tooltip("Batas atas supaya perahu dapat bergerak.")]
    [SerializeField] private float upperTreshold = 50f;

    [SerializeField] private bool isAutoMove;
    [SerializeField] private float timeCheck = 0f;
    private float currentTime = 0f;

    protected override void FixedUpdate()
    {
        if (!isStart || isEnd)
            return;

        if(currentTime > timeCheck)
        {
            isStart = false;
            Debug.Log("End Turn");
        }
        else
        {
            currentTime += Time.fixedDeltaTime;
        }

        if (isEnd)
        {
            ZeroPower();
            Move();
            AnimCheck();
            return;
        }

        if (isStunned)
        {
            AnimCheck();
            return;
        }

        Velocity();
        Move();
        AnimCheck();
    }

    protected override void Init()
    {
        base.Init();

        isPlayer = true;

        ZeroPower();
        AnimCheck();

        DayungPower(1f, 1f);
    }

    protected override void InputCheck()
    {
        if(isAutoMove)
        {
            OnDayungRight();
            OnDayungLeft();
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                OnDayungLeft();
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                OnDayungRight();
            }

            if (Input.GetKeyDown(KeyCode.W))
            {
                OnDayungRight();
                OnDayungLeft();
            }
        }
    }

    protected override void Velocity()
    {
        // Not move / Slow move
        if (rightPower <= midTreshold && leftPower <= midTreshold)
        {
            velocity = Vector2.right;
            currentSpeed = idleSpeed;
        }
        // Slightly turn left
        else if (rightPower <= midTreshold && leftPower > midTreshold)
        {
            velocity = new Vector2(1, 1).normalized;
            currentSpeed = slowTurnSpeed;
        }
        // Slightly turn right
        else if (rightPower > midTreshold && leftPower <= midTreshold)
        {
            velocity = new Vector2(1, -1).normalized;
            currentSpeed = slowTurnSpeed;
        }
        // Move
        else if (rightPower > midTreshold && leftPower > midTreshold && (rightPower <= upperTreshold && leftPower <= upperTreshold))
        {
            velocity = Vector2.right;
            currentSpeed = movementSpeed;
        }
        // Turn Right
        else if (rightPower > upperTreshold && leftPower <= upperTreshold)
        {
            velocity = new Vector2(1, -1).normalized;
            currentSpeed = fastTurnSpeed;
        }
        // Turn Left
        else if (rightPower <= upperTreshold && leftPower > upperTreshold)
        {
            velocity = new Vector2(1, 1).normalized;
            currentSpeed = fastTurnSpeed;
        }
        // Run
        else if (rightPower > upperTreshold && leftPower > upperTreshold)
        {
            velocity = Vector2.right;
            currentSpeed = runSpeed;
        }
    }

    protected override void Move()
    {
        currentSpeed = Mathf.Clamp(currentSpeed, idleSpeed, 99f);

        if (boostPowerOn)
            currentSpeed *= boostSpeedMultiplier;
        else
            currentSpeed = Mathf.Clamp(currentSpeed - debuffSpeed, idleSpeed, 99f);

        //Debug.Log($"Speed = {currentSpeed}, boost on = {boostPowerOn}");

        rb2d.MovePosition(rb2d.position + (velocity * currentSpeed) * Time.fixedDeltaTime);

        var tmpRight = rightPowerOn ? rightPower : rightPower - dayungPowerDegeneration;
        var tmpLeft = leftPowerOn ? leftPower : leftPower - dayungPowerDegeneration;
        DayungPower(tmpRight, tmpLeft);
    }

    // Merubah kekuatan dayung power secara instan
    protected void DayungPower(float right, float left)
    {
        rightPower = right;
        leftPower = left;

        rightPower = Mathf.Clamp(right, dayungPowerMinLimit, dayungPowerMaxLimit);
        leftPower = Mathf.Clamp(left, dayungPowerMinLimit, dayungPowerMaxLimit);
    }

    // Merubah kekuatan dayung power dan velocity menjadi 0 secara instan. Menghentikan perahu secara instan.
    protected void ZeroPower()
    {
        velocity = Vector2.zero;
        currentSpeed = 0f;
    }

    // Kekuatan dayung kanan. Aktif pada button untuk player input.
    public override void OnDayungRight()
    {
        rightPower += dayungPower;

        StopCoroutine(HoldPowerRight());
        StartCoroutine(HoldPowerRight());
    }

    // Kekuatan dayung kiri. Aktif pada button untuk player input.
    public override void OnDayungLeft()
    {
        leftPower += dayungPower;

        StopCoroutine(HoldPowerLeft());
        StartCoroutine(HoldPowerLeft());
    }

    // Time check untuk dayung kanan sehingga tidak terjadi degenerasi kekuatan
    IEnumerator HoldPowerRight()
    {
        rightPowerOn = true;

        yield return new WaitForSeconds(powerOn_Time);

        rightPowerOn = false;
    }

    // Time check untuk dayung kiri sehingga tidak terjadi degenerasi kekuatan
    IEnumerator HoldPowerLeft()
    {
        leftPowerOn = true;

        yield return new WaitForSeconds(powerOn_Time);

        leftPowerOn = false;
    }

    public override void GetStunned(float time)
    {
        base.GetStunned(time);

        DayungPower(1f, 1f);
        ZeroPower();   
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // What happen when hit another boat
            DayungPower(1f, 1f);
        }
    }
}
