using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StreetPioneerVoxel : Voxel
{
    public PioneerAgent agent;

    public StreetPioneerVoxel(PioneerAgent p, Vector3Int position) : base(7, position)
    {
        this.agent = p;
    }

    public PioneerAgent GetAgent()
    {
        return agent;
    }

}
