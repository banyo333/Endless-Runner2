using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{
    public GameObject gameOverUI;
    public PlayerScript playerScript;
    public GameObject CANVAS;
    public Image PlayerHealth;
    public TMP_Text PlayerHealthText;


    void Update()
    {
        if (playerScript != null && PlayerHealth != null)
        {
            // Convert to float to avoid integer division issue
            float healthPercentage = (float)playerScript.characterCurrentHealth / playerScript.characterMaxHealth;

            // Ensure fillAmount is within the correct range
            PlayerHealth.fillAmount = Mathf.Clamp01(healthPercentage);
        
            // Update health text
            PlayerHealthText.text = playerScript.characterCurrentHealth.ToString();
        }
    }

    public void gameOver()
    {
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(true);
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

       
    }
    public void QuitGame()
    {
        #if UNITY_STANDALONE
                Application.Quit();
        #endif
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #endif

    }

   
}
