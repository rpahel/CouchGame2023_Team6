using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube_Trap : Cube
{

    [SerializeField] private float _sideKnockForce;
    [SerializeField] private int _damageAmount;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Normal of the first point: " + collision.contacts[0].normal);
        if (!collision.gameObject.CompareTag("Player")) return;

        var playerSystemManager = collision.gameObject.GetComponent<PlayerSystemManager>();
        var vec = collision.contacts[0].normal;

        if (vec == new Vector2(0, -1))
        {
            Debug.Log("up");
            KnockbackRandom(playerSystemManager, Vector3.up);
        }
            
        else if (vec == new Vector2(0, 1))
        {
            Debug.Log("down");
            KnockbackRandom(playerSystemManager, Vector3.down);
        }
            
        else if (vec == new Vector2(1, 0) || (vec == new Vector2(-1, 0)))
        {
            Debug.Log("Side");
            KnockBack(new Vector2(-vec.x, -vec.y), playerSystemManager);
        }
            
    }

    void KnockbackRandom(PlayerSystemManager playerSystem, Vector3 sideDirection)
    {
        var rand = UnityEngine.Random.Range(0, 2);

        if (rand == 0)
        {
            KnockBack((Vector3.left / _sideKnockForce + sideDirection).normalized, playerSystem);
            Debug.DrawLine(transform.position, transform.position + (Vector3.left / _sideKnockForce + sideDirection).normalized);
        }
            
        else
        {
            KnockBack((Vector3.right / _sideKnockForce + sideDirection).normalized, playerSystem);
            Debug.DrawLine(transform.position, transform.position + (Vector3.right / _sideKnockForce + sideDirection).normalized);
        }

    }

    private void KnockBack(Vector3 vec, PlayerSystemManager playerSystem)
    {
        playerSystem.OnDamage(this, _damageAmount, vec * 100);
    }
}