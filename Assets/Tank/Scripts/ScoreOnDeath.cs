using UnityEngine;

public class ScoreOnDeath : MonoBehaviour
{
    [SerializeField] private int score = 100;
    public int Score { get { return score; } }
}
