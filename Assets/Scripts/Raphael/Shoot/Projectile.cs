using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO : Empecher les particules de disparaitre
public class Projectile : MonoBehaviour
{
    #region Variables
    //=============================================
    [HideInInspector] public PlayerManager owner;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private Collider2D col;
    private Vector2 currentVelocity;

    //=============================================
    [HideInInspector] public Color color;
    [HideInInspector] public float gravity;
    [HideInInspector] public float forceDuRebond;
    [HideInInspector] public int pourcentageInflige;
    [HideInInspector] public float knockBackForce;
    #endregion

    #region Unity_Functions
    //=============================================
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }

    private void OnEnable()
    {
        spriteRenderer.color = color;
        rb.gravityScale = gravity;
    }

    private void OnDisable()
    {
        spriteRenderer.color = Color.white;
        rb.gravityScale = 0;
        rb.velocity = Vector2.zero;
        col.isTrigger = true;
        transform.position = Vector2.zero;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject != owner && collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Vector2 sensDuKnockBack = (collision.transform.position - owner.transform.position).x > 0 ? new Vector2(1, 1) : new Vector2(-1, 1);
            collision.GetComponent<PlayerManager>().OnDamage(owner, pourcentageInflige, sensDuKnockBack * knockBackForce);
            rb.velocity = new Vector2(-sensDuKnockBack.x, sensDuKnockBack.y) * forceDuRebond;
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Trap"))
        {
            gameObject.SetActive(false);
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Destructible") || collision.gameObject.layer == LayerMask.NameToLayer("Indestructible"))
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == owner.gameObject)
        {
            col.isTrigger = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject != owner)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                Vector2 sensDuKnockBack = collision.GetContact(0).normal.x > 0 ? new Vector2(1, 1) : new Vector2(-1, 1);
                collision.gameObject.GetComponent<PlayerManager>().OnDamage(owner, pourcentageInflige, sensDuKnockBack * knockBackForce);
                rb.velocity = new Vector2(-sensDuKnockBack.x, sensDuKnockBack.y) * currentVelocity.magnitude * forceDuRebond;
            }
            else if (collision.gameObject.layer == LayerMask.NameToLayer("Trap"))
            {
                Vector2 sensDuRebond = Vector2.Reflect(currentVelocity, collision.GetContact(0).normal).normalized;
                rb.velocity = sensDuRebond * currentVelocity.magnitude * forceDuRebond;
            }
            else if (collision.gameObject.layer == LayerMask.NameToLayer("Destructible") || collision.gameObject.layer == LayerMask.NameToLayer("Indestructible"))
            {
                //TODO: Faire apparaitre cube
                gameObject.SetActive(false);
            }
        }
    }

    private void LateUpdate()
    {
        currentVelocity = rb.velocity;
    }
    #endregion

    #region Custom_Functions
    //=============================================
    public void Shoot(Vector2 dir, float speed)
    {
        rb.velocity = dir * speed;
    }
    #endregion
}
