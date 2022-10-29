using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Data;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;
using static UnityEngine.InputSystem.InputAction;

public class PlayerInputHandler : MonoBehaviour
{
    private PlayerConfiguration _playerConfig;
    [SerializeField] private MeshRenderer playerMesh;

    public float _holdCooldown;
    public bool _canHoldCooldown;

    

    [Header("Scripts")]
    private PlayerMovement _movement;
    private PlayerManager _playerManager;
    private ShootProjectile _shootProjectile;
    //private Eat _eat;
    [SerializeField] private EatScript _eat;
    private PlayerControls _controls;
    private ToolsManager _toolsManager;

    [Header("VariablesEat")]
    [SerializeField] private float MaxHoldLooseEat = 0.7f;
    [SerializeField] private float MediumHoldLooseEat = 0.7f;
    [SerializeField] private float MinHoldLooseEat = 0.7f;
    private void Awake()
    {
        _playerManager = gameObject.GetComponent<PlayerManager>();
        _movement = GetComponent<PlayerMovement>();
        _shootProjectile = gameObject.GetComponent<ShootProjectile>();
        //_eat = GetComponent<eatTest>();
        _controls = new PlayerControls();
        _toolsManager = gameObject.GetComponent<ToolsManager>();

    }

    public void InitializePlayer(PlayerConfiguration pc)
    {
        _playerConfig = pc;
        playerMesh.material = pc.PlayerMaterial;
        _playerManager.imageUI.color = pc.PlayerMaterial.color;
        _playerConfig.Input.onActionTriggered += Input_OnActionTriggered;
    }

    private void Input_OnActionTriggered(CallbackContext obj)
    {
        if (obj.action.name == _controls.Gameplay.Move.name)
        {
            OnMove(obj);
        }
        else if (obj.action.name == _controls.Gameplay.Jump.name)
        {
            OnJump(obj);
        }
        else if (obj.action.name == _controls.Gameplay.Shoot.name)
        {
            OnShoot(obj);
        }
        else if (obj.action.name == _controls.Gameplay.Aim.name)
        {
            OnAim(obj);
        }
        else if (obj.action.name == _controls.Gameplay.Eat.name)
        {
            OnEat(obj);
        }
        else if (obj.action.name == _controls.Gameplay.Dash.name)
        {
            OnDash(obj);
        }
        else if (obj.action.name == _controls.Gameplay.CheatMenu.name)
        {
            
            OnCheatMenu(obj);
        }


    }
    
    private void OnCheatMenu(CallbackContext context)
    {
        Debug.Log("coucou");
     
        _toolsManager.activeMenuCheat();
   
    }
    private void OnMove(CallbackContext context)
    {
        if(_playerManager != null)
            _playerManager.SetInputVector(context.ReadValue<Vector2>());
    }

    private void OnJump(CallbackContext context)
    {
        /*if (_movement != null && context.performed)
            _movement.Jump();
        else if (_movement != null && context.canceled)
            _movement.StopJump();*/
        
        if (_movement != null && context.started)
            _movement.OnJump();
 
        else if (_movement != null && context.performed)
            _movement.HoldJump = true;
 
        else if (_movement != null && context.canceled)
            _movement.HoldJump = false;
}

    private void OnShoot(CallbackContext context)
    {
        if (_shootProjectile != null && context.canceled) 
            _shootProjectile.Shoot();
    }

    private void OnAim(CallbackContext context)
    {
        if (_shootProjectile != null && context.performed) 
            _shootProjectile.Aim();
    }

    private void OnEat(InputAction.CallbackContext context)
    {
        /*if (_eat != null && context.performed)
            _eat.TryEat();*/
        if (_eat != null && context.started)
            _playerManager.SetCanEat(true);
        if (_eat != null && context.canceled)
            _playerManager.SetCanEat(false);
    }

    private void OnDash(CallbackContext context)
    {
        if (_movement != null && context.started && _movement._canDash == true)
        {
            _canHoldCooldown = true;
            _shootProjectile.Aim();
        }
         
        else if (_movement != null && context.canceled)
        {
            if (_movement._canDash == true)
            {
                if (_canHoldCooldown && _holdCooldown <= 0.99f)
                {
                    _movement.Dash();
                    _playerManager.eatAmount -= MinHoldLooseEat;
                    ResetCooldown();
                }
                else if (_holdCooldown >= 1f && _canHoldCooldown)
                {
                    _movement.Dash();
                    _playerManager.eatAmount -= MediumHoldLooseEat;
                    ResetCooldown();
                } 
            }
            else
            {
                _playerManager.SetPlayerState(PlayerState.Moving);
            }      
        }
        
    }

    private void Update()
    {
        if (_canHoldCooldown)
            _holdCooldown += Time.deltaTime;

        if (_holdCooldown >= 2f)
        {
            _movement.Dash();
            ResetCooldown();
        }

    }

    private void ResetCooldown()
    {
        _canHoldCooldown = false;
        _holdCooldown = 0f;
    }

}