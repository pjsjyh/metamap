using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class setViewTarget : MonoBehaviour
{
    Vector3 objBasicTransform;
    cameraMove targetViewSC;
    //mousewheelView targetWheelViewSC;

    MainCamera_Action targetMainCameraActionSC;

    ObjSetUi setUIcontents;
    int sign = 1;
    int sizeCount = 0;
    int sizeTotalCount = 0;
    public bool zoomCheck = false;

    public static UIexplain localUIControlCs;
    public static UIExplain_ver3 explain3;

    public static GameObject myOBJ = null;
    // Start is called before the first frame update

    void Start()
    {
        //targetViewSC = GameObject.Find("Main Camera").GetComponent<cameraRotation>();
        targetViewSC = GameObject.Find("Main Camera").GetComponent<cameraMove>();
        setUIcontents = gameObject.GetComponent<ObjSetUi>();
        objBasicTransform = gameObject.transform.localScale;
        targetMainCameraActionSC = GameObject.Find("Main Camera").GetComponent<MainCamera_Action>();
        localUIControlCs = GameObject.Find("UI_NAME_Canvas").GetComponent<UIexplain>();
        explain3 = GameObject.Find("info_toolkit").GetComponent<UIExplain_ver3>();


    }

    // Update is called once per frame
    void Update()
    {

    }


    //객체 마우스 오버 시 바운스 
    private void OnMouseEnter()  //
    {
        //마우스 오버
        //코루틴 진행
        if (!zoomCheck)
            StartCoroutine("ScaleControl");
    }

    private void OnMouseExit()
    {
        //마우스 아웃, 
        //코루틴 멈추고 원래 스케일로
        StopCoroutine("ScaleControl");
        //StopAllCoroutines();
        this.transform.localScale = objBasicTransform;
        sizeCount = 0;
        sizeTotalCount = 0;
    }

    IEnumerator ScaleControl()
    {

        this.transform.localScale += new Vector3(0.01f, 0.01f, 0.01f) * sign;
        sizeCount++;
        sizeTotalCount++;
        if (sizeCount > 10)
        {
            sizeCount = 0;
            sign = -sign;
        }
        if (sizeTotalCount > 20)
        {
            sizeTotalCount = 0;
            yield break;
        }
        yield return new WaitForSeconds(0.05f);
        StartCoroutine("ScaleControl");
    }



    private void OnMouseDown()
    {
        EnterBtnLoadScene.LoadScenename = gameObject.transform.GetComponent<ObjSetUi>().nextScene;
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            StartCoroutine(zoomInViewOBJ());
            if (myOBJ == gameObject)
            {
                explain3.click(myOBJ);
            }
            else
            {
                if (myOBJ != null)
                {
                    // localUIControlCs.objON = false;
                    //localUIControlCs.canvasOff();
                    explain3.infoOff();
                }

                myOBJ = gameObject;
                // localUIControlCs.clickObj(myOBJ);
                // localUIControlCs.followOn = true;
                explain3.click(myOBJ);

            }
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                targetMainCameraActionSC.Target = gameObject;
                targetMainCameraActionSC.offsetX = 8;
                targetMainCameraActionSC.offsetY = 4;
                targetMainCameraActionSC.offsetZ = 5;
                targetMainCameraActionSC.checkedCamReset = false;
                targetMainCameraActionSC.distanceRidance = 24;
                //targetMainCameraActionSC.zoomMax = 60f;
                GameObject.Find("Main Camera").GetComponent<Camera>().fieldOfView = 50f;

                //Debug.Log(gameObject.name);
                // targetViewSC.TargetViewAsset.transform.position = gameObject.transform.position;
                // targetViewSC.TargetViewAssets = gameObject;
                // targetViewSC.TargetViewAsset.transform.position += new Vector3(1.5f, 0.5f, 0);

                //targetWheelViewSC.cameraTarget = gameObject.transform;
                //뷰이동 Quaternion.slerp?써서 자연스럽게 확대되도록

                //targetViewSC.MoveCamlerp();
                // ShowExplaneUI();

                //StartCoroutine(zoomInView());
                zoomCheck = true;
            }
        }



    }


    public void ShowExplaneUI()
    {
        GameObject uiExplaneCanvas = GameObject.Find("ExplaneUICanvas").GetComponent<childcheck>().ExplaneUICanvas;
        uiExplaneCanvas.SetActive(true);

        uiExplaneCanvas.GetComponent<UIVisible>().OBJname = setUIcontents.objName;
        uiExplaneCanvas.GetComponent<UIVisible>().objExplane_ = setUIcontents.objExplane;
        uiExplaneCanvas.GetComponent<UIVisible>().AssetAddress = setUIcontents._address;
        if (setUIcontents._phonenum == null)
            uiExplaneCanvas.GetComponent<UIVisible>().phoneNum = " ";
        else
        {
            uiExplaneCanvas.GetComponent<UIVisible>().phoneNum = setUIcontents._phonenum;

        }
        UIVisible.explaneUrl = setUIcontents._explaneUrl;
        uiExplaneCanvas.GetComponent<UIVisible>().SetUICan();
    }

    IEnumerator zoomInView()
    {

        // Debug.Log(targetViewSC.distance);
        if (targetViewSC.distance < 20)
        {
            ShowExplaneUI();
            targetViewSC.zoomin = true;
            yield break;
        }
        yield return new WaitForSeconds(0.01f);

        targetViewSC.distance *= 0.83f;
        // Debug.Log(targetViewSC.distance);
        StartCoroutine(zoomInView());
    }
    IEnumerator zoomInViewOBJ()
    {
        yield return new WaitForSeconds(1);


        SetUIZoomObj.zoomInTargetObj = gameObject;
        GameObject.Find("objImage_zoomExplane").GetComponent<SetUIZoomObj>().viewTargetObj();
    }

}
