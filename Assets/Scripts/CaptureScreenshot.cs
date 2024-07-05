using System.Collections;
using UnityEngine;

public class CaptureScreenshot : MonoBehaviour
{
    public Camera cameraToCapture; // 指定要捕捉的相机
    public int width = 700; // 输出图片的宽度
    public int height = 500; // 输出图片的高度

    // 在Start方法中调用Capture方法来截图
    void Start()
    {
        Capture();
    }

    public void Capture()
    {
        StartCoroutine(CaptureScreenshotCoroutine());
    }

    IEnumerator CaptureScreenshotCoroutine()
    {
        // 等待渲染线程结束
        yield return new WaitForEndOfFrame();

        // 创建一个RenderTexture
        RenderTexture renderTexture = new RenderTexture(width, height, 24);
        cameraToCapture.targetTexture = renderTexture; // 设置相机的TargetTexture为我们创建的RenderTexture
        Texture2D screenshot = new Texture2D(width, height, TextureFormat.RGB24, false); // 创建一个Texture2D对象来保存截图

        cameraToCapture.Render(); // 手动渲染相机视图

        // 激活渲染纹理的上下文以进行读取
        RenderTexture.active = renderTexture;
        screenshot.ReadPixels(new Rect(0, 0, width, height), 0, 0); // 从RenderTexture中读取像素
        screenshot.Apply(); // 应用更改

        // 清理工作
        cameraToCapture.targetTexture = null;
        RenderTexture.active = null; // 非常重要
        Destroy(renderTexture);

        // 将Texture2D转换为字节数组，然后保存为文件
        byte[] bytes = screenshot.EncodeToPNG();
        System.IO.File.WriteAllBytes("Screenshot.png", bytes); // 保存图片到项目文件夹的根目录

        Debug.Log("Screenshot saved to: " + System.IO.Path.Combine(Application.dataPath, "Screenshot.png"));
    }
}
