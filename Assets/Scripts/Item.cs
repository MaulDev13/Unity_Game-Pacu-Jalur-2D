using UnityEngine;

public class Item : MonoBehaviour
{
    protected Rigidbody2D rb2d;
    protected Collider2D coll2d;

    protected SpriteRenderer spriteRenderer;

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        coll2d = GetComponent<Collider2D>();

        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public virtual void Acive(Movement collision)
    {

    }
}
