using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] public float speed = 7;
    [SerializeField] public float jumpSpeed = 7;
    public GameObject winnerText;
    private Animator _animator;
    private bool _isGoBack;
    private Rigidbody2D _rigidbody2D;
    private float _timeAfterWin;

    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _timeAfterWin = 3;
    }

    private void Update()
    {
        Move();
        Jump();
    }

    private void OnDestroy()
    {
        GameOver();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy")) GameOver();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Finish")) GameWin();
    }

    private void Move()
    {
        var horizontalMovement = Input.GetAxis("Horizontal");
        var isMoving = horizontalMovement != 0;

        ChangeAnimation(isMoving);

        if (ShouldFlip(horizontalMovement)) Flip();
        if (isMoving) MoveCharacter(horizontalMovement);
    }

    private void ChangeAnimation(bool isMoving)
    {
        if (isMoving && IsGrounded())
        {
            _animator.SetTrigger("toRun");
        }
        else if (IsGrounded())
        {
            _animator.SetTrigger("toIdle");
        }
        else 
        {
            var triger = _rigidbody2D.velocity.y > 0 ? "toUp" : "toDown";
            _animator.SetTrigger(triger);
        }
    }


    private bool ShouldFlip(float horizontalMovement)
    {
        return (horizontalMovement > 0 && _isGoBack) || (horizontalMovement < 0 && !_isGoBack);
    }

    private void Flip()
    {
        var scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
        _isGoBack = !_isGoBack;
    }

    private void MoveCharacter(float horizontalMovement)
    {
        var movement = new Vector2(horizontalMovement, 0f).normalized * (speed * Time.deltaTime);
        _rigidbody2D.MovePosition(_rigidbody2D.position + movement);
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
            _rigidbody2D.AddForce(new Vector2(0, jumpSpeed), ForceMode2D.Impulse);
    }


    private bool IsGrounded()
    {
        var cachedTransform = transform;
        return Physics2D.Raycast(cachedTransform.position - cachedTransform.localScale / 2, Vector2.down, 0.1f);
    }

    private void GameOver()
    {
        if (SceneManager.GetActiveScene().isLoaded)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void GameWin()
    {
        winnerText.SetActive(true);
        Destroy(gameObject, _timeAfterWin);
    }
}