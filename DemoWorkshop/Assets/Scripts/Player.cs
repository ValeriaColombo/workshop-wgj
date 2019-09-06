using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float RUN_SPEED = 1;
    [SerializeField] private float JUMP_SPEED = 3;
    
    [SerializeField] private Animator _animator;
    [SerializeField] private Rigidbody2D _rigidBody;

    private bool _canPlay = true;
    private bool _grounded = true;
    private Vector3 _initialPosition;

    private void Start()
    {
        _initialPosition = transform.localPosition;
    }

    private void Update()
    {
        if (!_canPlay) return;
        
        float speedX = Move();

        Jump();
        
        SetAnimationTriggers(speedX);
    }

    private float Move()
    {
        float speedX = 0;
        float speedY = _rigidBody.velocity.y;

        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.localRotation = Quaternion.Euler(0, 0, 0);
            speedX = RUN_SPEED;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.localRotation = Quaternion.Euler(0, 180, 0);
            speedX = -RUN_SPEED;
        }

        _rigidBody.velocity = new Vector2(speedX, speedY);
        return speedX;
    }

    private void Jump()
    {
        if (_grounded && Input.GetKeyDown(KeyCode.Space))
        {
            _grounded = false;
            _rigidBody.AddForce(new Vector2(0, JUMP_SPEED), ForceMode2D.Impulse);
        }
    }

    private void SetAnimationTriggers(float speedX)
    {
        _animator.SetFloat("velocityX", Mathf.Abs(speedX));
        _animator.SetBool("grounded", _grounded);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.transform.tag)
        {
            case "Ground":
                _grounded = true;
                break;
            case "Death":
                Die();
                break;
            case "Enemy":
                Die();
                break;
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collider)
    {
        switch (collider.transform.tag)
        {
            case "Collectible":
                break;
            case "LevelTarget":
                Win();
                break;
        }
    }

    private void Win()
    {
        _canPlay = false;
        _animator.SetTrigger("victory");
    }    

    private void Die()
    {
        _canPlay = false;
        _animator.SetBool("dead", true);
        _animator.SetTrigger("hurt");

        StartCoroutine(RespawnInAWhile());
    }

    private IEnumerator RespawnInAWhile()
    {
        yield return new WaitForSeconds(2);
        
        transform.localPosition = _initialPosition;
        _animator.SetBool("dead", false);

        yield return new WaitForSeconds(1);
        _canPlay = true;
    }
}
