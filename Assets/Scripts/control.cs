using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class control : MonoBehaviour
{
    //ТИП ИМЯ(ТИП ИМЯ, ТИП ИМЯ) { ТЕЛО_ФУНКЦИИ }
    //ТИП ИМЯ = ЗНАЧЕНИЕ
    //input = GetComponent<PlayerInput>();


    
    PlayerInput input = null;
    Rigidbody2D rigidbody2D = null;
    Collider2D collider2D = null;
    Vector2 sideBounds = new Vector2(-10, 10);
    Vector2 botTopBounds = new Vector2(0, 20);
    [SerializeField] float speed = 400;
    [SerializeField] float jumpSpeed = 3;
    [SerializeField] float jumpTime = 0.3f;
    bool isOnGround = false;
    Vector2 velocity = new Vector2();
    float jumpCurrentTime = 10;
    bool isSprint = false;
    bool isCanRun = true;

    public float health;
    public float stamina;
    [SerializeField] public float bounceCoeff = 0.3f;
    [SerializeField] public float runCoeff = 2f;
    [SerializeField] public float staminaRun = 10f;
    [SerializeField] private float staminaMax = 40f;
    [SerializeField] private float healthMax = 40f;

    public static control Instance { get; private set; }
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    bool isCompare(double smth, double etalon)
    {
        double mistake = 0.000009;
        if (smth >= etalon - mistake && smth <= etalon + mistake)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    bool isCompareInt(double smth, double etalon)
    {
        long multiple = 10000;
        long first = (int)(smth * multiple);
        long sec = (int)(etalon * multiple);
        return first == sec;
    }

    void Start()
    {
        input = GetComponent<PlayerInput>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        collider2D = GetComponent<Collider2D>();
        //RGBA = int;   
        //00FF00;
        //Первый байт - красный цвет
        //Второй байт - зелёный.
        uint color = 0xFF77FF;
        uint color2 = 0b11111111000000001111111111111111;
        uint color3 = 4294967295;

        /*
            1 - 1;
            2- 2;
            9 - 9;
            10 - A;
            11 - B;
            12 - c;
            13 - d;
            14 - e;
            15 - f;
        */

        //1111 - F, 1111 - F, 0000 - 0, 0000 - 0, 

    }

    void OnJump()
    {
        if (isOnGround && jumpCurrentTime >= jumpTime)
        {
            rigidbody2D.AddForceY(jumpSpeed * rigidbody2D.mass);
            isOnGround = false;
            jumpCurrentTime = 0;
        }
    }
    void OnMove()
    {
        velocity.x = input.actions.FindAction("Move").ReadValue<Vector2>().x * speed;        
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("EnemyWeapon"))
        {
            print("Игрок вошёл в зону врага!");
    }
}

    void Update()
    {
        isSprint = input.actions.FindAction("Sprint").IsPressed() && stamina > 0 && isCanRun;
        if (isSprint)
        {
            stamina -= staminaRun * Time.deltaTime;
            if (stamina <= 0)
            {
                isCanRun = false;
            }
        }
        else
        {
            stamina += staminaRun * Time.deltaTime;
        }
        stamina = Math.Clamp(stamina, 0, staminaMax);
        if (stamina > staminaMax / 4)
        {
            isCanRun = true;
        }
        jumpCurrentTime += Time.deltaTime;
        bool onGround = transform.position.y <= 0;
        if (onGround && !isOnGround)
        {
            if (velocity.y < 0)
            {
                velocity.y *= -1;
                velocity.y *= bounceCoeff;
            }
        }
        isOnGround = onGround;
        float coeff = 1;
        if (isSprint)
        {
            coeff = runCoeff; //Если бежим - то коэфф равняется 2
        }
        rigidbody2D.AddForceX(velocity.x * Time.deltaTime * coeff * rigidbody2D.mass);
    }
}
