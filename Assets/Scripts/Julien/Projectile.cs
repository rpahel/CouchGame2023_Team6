using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using static Unity.Mathematics.math;

public class Projectile : MonoBehaviour {
    
    
    private GameObject _playerGo;
    private float _shootImpactSatietyPercent;
    private float _shootBounce;
    private float _cubeBounce;
    private bool _alreadyHitPlayer;
    
    private void OnCollisionEnter2D(Collision2D col) {
        
        if (col.gameObject.transform.parent.TryGetComponent<Cube>(out Cube cube)) {
            
          /*  GameObject block = LevelGenerator.Instance.cubeEdible;
            Vector3 offset = (Vector2)sign(col.contacts[0].normal) * block.transform.localScale;
            
            Debug.Log("normal " + col.contacts[0].normal);
            Debug.Log("offset " + offset);
            
            Instantiate(block,col.gameObject.transform.parent.position + offset,Quaternion.identity);
            */
        }
        else if(col.gameObject.TryGetComponent<PlayerMovement>(out PlayerMovement mov)) {
            _alreadyHitPlayer = true;
            PlayerManager playerManager = mov.GetComponent<PlayerManager>();
            Rigidbody2D rb = mov.GetComponent<Rigidbody2D>();
            playerManager.eatAmount -= _shootImpactSatietyPercent / 100;
            rb.AddForce(-col.contacts[0].normal * _shootBounce,ForceMode2D.Impulse);
        }
        
        if (col.gameObject.transform.parent.gameObject.TryGetComponent<Cube_TNT>(out Cube_TNT tnt)) 
            tnt.Explode(col.gameObject.transform.parent);
        
        Destroy(transform.gameObject);
    }

    public void InitializeValue(float impactSatietyPercent, float force,float cubeBounce, GameObject player) {
        _shootImpactSatietyPercent = impactSatietyPercent;
        _shootBounce = force;
        _cubeBounce = cubeBounce;
        _playerGo = player;
    }
}
