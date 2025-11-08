using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    private int fireCollision = 0;    
    private int maxCollisions = 3;

    public void TakeFireDamage()
    {
        fireCollision++;
        Debug.Log($"Player hit by fire! Hits: {fireCollision}");

        if (fireCollision >= maxCollisions)
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        Debug.Log("Game Over! Player has taken too much fire damage.");
        // Time.timeScale = 0f; 
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // reload the game
    }
}
