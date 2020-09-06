using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class GameMotor : MonoBehaviour
{

    GameManager gameManager;
    Board board;

    Touch touch;

    [SerializeField]
    private GameObject CenterObj;

    public List<GameObject> hexagonsList;
    

    public bool donuyor,secilebilir,isDraging;

    Vector2 startTouch,swipeDelta;

    RaycastHit2D raycastHit;

    int koseSayaciXBuyuk,koseSayaciXKucuk;

    void Start()
    {
        board = FindObjectOfType<Board>();
        gameManager = FindObjectOfType<GameManager>();
    }
    

    private void Update()
    {

        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                startTouch = touch.position;
                secilebilir = false;
                isDraging = true;
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled && Vector2.Distance(startTouch, touch.position) < 50 && !donuyor)
            {
                Vector2 touchPos = Camera.main.ScreenToWorldPoint(touch.position);
                raycastHit = Physics2D.Raycast(touchPos, touch.position);
                if (raycastHit.collider)
                {
                    raycastHit.collider.GetComponent<Hexagons>().GetHexagon();
                }
                isDraging = false;
            }

            swipeDelta = Vector2.zero;
            if (isDraging)
            {
                secilebilir = false;
                swipeDelta = touch.position - startTouch;
            }

            if (swipeDelta.magnitude > 75 && !donuyor)
            {
                secilebilir = false;
                float x = swipeDelta.x;
                float y = swipeDelta.y;

                if (Mathf.Abs(x) > Mathf.Abs(y))
                {
                    if (x < 0)
                    {
                        donuyor = true;
                        isDraging = false;
                        StartCoroutine(Dondur(-120f));
                    }
                    else
                    {
                        donuyor = true;
                        isDraging = false;
                        StartCoroutine(Dondur(120f));
                    }
                }
                else
                {
                    if (y < 0)
                    {
                        donuyor = true;
                        isDraging = false;
                        StartCoroutine(Dondur(120f));
                    }
                    else
                    {
                        donuyor = true;
                        isDraging = false;
                        StartCoroutine(Dondur(-120f));
                    }
                }
            }

        }
        
    }

    public IEnumerator Dondur(float donusYonu)
    {
        if (donuyor)
        {
            for (int i = 0; i < hexagonsList.Count; i++)
            {
                yield return new WaitForSeconds(0.3f);
                for (int a = 0; a < hexagonsList.Count; a++)
                {
                    hexagonsList[a].GetComponent<Hexagons>().Patlatilabilir = true;
                    hexagonsList[a].transform.RotateAround(CenterObj.transform.position, new Vector3(0, 0, 5),  donusYonu);
                    hexagonsList[a].transform.rotation = Quaternion.Euler(new Vector3(hexagonsList[a].transform.rotation.x, hexagonsList[a].transform.rotation.y, 0));
                }
            } 
        }
        donuyor = false;
        secilebilir = true;
    }

    

    public void GetHexagons(GameObject gon, GameObject gon1,GameObject gon2)  // Döndürmek için yanyana olan 3 hexagonu alıyoruz
    {
        koseSayaciXBuyuk = 0;
        koseSayaciXKucuk = 0;
        hexagonsList.Clear();
        hexagonsList.Add(gon);
        hexagonsList.Add(gon1);
        hexagonsList.Add(gon2);
        Vector3 CenterPos = hexagonsList[0].transform.position + hexagonsList[1].transform.position + hexagonsList[2].transform.position;
        CenterPos /= 3;
        CenterObj.transform.position = CenterPos;
        for (int i = 1; i < hexagonsList.Count; i++)                                 // seçilen objeyi belirtmek için kullanılan çerçevenin yönü ayarlanıyor
        {
            if (hexagonsList[i].transform.parent.position.x > hexagonsList[0].transform.parent.position.x)
            {
                koseSayaciXBuyuk++;
            }
            else if (hexagonsList[i].transform.parent.position.x < hexagonsList[0].transform.parent.position.x)
            {
                koseSayaciXKucuk++;
            }
        }
        if (koseSayaciXBuyuk > 1 )
        {
            CenterObj.transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else if (koseSayaciXBuyuk > 0 || koseSayaciXKucuk > 1)
        {
            CenterObj.transform.eulerAngles = new Vector3(0, 0, 60);
        }
        else if (koseSayaciXKucuk > 0)
        {
            CenterObj.transform.eulerAngles = new Vector3(0, 0, 0);
        }

        if (gameManager.baslangic)
        {
            gameManager.baslangic = false;
        }
    }



    public void Durdur()
    {
        CenterObj.transform.DOMoveX(50, 0);
        hexagonsList.Clear();
        StopCoroutine(Dondur(0f));
        donuyor = false;

    }


}