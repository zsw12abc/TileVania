using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour
{
    // Config
    [SerializeField] private float runSpeed = 5f;
    [SerializeField] private float jumpSpeed = 5f;
    [SerializeField] private float climbSpeed = 5f;

    // State
    private int _playerScale = 3;
    private bool _isAlive = true;

    //Cached component references
    private Rigidbody2D _myRigidbody;
    private Animator _myAnimator;
    private Collider2D _myCollider2D;

    // Start is called before the first frame update
    void Start()
    {
        _myRigidbody = GetComponent<Rigidbody2D>();
        _myAnimator = GetComponent<Animator>();
        _myCollider2D = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Run();
        Jump();
        FlipSprite();
        ClimbLadder();
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
        if (!_myCollider2D.IsTouchingLayers(LayerMask.GetMask("Groud")))
        {
            return;
        }

        if (CrossPlatformInputManager.GetButtonDown("Jump"))
        {
            var jumpVelocityToAdd = new Vector2(0f, jumpSpeed);
            _myRigidbody.velocity += jumpVelocityToAdd;
            Debug.Log("Jump");
        }
    }

    private void ClimbLadder()
    {
        if (!_myCollider2D.IsTouchingLayers(LayerMask.GetMask("Groud")))
        {
            return;
        }

        var controlThrowY = CrossPlatformInputManager.GetAxis("Vertical") * runSpeed; // -1 ~ +1
        var climbVelocity = new Vector2(_myRigidbody.velocity.x, controlThrowY);
        _myRigidbody.velocity = climbVelocity;

        var playerHasVerticalSpeed = Math.Abs(_myRigidbody.velocity.y) > Mathf.Epsilon;
        _myAnimator.SetBool("Climbing", playerHasVerticalSpeed);
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