using UnityEngine;
using UnityEngine.SceneManagement;

public class ScriptsPlayerManager : MonoBehaviour
{
    [SerializeField] GameObject canva;

    PlayerMovement playerMovement;
    PlayerTyping playerTyping;

    void Start()
    {
        playerTyping = GetComponent<PlayerTyping>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        Scene currentScene = SceneManager.GetActiveScene();

        string sceneName = currentScene.name;

        if (sceneName == "Words")
        {
            playerMovement.enabled = false;
            playerTyping.enabled = true;
            canva.SetActive(false);
        }
        else
        {
            playerMovement.enabled = true;
            playerTyping.enabled = false;
            canva.SetActive(true);
        }
    }
}
