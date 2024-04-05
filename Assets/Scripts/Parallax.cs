using UnityEngine;

/// <summary>
/// Parallax digunakan pada latar belakang (yaitu gambar) sehingga dapat dipindahkan dengan kecepatan berbeda
/// </summary>

public class Parallax : MonoBehaviour
{
    private float length, startPosition;

    [SerializeField] private GameObject cam;

    [SerializeField] private float parallaxEffect;
    [SerializeField] private float offest = 0f;

    private void Start()
    {
        // Inisiasi
        startPosition = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x + offest;

        cam = Camera.main.gameObject;

        //Debug.Log($"{gameObject.name} length: {length}");
    }

    private void Update()
    {
        // Menghitung tempPosition dan distance
        float tempPosition = cam.transform.position.x * (1 - parallaxEffect);
        float distance = cam.transform.position.x * parallaxEffect;

        

        // Movement object
        transform.position = new Vector3(startPosition + distance, transform.position.y, transform.position.z);

        if (tempPosition > startPosition + length)
            startPosition += length;
        else if (tempPosition < startPosition - length)
            startPosition -= length;
    }
}
