using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public static ObjectManager Instance;

    [Header("Object Settings")]
    public GameObject objPrefab;
    //public int objCount = 20;
    
    private int remainObj;

    private readonly List<GameObject> objs = new List<GameObject>();

    void Awake()
    {
        Instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    /*
    void Awake()
    {
        Instance = this;

        if (objPrefab == null)
        {
            Debug.LogError("No prefab was assigned to ObjectManager!");
            return;
        }

        for (int i = 0; i < objCount; i++)
        {
            GameObject obj = Instantiate(objPrefab, transform);
            obj.SetActive(false);
            objs.Add(obj);
        }

        remainObj = objCount;
    }
    */

    public void InitializePool(int numberOfBall)
    {
        // Xóa pool cũ nếu bắt đầu lại trong cùng scene
        foreach (GameObject obj in objs)
        {
            if (obj != null)
                Destroy(obj);
        }

        objs.Clear();

        for (int i = 0; i < numberOfBall; i++)
        {
            GameObject obj = Instantiate(objPrefab, transform);
            obj.SetActive(false);
            objs.Add(obj);
        }

        remainObj = numberOfBall;
    }

    public GameObject GetObject()
    {
        foreach (GameObject obj in objs)
        {
            if (obj != null && !obj.activeSelf)
            {
                obj.SetActive(true);
                return obj;
            }
        }

        Debug.LogWarning("ObjectManager đã hết Ball!");
        return null;
    }

    public bool AreAllObjectsInactive()
    {
        foreach (GameObject obj in objs)
        {
            if (obj != null && obj.activeInHierarchy)
                return false;
        }

        return true;
    }

    public int GetRemainObj()
    {
        return remainObj;
    }

    public void DecreaseObj()
    {
        if (remainObj <= 0)
            return;

        remainObj--;
        //Debug.Log("Remaining Object: " + remainObj);
    }
}
