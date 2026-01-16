using TMPro;
using UnityEngine;

public class TankGameManager : MonoBehaviour
{
    [SerializeField] GameObject titlePanel;
    [SerializeField] TMP_Text scoreText;
    [SerializeField] bool debug = false;

    public int score = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Time.timeScale = (debug) ? 1.0f : 0;
        titlePanel.SetActive(!debug);
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = $"Score: {score.ToString("0000")}";
    }

    public void OnGameStart()
    {
        Time.timeScale = 1.0f;
        titlePanel.SetActive(false);
    }
}
