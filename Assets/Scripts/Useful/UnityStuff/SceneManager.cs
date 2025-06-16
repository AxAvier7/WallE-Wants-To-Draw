using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;

    void Awake()
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

    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex != 0 && Input.GetKeyDown(KeyCode.Escape))
        {
            LoadScene0();
        }
    }

    public void LoadScene0() => SceneManager.LoadScene(0);
    public void LoadScene1() => SceneManager.LoadScene(1);
    public void LoadScene2() => SceneManager.LoadScene(2);
    public void Quit() => Application.Quit();
}