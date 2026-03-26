using UnityEngine;
using UnityEngine.Video;

public class VideoUIController : MonoBehaviour
{
    // =========================
    // VIDEO REFERENCES
    // =========================

    // The VideoPlayer component (plays your tutorial video)
    public VideoPlayer videoPlayer;

    // The entire UI panel that holds the video + buttons
    public GameObject videoPanel;

    // =========================
    // START
    // =========================

    void Start()
    {
        // Make sure video UI starts hidden
        if (videoPanel != null)
        {
            videoPanel.SetActive(false);
        }
    }

    // =========================
    // OPEN VIDEO
    // =========================

    // Call this when you want to SHOW the tutorial video
    public void PlayVideo()
    {
        // Show the UI panel
        videoPanel.SetActive(true);

        // Start video from beginning
        videoPlayer.Stop();
        videoPlayer.Play();
    }

    // =========================
    // RESTART BUTTON FUNCTION
    // =========================

    // Hook this to your "Restart" button
    public void RestartVideo()
    {
        // Reset video time to start
        videoPlayer.Stop();

        // Play again from beginning
        videoPlayer.Play();
    }

    // =========================
    // EXIT BUTTON FUNCTION
    // =========================

    // Hook this to your "Exit" button
    public void ExitVideo()
    {
        // Stop video playback
        videoPlayer.Stop();

        // Hide the video UI
        videoPanel.SetActive(false);
    }
}