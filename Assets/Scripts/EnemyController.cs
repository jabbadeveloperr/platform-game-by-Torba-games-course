using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float speed = 1;
    [SerializeField] private float rotationSpeed = 1f;

    [SerializeField] private Vector3 relativePosition;

    private Vector3 end;

    private Vector3 start;


    // Start is called before the first frame update
    private void Start()
    {
        start = transform.position;
        end = start + relativePosition;
    }

    // Update is called once per frame
    private void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, end, speed * Time.deltaTime);
        if (transform.position == end)
        {
            end = start;
            start = transform.position;
        }

        if (rotationSpeed > 0) transform.Rotate(new Vector3(0, 0, rotationSpeed * Time.deltaTime));
    }
}