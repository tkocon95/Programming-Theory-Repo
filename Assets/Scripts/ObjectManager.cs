using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public static ObjectManager Instance;

    [Header("Object Settings")]
    public GameObject objPrefab;
    //public int objCount = 20;
    
    private int remainObj;

    private readonly List<GameObject> objs = new();

    void Awake()
    {
        Instance = this;
    }

    public void InitializePool(int numberOfBall)
    {
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
    }
}
