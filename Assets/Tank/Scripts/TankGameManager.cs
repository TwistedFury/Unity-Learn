using TMPro;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class TankGameManager : MonoBehaviour
{
    [SerializeField] GameObject titlePanel;
    [SerializeField] GameObject winPanel;
    [SerializeField] GameObject losePanel;
    [SerializeField] TMP_Text scoreText;
    [SerializeField] bool debug = false;

    public int score = 0;

    private static TankGameManager _instance;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Time.timeScale = (debug) ? 1.0f : 0;
        titlePanel.SetActive(!debug);
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = $"Score: {score:0000}";

        if (FindFirstObjectByType<Turret>() == null) { GameWin(); }
    }

    public void OnGameStart()
    {
        Time.timeScale = 1.0f;
        titlePanel.SetActive(false);
    }

    void GameWin()
    {
        Time.timeScale = 0;
        winPanel.SetActive(true);
    }

    public void GameLoss()
    {
        Time.timeScale = 0;
        losePanel.SetActive(true);
    }

    public void OnQuit()
    {
        #if UNITY_EDITOR
            EditorApplication.isPlaying = false;
        // This will be executed in a built application (PC, Mac, Android, etc.)
        #else
            Application.Quit();
        #endif
    }

    public static TankGameManager Instance { get { return _instance; } }
}
