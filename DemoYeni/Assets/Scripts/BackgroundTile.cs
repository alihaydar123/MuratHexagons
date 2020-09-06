using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BackgroundTile : MonoBehaviour
{
    [SerializeField]
    private GameObject[] Altigenler;

    [SerializeField]
    private Sprite[] Bombs;
    
    GameManager gameManager;
    

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        SetUp();
    }
    

    public void SetUp()
    {
        StartCoroutine(Olustur());
    }

    private IEnumerator Olustur()
    {
        if (!gameManager.gameOver)
        {
            yield return new WaitForSeconds(0.2f);
            int rastgele = Random.Range(0, Altigenler.Length);
            if (transform.childCount == 0)
            {
                GameObject altigen = Instantiate(Altigenler[rastgele], new Vector3(transform.position.x, transform.position.y + 5, transform.position.z), Quaternion.identity);
                altigen.transform.parent = transform;
                altigen.transform.DOMove(transform.position, 0.2f);
                altigen.name = name + " child";

                if (gameManager.puan > gameManager.bombaHesap)
                {
                    gameManager.bombaHesap += 1000;
                    altigen.GetComponent<SpriteRenderer>().sprite = Bombs[rastgele];
                    altigen.GetComponent<SpriteRenderer>().sortingOrder = 2;
                    altigen.GetComponent<Hexagons>().BombaMi = true;
                    altigen.transform.GetChild(0).gameObject.SetActive(true);
                    gameManager.Bomba = altigen;
                }

                if (transform.childCount > 1)
                {
                    Destroy(transform.GetChild(0));
                }

            }
        }
    }

    public void KontroluSagla()
    {
        StartCoroutine(Kontrol());
    }

    public IEnumerator Kontrol()
    {
        yield return new WaitForSeconds(3f);
        if (transform.childCount == 0)
        {
            StartCoroutine(Olustur());
        }
    }
}


