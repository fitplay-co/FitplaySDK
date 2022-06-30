using System.Collections.Generic;
using UnityEngine;

public class ActionRecognizer
{
    private List<IActionRecon> reconList;

    public ActionRecognizer()
    {
        reconList = new List<IActionRecon>();
    }

    public void OnUpdate(List<Vector3> keyPoints)
    {
        for(int i = 0; i < reconList.Count; i++)
        {
            reconList[i].OnUpdate(keyPoints);
        }
    }

    public void AddRecon(IActionRecon recon)
    {
        reconList.Add(recon);
    }

    public void SetDebug(bool isDebug)
    {
        foreach(var recon in reconList)
        {
            recon.SetDebug(isDebug);
        }
    }
}