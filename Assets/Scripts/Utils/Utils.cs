using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public static class Utils
{
    public static void HandleBackButton()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Handle back button press
            BackButtonPressed();
        }
    }

    private static void BackButtonPressed()
    {
        // Check if the current scene is not the main menu scene
        if (!IsQuitSceneScene())
        {
            LoadQuitScene();
        }
        else
        {
            // Handle exit or other behavior when already in the main menu
            Application.Quit();
        }
    }

    private static bool IsQuitSceneScene()
    {
        return UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Quit";
    }

    private static void LoadQuitScene()
    {
        // Load the main menu scene
        GameManager.Goto("Quit");
    }

    public static Vector2 GetDisplayedDimensions(Image image)
    {
        RectTransform rectTransform = image.GetComponent<RectTransform>();
        // Get the sprite assigned to the Image component
        Sprite sprite = image.sprite;

        if (sprite == null)
        {
            Debug.LogError("Image component does not have a sprite assigned.");
            return Vector2.zero;
        }

        // Calculate the displayed dimensions based on the aspect ratio and RectTransform size
        float spriteWidth = sprite.rect.width;
        float spriteHeight = sprite.rect.height;

        // Get the RectTransform size
        float rectWidth = rectTransform.rect.width;
        float rectHeight = rectTransform.rect.height;

        // Calculate displayed dimensions considering aspect ratio
        float aspectRatio = spriteWidth / spriteHeight;
        float displayedWidth = rectWidth;
        float displayedHeight = rectWidth / aspectRatio;

        if (displayedHeight > rectHeight)
        {
            displayedHeight = rectHeight;
            displayedWidth = rectHeight * aspectRatio;
        }

        return new Vector2(displayedWidth, displayedHeight);
    }

    public static void Shuffle<T>(T[] array)
    {
        System.Random rng = new System.Random();

        int n = array.Length;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = array[k];
            array[k] = array[n];
            array[n] = value;
        }
    }
}
