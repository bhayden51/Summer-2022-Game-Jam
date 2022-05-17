using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static int level = 1;

    public GameObject PauseMenu;
    public TextMeshProUGUI levelTitle;

    [HideInInspector]
    public bool gamePaused;

    // Start is called before the first frame update
    void Start()
    {
        PauseMenu.SetActive(false);
        levelTitle.text = "Level " + level;
    }

    // Update is called once per frame
    void Update()
    {
        if(!gamePaused && Time.timeScale != 0 && Input.GetKeyDown(KeyCode.Escape))
        {
            gamePaused = true;
            Time.timeScale = 0;
            PauseMenu.SetActive(true);
        }
        else if (gamePaused && Input.GetKeyDown(KeyCode.Escape))
        {
            gamePaused = false;
            Time.timeScale = 1;
            PauseMenu.SetActive(false);
        }
    }

    public void NextLevel()
    {
        levelTitle.text = "Level Cleared";
        levelTitle.GetComponent<Animator>().SetTrigger("Level Cleared");
        level++;
        FindObjectOfType<SceneTransition>().ChangeScene("Level " + level, 2.5f);
    }
}
