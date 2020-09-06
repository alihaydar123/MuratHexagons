using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    [SerializeField]
    private Text ScoreTxt, MoveText,GOScoreTxt,GOMoveText;

    [SerializeField]
    private GameObject GameOverPanel;

    public GameObject Bomba;

    GameMotor gameMotor;
    Board board;


    public int bombaHesap;
    public int puan;

    public int Adim;

    public bool adimEklecoroutine,gameOver;
    public bool baslangic = true;


    void Start()
    {
        GameOverPanel.SetActive(false);
        board = FindObjectOfType<Board>();
        gameMotor = FindObjectOfType<GameMotor>();
    }
    

    public void PuanEkle()
    {
        if (!adimEklecoroutine)
        {
            Adim++;
            if (baslangic)
            {
                Adim = 0;
            }
            StartCoroutine(board.BoslukDoldur());
            MoveText.text = Adim.ToString();
            if (Bomba != null)
            {
                Bomba.GetComponent<Hexagons>().Bomba();
            }
            adimEklecoroutine = true;
        }
        if (!baslangic)
        {
            puan += 5;
        }
        ScoreTxt.text = puan.ToString();
    }


    public IEnumerator GameOver()
    {
        GameOverPanel.SetActive(true);
        GameOverPanel.transform.DOScale(1, 2f);
        GOMoveText.text = Adim.ToString();
        GOScoreTxt.text = puan.ToString();
        for (int i = 0; i < board.transform.childCount; i++)
        {
            yield return new WaitForSeconds(0.05f);
            board.transform.GetChild(i).DOMoveY(-25, 0.1f);
        }
    }

    public void YenidenBasla()
    {
        SceneManager.LoadScene(1);
    }
}
