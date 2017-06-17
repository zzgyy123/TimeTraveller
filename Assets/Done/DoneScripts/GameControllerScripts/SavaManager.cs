using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavaManager : MonoBehaviour {

    /* 
     *  是否加密，默认为否
     *  保存路径
     *  保存时间信息的sav文件名
     *  保存盒子信息的sav文件名
    
     *  游戏设置
     *  时间控制器
     *  
     */
    public bool bIsEcypt=false;
    private string SavePath = "./Save/";
    public string timeControllerSaveName = "TimeController.sav";
    public string boxesnSaveName = "Boxes.sav";

    private GameSetting gameSetting;
    private TimeController timeController;
    public List<TestBoxSave> boxLists = new List<TestBoxSave>();

    private void Awake()
    {
        IOHelper.CreateDirectory(SavePath);
    }

    // Use this for initialization
    void Start () {
        gameSetting = GameObject.FindGameObjectWithTag(DoneTags.gameController).GetComponent<GameSetting>();

        timeController = GameObject.FindGameObjectWithTag(DoneTags.gameController).GetComponent<TimeController>();

        if (gameSetting.IsFromLastPlayTime)
        {
            TimeController SaveTimeController = (TimeController)IOHelper.GetData(SavePath + timeControllerSaveName, typeof(TimeController), bIsEcypt);
            timeController.UpdateBySaveFile(SaveTimeController);

            List<TestBoxSave> readBoxList = new List<TestBoxSave>();
            readBoxList=(List<TestBoxSave>)IOHelper.GetData(SavePath + boxesnSaveName, typeof(List<TestBoxSave>), bIsEcypt);
            for (int i=0;i<boxLists.Count;++i)
            {
                boxLists[i].transform.position = readBoxList[i].selfPosition;
                boxLists[i].transform.rotation = readBoxList[i].selfRotation;
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnDisable()
    {   
        //save TimeController
        string filename = SavePath +timeControllerSaveName;
        IOHelper.SetData(filename, timeController,bIsEcypt);

        for (int i = 0; i < boxLists.Count; ++i)
        {
            boxLists[i].UpdateSelfTransform();
        }
        filename = SavePath + boxesnSaveName;
        IOHelper.SetData(filename, boxLists, bIsEcypt);
    }
}
