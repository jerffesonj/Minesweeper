using UnityEngine;
using UnityEngine.UI;

public class CellSprite : MonoBehaviour
{
    [SerializeField] private Image spriteRenderer;
    [SerializeField] private Sprite[] numberSprites;
    [SerializeField] private Sprite cellClosedSprite;
    [SerializeField] private Sprite cellBombSprite;
    [SerializeField] private Sprite flagSprite;
    [SerializeField] private Sprite wrongFlag;

    public void OpenSprite()
    {
        ChangeSprite(numberSprites[0]);
    }

    public void CloseSprite()
    {
        ChangeSprite(cellClosedSprite);
    }

    public void FlagSprite()
    {
        ChangeSprite(flagSprite);
    }

    public void BombSprite()
    {
        ChangeSprite(cellBombSprite);
    }

    public void SpriteNumber(int value)
    {
        ChangeSprite(numberSprites[value]);
    }

    public void WrongFlag()
    {
        ChangeSprite(wrongFlag);
    }

    void ChangeSprite(Sprite sprite)
    {
        spriteRenderer.sprite = sprite;
    }
}
