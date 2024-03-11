using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = ConstantsPlayer.DefaultSpeed;
    [SerializeField] private float jumpSpeed = ConstantsPlayer.DefaultJumpSpeed;
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
        var isMoving = horizontalMovement != ConstantsPlayer.ZeroFloat;

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
        transform.Rotate(ConstantsPlayer.ZeroFloat, ConstantsPlayer.FlipRotationAngle, ConstantsPlayer.ZeroFloat);
        isFacingLeft = !isFacingLeft;
    }

    private void MoveCharacter(float horizontalMovement)
    {
        if (IsGrounded())
        {
            var movement = new Vector2(horizontalMovement, ConstantsPlayer.ZeroFloat) * (speed * Time.deltaTime);
            rb.AddForce(movement, ForceMode2D.Impulse);

            if (Mathf.Abs(rb.velocity.x) > ConstantsPlayer.MaxSpeedFactor)
                rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * ConstantsPlayer.MaxSpeedFactor, rb.velocity.y);
        }
    }

    private void StopCharacter()
    {
        if (IsGrounded())
            rb.velocity = new Vector2(rb.velocity.x * ConstantsPlayer.SlowFactor, rb.velocity.y);
    }

    private void Jump()
    {
        var horizontalMovement = Input.GetAxis("Horizontal");
        var isMoving = horizontalMovement != ConstantsPlayer.ZeroFloat;

        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
            rb.AddForce(
                new Vector2(
                    isMoving ? Mathf.Sign(horizontalMovement) * ConstantsPlayer.JumpHorizontalForce : ConstantsPlayer.ZeroFloat,
                    jumpSpeed), ForceMode2D.Impulse);
    }

    private bool IsGrounded()
    {
        return Physics2D.Raycast(transform.position - transform.localScale / ConstantsPlayer.TwoFloat, Vector2.down,
            ConstantsPlayer.GroundRaycastDistance);
    }

    private void GameOver()
    {
        if (SceneManager.GetActiveScene().isLoaded)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void GameWin()
    {
        winnerText.SetActive(true);
        Destroy(gameObject, ConstantsPlayer.TimeAfterWin);
    }
}