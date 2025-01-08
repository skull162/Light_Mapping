using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using TMPro;

public class MovementController : MonoBehaviour
{
    // SERIALIZED MEMBERS
    [Header("Settings")]
    [SerializeField] private float _movementVelocity;
    [SerializeField] private float _jumpForce;

    [Header("References")]
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private NetworkInput _networkInput;

    // PRIVATE MEMBERS
    private MovementInput _input = MovementInput.Create();
    private bool _isJumping = false;

    private bool isFacingRight = true;
    private Animator animator;
    public bool insideLamp;
    [SerializeField]private ObjectInteraction objectInteract;

    public TMP_Text textNama;

    // Mengecek apakah karakter berada di tanah
    private bool isGrounded;
    // Layer untuk tanah
    public LayerMask groundLayer;

    // MonoBehaviour INTERFACE
    private void OnValidate()
    {
        Assert.IsNotNull(_rigidbody);
        Assert.IsNotNull(_networkInput);
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        PollInput();
        ApplyForces();
        UpdateState();

        // Update Animator parameters
        animator.SetBool("isJump", !isGrounded);

        if (_rigidbody.velocity.y < 0) // Karakter bergerak ke bawah
        {
            if (Physics2D.Raycast(transform.position, Vector2.down, 0.1f, groundLayer))
            {
                isGrounded = true;
            }
        }

        // Tetapkan parameter "isWalking" di Animator hanya jika input signifikan
        animator.SetBool("isWalking", Mathf.Abs(_rigidbody.velocity.x) > 0.1f);

        if (_input.IsPick && insideLamp)
        {
            if(objectInteract != null)
            {
                objectInteract.PickedItem();
            }
        }
    }

    // PRIVATE METHODS
    private void PollInput()
    {
        // Create empty input struct
        var input = MovementInput.Create();

        // Populate input struct
        input.IsLeftPressed = Input.GetKey(KeyCode.A) || _networkInput.IsLeftPressed;
        input.IsRightPressed = Input.GetKey(KeyCode.D) || _networkInput.IsRightPressed;
        input.IsJumpPressed = Input.GetKey(KeyCode.Space) || _networkInput.IsJumpPressed;
        input.IsPick = Input.GetKey(KeyCode.F) || _networkInput.IsPick;

        // Save input struct
        _input = input;
    }

    private void ApplyForces()
    {
        // Jump
        if (_input.IsJumpPressed && !_isJumping)
        {
            _rigidbody.AddForce(_jumpForce * Vector2.up, ForceMode2D.Impulse);
            _isJumping = true;
        }

        // Move Right
        if (!_input.IsLeftPressed && _input.IsRightPressed)
        {
            var velocity = _rigidbody.velocity;
            velocity.x = _movementVelocity;
            _rigidbody.velocity = velocity;

            // Flip to the right only if necessary
            if (!isFacingRight)
            {
                Flip();
            }
        }

        // Move Left
        if (_input.IsLeftPressed && !_input.IsRightPressed)
        {
            var velocity = _rigidbody.velocity;
            velocity.x = -1f * _movementVelocity;
            _rigidbody.velocity = velocity;

            // Flip to the left only if necessary
            if (isFacingRight)
            {
                Flip();
            }
        }
    }

    private void UpdateState()
    {
        // if not moving vertically (within a 0.01f threshold), then allow jumping
        if (Mathf.Abs(_rigidbody.velocity.y) <= 0.01f)
        {
            _isJumping = false;
        }
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
        else if (collision.gameObject.CompareTag("Lamp"))
        {
            objectInteract = null;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Lamp"))
        {
            objectInteract = other.gameObject.GetComponent<ObjectInteraction>();
            Debug.Log("Masuk");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Lamp"))
        {
            objectInteract = null;
        }
    }
}

public struct MovementInput
{
    public bool IsLeftPressed;
    public bool IsRightPressed;
    public bool IsJumpPressed;
    public bool IsPick;

    public static MovementInput Create()
    {
        return new()
        {
            IsLeftPressed = false,
            IsRightPressed = false,
            IsJumpPressed = false,
        };
    }
}
