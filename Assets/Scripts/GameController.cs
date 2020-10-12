using System;

using UnityEngine;

[RequireComponent(typeof(Animator))]
public class GameController : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth = default;
    [SerializeField] private SceneController sceneController = default;

    private readonly int GameOverInTriggerId = Animator.StringToHash("GameOverIn");
    private readonly int GameOverOutTriggerId = Animator.StringToHash("GameOverOut");

    private Action gameOverInAction = delegate { };
    private Action gameOverOutAction = delegate { };

    private Animator animator = default;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerHealth.Die += OnPlayerDestroy;
    }

    private void OnPlayerDestroy()
    {
        animator.SetTrigger(GameOverInTriggerId);
    }

    public void RestartGame()
    {
        gameOverOutAction = () => sceneController.ReloadCurrentScene();
        animator.SetTrigger(GameOverOutTriggerId);
    }

    public void BackToMenu()
    {
        gameOverOutAction = () => sceneController.LoadPrevScene();
        animator.SetTrigger(GameOverOutTriggerId);
    }

    public void OnGameOverInComplete()
    {
        gameOverInAction?.Invoke();
    }

    public void OnGameOverOutComplete()
    {
        gameOverOutAction?.Invoke();
    }
}