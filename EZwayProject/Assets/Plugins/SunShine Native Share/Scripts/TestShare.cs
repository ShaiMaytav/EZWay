using System.Collections;
using UnityEngine;

public class TestShare : MonoBehaviour
{

    private SunShineNativeShare _sunshineNativeShare;

    public string ShareQuestText;

    private void Awake()
    {
       //_sunshineNativeShare = Find
    }

    public void takeScreenShotAndShare()
    {
        StartCoroutine(takeScreenshotAndSave());
    }

    public void ShareMultipleScreenShot()
    {
        StartCoroutine(takeMultipleScreenshotAndSave());
    }

    private IEnumerator takeScreenshotAndSave()
    {
        yield return new WaitForEndOfFrame();
        Texture2D screenImage = new Texture2D(Screen.width, Screen.height);

        //Get Image from screen
        screenImage.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        screenImage.Apply();

        //Convert to png
        byte[] imageBytes = screenImage.EncodeToPNG();
        string img_name = "ScreenShot.png";
        string destination_path = Application.persistentDataPath + "/" + img_name; ;
        System.IO.File.WriteAllBytes(destination_path, imageBytes);

        //Call Share Function
        shareScreenshot(destination_path);
    }

    private IEnumerator takeMultipleScreenshotAndSave()
    {
        yield return new WaitForEndOfFrame();
        Texture2D screenImage = new Texture2D(Screen.width, Screen.height);

        //Get Image from screen
        screenImage.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        screenImage.Apply();

        //Convert to png
        byte[] imageBytes = screenImage.EncodeToPNG();
        string img_name1 = "ScreenShot1.png";
        string destination_path1 = Application.persistentDataPath + "/" + img_name1; ;
        System.IO.File.WriteAllBytes(destination_path1, imageBytes);

        string img_name2 = "ScreenShot2.png";
        string destination_path2 = Application.persistentDataPath + "/" + img_name2; ;
        System.IO.File.WriteAllBytes(destination_path2, imageBytes);


        string[] listOfPaths = new string[2];

        listOfPaths[0] = destination_path1;
        listOfPaths[1] = destination_path2;
        //Call Share Function
        shareMultipleFileOfSameType(listOfPaths);
    }

    private void shareScreenshot(string path)
    {
#if UNITY_IOS
        SunShineNativeShare.instance.ShareText(ShareQuestText + "מה לדעתך התשובה? \n להורדת האפליקציה: https://www.ezway.co.il/practice/psychometric-vocabulary/psychometric-apps/", "Share EZWay");
#endif

#if UNITY_ANDROID
        SunShineNativeShare.instance.ShareSingleFile(path, SunShineNativeShare.TYPE_IMAGE, "מה לדעתך התשובה? \n להורדת האפליקציה: https://www.ezway.co.il/practice/psychometric-vocabulary/psychometric-apps/", "Share EZWay");
#endif
    }

    public void ShareText()
    {
        SunShineNativeShare.instance.ShareText("https://www.ezway.co.il/practice/psychometric-vocabulary/psychometric-apps/", "Share EZWay");
    }


    private void shareMultipleFileOfSameType(string[] listOfPaths)
    {
        SunShineNativeShare.instance.ShareMultipleFileOfSameType(listOfPaths, SunShineNativeShare.TYPE_IMAGE, "Share Message", "Share By sunshine");

        //For Multiple file type shareing
        //SunShineNativeShare.ShareMultipleFileOfMultipleType(listOfPaths, "Share Message","Share By sunshine");
    }



}
