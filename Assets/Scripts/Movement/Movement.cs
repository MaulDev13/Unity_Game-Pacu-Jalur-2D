using System.Collections;
using UnityEngine;

/// <summary>
/// Movement system yang digunakan. Ini dibuat dengan User Input sebagai acuan
/// </summary>
public class Movement : MonoBehaviour
{
    protected Rigidbody2D rb2d;

    protected Animator anim;

    protected Vector2 velocity;

    public delegate void OnEvent();
    public OnEvent OnTriggerBoost;
    public OnEvent OnRankingChange;
    public OnEvent OnFinishLine;

    [Header("Boat Stat")]
    [Tooltip("Apakah boat ini yang digunakan oleh player?")]
    [SerializeField] protected bool isPlayer = false;

    [Tooltip("Speed yang digunakan ketika idle/tidak mendayung atau dayungPower (kekuatan mendayung) kurang dari mid treshold.")]
    [SerializeField] protected float idleSpeed = 0.2f;
    [Tooltip("Speed yang digunakan ketika dayungPower (kekuatan mendayung) lebih besar dari mid treshold tetapi kurang dari upper treshold.")]
    [SerializeField] protected float movementSpeed = 2f;
    [Tooltip("Speed yang digunakan ketika dayungPower (kekuatan mendayung) lebih besar dari upper treshold.")]
    [SerializeField] protected float runSpeed = 4f;
    [Tooltip("Speed yang digunakan ketika akan berbelok dan dayungPower (kekuatan mendayung) lebih besar dari mid treshold tetapi kurang dari upper treshold.")]
    [SerializeField] protected float fastTurnSpeed = 2f;
    [Tooltip("Speed yang digunakan ketika akan berbelok dan dayungPower (kekuatan mendayung) lebih kecil dari mid treshold.")]
    [SerializeField] protected float slowTurnSpeed = 1f;
    [Tooltip("Speed boost multiplier. speed * boostSpeedMultiplier.")]
    [SerializeField] protected float boostSpeedMultiplier = 4f;
    protected float debuffSpeed = 0f;
    protected float currentSpeed = 0f;

    protected bool boostPowerOn = false;
    [Tooltip("Dapat menggunakan boost ketika avaliable = true.")]
    [SerializeField] protected bool isBoostAvaliable = false;
    public bool IsBoostAvaliable => isBoostAvaliable;

    [Tooltip("Lama boost akan aktif.")]
    [SerializeField] protected float boostTime = 3f;
    protected float currentBoostTime = 0f;

    protected bool isStunned = false;

    protected float currentSlowTime = 0f;

    [HideInInspector] public int ranking = 0;

    [SerializeField] protected bool isMoveAfterEnd = true;
    [HideInInspector] protected bool isEnd = false;
    public bool isStart = false;

    protected virtual void Start()
    {
        Init();
    }

    protected virtual void Update()
    {
        Ranking();

        if (!isStart || isEnd)
            return;

        InputCheck();
        TimeCheck();
    }

    protected virtual void FixedUpdate()
    {
        if (!isStart)
            return;

        if (isEnd)
        {
            if(isMoveAfterEnd)
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
    }

    protected virtual void InputCheck()
    {
        
    }

    // Get velocity of the boat depending on rightPower and leftPower
    protected virtual void Velocity()
    {
    }

    protected virtual void Move()
    {

    }

    // Button Dayung Right
    public virtual void OnDayungRight() { }

    public virtual void OnDayungLeft() { }

    // Untuk mengaktifkan boost. Button input.
    public virtual void TriggerBoost()
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

    public virtual void GetBoost()
    {
        isBoostAvaliable = true;

        if (OnTriggerBoost != null)
            OnTriggerBoost();
    }

    protected virtual void TimeCheck()
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

    public virtual void GetStunned(float time)
    {
        if (isStunned || boostPowerOn)
            return;

        StartCoroutine(StunnedCount(time));
    }

    public virtual void GetDebuff(float _time, float _debuffSpeed)
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

    protected void AnimCheck()
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

    protected void Ranking()
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
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Item"))
        {
            var itemScript = collision.GetComponent<Item>();
            itemScript.Acive(this);
        }
    }

    // It will be for player/other boat collision
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // What happen when hit another boat
            
        }
    }
}
