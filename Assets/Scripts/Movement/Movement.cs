using System.Collections;
using UnityEngine;

/// <summary>
/// Movement system yang digunakan. Ini dibuat dengan User Input sebagai acuan
/// </summary>
public class Movement : MonoBehaviour
{
    private Rigidbody2D rb2d;

    private Animator anim;

    private Vector2 velocity;

    private float rightPower;
    private float leftPower;

    public delegate void OnEvent();
    public OnEvent OnTriggerBoost;
    public OnEvent OnRankingChange;
    public OnEvent OnFinishLine;

    [Header("Boat Stat")]
    [Tooltip("Apakah boat ini yang digunakan oleh player?")]
    [SerializeField] private bool isPlayer = false;

    [Tooltip("Speed yang digunakan ketika idle/tidak mendayung atau dayungPower (kekuatan mendayung) kurang dari mid treshold.")]
    [SerializeField] private float idleSpeed = 0.2f;
    [Tooltip("Speed yang digunakan ketika dayungPower (kekuatan mendayung) lebih besar dari mid treshold tetapi kurang dari upper treshold.")]
    [SerializeField] private float movementSpeed = 2f;
    [Tooltip("Speed yang digunakan ketika dayungPower (kekuatan mendayung) lebih besar dari upper treshold.")]
    [SerializeField] private float runSpeed = 4f;
    [Tooltip("Speed yang digunakan ketika akan berbelok dan dayungPower (kekuatan mendayung) lebih besar dari mid treshold tetapi kurang dari upper treshold.")]
    [SerializeField] private float fastTurnSpeed = 2f;
    [Tooltip("Speed yang digunakan ketika akan berbelok dan dayungPower (kekuatan mendayung) lebih kecil dari mid treshold.")]
    [SerializeField] private float slowTurnSpeed = 1f;
    [Tooltip("Speed boost multiplier. speed * boostSpeedMultiplier.")]
    [SerializeField] private float boostSpeedMultiplier = 4f;
    private float debuffSpeed = 0f;
    private float currentSpeed = 0f;

    [Tooltip("Nilai peningkatan kekuatan mendayung setiap dayungan.")]
    [SerializeField] private float dayungPower = 10f;
    [Tooltip("Nilai pengurangan kekuatan mendayung ketika tidak didayung. Perubahan ini terjadi setelah rentang waktu powerOn_Time berakhir.")]
    [SerializeField] private float dayungPowerDegeneration = 1f;

    [Tooltip("Batas minimum dari rentang dayungPower (kekuatan mendayung).")]
    [SerializeField] private float dayungPowerMinLimit = 0f;
    [Tooltip("Batas maximum dari rentang dayungPower (kekuatan mendayung).")]
    [SerializeField] private float dayungPowerMaxLimit = 100f;

    private bool rightPowerOn = false;
    private bool leftPowerOn = false;

    private bool boostPowerOn = false;
    [Tooltip("Dapat menggunakan boost ketika avaliable = true.")]
    [SerializeField] private bool isBoostAvaliable = false;
    public bool IsBoostAvaliable => isBoostAvaliable;

    [Tooltip("Lama boost akan aktif.")]
    [SerializeField] private float boostTime = 3f;
    private float currentBoostTime = 0f;

    private bool isStunned = false;

    private float currentSlowTime = 0f;

    [HideInInspector] public int ranking = 0;

    [HideInInspector] private bool isEnd = false;
    public bool isStart = false;
    [Tooltip("Rentang waktu untuk dayungPowerDegeneration aktif setelah melakukan dayung.")]
    [SerializeField] private float powerOn_Time = 0.1f;

    [Tooltip("Batas tengah supaya perahu dapat bergerak.")]
    [SerializeField] private float midTreshold = 20f;
    [Tooltip("Batas atas supaya perahu dapat bergerak.")]
    [SerializeField] private float upperTreshold = 50f;


    private void Start()
    {
        Init();
    }

    private void Update()
    {
        Ranking();

        if (!isStart)
            return;

        if (isEnd)
            return;

        InputCheck();
        TimeCheck();
    }

    private void FixedUpdate()
    {
        if (!isStart)
            return;

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

    protected virtual void Init()
    {
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponent<Animator>();

        ZeroPower();
        AnimCheck();

        DayungPower(1f, 1f);
    }

    protected virtual void InputCheck()
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

    // Get velocity of the boat depending on rightPower and leftPower
    void Velocity()
    {
        //velocity = Vector2.zero;

        // Not move / Slow move
        if (rightPower <= midTreshold && leftPower <= midTreshold)
        {
            velocity = Vector2.right;
            currentSpeed = idleSpeed;
        }
        // Slightly turn left
        else if (rightPower <= midTreshold && leftPower > midTreshold)
        {
            velocity = new Vector2(1, -1).normalized;
            currentSpeed = slowTurnSpeed;
        }
        // Slightly turn right
        else if (rightPower > midTreshold && leftPower <= midTreshold)
        {
            velocity = new Vector2(1, 1).normalized;
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
            velocity = new Vector2(1, 1).normalized;
            currentSpeed = fastTurnSpeed;
        }
        // Turn Left
        else if (rightPower <= upperTreshold && leftPower > upperTreshold)
        {
            velocity = new Vector2(1, -1).normalized;
            currentSpeed = fastTurnSpeed;
        }
        // Run
        else if(rightPower > upperTreshold && leftPower > upperTreshold)
        {
            velocity = Vector2.right;
            currentSpeed = runSpeed;
        }
    }

    void Move()
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
    void DayungPower(float right, float left)
    {
        rightPower = right;
        leftPower = left;

        rightPower = Mathf.Clamp(right, dayungPowerMinLimit, dayungPowerMaxLimit);
        leftPower = Mathf.Clamp(left, dayungPowerMinLimit, dayungPowerMaxLimit);
    }

    // Merubah kekuatan dayung power dan velocity menjadi 0 secara instan. Menghentikan perahu secara instan.
    void ZeroPower()
    {
        velocity = Vector2.zero;
        currentSpeed = 0f;
    }

    // Kekuatan dayung kanan. Aktif pada button untuk player input.
    public void OnDayungRight()
    {
        rightPower += dayungPower;

        StopCoroutine(HoldPowerRight());
        StartCoroutine(HoldPowerRight());
    }

    // Kekuatan dayung kiri. Aktif pada button untuk player input.
    public void OnDayungLeft()
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

    // Untuk mengaktifkan boost. Button input.
    public void TriggerBoost()
    {
        if (!isBoostAvaliable)
            return;


        isBoostAvaliable = false;
        boostPowerOn = true;
        currentBoostTime += boostTime;

        if (OnTriggerBoost != null)
            OnTriggerBoost();

        Debug.Log($"Boost On = {boostPowerOn}");
    }

    public void GetBoost()
    {
        isBoostAvaliable = true;

        if (OnTriggerBoost != null)
            OnTriggerBoost();
    }

    private void TimeCheck()
    {
        // Boost power time check
        if(currentBoostTime > 0f)
        {
            currentBoostTime -= Time.deltaTime;
        }
        else
        {
            if(boostPowerOn)
            {
                boostPowerOn = false;
            }

            currentBoostTime = 0f;
        }

        // Slow check
        if(!boostPowerOn)
        {
            if(currentSlowTime > 0f)
            {
                currentSlowTime -= Time.deltaTime;
            }
            else
            {
                debuffSpeed = 0f;
                currentSlowTime = 0f;
            }
        }
        else
        {
            debuffSpeed = 0f;
            currentSlowTime = 0f;
        }
    }

    public void GetStunned(float time)
    {
        if (isStunned || boostPowerOn)
            return;

        DayungPower(1f, 1f);
        ZeroPower();
        StartCoroutine(StunnedCount(time));
    }

    public void GetDebuff(float _time, float _debuffSpeed)
    {
        currentSlowTime = _time;
        debuffSpeed = _debuffSpeed;
    }

    IEnumerator StunnedCount(float time)
    {
        isStunned = true;

        yield return new WaitForSeconds(time);

        isStunned = false;
    }

    // Item IFinish
    public void GetEnd()
    {
        if (isEnd)
            return;

        isEnd = true;

        if (isPlayer)
        {
            GameManager.instance.PlayerFinish(ranking);
        }

        if (OnFinishLine != null)
            OnFinishLine();
    }

    private void AnimCheck()
    {
        if(velocity == Vector2.zero)
        {
            if(isStunned)
                anim.SetBool("isMove", true);
            else
                anim.SetBool("isMove", false);
        } else
        {
            anim.SetBool("isMove", true);
        }
    }

    void Ranking()
    {
        if (isEnd)
            return;

        int tmpRanking = 1;

        // Check ranking
        foreach (Movement boat in GameManager.instance.boats)
        {
            if (boat != this)
            {
                if (boat.transform.position.x >= this.transform.position.x)
                {
                    tmpRanking++;
                }
            }
        }

        // Event trigger
        if(tmpRanking != ranking)
        {
            ranking = tmpRanking;

            if (OnRankingChange != null)
                OnRankingChange();
        }
    }

    // It will be for item
    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Item"))
        {
            var itemScript = collision.GetComponent<Item>();
            itemScript.Acive(this);
        }
    }

    // It will be for player/other boat collision
    protected void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // What happen when hit another boat
            DayungPower(1f, 1f);
        }
    }
}
