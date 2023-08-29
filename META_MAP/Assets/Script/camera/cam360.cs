using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cam360 : MonoBehaviour
{
    public bool canvasClickedCheck = false;
    public bool touchcheck = true;  //true시 터치 스크린 버전
    public float speed = 1;

    float xmove = 0;
    float ymove = 0;
    float originEulerAnglesY;
    public float perspectiveZoomSpeed = 0.5f;  //줌인,줌아웃할때 속도(perspective모드 용)      
    public float orthoZoomSpeed = 0.5f;      //줌인,줌아웃할때 속도(OrthoGraphic모드 용) 

    public static bool iszoom = false;

    GameObject targetpos;
    public Transform original;
    Camera camera;
    GameObject kioskclosebtn;

    GameObject originkiosk;
    GameObject openkiosk;

    private void Awake()
    {

        GameObject.Find("blur-dial").GetComponent<Canvas>().worldCamera = gameObject.GetComponent<Camera>();
        touchcheck = DontDestioryObj.instance.touch_check;
        targetpos = GameObject.Find("targetpos");
        kioskclosebtn = GameObject.Find("kioskbtnCanvas").transform.GetChild(0).transform.gameObject;
        originkiosk = GameObject.Find("BODY.001").transform.GetChild(0).transform.gameObject;
        openkiosk = GameObject.Find("BODY.001").transform.GetChild(1).transform.gameObject;
    }
    private void Start()
    {
        originEulerAnglesY = transform.eulerAngles.x;
        camera = transform.GetComponent<Camera>();

    }
    // Update is called once per frame
    void Update()
    {
        if (!canvasClickedCheck)
        {
            if (touchcheck)
                touchscreenRotation();
            else
                mouseclickedRotation();

        }
        if (iszoom)
        {
            movepos();
        }
        if (!iszoom)
        {
            goorigin();
        }
    }


    void mouseclickedRotation()
    {
        if (Input.GetMouseButton(0))
        {
            xmove = Input.GetAxis("Mouse X");
            ymove += Input.GetAxis("Mouse Y");
            //original.eulerAngles = original.eulerAngles + new Vector3(0, xmove * speed, 0);
            transform.eulerAngles = transform.eulerAngles + new Vector3(0, xmove * speed, 0);

            if (transform.eulerAngles.y < 40)
                // original.eulerAngles = new Vector3(transform.eulerAngles.x, 40, transform.eulerAngles.z);
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, 40, transform.eulerAngles.z);
            if (transform.eulerAngles.y > 140)
                //original.eulerAngles = new Vector3(transform.eulerAngles.x, 140, transform.eulerAngles.z);
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, 140, transform.eulerAngles.z);
            if (ymove > 15)
                ymove = 15;
            if (ymove < -15)
                ymove = -15;
            //original.eulerAngles = new Vector3(originEulerAnglesY - ymove, transform.eulerAngles.y, transform.eulerAngles.z);
            transform.eulerAngles = new Vector3(originEulerAnglesY - ymove, transform.eulerAngles.y, transform.eulerAngles.z);

        }
    }


    private Vector2 nowPos, prePos;
    private Vector2 movePosDiff;
    void touchscreenRotation()
    {
        if (Input.touchCount == 1)
        {

            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                prePos = touch.position - touch.deltaPosition;
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                nowPos = touch.position - touch.deltaPosition;
                movePosDiff = (Vector2)(prePos - nowPos) * Time.deltaTime;
                prePos = touch.position - touch.deltaPosition;
            }


            xmove = movePosDiff.x;
            ymove += movePosDiff.y;

            transform.eulerAngles = transform.eulerAngles + new Vector3(0, xmove * speed, 0);

            if (transform.eulerAngles.y < 40)
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, 40, transform.eulerAngles.z);
            if (transform.eulerAngles.y > 140)
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, 140, transform.eulerAngles.z);
            if (ymove > 15)
                ymove = 15;
            if (ymove < -15)
                ymove = -15;

            transform.eulerAngles = new Vector3(originEulerAnglesY - ymove, transform.eulerAngles.y, transform.eulerAngles.z);


        }
        else if (Input.touchCount == 2)
        {
            Touch touchZero = Input.GetTouch(0); //첫번째 손가락 터치를 저장
            Touch touchOne = Input.GetTouch(1); //두번째 손가락 터치를 저장

            //터치에 대한 이전 위치값을 각각 저장함
            //처음 터치한 위치(touchZero.position)에서 이전 프레임에서의 터치 위치와 이번 프로임에서 터치 위치의 차이를 뺌
            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition; //deltaPosition는 이동방향 추적할 때 사용
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            // 각 프레임에서 터치 사이의 벡터 거리 구함
            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude; //magnitude는 두 점간의 거리 비교(벡터)
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            // 거리 차이 구함(거리가 이전보다 크면(마이너스가 나오면)손가락을 벌린 상태_줌인 상태)
            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

            // 만약 카메라가 OrthoGraphic모드 라면
            if (camera.orthographic)
            {
                camera.orthographicSize += deltaMagnitudeDiff * orthoZoomSpeed;
                camera.orthographicSize = Mathf.Max(camera.orthographicSize, 0.1f);
            }
            else
            {
                camera.fieldOfView += deltaMagnitudeDiff * perspectiveZoomSpeed;
                camera.fieldOfView = Mathf.Clamp(camera.fieldOfView, 0.1f, 179.9f);
                if (camera.fieldOfView < 30)
                    camera.fieldOfView = 30;
                if (camera.fieldOfView > 95)
                    camera.fieldOfView = 95;
            }

        }
    }
    public void movepos()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, targetpos.transform.rotation, Time.deltaTime * 2f);
        transform.position = Vector3.Lerp(transform.position, targetpos.transform.position, Time.deltaTime * 3f);

        float dis = Vector3.Distance(transform.position, targetpos.transform.position);
        ymove = 0;

    }
    public void goorigin()
    {
        //transform.rotation = Quaternion.Slerp(transform.rotation, original.transform.rotation, Time.deltaTime * 2f);
        transform.position = Vector3.Lerp(transform.position, original.transform.position, Time.deltaTime * 3f);
    }
    public void goout()
    {
        iszoom = false;
        kioskclosebtn.SetActive(false);
        originkiosk.SetActive(true);
        openkiosk.SetActive(false);
    }
}
