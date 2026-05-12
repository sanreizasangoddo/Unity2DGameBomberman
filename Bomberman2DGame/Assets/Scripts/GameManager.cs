using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject[] _players;
    private bool _roundEnding = false;

    private void Start()
    {
        int playerLayer = LayerMask.NameToLayer("Player");
        Physics2D.IgnoreLayerCollision(playerLayer, playerLayer, true);
    }

    // Controleert hoeveel spelers nog leven.
    // Wordt aangeroepen wanneer een speler doodgaat.
    public void CheckWinState()
    {
        if (_roundEnding) return;

        int aliveCount = 0;

        foreach (GameObject player in _players)
        {
            if (player.activeSelf)
            {
                aliveCount++;
            }
        }

        // Als er 1 of minder spelers over zijn -? ronde eindigt
        if (aliveCount <= 1)
        {
            _roundEnding = true;
            Invoke(nameof(NextRound), 3f); // kleine delay voor effect
        }
    }

    // Start de volgende ronde door de scene opnieuw te laden
    private void NextRound()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}