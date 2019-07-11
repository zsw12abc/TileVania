using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour
{
    // Config
    [SerializeField] private float runSpeed = 5f;
    [SerializeField] private float jumpSpeed = 5f;

    // State
    private int _playerScale = 3;
    private bool _isAlive = true;

    //Cached component references
    private Rigidbody2D _myRigidbody;
    private Animator _myAnimator;

    // Start is called before the first frame update
    void Start()
    {
        _myRigidbody = GetComponent<Rigidbody2D>();
        _myAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Run();
        Jump();
        FlipSprite();
    }

    private void Run()
    {
        var controlThrow = CrossPlatformInputManager.GetAxis("Horizontal") * runSpeed; // -1 ~ +1
        var playerVelocity = new Vector2(controlThrow, _myRigidbody.velocity.y);
        _myRigidbody.velocity = playerVelocity;
        var playerHasHorizontalSpeed = Math.Abs(_myRigidbody.velocity.x) > Mathf.Epsilon;
        _myAnimator.SetBool("Running", playerHasHorizontalSpeed);
    }

    private void Jump()
    {
        if (CrossPlatformInputManager.GetButtonDown("Jump"))
        {
            var jumpVelocityToAdd = new Vector2(0f, jumpSpeed);
            _myRigidbody.velocity += jumpVelocityToAdd;
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