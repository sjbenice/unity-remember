using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static int level = 0;

    private Stack<string> _sceneNameStack = new Stack<string>();

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public static GameManager Instance;

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void Goto(string sceneName)
    {
        if (GameManager.Instance)
        {
            if (sceneName == null)
            {
                if (GameManager.Instance._sceneNameStack.Count > 1)
                {
                    GameManager.Instance._sceneNameStack.Pop();
                    sceneName = GameManager.Instance._sceneNameStack.Peek();
                }
            }
            else
            {
                if (GameManager.Instance._sceneNameStack.Count  == 0 && SceneManager.GetActiveScene() != null)
                {
                    GameManager.Instance._sceneNameStack.Push(SceneManager.GetActiveScene().name);
                }
                GameManager.Instance._sceneNameStack.Push(sceneName);
            }
        }

        if (sceneName != null)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
