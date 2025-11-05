using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelCoinWatcher : MonoBehaviour
{
    [Header("References")]
    public PlayerInventory playerInventory;   

    [Header("Flow")]
    [Tooltip("Scene to load when ALL coins in the current scene are collected.")]
    public string nextSceneName;              

    private int totalCoinsInScene;

    void Awake()
    {
        
        totalCoinsInScene = FindObjectsOfType<Coins>(true).Length;

        if (playerInventory == null)
        {
            playerInventory = FindObjectOfType<PlayerInventory>();
        }
    }

    void OnEnable()
    {
        if (playerInventory != null)
            playerInventory.OnCoinCollected.AddListener(OnCoinCollected);
    }

    void OnDisable()
    {
        if (playerInventory != null)
            playerInventory.OnCoinCollected.RemoveListener(OnCoinCollected);
    }

    private void OnCoinCollected(PlayerInventory inv)
    {
        
        if (inv.NumberofCoins >= totalCoinsInScene && !string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
