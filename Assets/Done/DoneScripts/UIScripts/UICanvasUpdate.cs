using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UICanvasUpdate : MonoBehaviour {

    public GameObject TTGameController;
    public TimeController TimeControllerObject;


    private string cycleName = "CycleText";
    private string timeName = "TimeText";
    Text CycleText;
    Text TimeText;

    private void Awake()
    {
        TTGameController = GameObject.FindGameObjectWithTag(DoneTags.gameController);
        TimeControllerObject = TTGameController.GetComponent<TimeController>();
        Text[] textComps;
        textComps = this.GetComponentsInChildren<Text>();
        for (int i = 0; i < textComps.Length; ++i)
        {
            if(textComps[i].name==cycleName)
            {
                CycleText = textComps[i];
            }
            else if(textComps[i].name==timeName)
            {
                TimeText = textComps[i];
            }
        }
    }

    private void Update()
    {
        CycleText.text = "Cycle: "+TimeControllerObject.CurrentGameCycle;
        double CurrentSpanTime = TimeControllerObject.CurrentSpanTime;
        int hour ,minute,second, millisecond;
        hour = (int)CurrentSpanTime / 3600;
        minute = ((int)CurrentSpanTime - hour * 3600) / 60;
        second = (int)CurrentSpanTime - hour * 3600 - minute * 60;
        millisecond = (int)((CurrentSpanTime - (int)CurrentSpanTime) * 1000);

        TimeText.text = "Time: " + string.Format("{0:D2}:{1:D2}:{2:D2}.{3:D3}", hour, minute, second, millisecond);
    }
}
