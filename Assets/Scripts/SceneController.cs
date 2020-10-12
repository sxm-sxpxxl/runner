using System;

using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Animator))]
public class SceneController : MonoBehaviour
{
    private readonly int FadeOutTriggerId = Animator.StringToHash("FadeOut");

    private Animator animator = default;
    private Action sceneAction = delegate { };

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void LoadNextScene()
    {
        sceneAction = () =>
        {
            int nextSceneBuildIndex = Mathf.Min(SceneManager.sceneCountInBuildSettings - 1, SceneManager.GetActiveScene().buildIndex + 1);
            SceneManager.LoadScene(nextSceneBuildIndex);
        };
        animator.SetTrigger(FadeOutTriggerId);
    }

    public void LoadPrevScene()
    {
        sceneAction = () =>
        {
            int prevSceneBuildIndex = Mathf.Max(0, SceneManager.GetActiveScene().buildIndex - 1);
            SceneManager.LoadScene(prevSceneBuildIndex);
        };
        animator.SetTrigger(FadeOutTriggerId);
    }

    public void ReloadCurrentScene()
    {
        sceneAction = () =>
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        };
        animator.SetTrigger(FadeOutTriggerId);
    }

    public void QuitFromScene()
    {
        sceneAction = () =>
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        };
        animator.SetTrigger(FadeOutTriggerId);
    }

    public void OnFadeOutComplete()
    {
        sceneAction?.Invoke();
    }
}