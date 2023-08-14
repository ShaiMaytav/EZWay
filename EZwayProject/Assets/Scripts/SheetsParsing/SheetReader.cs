using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Google.Apis.Services;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using UnityEngine.Networking;

public class SheetReader : MonoBehaviour
{
    static private string spreadsheetID = "1tcVJSb9NW_yBtUxDq_TNpbaMKkAyB7r_xy4o5pT2oCY";
    static private string jsonName = "/ezwayanalogies-98399165690d.json";
    static private SheetsService service;

    private string range = "Levels!A2:E";

    private string credentialsJson;



    private IEnumerator Start()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        string androidStreamingAssetsPath = "jar:file://" + Application.dataPath + "!/assets";
        string androidJsonPath = androidStreamingAssetsPath + jsonName;
        UnityWebRequest www = UnityWebRequest.Get(androidJsonPath);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Can't read file");
        }
        else
        {
            credentialsJson = www.downloadHandler.text;
        }
#endif

#if UNITY_IOS && !UNITY_EDITOR
        string iOSJsonPath = Application.streamingAssetsPath + jsonName;
        if (File.Exists(iOSJsonPath))
        {
            credentialsJson = File.ReadAllText(iOSJsonPath);
            Debug.Log("JSON loaded successfully: " + jsonContent);
        }
#endif

#if UNITY_EDITOR
        string winJsonPath = Application.streamingAssetsPath + jsonName;
        if (File.Exists(winJsonPath))
        {
            using (StreamReader reader = new StreamReader(winJsonPath))
            {
                credentialsJson = reader.ReadToEnd();
                Debug.Log("JSON Content: " + credentialsJson);
                // Now you have the JSON content as a string (jsonContent)
            }
        }
        else
        {
            Debug.LogError("JSON file not found at path: " + winJsonPath);
        }
#endif
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            Stream creds = GenerateStreamFromString(credentialsJson);
            ServiceAccountCredential serviceAccountCredential = ServiceAccountCredential.FromServiceAccountData(creds);
            service = new SheetsService(new BaseClientService.Initializer() { HttpClientInitializer = serviceAccountCredential });

            var request = service.Spreadsheets.Values.Get(spreadsheetID, range);
            var response = request.Execute();
            var values = response.Values;
        }
        yield return null;
    }

    public static Stream GenerateStreamFromString(string s)
    {
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream);
        writer.Write(s);
        writer.Flush();
        stream.Position = 0;
        return stream;
    }

    public IList<IList<object>> getSheetRange()
    {
        SpreadsheetsResource.ValuesResource.GetRequest request = service.Spreadsheets.Values.Get(spreadsheetID, range);

        ValueRange response = request.Execute();
        IList<IList<object>> values = response.Values;
        if (values != null && values.Count > 0)
        {
            return values;
        }
        else
        {
            Debug.Log("No data found.");
            return null;
        }
    }
}
