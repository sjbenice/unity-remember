using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DOTweenEffects : MonoBehaviour
{
    public float dropdownDuration = 0.5f; // Duration of dropdown animation
    public float bounceDuration = 0.5f;   // Duration of bounce animation
    public float timeVariation = 0.3f;
    public float bounceHeight = 100f;     // Height of bounce

    private Vector3 originalPosition; // Store the original position of the button

    void Start()
    {
        originalPosition = transform.position; // Store the original position

        dropdownDuration += Random.Range(-timeVariation, timeVariation);
        bounceDuration += Random.Range(-timeVariation, timeVariation);

        // Apply dropdown effect on Start
        if (dropdownDuration > 0 )
        {
            ApplyDropdownEffect();
        }
        else
        {
            if (bounceDuration > 0 )
            {
                ApplyBounceEffect();
            }
        }

    }

    public void ApplyDropdownEffect()
    {
        // Store the current position of the button
        Vector3 currentPos = transform.position;

        // Move the button upwards (out of the screen)
        transform.position = new Vector3(currentPos.x, currentPos.y + bounceHeight, currentPos.z);

        // Move the button back to its original position with a bounce effect
        transform.DOMove(originalPosition, dropdownDuration)
            .SetEase(Ease.OutCubic)
            .OnComplete(() =>
            {
                if (bounceDuration > 0)
                {
                    // Apply bounce effect after dropdown animation completes
                    ApplyBounceEffect();
                }
            });
    }

    public void ApplyBounceEffect()
    {
        // Move the button upwards slightly for the bounce effect
        Vector3 upPos = new Vector3(originalPosition.x, originalPosition.y + bounceHeight / 2f, originalPosition.z);

        // Move the button up and down to simulate a bounce effect
        transform.DOMove(upPos, bounceDuration / 2f)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                transform.DOMove(originalPosition, bounceDuration / 2f)
                    .SetEase(Ease.OutBounce);
            });
    }
}
