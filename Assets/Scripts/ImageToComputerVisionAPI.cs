using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.Networking;
using System;
using System.Collections.Generic;
using HoloToolkit.Unity.InputModule;
using UnityEngine.UI;

public class ImageToComputerVisionAPI : MonoBehaviour, IInputClickHandler
{

    string VISIONKEY = "VISIONKEY"; // replace with your Computer Vision API Key

    string visionURL = "https://westeurope.api.cognitive.microsoft.com/vision/v1.0/analyze";

    public string fileName { get; private set; }
    string responseData;

    private ShowImageOnPanel panel; //サンプルに対して追加
    public Text text;               //サンプルに対して追加

    // Use this for initialization
    void Start () {
	    fileName = Path.Combine(Application.streamingAssetsPath, "cityphoto.jpg"); // Replace with your file

        InputManager.Instance.PushFallbackInputHandler(gameObject); //サンプルに対して追加
        panel = gameObject.GetComponent<ShowImageOnPanel>();        //サンプルに対して追加
    }
	
	// Update is called once per frame
	void Update () {
	
        // This will be called with your specific input mechanism
        if(Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(GetVisionDataFromImages());
        }

	}
    /// <summary>
    /// Get Vision data from the Cognitive Services Computer Vision API
    /// Stores the response into the responseData string
    /// </summary>
    /// <returns> IEnumerator - needs to be called in a Coroutine </returns>
    IEnumerator GetVisionDataFromImages()
    {
        byte[] bytes = UnityEngine.Windows.File.ReadAllBytes(fileName);

        var headers = new Dictionary<string, string>() {
            { "Ocp-Apim-Subscription-Key", VISIONKEY },
            { "Content-Type", "application/octet-stream" }
        };

        WWW www = new WWW(visionURL, bytes, headers);

        yield return www;
        responseData = www.text; // Save the response as JSON string
        GetComponent<ParseComputerVisionResponse>().ParseJSONData(responseData);

        text.text = responseData;//mori
    }

    public void OnInputClicked(InputEventData eventData) //サンプルに対して追加
    {
        panel.DisplayImage();

        StartCoroutine(GetVisionDataFromImages());

    }
}
