using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

[JsonObject(MemberSerialization.OptIn)]
public class TimeController : MonoBehaviour {

    public double PerLifeDuration=15.0f;
    public bool bIsStop=false;
    public float TimeRatio = 1.0f;

    //当前游戏周目
    [JsonProperty]
    public int CurrentGameCycle = 0;
    //转生当前经历的时长
    [JsonProperty]
    public double CurrentSpanTime = 0.0;
    //上次结束游戏date信息
    [JsonProperty]
    public System.DateTime LastEndDateTime;

    private GameSetting gameSetting;

    private void Awake()
    {
        //Initial，读取本地存储，如果有，赋值，没有，归0
        //检查gameController是否从上次存档接着玩
        gameSetting = GameObject.FindGameObjectWithTag(DoneTags.gameController).GetComponent<GameSetting>();

        CurrentSpanTime = 0.0;
        CurrentGameCycle = 0;
        LastEndDateTime = System.DateTime.Now;
    }

    void Update()
    {
        if(!bIsStop)
        {
            CurrentSpanTime += Time.deltaTime;
            if(CurrentSpanTime>=PerLifeDuration)
            {
                int cycleNum = (int)(CurrentSpanTime/PerLifeDuration);
                CurrentGameCycle += cycleNum;
                CurrentSpanTime -= cycleNum * PerLifeDuration;
            }
        }
        TimeRatio = (float)(CurrentSpanTime / PerLifeDuration);
    }

    public void UpdateBySaveFile(TimeController SaveTimeController)
    {
        System.TimeSpan timeSpan = System.DateTime.Now - SaveTimeController.LastEndDateTime;
        CurrentGameCycle = SaveTimeController.CurrentGameCycle;
        CurrentSpanTime = SaveTimeController.CurrentSpanTime;
        Debug.Log("Last time you have played " + CurrentGameCycle + "GameCycle");
        Debug.Log("Last time your span time is " + CurrentSpanTime);
        double duration = timeSpan.TotalSeconds;
        if (duration >= PerLifeDuration)
        {
            int cycleNum = (int)(duration / PerLifeDuration);
            CurrentGameCycle += cycleNum;
            duration -= cycleNum * PerLifeDuration;
            CurrentSpanTime = duration;
        }
        Debug.Log("You have closed the game world for " + timeSpan.TotalSeconds + "seconds");
    }

    //private void OnDestroy()
    //{
    //    //Debug.Log("Time Controlled is destroyed at:"+System.DateTime.Now);
    //    ////存储必要信息
    //    ////定义存档路径
    //    //string dirpath = "./Save";
    //    ////创建存档文件夹
    //    //IOHelper.CreateDirectory(dirpath);
    //    ////定义存档文件路径
    //    //string filename = dirpath + "/TimeController.sav";
    //    ////保存数据
    //    //IOHelper.SetData(filename, this,false);
    //    ////读取数据
    //    //TimeController t1 = (TimeController)IOHelper.GetData(filename, typeof(TimeController),false);

    //    //Debug.Log("aaaa:" + t1.LastEndDateTime);
    //}
}
