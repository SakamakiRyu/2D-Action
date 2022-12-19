using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMover : MonoBehaviour
{
    [SerializeField]
    private PlayerInput _input;

    [SerializeField]
    private float _walkPower;

    [SerializeField]
    private float _maxSpeed;

    [SerializeField]
    private float _jumpPower;

    private Rigidbody2D _rb2d;

    private Vector2 _inputAxis;

    #region Unity Function
    private void Start()
    {
        _rb2d = GetComponent<Rigidbody2D>();
        RegistToInputActions();
    }

    private void FixedUpdate()
    {
        Walk();
    }
    #endregion

    #region InputSystem Callbacks
    private void OnWalk(InputAction.CallbackContext context)
    {
        _inputAxis = context.ReadValue<Vector2>();
    }

    private void OnWalkZero(InputAction.CallbackContext context)
    {
        _inputAxis = Vector2.zero;
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        Jump();
    }
    #endregion

    #region Private Function
    /// <summary>
    /// InputSystemÇ…ÉÅÉ\ÉbÉhÇìoò^Ç∑ÇÈ
    /// </summary>
    private void RegistToInputActions()
    {
        _input.actions["Walk"].performed += OnWalk;
        _input.actions["Walk"].canceled += OnWalkZero;

        _input.actions["Jump"].started += OnJump;
    }

    /// <summary>
    /// ï‡Ç≠
    /// </summary>
    private void Walk()
    {
        if (_rb2d)
        {
            if (IsGrounded())
            {
                _rb2d.AddForce(_inputAxis * _walkPower, ForceMode2D.Force);
            }

            var currentVelocity = _rb2d.velocity;
            currentVelocity = FixedSpeed(currentVelocity);

            _rb2d.velocity = currentVelocity;
        }
    }

    /// <summary>
    /// à⁄ìÆë¨ìxÇÃèCê≥
    /// </summary>
    /// <returns>èCê≥Ç≥ÇÍÇΩíl</returns>
    private Vector2 FixedSpeed(Vector2 currentVelocity)
    {
        if (_rb2d.velocity.x > _maxSpeed)
        {
            currentVelocity.x = _maxSpeed;

        }
        else if (_rb2d.velocity.x < -_maxSpeed)
        {
            currentVelocity.x = -_maxSpeed;
        }

        return currentVelocity;
    }

    /// <summary>
    /// îÚÇ‘
    /// </summary>
    private void Jump()
    {
        if (_rb2d)
        {
            if (IsGrounded())
            {
                _rb2d.AddForce(Vector2.up * _jumpPower, ForceMode2D.Impulse);
            }
        }
    }

    /// <summary>
    /// ê⁄ínîªíË
    /// </summary>
    private bool IsGrounded()
    {
        var start = this.transform.position;
        var end = new Vector3(start.x, start.y - 1.1f, start.z);
        var layer = LayerMask.GetMask("Ground");
        return Physics2D.Linecast(start, end, layer); ;
    }
    #endregion
}
