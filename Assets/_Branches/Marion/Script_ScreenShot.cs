using UnityEngine;

public class ScreenshotHandler : MonoBehaviour
{
    private int screenshotCount = 0;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            TakeScreenshot();
        }
    }
    
    void TakeScreenshot()
    {
        string filename = "Screenshot_" + screenshotCount + ".png";
        ScreenCapture.CaptureScreenshot(filename);
        screenshotCount++;
        Debug.Log("Screenshot saved as: " + filename);
    }
}