using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetUIZoomObj : MonoBehaviour
{
    public static GameObject zoomInTargetObj = null;
    public GameObject testObvJ;
    public GameObject canvas_ObjZoomInView;
    public GameObject canvasInfoBox;
    public GameObject zoomInUIname;

    private void Awake()
    {
    }
    private void Start()
    {

    }
    public void viewTargetObj()
    {
        canvas_ObjZoomInView.SetActive(true);
        canvasInfoBox.SetActive(true);
        GameObject TargetModel = Instantiate(zoomInTargetObj, new Vector3(0, 0, 0), Quaternion.identity);

        TargetModel.transform.SetParent(gameObject.transform, false);
        TargetModel.transform.localScale = new Vector3(100f, 100f, 100f);
        //자식객체에도 layer추가해주기
        for (int i = 0; i < TargetModel.transform.childCount; i++)
        {
            TargetModel.transform.GetChild(i).gameObject.layer = 11;
        }
        TargetModel.layer = 11;
        TargetModel.AddComponent<zoomObjRotation>();

        zoomInUIname.GetComponent<set_UI_background>().text.text = zoomInTargetObj.name;
        zoomInUIname.GetComponent<set_UI_background>().name();
    }

    public void DestroyTargetObj()
    {

        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            Destroy(gameObject.transform.GetChild(i).gameObject);
        }
        gameObject.transform.DetachChildren();
    }
    public void hiddenOBJView()
    {
        DestroyTargetObj();
        canvas_ObjZoomInView.SetActive(false);

    }
}
