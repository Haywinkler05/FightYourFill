using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class moneySystemCooking : MonoBehaviour
{

    public TMP_Text AmountReqText;
    public TMP_Text AmountTotalText;
    public GameObject exitButton;

    int total;
    int req;
    [SerializeField]
    bool moneyDone = false;

    public static moneySystemCooking refMoney;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        refMoney = this;

        AmountTotalText.text = "" + 0;
        randomReq();
    }

    // Update is called once per frame
    void Update()
    {

    }


    void randomReq()
    {
        req = Random.Range(3,8) * 100;
        AmountReqText.text = "" + req;
    }

    public void updateTotal(int num)
    {
        total += num;
        AmountTotalText.text = total + "";
        if(total >= req)
        {
            AmountTotalText.color = Color.green;
            Debug.Log("Total reached required amount!!! Debt payed for today");
            moneyDone = true;
            //bring up exit button
            exitButton.SetActive(true);
        }
    }
}
