using System.Collections;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private Rigidbody2D rb2d;

    private Animator anim;

    private Vector2 velocity;

    private float rightPower;
    private float leftPower;

    [Header("Boat Stat")]
    [SerializeField] private bool isPlayer = false;

    [SerializeField] private float movementSpeed = 2f;
    [SerializeField] private float runSpeed = 4f;
    [SerializeField] private float fastTurnSpeed = 2f;
    [SerializeField] private float slowTurnSpeed = 1f;
    private float currentSpeed = 0f;

    [SerializeField] private float dayungPower = 10f;
    [SerializeField] private float dayungPowerDegeneration = 1f;

    [SerializeField] private float dayungPowerMinLimit = 0f;
    [SerializeField] private float dayungPowerMaxLimit = 100f;

    private bool rightPowerOn = false;
    private bool leftPowerOn = false;
    private bool boostPowerOn = false;
    private bool isStunned = false;
    private bool isEnd = false;
    public bool isStart = false;
    [SerializeField] private float powerOn_Time = 0.1f;

    [SerializeField] private float midTreshold = 20f;
    [SerializeField] private float upperTreshold = 50f;

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        if (!isStart)
            return;

        if (isEnd)
            return;

        CheckInput();
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

        //InvokeRepeating("OnDayungRight", 1f, 0.1f);
        //InvokeRepeating("OnDayungLeft", 1f, 0.1f);
    }

    protected virtual void CheckInput()
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

    void Velocity()
    {
        // Not move
        if(rightPower <= midTreshold && leftPower <= midTreshold)
        {
            velocity = Vector2.zero;
            currentSpeed = 0f;
        }
        // Slightly turn left
        else if(rightPower <= midTreshold && leftPower > midTreshold)
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
        else if(rightPower > midTreshold && leftPower > midTreshold && (rightPower <= upperTreshold && leftPower <= upperTreshold))
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
        if (boostPowerOn)
            currentSpeed *= 2;

        rb2d.MovePosition(rb2d.position + (velocity * currentSpeed) * Time.fixedDeltaTime);

        var tmpRight = rightPowerOn ? rightPower : rightPower - dayungPowerDegeneration;
        var tmpLeft = leftPowerOn ? leftPower : leftPower - dayungPowerDegeneration;
        DayungPower(tmpRight, tmpLeft);
    }

    void DayungPower(float right, float left)
    {
        rightPower = right;
        leftPower = left;

        rightPower = Mathf.Clamp(right, dayungPowerMinLimit, dayungPowerMaxLimit);
        leftPower = Mathf.Clamp(left, dayungPowerMinLimit, dayungPowerMaxLimit);
    }

    void ZeroPower()
    {
        velocity = Vector2.zero;
        currentSpeed = 0f;
    }

    public void OnDayungRight()
    {
        rightPower += dayungPower;

        StopCoroutine(HoldPowerRight());
        StartCoroutine(HoldPowerRight());
    }

    public void OnDayungLeft()
    {
        leftPower += dayungPower;

        StopCoroutine(HoldPowerLeft());
        StartCoroutine(HoldPowerLeft());
    }

    IEnumerator HoldPowerRight()
    {
        rightPowerOn = true;

        yield return new WaitForSeconds(powerOn_Time);

        rightPowerOn = false;
    }

    IEnumerator HoldPowerLeft()
    {
        leftPowerOn = true;

        yield return new WaitForSeconds(powerOn_Time);

        leftPowerOn = false;
    }

    public void GetStunned(float time)
    {
        if (isStunned)
            return;

        DayungPower(1f, 1f);
        ZeroPower();
        StartCoroutine(StunnedCount(time));
    }

    IEnumerator StunnedCount(float time)
    {
        isStunned = true;

        yield return new WaitForSeconds(time);

        isStunned = false;
    }

    public void GetEnd()
    {
        if (isEnd)
            return;

        isEnd = true;
        
        if(isPlayer)
        {
            GameManager.instance.PlayerFinish();
        }
    }

    private void AnimCheck()
    {
        if(velocity == Vector2.zero)
        {
            anim.SetBool("isMove", false);
        } else
        {
            anim.SetBool("isMove", true);
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

    // It will be for olayer/other boat
    protected void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // What happen when hit another boat
            DayungPower(1f, 1f);
        }
    }
}
