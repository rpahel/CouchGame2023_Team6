using UnityEngine;

public class PlayerStateSystem : StateMachine
{
    public PlayerManager PlayerManager { get; private set; }
    public CooldownManager CooldownManager { get; private set; }
    public PlayerInputs PlayerInputs { get; private set; }
    public State PlayerState => State;

    private void Awake()
    {
        PlayerManager = GetComponent<PlayerManager>();
        CooldownManager = GetComponent<CooldownManager>();
        PlayerInputs = GetComponent<PlayerInputs>();
    }

    public void Start()
    {
        SetState((new Moving(this)));
    }

    public void SetKnockback(Vector2 knockBackForce)
    {
        SetState((new Knockback(this)));
        State?.OnKnockback(knockBackForce);
    }

    public void SetStun<T>(T damageDealer, int damage, Vector2 knockBackForce)
    {
        if (State is Special) return;

        SetState((new Stun(this)));
        State?.OnStun<T>(damageDealer, damage, knockBackForce);
    }

    public void Update()
    {
        State?.Update();
    }

    private void FixedUpdate()
    {
        State?.FixedUpdate();
    }

    public void OnMove()
    {
        State?.OnMove();
    }

    public void OnJump()
    {
        State?.OnJump();
    }

    public void OnEat()
    {
        State?.OnEat();
    }

    public void OnHoldShoot()
    {
        State?.OnHoldShoot();
    }

    public void OnShoot()
    {
        State?.OnShoot();
    }
    
    public void OnHoldSpecial()
    {
        State?.OnHoldSpecial();
    }

    public void OnSpecial()
    {
        State?.OnSpecial();
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        State?.OnCollisionEnter(collision);
    }
    
    private void OnCollisionStay2D(Collision2D collision)
    {
        State?.OnCollisionStay(collision);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        State?.OnTriggerEnter(col);
    }
}