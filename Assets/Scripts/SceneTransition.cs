using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    private Animator anim;
    private string newScene;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void ChangeScene(string scene)
    {
        if (scene == "Level 1")
            GameManager.level = 1;

        newScene = scene;
        anim.SetTrigger("Scene End");
    }

    public void ChangeScene(string scene, float delay)
    {
        StartCoroutine(ChangeSceneEnum(scene, delay));
    }

    private IEnumerator ChangeSceneEnum(string scene, float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        newScene = scene;
        anim.SetTrigger("Scene End");
    }

    public void RestartScene(float delay)
    {
        StartCoroutine(RestartSceneEnum(delay));
    }

    private IEnumerator RestartSceneEnum(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        newScene = SceneManager.GetActiveScene().name;
        anim.SetTrigger("Scene End");
    }

    public void ChangeSceneEnd()
    {
        SceneManager.LoadScene(newScene);
    }
}
