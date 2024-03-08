using UnityEngine;

public class ChestController : MonoBehaviour
{
    private Animator _animator;
    private static readonly int ToOpening = Animator.StringToHash("toOpening");

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) _animator.SetTrigger(ToOpening);
    }
}