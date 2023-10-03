using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UI : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private TextMeshProUGUI score, finalScore;
    [SerializeField] AudioClip[] uiSound;
    [SerializeField] GameObject[] descont, desc;
    [SerializeField] private int minScore;
    [SerializeField] GameObject buttonPlay;

    public bool active;

    private void Start()
    {
        player.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        score.text = player.SetScore().ToString();
        finalScore.text = player.SetScore().ToString();

        if (active)
        {
            desc[0].SetActive(true);
        }

        if (player.SetScore() < minScore)
        {
            descont[0].SetActive(false);
            descont[1].SetActive(false);
        }
        else
        {
            descont[0].SetActive(true);
            descont[1].SetActive(true);
        }
    }


    public void RestartGame()
    {
        StartCoroutine("play");
    }

    IEnumerator play()
    {
        GetComponent<AudioSource>().clip = uiSound[0];
        GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(0);
    }

    public void Play()
    {
        Time.timeScale = 1;
        player.enabled = true;
        buttonPlay.SetActive(false);
        desc[0].SetActive(false);
        desc[1].SetActive(false);
    }

    public void Config()
    {
        Time.timeScale = 0;
        desc[0].SetActive(true);
        desc[1].SetActive(true);
    }
}
