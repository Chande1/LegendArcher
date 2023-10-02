using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Tooltip("(확인)오른손 설정")]
    [SerializeField] GameObject Hand;
    [Tooltip("(확인)활 설정")]
    [SerializeField] GameObject Bow;
    [Tooltip("(필수)화살 설정")]
    [SerializeField] GameObject Arrow;
    [Tooltip("(확인)화살 포인트 설정")]
    [SerializeField] GameObject Arrowpoint;
    GameObject arrow;
    cHand hand;
    cBow bow;

    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.Find("CustomHandRight"))
        {
            Hand = GameObject.Find("CustomHandRight");
        }
        else if(GameObject.Find("RightControllerAnchor"))
            Hand = GameObject.Find("RightControllerAnchor");
        Bow = GameObject.Find("Bow");
        Arrowpoint = GameObject.Find("arrowpoint");
    }

    // Update is called once per frame
    void Update()
    {
        hand = Hand.GetComponent<cHand>();
        bow = Bow.GetComponent<cBow>();


        //그랩
        if (Arrowpoint.GetComponent<OVRGrabbable>().isGrabbed)       //그랩
        {
            switch (bow.bstate)
            {
                case BowState.Non:
                    arrow = Instantiate(Arrow, Arrowpoint.gameObject.transform.position, Arrowpoint.transform.rotation) as GameObject;    //화살 생성
                    hand.hstate = HandState.TakeString;
                    bow.bstate = BowState.Arrowning;
                    break;
                case BowState.Arrowning:
                    if (Arrowpoint.GetComponent<OVRGrabbable>().isGrabbed)          //포인트가 작동중이면
                    {
                        Debug.Log("시위----------------------------------------");
                        hand.hstate = HandState.TakeString;
                        bow.bstate = BowState.Drawing;
                    }
                    break;
            }
        }
        else if (!Arrowpoint.GetComponent<OVRGrabbable>().isGrabbed)    //포인트를 잡고 있지 않은 경우
        {
            if (hand.hstate == HandState.TakeString)   //줄을 손이 잡고 있었을때
            {
                if (bow.bstate == BowState.Non)
                    hand.hstate = HandState.Non;
                if (bow.bstate == BowState.Drawing)
                {
                    //Debug.Log("탕!");
                    hand.hstate = HandState.Non;
                    bow.bstate = BowState.Shoot;
                    bow.SetBowAni();

                    arrow.GetComponent<ArrowController>().astate = ArrowState.Shoot; //화살 날아가는중
                    //arrow.GetComponent<ArrowController>().Shoot(arrowspeed);
                    //arrow.GetComponent<ArrowController>().Shoot(new Vector3(0,0.5f,3)* arrowspeed);
                }
            }
        }
    }
}
