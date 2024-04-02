using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float offset;

    private Vector2 startPosition;

    private float nextPosition;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        nextPosition = Mathf.Repeat(Time.time * moveSpeed, offset);

        transform.position = startPosition + Vector2.right * nextPosition;
    }
}
