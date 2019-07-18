using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour
{
    // Config
    [SerializeField] private float runSpeed = 5f;
    [SerializeField] private float jumpSpeed = 5f;
    [SerializeField] private float climbSpeed = 5f;
    [SerializeField] private Vector2 deathKick = new Vector2(25f, 25f);

    // State
    private int _playerScale = 3;
    private bool _isAlive = true;

    //Cached component references
    private Rigidbody2D _myRigidbody;
    private Animator _myAnimator;
    private CapsuleCollider2D _myCollider;
    private BoxCollider2D _myFeetCollider;
    private float _gravityScaleAtStart;

    // Start is called before the first frame update
    void Start()
    {
        _myRigidbody = GetComponent<Rigidbody2D>();
        _myAnimator = GetComponent<Animator>();
        _myCollider = GetComponent<CapsuleCollider2D>();
        _myFeetCollider = GetComponent<BoxCollider2D>();
        _gravityScaleAtStart = _myRigidbody.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isAlive)
        {
            return;
        }
        Run();
        Jump();
        FlipSprite();
        ClimbLadder();
        Die();
    }

    private void Run()
    {
        var controlThrow = CrossPlatformInputManager.GetAxis("Horizontal") * runSpeed; // -1 ~ +1
        var runVelocity = new Vector2(controlThrow, _myRigidbody.velocity.y);
        _myRigidbody.velocity = runVelocity;

        var playerHasHorizontalSpeed = Math.Abs(_myRigidbody.velocity.x) > Mathf.Epsilon;
        _myAnimator.SetBool("Running", playerHasHorizontalSpeed);
    }

    private void Jump()
    {
        if (!_myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Groud")))
        {
            return;
        }

        if (CrossPlatformInputManager.GetButtonDown("Jump"))
        {
            var jumpVelocityToAdd = new Vector2(0f, jumpSpeed);
            _myRigidbody.velocity += jumpVelocityToAdd;
        }
    }

    private void ClimbLadder()
    {
        if (!_myCollider.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            _myAnimator.SetBool("Climbing", false);
            _myRigidbody.gravityScale = _gravityScaleAtStart;
            return;
        }

        var controlThrowY = CrossPlatformInputManager.GetAxis("Vertical") * runSpeed; // -1 ~ +1
        var climbVelocity = new Vector2(_myRigidbody.velocity.x, controlThrowY);
        _myRigidbody.velocity = climbVelocity;
        _myRigidbody.gravityScale = 0f;

        var playerHasVerticalSpeed = Math.Abs(_myRigidbody.velocity.y) > Mathf.Epsilon;
        _myAnimator.SetBool("Climbing", playerHasVerticalSpeed);
    }

    private void Die()
    {
        if (_myRigidbody.IsTouchingLayers(LayerMask.GetMask("Enemy", "Hazards")))
        {
            _myAnimator.SetTrigger("Dying");
            GetComponent<Rigidbody2D>().velocity = deathKick;
            _isAlive = false;
        }
    }


    private void FlipSprite()
    {
        //if the player is moving horizontally, reverse the current scaling of x axis
        var playerHasHorizontalSpeed = Math.Abs(_myRigidbody.velocity.x) > Mathf.Epsilon;
        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(_myRigidbody.velocity.x) * _playerScale, 1f * _playerScale);
        }
    }
}