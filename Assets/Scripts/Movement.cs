using System.Collections;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private Rigidbody2D rb2d;

    private Vector2 velocity;

    private float rightPower;
    private float leftPower;

    [Header("Boat Stat")]
    [SerializeField] private float movementSpeed = 1.75f;
    [SerializeField] private float runSpeed = 1f;
    [SerializeField] private float belokanSpeed = 1.5f;

    [SerializeField] private float dayungPower = 10f;
    [SerializeField] private float dayungPowerDegeneration = 1f;

    [SerializeField] private float dayungPowerMinLimit = 0f;
    [SerializeField] private float dayungPowerMaxLimit = 100f;

    private bool rightPowerOn = false;
    private bool leftPowerOn = false;
    [SerializeField] private float powerOn_Time = 0.5f;

    [SerializeField] private float midTreshold = 20f;
    [SerializeField] private float upperTreshold = 50f;

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        CheckInput();
    }

    private void FixedUpdate()
    {
        Velocity();
        Move();
    }

    protected virtual void Init()
    {
        rb2d = gameObject.GetComponent<Rigidbody2D>();

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
        }
        // Slightly turn left
        else if(rightPower <= midTreshold && leftPower > midTreshold)
        {
            velocity = new Vector2(movementSpeed / 2, belokanSpeed / 2);
        }
        // Slightly turn right
        else if (rightPower > midTreshold && leftPower <= midTreshold)
        {
            velocity = new Vector2(movementSpeed / 2, -belokanSpeed / 2);
        }
        // Move
        else if(rightPower > midTreshold && leftPower > midTreshold && (rightPower <= upperTreshold && leftPower <= upperTreshold))
        {
            velocity = new Vector2(movementSpeed, 0f);
        }
        // Turn Right
        else if (rightPower > upperTreshold && leftPower <= upperTreshold && leftPower > midTreshold)
        {
            velocity = new Vector2(movementSpeed, belokanSpeed);
        }
        // Turn Left
        else if (rightPower <= upperTreshold && leftPower > upperTreshold && rightPower > midTreshold)
        {
            velocity = new Vector2(movementSpeed, -belokanSpeed);
        }
        // Run
        else if(rightPower > upperTreshold && leftPower > upperTreshold)
        {
            velocity = new Vector2(movementSpeed + runSpeed, 0f);
        }
    }

    void Move()
    {
        rb2d.MovePosition(rb2d.position + velocity * Time.fixedDeltaTime);

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
}
