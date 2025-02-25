using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class LoadingSceneManager : MonoBehaviour
{

    public static string nextScene;

    [SerializeField]
    Image progressBar;
    [SerializeField]
    RawImage loadingBar;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadScene());
    }
    public static void LoadScene(string nextSceneString)
    {
        nextScene = nextSceneString;
        SceneManager.LoadScene("LoadingScene_bound", LoadSceneMode.Additive);
    }

    IEnumerator LoadScene()
    {
        yield return null;
        AsyncOperation op;
        op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;

        while (!op.isDone)
        {
            yield return null;
            if (op.progress >= 0.9f)
            {

                yield return new WaitForSeconds(1f);
                op.allowSceneActivation = true;

                yield return new WaitForSeconds(5f);

                yield break;

            }
        }
    }

    IEnumerator LoadSceneProgress()
    {
        yield return null;
        AsyncOperation op;
        op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;
        float timer = 0.0f;

        while (!op.isDone)
        {
            yield return null;
            timer += Time.deltaTime;

            if (op.progress > 0.98f)
            {
                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, op.progress, timer);
                if (progressBar.fillAmount >= op.progress)
                {
                    timer = 0f;
                }
            }
            else
            {
                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, 1f, timer);
                if (progressBar.fillAmount == 1.0f)
                {
                    op.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }
}
