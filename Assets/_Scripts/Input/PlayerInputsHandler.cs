using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;
public class PlayerInputsHandler : MonoBehaviour
{
    #region Autres Scripts
    //==========================================================================

    private PlayerManager _playerManager;
    private PlayerStateSystem _playerSystem;
    private PlayerConfiguration _playerConfig;
    private PlayerInputs controls;
    private Transform gameObjectTransform;
    public bool InputIsEnabled { get; private set; }
    #endregion

    //==========================================================================
    private void Awake()
    {
        _playerSystem = GetComponent<PlayerStateSystem>();
        _playerManager = GetComponent<PlayerManager>();
        controls = new PlayerInputs();
        InputIsEnabled = true;
        gameObjectTransform = gameObject.GetComponent<Transform>();
    }
    
    public void InitializePlayer(PlayerConfiguration pc)
    {
        _playerConfig = pc;
        _playerManager.sprite.sprite = pc.PlayerSprite;
        _playerManager.insideSprite.sprite = pc.PlayerSprite;
        _playerManager.GetComponent<FaceManager>().SetFaceNormal(pc.PlayerFaceSprite);
        _playerManager.imageUI.sprite = pc.PlayerIcon;
        _playerManager.color = pc.PlayerColor;
        _playerConfig.Input.onActionTriggered += Input_OnActionTriggered;
    }

    private void Input_OnActionTriggered(CallbackContext obj)
    {

        if (obj.action.name == controls.Game.Move.name)
        {
            OnMove(obj);
        }
        
        if (!InputIsEnabled) return;

        else if (obj.action.name == controls.Game.Jump.name)
        {
            OnJump(obj);
        }
        else if (obj.action.name == controls.Game.Shoot.name)
        {
            OnShoot(obj);
        }
        else if (obj.action.name == controls.Game.Eat.name)
        {
            OnEat(obj);
        }
        else if (obj.action.name == controls.Game.Special.name)
        {
            OnSpecial(obj);
        }
    }

    private void OnMove(CallbackContext input)
    {
        _playerSystem.PlayerManager.inputVectorDirection = input.ReadValue<Vector2>().normalized;
        
        _playerSystem.PlayerManager.LookDirection = _playerSystem.PlayerManager.inputVectorDirection.x switch
        {
            // Redondance (x est pas cense valoir 0 mais on sait jamais)
            > 0 => Vector2.right,
            < 0 => Vector2.left,
            _ => _playerSystem.PlayerManager.LookDirection
        };

        gameObjectTransform.rotation = _playerSystem.PlayerManager.LookDirection == Vector2.left ? Quaternion.Euler(0, 180, 0) : Quaternion.Euler(0, 0, 0);
        
        if (!InputIsEnabled) return;
        _playerSystem.OnMove();
    }

    private void OnJump(CallbackContext input)
    {
        if (input.started)
            _playerSystem.OnJump();

        else if (input.performed)
            _playerManager.holdJump = true;

        else if (input.canceled)
            _playerManager.holdJump = false;
    }

    private void OnEat(CallbackContext input)
    {
        if (input.started)
            _playerManager.OnEat();

        else if (input.performed)
            _playerManager.holdEat = true;

        else if (input.canceled)
            _playerManager.holdEat = false;
    }

    private void OnShoot(CallbackContext input)
    {
        if (_playerManager.fullness < _playerManager.NecessaryFood) return;

        if (input.performed)
        {
            _playerSystem.OnHoldShoot();
        }

        if (!input.canceled) return;
        
        switch (_playerSystem.PlayerState)
        {
            case Moving:
                _playerSystem.PlayerManager.Shoot();
                break;
            case AimShoot:
                _playerSystem.OnShoot();
                break;
        }
    }

    private void OnSpecial(CallbackContext input)
    {
        if (_playerManager.fullness < _playerManager.NecessaryFoodSpecial) return;

        if (input.performed)
        {
            _playerSystem.OnHoldSpecial();
        }

        if (input.canceled)
        {
            _playerSystem.OnSpecial();
        }
    }
    
    public void SetEnableInput(bool result)
    {
        InputIsEnabled = result;
    }
}