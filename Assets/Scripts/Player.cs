using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour
{
    [SerializeField] private float runSpeed = 5f;
    private Rigidbody2D _myRigidbody;

    // Start is called before the first frame update
    void Start()
    {
        _myRigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Run();
    }

    private void Run()
    {
        var controlThrow = CrossPlatformInputManager.GetAxis("Horizontal") * runSpeed; // -1 ~ +1
        var playerVelocity = new Vector2(controlThrow, _myRigidbody.velocity.y);
        _myRigidbody.velocity = playerVelocity;
    }
}