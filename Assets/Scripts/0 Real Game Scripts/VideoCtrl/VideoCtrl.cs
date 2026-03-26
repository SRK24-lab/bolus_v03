using UnityEngine;
using UnityEngine.Video;
using TMPro;

public class VideoCtrl : MonoBehaviour
{
    [SerializeField] private TMP_Text textSlot;
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private string textAfterVideo = "Start Game";

    void Start()
    {
        if (textSlot != null)
            textSlot.text = "Skip Cutscene";

        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached += OnVideoFinished;
        }
        else
        {
            Debug.LogWarning("VideoCtrl: VideoPlayer reference is not set.");
        }
    }

    private void OnDestroy()
    {
        if (videoPlayer != null)
            videoPlayer.loopPointReached -= OnVideoFinished;
    }

    private void OnVideoFinished(VideoPlayer source)
    {
        ChangeText(textAfterVideo);
    }

    public void ChangeText(string newText)
    {
        if (textSlot != null)
            textSlot.text = newText;
    }

    public void ChangeTextAfter(float seconds, string newText)
    {
        StartCoroutine(ChangeTextAfterCoroutine(seconds, newText));
    }

    private System.Collections.IEnumerator ChangeTextAfterCoroutine(float seconds, string newText)
    {
        yield return new WaitForSeconds(seconds);
        ChangeText(newText);
    }
}
