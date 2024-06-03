using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Key : MonoBehaviour
{
    public Animator anim;
    public GameObject _LoadSecne;
    private StorageHelper storageHelper;
    private GameDataPlayed _played;

    private void Start()
    {
        storageHelper = new StorageHelper();
        storageHelper.LoadData();
        _played = StorageHelper.played;
    }

    private void Awake()
    {
        _LoadSecne.SetActive(true);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == Tag.Playertag)
        {
            StartCoroutine(LoadSecne());
            
            var score = FindObjectOfType<Score>().GetScore();
            var gameData = new GameData()
            {
                score = score,
                timePlayed = DateTime.Now.ToString("yyyy/mm/dd HH:mm:ss")
            };
            _played.plays.Add(gameData);
            storageHelper.SaveData();
            storageHelper.LoadData();
            _played = StorageHelper.played;
            Debug.Log("Count played: "+_played.plays.Count);
        }
    }
    IEnumerator LoadSecne()
    {
        anim.SetTrigger("end");
        yield return new WaitForSecondsRealtime(1);
        anim.SetTrigger("start");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
