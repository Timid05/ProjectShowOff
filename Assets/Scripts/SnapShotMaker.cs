using UnityEngine;
using System.IO;

public class ScreenshotCapturer : MonoBehaviour
{
    public int width = 1920;
    public int height = 1080;
    public string folderName = "Screenshots";

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            TakeScreenshot();
        }
    }

    void TakeScreenshot()
    {
        string directoryPath = Path.Combine(Application.dataPath, folderName);
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        string fileName = "screenshot_" + System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".png";
        string fullPath = Path.Combine(directoryPath, fileName);

        ScreenCapture.CaptureScreenshot(fullPath, Mathf.RoundToInt(width / (float)Screen.width));
        Debug.Log($"Screenshot saved to: {fullPath}");
    }
}
