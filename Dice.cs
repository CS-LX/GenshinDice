using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Dice : MonoBehaviour
{
    public List<TransformData> recodeData = new List<TransformData>();
    private bool isRecoding;
    private bool isReplaying;
    private int stateIndex;
    public bool recordEnd = true;
    public GameObject upFace;
    private List<GameObject> faces = new List<GameObject>();
    private TransformData startData = new TransformData();
    public List<Color> eleColors = new List<Color>();
    public ElementType upElement;
    public void Start()
    {
        //记录初始状态用于初始化
        startData.rotation = transform.rotation;
        startData.position = transform.position;
        startData.scale = transform.localScale;
        foreach (Transform t in GetComponentsInChildren<Transform>())//获取六个面
        {
            if (t.name.Contains("F")) faces.Add(t.gameObject);
        }
        初始化();
        //初始化后开始模拟
        //StartRecode();
    }
    public void FixedUpdate()
    {
        if (isRecoding)
        {
            Vector3 position = transform.position;
            Quaternion rotation = transform.rotation;
            Vector3 scale = transform.localScale;
            TransformData transformData = new TransformData
            {
                position = position,
                rotation = rotation,
                scale = scale
            };
            recodeData.Add(transformData);
            //隐藏骰子
            foreach (GameObject gameObject in faces)
            {
                gameObject.GetComponent<MeshRenderer>().enabled = false;
            }
            GetComponent<MeshRenderer>().enabled = false;
            if (GetComponent<Rigidbody>().IsSleeping())//停下来了
            {
                foreach (GameObject gameObject in faces)
                {
                    //gameObject.GetComponent<MeshRenderer>().enabled = true;
                    if (gameObject.TryGetComponent<DiceFace>(out DiceFace face))
                    {
                        if (face.IsUp()) upFace = gameObject;//哪个面朝上
                    }
                }
                EndRecode();
                SetColor();
            }
        }
        if (isReplaying)
        {
            //显示骰子
            GetComponent<MeshRenderer>().enabled = true;//显示骰子
            foreach (GameObject gameObject in faces)
            {
                gameObject.GetComponent<MeshRenderer>().enabled = true;
            }
            if (stateIndex < recodeData.Count)
            {
                transform.position = recodeData[stateIndex].position;
                transform.rotation = recodeData[stateIndex].rotation;
                transform.localScale = recodeData[stateIndex].scale;
                stateIndex++;
            }
            else
            {
                EndPlay();
            }
        }
    }
    public void 初始化()
    {
        isRecoding = false;
        recordEnd = true;
        GetComponent<Rigidbody>().isKinematic = true;
        recodeData.Clear();
        stateIndex = 0;
        upFace = null;
        transform.position = startData.position;
        transform.rotation = startData.rotation;
        transform.localScale = startData.scale;
        foreach (GameObject f in faces)
        {
            f.GetComponent<MeshRenderer>().material.color = Color.white;
        }
        GetComponent<MeshRenderer>().enabled = true;//显示骰子
        foreach (GameObject gameObject in faces)
        {
            gameObject.GetComponent<MeshRenderer>().enabled = true;
        }
    }
    [ContextMenu("开始模拟")]
    public void StartRecode()
    {
        recordEnd = false;
        upFace = null;
        recodeData.Clear();
        GetComponent<Rigidbody>().isKinematic = false;
        isRecoding = true;
    }
    public void EndRecode()
    {
        GetComponent<Rigidbody>().isKinematic = true;
        isRecoding = false;
        recordEnd = true;
    }
    [ContextMenu("开始播放")]
    public void StartPlay()
    {
        GetComponent<Rigidbody>().isKinematic = true;//不模拟
        isRecoding = false;
        isReplaying = true;
    }
    public void EndPlay()
    {
        stateIndex = 0;
        isReplaying = false;
    }
    [ContextMenu("上颜色")]
    public void SetColor()
    {
        /*
        int faceIndex = int.Parse(upFace.name.Replace("F", ""));
        int elemIndex = (int)upElement;
        int delta = faceIndex - elemIndex;
        */
        int elemIndex = (int)upElement;
        if (upFace != null) upFace.GetComponent<Renderer>().material.color = eleColors[elemIndex];
    }
    public void SetElement(ElementType elementType)
    {
        upElement = elementType;
    }
}
[System.Serializable]
public class TransformData
{
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 scale;
}