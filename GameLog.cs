﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;

public class GameLog : MonoBehaviour
{
    public string secretKey;
    private string PostLogURL = "http://TheGreatJourney.se/GameLog.php";
    public string GameName, StudioName;

    Time SessionStart;
    float SessionTime0;
    
    //We start by just getting the HighScores, this should be removed, when you are done setting up.
    void Start() {
        SessionTime0 = Time.time;
        StartCoroutine(PostLog(1));
    }
    private void Update() {

    }
	bool notNull(string s){return (s!=null&&s!="");}
    //This is where we post
    //allow for posting multiple values. In an array or whatnot
    IEnumerator PostLog(int NewSession) {
		if(notNull(GameName)&&notNull(StudioName)){
			string hash = Md5Sum(GameName + StudioName + secretKey);
			WWWForm form = new WWWForm();
			form.AddField("GameName", GameName);
			form.AddField("NewSession", NewSession);
			SessionTime0 = Time.time - SessionTime0;
			form.AddField("SessionTime", (int)(SessionTime0));
			form.AddField("StudioName", StudioName);
			form.AddField("md5sum", hash);
			using (UnityWebRequest www = UnityWebRequest.Post(PostLogURL, form)) {
				yield return www.SendWebRequest();
				if (www.isNetworkError || www.isHttpError)
					Debug.Log(www.error);
				else
					Debug.Log(www.downloadHandler.text);
			}
		}else{
			Debug.Log("Enter GameName and StudioName to upload!\n");
		}
    }
    // This is used to create a md5sum - so that we are sure that only legit scores are submitted.
    // We use this when we post the scores.
    // This should probably be placed in a seperate class. But isplaced here to make it simple to understand.
    public string Md5Sum(string strToEncrypt) {
        System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding();
        byte[] bytes = ue.GetBytes(strToEncrypt);
        // encrypt bytes
        System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
        byte[] hashBytes = md5.ComputeHash(bytes);
        // Convert the encrypted bytes back to a string (base 16)
        string hashString = "";
        for (int i = 0; i < hashBytes.Length; i++) {
            hashString += System.Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
        }
        return hashString.PadLeft(32, '0');
    }
    //when application closes
    void OnApplicationQuit() {
        StartCoroutine(PostLog(0));
        Debug.Log("Bye bye! " + SessionTime0);
    }

}