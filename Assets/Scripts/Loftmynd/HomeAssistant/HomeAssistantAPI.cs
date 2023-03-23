using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using SimpleJSON;

namespace Loftmynd.HomeAssistant
{
    public class HomeAssistantAPI : MonoBehaviour
    {
        private string apiUrl => PlayerPrefs.GetString("Loftmynd_API_URL", "");
        private string apiToken => PlayerPrefs.GetString("Loftmynd_API_TOKEN", "");
        private const string AUTH_HEADER = "Authorization";
        private const string BEARER_PREFIX = "Bearer ";

        public IEnumerator CallService(string domain, string service, string data = null)
        {
            string url = apiUrl + "services/" + domain + "/" + service;
            UnityWebRequest request = new UnityWebRequest(url, UnityWebRequest.kHttpVerbPOST);

            if (data != null)
            {
                byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(data);
                request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            }

            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader(AUTH_HEADER, BEARER_PREFIX + apiToken);

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(request.error);
            }
            else
            {
                Debug.Log("Service called successfully");
            }
        }

        public IEnumerator GetEntityState(string entityId)
        {
            string url = apiUrl + "states/" + entityId;
            UnityWebRequest request = UnityWebRequest.Get(url);
            request.SetRequestHeader(AUTH_HEADER, BEARER_PREFIX + apiToken);

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(request.error);
            }
            else
            {
                var json = JSON.Parse(request.downloadHandler.text);
                Debug.Log("Entity state: " + json["state"]);
                // You can parse more information from the JSON based on your needs.
            }
        }

        public IEnumerator UpdateEntityState(string entityId, string newState)
        {
            string url = apiUrl + "states/" + entityId;
            string data = "{\"state\": \"" + newState + "\"}";

            UnityWebRequest request = new UnityWebRequest(url, UnityWebRequest.kHttpVerbPOST);
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(data);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader(AUTH_HEADER, BEARER_PREFIX + apiToken);

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(request.error);
            }
            else
            {
                Debug.Log("Entity state updated successfully");
            }
        }
    }
}