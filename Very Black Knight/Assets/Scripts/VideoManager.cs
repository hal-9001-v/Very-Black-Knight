using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.Events;

public class VideoManager : MonoBehaviour
{
    VideoPlayer myVideoPlayer;
    private RawImage rawImage;
    public bool readyToPlay;
    public UnityEvent atEndActions;

    //IMPORTANT: Remember to disable raw Image before playing!!!

    // Start is called before the first frame update
    void Awake()
    {
        myVideoPlayer = gameObject.GetComponent<VideoPlayer>();
        rawImage = gameObject.GetComponent<RawImage>();


        if (readyToPlay)

            playClip();

        if (!myVideoPlayer.isLooping)
            myVideoPlayer.loopPointReached += EndFunction;
    }

    void EndFunction(VideoPlayer vp)
    {
        atEndActions.Invoke();
    }


    public void playClip()
    {
        StartCoroutine(playVideo());
    }

    public void stopClip()
    {
        myVideoPlayer.Stop();
        rawImage.enabled = false;
    }

    IEnumerator playVideo()
    {

        myVideoPlayer.Prepare();

        while (!myVideoPlayer.isPrepared)
        {

            yield return new WaitForSeconds(0.1f);

        }
        rawImage.enabled = true;
        rawImage.texture = myVideoPlayer.texture;
        myVideoPlayer.Play();

        yield return 0;
    }


}
