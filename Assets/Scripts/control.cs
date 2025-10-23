using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

enum MagicEffects
{
    Poison = 1,
    Ice = 2,
    Lightning = 4,
    Fire = 8,
    Lava = 16,
}

enum WindowHint
{
    Frameless = 1,
    FullScreen = 2,
    WithoutBar = 4,
    HalfOpacity = 8,
    ClickHandler = 16,
    DragHandler = 32,
    VerticalScrollBar = 64,
    Minimize = 128,
}

enum PlayerState
{
    Run,
    Idle,
    StaminaOut,
}

class Example
{
    void smth()
    {
        bool isPoison;
        bool isLightning;
        bool isFire;
        bool isLAva;
        bool isIce;
    }
}

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

    int magicEffects = 0;
    void refreshEffects()
    {
        magicEffects = 0; //ЭТО ИМБА
    }

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

    void InitWindow(int flags)
    {
        List<string> output = new(); //TODO заменить на List<string>
        List<int> numbers = new();

        if((flags & (int)WindowHint.Frameless) != 0)
        {
            output.Add("безрамочное");
        }
        if ((flags & (int)WindowHint.FullScreen) != 0)
        {
            output.Add("полноэкранное");
        }
        if ((flags & (int)WindowHint.WithoutBar) != 0)
        {
            output.Add("без меню");
        }
        if ((flags & (int)WindowHint.HalfOpacity) != 0)
        {
            output.Add("полупрозрачное");
        }
        if ((flags & (int)WindowHint.ClickHandler) != 0)
        {
            output.Add("поддерживает нажатие");
        }
        if ((flags & (int)WindowHint.DragHandler) != 0)
        {
            output.Add("поддерживает Drag'n'Drop");
        }
        if ((flags & (int)WindowHint.VerticalScrollBar) != 0)
        {
            output.Add("поддерживает вертикальный скролл бар");
        }
        if ((flags & (int)WindowHint.Minimize) != 0)
        {
            output.Add("сворачивается");
        }
        string result = string.Join(",", output);
        print(result);
    }
    void Start()
    {
        input = GetComponent<PlayerInput>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        collider2D = GetComponent<Collider2D>();
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

    void OnTriggerEnter2D(Collider2D other)
    {
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
