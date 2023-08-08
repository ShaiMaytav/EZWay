using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Google.Apis.Services;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;

public class SheetReader :MonoBehaviour
{
    static private string spreadsheetID = "1tcVJSb9NW_yBtUxDq_TNpbaMKkAyB7r_xy4o5pT2oCY";
    static private string jsonPath = "/StreamingAssets/ezwayanalogies-98399165690d.json";
    static private SheetsService service;

    private string range = "Levels!A2:E";

    private void Awake()
    {
        string fullJsonPath = Application.dataPath + jsonPath;

        Stream jsonCreds = (Stream)File.Open(fullJsonPath, FileMode.Open);

        ServiceAccountCredential credential = ServiceAccountCredential.FromServiceAccountData(jsonCreds);

        service = new SheetsService(new BaseClientService.Initializer()
        {
            HttpClientInitializer = credential,
        });

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
