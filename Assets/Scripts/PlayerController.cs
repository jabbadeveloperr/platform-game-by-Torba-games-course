using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = Constants.DefaultSpeed;
    [SerializeField] private float jumpSpeed = Constants.DefaultJumpSpeed;
    [SerializeField] private GameObject winnerText;
    private PlayerAnimatorController animatorController;

    private bool isFacingLeft;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animatorController = new PlayerAnimatorController(GetComponent<Animator>(), rb);
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
        if (other.gameObject.CompareTag(Tag.Enemy.ToString()))
            GameOver();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(Tag.Finish.ToString()))
            GameWin();
    }

    private void Move()
    {
        var horizontalMovement = Input.GetAxis("Horizontal");
        var isMoving = horizontalMovement != Constants.ZeroFloat;

        animatorController.ChangeAnimation(isMoving, IsGrounded());

        if (ShouldFlip(horizontalMovement))
            Flip();

        if (isMoving)
            MoveCharacter(horizontalMovement);
        else
            StopCharacter();
    }

    private bool ShouldFlip(float horizontalMovement)
    {
        return (horizontalMovement > 0 && isFacingLeft) || (horizontalMovement < 0 && !isFacingLeft);
    }

    private void Flip()
    {
        transform.Rotate(Constants.ZeroFloat, Constants.FlipRotationAngle, Constants.ZeroFloat);
        isFacingLeft = !isFacingLeft;
    }

    private void MoveCharacter(float horizontalMovement)
    {
        if (IsGrounded())
        {
            var movement = new Vector2(horizontalMovement, Constants.ZeroFloat) * (speed * Time.deltaTime);
            rb.AddForce(movement, ForceMode2D.Impulse);

            if (Mathf.Abs(rb.velocity.x) > Constants.MaxSpeedFactor)
                rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * Constants.MaxSpeedFactor, rb.velocity.y);
        }
    }

    private void StopCharacter()
    {
        if (IsGrounded())
            rb.velocity = new Vector2(rb.velocity.x * Constants.SlowFactor, rb.velocity.y);
    }

    private void Jump()
    {
        var horizontalMovement = Input.GetAxis("Horizontal");
        var isMoving = horizontalMovement != Constants.ZeroFloat;

        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
            rb.AddForce(
                new Vector2(
                    isMoving ? Mathf.Sign(horizontalMovement) * Constants.JumpHorizontalForce : Constants.ZeroFloat,
                    jumpSpeed), ForceMode2D.Impulse);
    }

    private bool IsGrounded()
    {
        return Physics2D.Raycast(transform.position - transform.localScale / Constants.TwoFloat, Vector2.down,
            Constants.GroundRaycastDistance);
    }

    private void GameOver()
    {
        if (SceneManager.GetActiveScene().isLoaded)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void GameWin()
    {
        winnerText.SetActive(true);
        Destroy(gameObject, Constants.TimeAfterWin);
    }
}