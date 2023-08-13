using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Google.Apis.Services;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using UnityEngine.Networking;

public class SheetReader :MonoBehaviour
{
    static private string spreadsheetID = "1tcVJSb9NW_yBtUxDq_TNpbaMKkAyB7r_xy4o5pT2oCY";
    static private string jsonPath = "/StreamingAssets/ezwayanalogies-98399165690d.json";
    static private SheetsService service;

    private string range = "Levels!A2:E";


    public const string CREDENTIL_JSON_STRING = @"{
  ""type"": ""service_account"",
  ""project_id"": ""ezwayanalogies"",
  ""private_key_id"": ""98399165690dd9b6e4450a7ff2c83033db54c1a3"",
  ""private_key"": ""-----BEGIN PRIVATE KEY-----\nMIIEvwIBADANBgkqhkiG9w0BAQEFAASCBKkwggSlAgEAAoIBAQDBeaZGHXiM+F2f\nau4QZ9GbamheDxvu7st8S84wWGmJPyQRCkMEDB43a1rSd8ke18dGvVMaBB3ZiV6J\n6DRumhb6RD9ijcWp9SspwviZmu/Sg3SjMlgb5GXOJ7XaaxSQAXQ97jdoufylTRpm\njTYdJ+zXuYAPyIYgQwT+OZjaRgHydcaaPAAv1A2RQa8148e5TOGehGBGmiK+9FZP\nlDIdoFXX7nx/AFeoYeQTbmxnXWsYk2EX23wON2RQZqIAO3gAN63C0npuQi7EH7YL\n/q1p69rwf4y0w7ATZnIE0uhl/zfuP0fdx+7bLYWGMBp5V7vkz/0TUIo6nHSU9GQw\n00c2zognAgMBAAECggEAG82pkWXmzwtaGPC66Wyh+J2gBfRR5VI3t9ME/HmYIPsL\n6c75tnkvVnN9B8XjByWnI4lNie2RhJNzHIpcDpDiQHDSUw1gnH35B/VW2GwljrEJ\nb/JqKwf1gVy/i3Xb7J3o4IV9PPWNUwvzgU1Fu4bjSFUGDD6a18QJ0abhSmzJFtiJ\nkXFFYlYKSV7bqM+V/ae+0k8B0j8IfvYMepZRy6KBNZxnGXj3oc2NMWNBO7ZLPEfA\nWlRDwrifAeB5xGsvES67c2Ias1UTZ+Xq6pB2aCXu7Fli7ahZL2nsb/EXUlkrL8yq\noN8sWi8vECfduku0VTSz13Jw9K/RsZ7nomvPNAKX4QKBgQD7mwESbE1H+BGQ1Jvb\nArGtj3prb8VeoGuRxJB2WW8gP1OBi6NupjY7L0oQukzhCNnfV73oHaJYQaZmxyo3\nw6byWj4H/y2qwnhuIJbrFvDXFC//D7Uc01kqXRhOTKmzSwo7o+8GKYNqyWkmUDLT\nbd0jGSazN5GQGbDJcdONTEHrqQKBgQDE2rqpYnKx029ez3Fh8VOX0LAkwQ/yw4Oe\nTs/cHTt2EZNnhNbRjoGVBdXk3UxeSGZCJHRlDRzo+rRjwQ9qoWp/OXVZOW3Qy9n6\n+z37LBndXPtc9ep0YL+04EfAOS2Svu/UrbPMqN6/ze5RAo+pbppRhQBZZoHwdPEL\nu5bQX5m3TwKBgQD3glYdn3iusbPl+6COZCoRiLFu+vb3zWvEa8+I/RW6bVFigNF3\nSf7TXgPITNzQCvr5IMFqk6xekcZ2GLrJPKkAhBG9oCN2dfQusdSE0358thk5GmSa\nRLbUW9xLOlM+UzVv0iaSs0faRuQvknlNUChYHKfbaXDChuNIZ7cqqre2cQKBgQCs\n5ZPW2+Hy78q8kiR6qrQJ/kAHOA5i4XgDniMQqBgmnk9CN7lZuvlzQvnXlsrRcHNo\nGo6khnzi3/d6OgHpXLal4XiBEmCyZ6kfuHV62wQomdcokdEHwEYAfpBmc5HkdpE8\n0ge+dH+YJS3wtvgpDzy45sEkxFTSNlPUQ0G16JGhHwKBgQD2t9OcNlE3n0NTTJm+\nYVxDkW3uIY43VRNSJHWub5eCsCy5cj5DxSxmMPscDLJSHqqaUn4gKdH+qIo2iyhT\nb9RYYpKTQ033tTkpv4vw3yaOM/gPJ76hWYugZjTylOSLpSGycR4JT0yA+qNsRQzn\nSnZPGRWmx3PwrnOlM4i46Ws1lw==\n-----END PRIVATE KEY-----\n"",
  ""client_email"": ""ezwayanalogies@ezwayanalogies.iam.gserviceaccount.com"",
  ""client_id"": ""117964868138744669801"",
  ""auth_uri"": ""https://accounts.google.com/o/oauth2/auth"",
  ""token_uri"": ""https://oauth2.googleapis.com/token"",
  ""auth_provider_x509_cert_url"": ""https://www.googleapis.com/oauth2/v1/certs"",
  ""client_x509_cert_url"": ""https://www.googleapis.com/robot/v1/metadata/x509/ezwayanalogies%40ezwayanalogies.iam.gserviceaccount.com"",
  ""universe_domain"": ""googleapis.com""
}";


    private void Awake()
    {

        Stream creds = GenerateStreamFromString(CREDENTIL_JSON_STRING);
        ServiceAccountCredential serviceAccountCredential = ServiceAccountCredential.FromServiceAccountData(creds);
        service = new SheetsService(new BaseClientService.Initializer() { HttpClientInitializer = serviceAccountCredential });

        var request = service.Spreadsheets.Values.Get(spreadsheetID, range);
        var response = request.Execute();
        var values = response.Values;


        //Stream jsonCreds = (Stream)File.Open(X, FileMode.Open);

        //ServiceAccountCredential credential = ServiceAccountCredential.FromServiceAccountData(jsonCreds);

        //service = new SheetsService(new BaseClientService.Initializer()
        //{
        //    HttpClientInitializer = credential,
        //});

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
