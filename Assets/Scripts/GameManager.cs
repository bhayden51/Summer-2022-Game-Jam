using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static int level = 1;

    public TextMeshProUGUI levelTitle;

    // Start is called before the first frame update
    void Start()
    {
        levelTitle.text = "Level " + level;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NextLevel()
    {
        levelTitle.text = "Level Cleared";
        levelTitle.GetComponent<Animator>().SetTrigger("Level Cleared");
        level++;
        FindObjectOfType<SceneTransition>().ChangeScene("Level " + level, 2.5f);
    }
}
