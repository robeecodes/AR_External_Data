using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using UnityEngine.Networking;
using System.Linq;

public class GetData : MonoBehaviour {
    [SerializeField] private string DataURL;

    private string results;
    
    private void Start() {
        StartCoroutine(RequestData());
    }

    private IEnumerator RequestData() {
        using UnityWebRequest request = UnityWebRequest.Get(DataURL);
        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError) {
            Debug.Log(request.error);
        } else {
            string json = request.downloadHandler.text;
            Debug.Log(json);
            ReadJSON(json);
        }
    }

    private void ReadJSON(string jsonString) {
        JSONNode node = JSON.Parse(jsonString);
        JSONObject jsonObject = node.AsObject;
        
        var dangerousAsteroids = jsonObject["near_earth_objects"].Children.Where(x => x["is_potentially_hazardous_asteroid"].AsBool).ToList();

        Debug.Log(dangerousAsteroids.Count());
        
        dangerousAsteroids.ForEach(x => {
            Debug.Log(x["name"].Value);
            Debug.Log(x["estimated_diameter"]["kilometers"]["estimated_diameter_min"].Value);
            Debug.Log(x["estimated_diameter"]["kilometers"]["estimated_diameter_max"].Value);
        });
    }

    private void GetDangerousAsteroidCount() {
        
    }
}