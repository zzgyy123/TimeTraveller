using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

[JsonObject(MemberSerialization.OptIn)]
public class TestBoxSave : MonoBehaviour
{
    [JsonProperty]
    public SaveVector3 selfPosition;
    [JsonProperty]
    public SaveQuaternion selfRotation;


    private SavaManager saveManager;

	void Awake ()
    {
        UpdateSelfTransform();
        saveManager = GameObject.FindGameObjectWithTag(DoneTags.gameController).GetComponent<SavaManager>();
        saveManager.boxLists.Add(this);
	}

    public void UpdateSelfTransform()
    {
        selfPosition = transform.position;
        selfRotation = transform.rotation;
    }

    private void OnDestroy()
    {
        //saveManager.boxLists.Remove(this);
    }
}
