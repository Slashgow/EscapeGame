using System.Collections.Generic;
using DG.Tweening;
using Newtonsoft.Json.Bson;
using PurrNet;
using UnityEngine;

public class FirstPersonController : NetworkBehaviour
{
    public static FirstPersonController localFirstPersonController;

    [Header("Movement Speeds")]
    [SerializeField] private float walkSpeed = 3.0f;
    [SerializeField] private float sprintMultiplier = 2.0f;

    [Header("Jump Parameters")]
    [SerializeField] private float jumpForce = 5.0f;
    [SerializeField] private float gravityMultiplier = 1.0f;

    [Header("Look Parameters")]
    [SerializeField] private float mouseSensitivity = 0.1f;
    [SerializeField] private float upDownLookRange = 80f;

    [Header("References")]
    [SerializeField] private Vector3 cameraOffset;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private PlayerInputHandler playerInputHandler;
    [SerializeField] private Animator animator;
    [SerializeField] private List<Renderer> playerRenderers;
    [SerializeField] private Transform itemAttachPoint;

    [SerializeField, Range(0f,0.02f)] private float movementDeltaForAnimation = 0.005f;

    private Camera playerCamera;

    private Vector3 currentMovement;
    private float verticalRotation;
    private float CurrentSpeed => walkSpeed * (playerInputHandler.SprintTriggered ? sprintMultiplier : 1);

    private readonly int IS_SPRINTING = Animator.StringToHash("Sprinting");
    private bool isSprinting;
    private readonly int IS_WALKING = Animator.StringToHash("Walking");
    private bool isWalking;
    private readonly int JUMP = Animator.StringToHash("Jump");
    private bool isJumping;

    protected override void OnSpawned()
    {
        base.OnSpawned();

        enabled = isOwner;

        if (!isOwner)
        {
            localFirstPersonController = null;
            return;
        }
        
        if (isOwner)
        {
            playerRenderers.ForEach(renderer => renderer.enabled = false);
            localFirstPersonController = this;
        }
         

        playerCamera = Camera.main;
        playerCamera.transform.SetParent(transform);
        playerCamera.transform.localPosition = cameraOffset;
        //itemAttachPoint.SetParent(playerCamera.transform);
        if(playerCamera == null)
        {
            enabled = false;
        }
        //if (!isOwner)
        //    Destroy(playerCamera.gameObject);
    }


    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.Instance.IsInMenu)
            return;

        HandleMovement();
        HandleRotation();
    }

    private Vector3 CalculateWorldDirection()
    {
        Vector3 inputDirection = new Vector3(playerInputHandler.MovementInput.x, 0f, playerInputHandler.MovementInput.y);
        Vector3 worldDirection = transform.TransformDirection(inputDirection);
        return worldDirection.normalized;
    }

    private void HandleJumping()
    {
        if (characterController.isGrounded)
        {
            if (isJumping)
            {
                animator.SetBool(JUMP, false);
                isJumping = false;
                Debug.Log("stop jumping");
            }
            currentMovement.y = -0.5f;

            if (playerInputHandler.JumpTriggered)
            {
                if (!isJumping)
                {
                    Debug.Log("start jumping");
                    animator.SetBool(JUMP, true);
                    isJumping = true;
                }
             
                currentMovement.y = jumpForce;
            }
        }
        else
        {
            currentMovement.y += Physics.gravity.y * gravityMultiplier * Time.deltaTime;
        }
    }

    private void HandleMovement()
    {
        Vector3 worldDirection = CalculateWorldDirection();
        currentMovement.x = worldDirection.x * CurrentSpeed;
        currentMovement.z = worldDirection.z * CurrentSpeed;

        HandleAnimationState();

        HandleJumping();
        characterController.Move(currentMovement * Time.deltaTime);
    }

    private void HandleAnimationState()
    {
        if (isWalking && playerInputHandler.MovementInput.x == 0 && playerInputHandler.MovementInput.y == 0)
        {
            Debug.Log("stop walking");
            animator.SetBool(IS_WALKING, false);
            isWalking = false;
        }
        else if(!isWalking && (playerInputHandler.MovementInput.x > 0f || playerInputHandler.MovementInput.y > 0f))
        {
            Debug.Log("start walking");
            animator.SetBool(IS_WALKING, true);
            isWalking = true;
        }
        
        if(isWalking && !isSprinting && CurrentSpeed > walkSpeed)
        {
            animator.SetBool(IS_SPRINTING, true);
            isSprinting = true;
        }
        else if(isSprinting && CurrentSpeed <= walkSpeed)
        {
            animator.SetBool(IS_SPRINTING, false);
            isSprinting = false;
        }
       
    }

    public void MovePlayerTo(Transform targetTransform)
    {
        characterController.enabled = false;
        this.transform.position = targetTransform.position;
        this.transform.rotation = targetTransform.rotation;
        characterController.enabled = true;
    }

    public void MovePlayerTo(Transform targetTransform, float timeToMove, Ease easing)
    {
        characterController.enabled = false;
        this.transform.DOMove(targetTransform.position, timeToMove).SetEase(easing).OnComplete(() => characterController.enabled = true);
        this.transform.DORotateQuaternion(targetTransform.rotation, timeToMove).SetEase(easing);
    }
    public void MoveVerticalRotationTo(Quaternion endRotation, float timeToMove, Ease easing)
    {
        playerCamera.transform.DORotateQuaternion(endRotation, timeToMove).SetEase(easing);
    }
    private void ApplyHorizontalRotation(float rotationAmount)
    {
        transform.Rotate(0, rotationAmount, 0);
    }

    private void ApplyVerticalRotation(float rotationAmount)
    {
        verticalRotation = Mathf.Clamp(verticalRotation - rotationAmount, -upDownLookRange, upDownLookRange);
        playerCamera.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
    }

    private void HandleRotation()
    {
        float mouseXRotation = playerInputHandler.RotationInput.x * mouseSensitivity;
        float mouseYRotation = playerInputHandler.RotationInput.y * mouseSensitivity;

        ApplyHorizontalRotation(mouseXRotation);
        ApplyVerticalRotation(mouseYRotation);
    }
}
