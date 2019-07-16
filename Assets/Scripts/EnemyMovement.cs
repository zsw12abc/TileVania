using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f;

    private Rigidbody2D _MyRigidbody2D;


    // Start is called before the first frame update
    void Start()
    {
        _MyRigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsFacingRight())
        {
            _MyRigidbody2D.velocity = new Vector2(moveSpeed, 0f);
        }
        else
        {
            _MyRigidbody2D.velocity = new Vector2(-moveSpeed, 0f);
        }
    }

    private bool IsFacingRight()
    {
        return transform.localScale.x > 0;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        transform.localScale = new Vector2(-(Mathf.Sign(_MyRigidbody2D.velocity.x)), 1f);
    }
}