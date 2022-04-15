using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadSystem 
{
    public string tag;
    public int importance;
    public List<Vector3Int> points;

    public RoadSystem(string tag)
    {
        this.tag = tag;
        points = new List<Vector3Int>();
        importance = 0;
    }

    public void AddPoints(RoadSystem r)
    {

        for(int i = 0; i < r.points.Count; i++)
        {
            
            points.Add(r.points[i]);
        }
        
    }

    public void AddImportance(int i)
    {
        importance += i;
    }


}
