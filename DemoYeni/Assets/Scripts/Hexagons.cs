using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Hexagons : MonoBehaviour
{

    public bool Patlatilabilir = false;
    public bool AsagiKay = false;
    public bool BombaMi = false;

    int BombaSayac = 6;

    public List<GameObject> sameGonsList;
    public List<GameObject> contactGonsList;
    public List<GameObject> patlatilacakGonsList;

    Board board;
    GameMotor gameMotor;
    GameManager gameManager;

    Touch touch;

    int sayac = 0;

    private void Start()
    {
        gameMotor = FindObjectOfType<GameMotor>();
        board = FindObjectOfType<Board>();
        gameManager = FindObjectOfType<GameManager>();
        Patlatilabilir = true;
        AsagiIn();
    }
    
    

    private void OnTriggerExit2D(Collider2D collision)
    {
        sameGonsList.Remove(collision.gameObject);
        contactGonsList.Remove(collision.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "kutu")
        {
            transform.parent = collision.transform;
            StopCoroutine(GetComponentInParent<BackgroundTile>().Kontrol());
        }
        if (collision.tag == tag)
        {
            if (IsCanAdd(collision, sameGonsList))
            {
                sameGonsList.Add(collision.transform.gameObject);
            }
            if (sameGonsList.Count >1)
            {
                for (int i = 0; i < sameGonsList.Count; i++)
                {
                    if (sameGonsList[i] == null)
                    {
                        return;
                    }
                }
                StartCoroutine(Destroy());
            }
        }
        if (collision.gameObject.tag != "kutu")
        {
            if (IsCanAdd(collision, contactGonsList))
            {
                contactGonsList.Add(collision.gameObject);
            }
        }

        
    }

    

    public bool IsCanAdd(Collider2D collision, List<GameObject> gonList)
    {
        foreach (GameObject gon in gonList)
        {
            if (collision.gameObject == gon)
            {
                return false;
            }
        }
        return true;
    }

    IEnumerator Destroy()
    {
        if (Patlatilabilir)
        {
            yield return new WaitForSeconds(0.1f);
            if (sameGonsList.Count >= 2)
            {

                for (int i = 0; i < sameGonsList.Count; i++)
                {

                    Hexagons hex = sameGonsList[i].GetComponent<Hexagons>();
                    if (hex.sameGonsList.Count >= 2)
                    {
                        foreach (var gons in hex.sameGonsList)
                        {
                            foreach (var gonns in sameGonsList)
                            {
                                if (gons == gonns && gons != null)
                                {
                                    patlatilacakGonsList.Add(gons);
                                    break;
                                }
                            }

                        }
                    }
                }

            }
            yield return new WaitForSeconds(0.1f);
            if (patlatilacakGonsList.Count >=2)
            {
                gameMotor.Durdur();
                for (int i = 0; i < patlatilacakGonsList.Count; i++)
                {
                    if (patlatilacakGonsList[i] != null)
                    {
                        patlatilacakGonsList[i].transform.DOScale(0, 0.2f);
                        Destroy(patlatilacakGonsList[i], 0.3f);
                    }
                }
                patlatilacakGonsList.Clear();
                transform.DOScale(0, 0.1f);
                Destroy(gameObject,0.3f);
            }
            Patlatilabilir = false;
        }

    }

    public void GetHexagon()
    {
        if (!gameMotor.donuyor)
        {
            gameManager.adimEklecoroutine = false;

            if (sayac >= contactGonsList.Count - 1) // sırayla diğer köşedekileri seçmek için
            {
                sayac = 0;
            }
            else
            {
                sayac++;
            }
            foreach (var gons in contactGonsList[sayac].GetComponent<Hexagons>().contactGonsList)
            {
                foreach (var gonns in contactGonsList)
                {
                    if (gons == gonns)
                    {

                        gameMotor.GetHexagons(gameObject, contactGonsList[sayac], gonns);
                        return;
                    }
                }
            }

        }

    }

    public void AsagiIn()
    {
        AsagiKay = true;
        StartCoroutine(AsagiIndir());

    }

    public void UstleIletisim()
    {
        string namesira1 = transform.parent.name.Substring(0, 1);
        string namesira2 = transform.parent.name.Substring(2);

        int sira2 = int.Parse(namesira2);

        Transform ust = transform.parent.parent.Find(namesira1 + "," + (sira2 + 2).ToString());

        if (ust != null && ust.childCount != 0)
        {
            ust.GetChild(0).GetComponent<Hexagons>().AsagiIn();
        }
        else
        {
            if (sira2 > board.height - 3  && sira2 < board.height)
            {
                Transform ustSon = transform.parent.parent.Find(namesira1 + "," + sira2.ToString());
                ustSon.GetComponent<BackgroundTile>().SetUp();
            }
        }


    }

    private IEnumerator AsagiIndir()
    {
        yield return new WaitForSeconds(0.1f);
        while (AsagiKay)
        {

            yield return new WaitForSeconds(0.1f);
            string namesira1 = transform.parent.name.Substring(0, 1);
            string namesira2 = transform.parent.name.Substring(2);

            int sira2 = int.Parse(namesira2);

            Transform alt = transform.parent.parent.Find(namesira1 + "," + (sira2 - 2).ToString());
            if (alt != null && alt.childCount == 0)
            {
                transform.DOMoveY(alt.position.y, 0.1f);
                UstleIletisim();

            }
            else
            {
                AsagiKay = false;
            }
        }

        yield return new WaitForSeconds(1f);
        Patlatilabilir = true;
        StartCoroutine(Destroy());
        yield return new WaitForSeconds(0.2f);
        Patlatilabilir = false;
    }
    
    public void Bomba()
    {
        if (BombaMi)
        {
            BombaSayac--;
            if (BombaSayac == 0)
            {
                StartCoroutine(gameManager.GameOver());
            }
            transform.GetChild(0).GetChild(0).GetComponent<Text>().text = BombaSayac.ToString();
        }

    }

    private void OnDestroy()
    {
        GetComponentInParent<BackgroundTile>().KontroluSagla();
        gameManager.PuanEkle();
        UstleIletisim();
    }
}
