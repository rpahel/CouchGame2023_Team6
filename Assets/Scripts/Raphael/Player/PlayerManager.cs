using Data;
using System;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    #region Autres Scripts
    //============================
    private Rigidbody2D rb2D;
    private Collider2D coll;
    private PlayerMovement pMovement;
    private PlayerEat pEat;
    private PlayerInputs pInputs;

    // Getter
    public Rigidbody2D Rb2D => rb2D;
    public Collider2D PCollider => coll;
    public PlayerMovement PMovement => pMovement;
    public PlayerEat PEat => pEat;
    public PlayerInputs PInputs => pInputs;
    #endregion

    #region Variables
    //============================
    public PLAYER_STATE PlayerState { get; set; }

    //============================
    public Vector2 AimDirection { get; set; }
    #endregion

    #region Unity_Functions
    private void Awake()
    {
        TryGetAllComponents();
        SetManagerInComponents();
    }

    private void Update()
    {
        #if UNITY_EDITOR
            Debug.DrawRay(transform.position - Vector3.forward, AimDirection * 5f, Color.cyan, Time.deltaTime);
        #endif
    }
    #endregion

    #region Custom_Functions
    /// <summary>
    /// Essaie de r�cup�rer, dans le Player gameObject, le Component donn�. Arr�te le mode Play et retourne une erreur lorsque le Component n'est pas trouv�.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="component">Le component � r�cup�rer.</param>
    /// <exception cref="Exception">Le component n'a pas �t� trouv�</exception>
    private void TryGetPlayerComponent<T>(out T component)
    {
        //Debug.Log($"Trying to get {typeof(T)} from the player game object...");

        if (!TryGetComponent<T>(out component))
        {
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #endif
            throw new Exception($"No {typeof(T)} component found in Player game object !");
        }
    }

    private void TryGetAllComponents()
    {
        TryGetPlayerComponent(out rb2D);
        TryGetPlayerComponent(out coll);
        TryGetPlayerComponent(out pMovement);
        TryGetPlayerComponent(out pEat);
        TryGetPlayerComponent(out pInputs);
    }

    private void SetManagerInComponents()
    {
        pMovement.PManager = this;
        pEat.PManager = this;
        pInputs.PManager = this;
    }
    #endregion
}
