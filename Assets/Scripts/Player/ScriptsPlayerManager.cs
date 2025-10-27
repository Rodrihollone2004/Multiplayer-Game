using UnityEngine;
using UnityEngine.SceneManagement;

public class ScriptsPlayerManager : MonoBehaviour
{
    [SerializeField] GameObject canva;

    PlayerMovement playerMovement;
    WordManager wordManager;

    void Start()
    {
        wordManager = GetComponent<WordManager>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        Scene currentScene = SceneManager.GetActiveScene();

        string sceneName = currentScene.name;

        if (sceneName == "Words")
        {
            playerMovement.enabled = false;
            wordManager.enabled = true;
            canva.SetActive(false);
        }
        else
        {
            playerMovement.enabled = true;
            wordManager.enabled = false;
            canva.SetActive(true);
        }
    }
}
