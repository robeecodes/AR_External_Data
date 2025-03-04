using System.Collections;
using UnityEngine;
using SimpleJSON;
using UnityEngine.Networking;
using System.Linq;
using Random = UnityEngine.Random;

public class GetData : MonoBehaviour {
    [SerializeField] private GameObject RockText;
    [SerializeField] private GameObject[] RockInstances;

    private string _results;
    private string _dataURL;

    private void Start() {
        _dataURL = "https://api.nasa.gov/neo/rest/v1/neo/browse?api_key=";
        _dataURL += Secrets.NASA_API_KEY;
        StartCoroutine(RequestData());
    }

    private IEnumerator RequestData() {
        using UnityWebRequest request = UnityWebRequest.Get(_dataURL);
        yield return request.SendWebRequest();
        if (!(request.result == UnityWebRequest.Result.ConnectionError ||
              request.result == UnityWebRequest.Result.ProtocolError)) {
            string json = request.downloadHandler.text;
            ReadJSON(json);
        }
    }

    private void ReadJSON(string jsonString) {
        JSONNode node = JSON.Parse(jsonString);
        JSONObject jsonObject = node.AsObject;

        var dangerousAsteroids = jsonObject["near_earth_objects"].Children
            .Where(x => x["is_potentially_hazardous_asteroid"].AsBool).ToList();

        dangerousAsteroids.ForEach(d => {
            Vector3 pos = new Vector3(Random.Range(-10.0f, 5.0f), Random.Range(0f, 10.0f), Random.Range(2.0f, 10.0f));
            Instantiate(RockInstances[Random.Range(0, RockInstances.Length)], pos, Random.rotation);
        });
    }
}