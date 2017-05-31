using UnityEngine;
using System;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Text;
using System.Security.Cryptography;

public class TestVersion : MonoBehaviour
{
    private class Person
    {
        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        private int age;
        public int Age
        {
            get { return age; }
            set { age = value; }
        }

        public Child myChild;
    }

    private class Child
    {
        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        private int age;
        public int Age
        {
            get { return age; }
            set { age = value; }
        }
    }

    void Start()
    {
        //CheckVersion();
        //TestJsonSave();
    }

    private void Update()
    {
        
    }

    void CheckVersion()
    {
        //TestVersion
        Type type = Type.GetType("Mono.Runtime");
        if (type != null)
        {
            MethodInfo info = type.GetMethod("GetDisplayName", BindingFlags.NonPublic | BindingFlags.Static);

            if (info != null)
                Debug.Log(info.Invoke(null, null));
        }
    }

    void TestJsonSave()
    {
        //Test Json
        Person person = new Person();
        person.Name = "GoldenEasy";
        person.Age = 25;
        Child child = new Child();
        child.Name = "childName";
        child.Age = 3;
        person.myChild = child;
        string strSerializeJSON = JsonConvert.SerializeObject(person);
        Debug.Log(strSerializeJSON);

        Person person2 = JsonConvert.DeserializeObject<Person>(strSerializeJSON);
        Debug.Log(person2.Name);

        //定义存档路径
        string dirpath = "./Save";//= Application.persistentDataPath + "/Save";
        //创建存档文件夹
        IOHelper.CreateDirectory(dirpath);
        //定义存档文件路径
        string filename = dirpath + "/GameData.sav";
        //保存数据
        IOHelper.SetData(filename, person);
        //读取数据
        Person t1 = (Person)IOHelper.GetData(filename, typeof(Person));
        if (t1 is Person)
        {

        }

        Debug.Log(t1.Name);
        Debug.Log(t1.Age);
        Debug.Log(t1.myChild.Name);
    }

}
