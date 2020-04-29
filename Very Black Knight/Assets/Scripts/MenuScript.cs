using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class MenuScript : MonoBehaviour
{
    public UnityEvent startGame;

    void Start() {
        startGame.Invoke();
    }

    //Apply fadeout effect on gameObject
    public void fadeOutGameObject(GameObject go)
    {
        float seconds = 0.01f;

        if (go.GetComponent<CanvasRenderer>() != null)
        {
            StartCoroutine(FadeOutGameObject(go, seconds));
        }
        else
        {
            Debug.LogError("CanvasRenderer not set in GameObject");
        }

    }

    //Load new Scene with index
    public void loadScene(int index) {

        StartCoroutine(LoadScene(index));
    }

    IEnumerator LoadScene(int index) {
        yield return new WaitForSeconds(2.5f);
        UnityEngine.SceneManagement.SceneManager.LoadScene(index);

        yield return 0;
    }

    public void exitGame(){
        Debug.LogWarning("End of game");
        Application.Quit();
    } 

    //Apply fade in Effect on GameObject
    public void fadeInGameObject(GameObject go) {

        FadeObject fo = go.GetComponent<FadeObject>();
        float waitUntilStartTime;

        if (fo == null) waitUntilStartTime = 0;
        else {
            waitUntilStartTime = fo.waitUntilFadeTime;
        }

        float seconds = 0.05f;

        if (go.GetComponent<CanvasRenderer>() != null) {
            StartCoroutine(FadeInGameObject(go.GetComponent<CanvasRenderer>(), seconds, waitUntilStartTime));

        }
    }

    //Increase Alpha from 0 to apply fade In Effect
    private IEnumerator FadeInGameObject(CanvasRenderer myCanvasRenderer, float seconds, float waitUntilStartTime) {
        float alpha = 0;
        myCanvasRenderer.SetAlpha(alpha);

        yield return new WaitForSeconds(waitUntilStartTime);

        while (alpha < 1) {
            
            myCanvasRenderer.SetAlpha(alpha);
            alpha += 0.01f;

            //Wait for this time on every frame
            yield return new WaitForSeconds(seconds);
        }

        yield return 0;


    }

    private IEnumerator FadeOutGameObject(GameObject go, float seconds)
    {

        CanvasRenderer myCanvasRenderer = go.GetComponent<CanvasRenderer>();
        float alpha = myCanvasRenderer.GetAlpha();

        while (alpha > 0)
        { 
            myCanvasRenderer.SetAlpha(alpha);
            alpha -= 0.01f;

            //Wait for this time on every frame
            yield return new WaitForSeconds(seconds);

        }
        yield return 0;
    }
}
