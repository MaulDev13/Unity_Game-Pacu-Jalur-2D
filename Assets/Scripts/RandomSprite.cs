using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSprite : MonoBehaviour
{
    [SerializeField] private List<Sprite> sprites = new List<Sprite>();

    [SerializeField] private SpriteRenderer spriteRenderer;

    private void Start()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        Init();
    }

    void Init()
    {
        var randomIndex = Random.Range(0, sprites.Count);

        spriteRenderer.sprite = sprites[randomIndex];
    }
}
