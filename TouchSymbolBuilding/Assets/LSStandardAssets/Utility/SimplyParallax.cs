using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SimplyParallax : MonoBehaviour
{
    [Tooltip("Measured in units per second")]
    public float speed;

    private SpriteRenderer ActiveRenderer;
    private float SpriteWidth;

    private void Awake()
    {
        ActiveRenderer = GetComponent<SpriteRenderer>();
        Sprite sprite = ActiveRenderer.sprite;
        SpriteWidth = sprite.textureRect.width / sprite.pixelsPerUnit;
    }

    private void Start()
    {
        SpriteRenderer clonedActive = Instantiate(ActiveRenderer, transform);

        Destroy(clonedActive.GetComponent<SimplyParallax>());

        Vector3 NewPos = clonedActive.transform.localPosition;
        NewPos.x += -1 * Mathf.Sign(speed) * SpriteWidth;

        clonedActive.transform.localPosition = NewPos;
        StartCoroutine(MoveIndefinitely());
    }

    private IEnumerator MoveIndefinitely()
    {
        float moveBy = speed * Time.deltaTime;
        Vector3 StartPosition = transform.localPosition;
        Vector3 NextPosition = StartPosition;
        float negWidth = SpriteWidth * -1;
        while (true)
        {
            transform.localPosition = NextPosition;
            yield return new WaitForEndOfFrame();

            NextPosition.x += moveBy;
            if (NextPosition.x >= SpriteWidth || NextPosition.x <= negWidth)
            {
                NextPosition.x = StartPosition.x;
            }
        }
    }

}
