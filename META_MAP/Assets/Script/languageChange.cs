using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class languageChange : MonoBehaviour
{

    UIName uIName;
    private void Awake()
    {
        uIName = GameObject.Find("GameObject").GetComponent<UIName>();
    }


    public void onClick_ENG()
    {
        StartCoroutine("onclick_ENG");
    }
    public void onClick_KOR()
    {
        StartCoroutine("onclick_KOR");
    }
    public void UserLocalization(int index)
    {
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[index];
    }

    IEnumerator onclick_ENG()
    {
        UserLocalization(0);
        yield return new WaitForSeconds(0.11f);
        uIName.changeUI(0);
    }

    IEnumerator onclick_KOR()
    {
        UserLocalization(1);
        yield return new WaitForSeconds(0.11f);
        uIName.changeUI(1);
    }
}
