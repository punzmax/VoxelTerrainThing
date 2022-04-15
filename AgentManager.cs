using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AgentManager 
{
    public VoxelWorld v;
    public int numAgents;
    protected List<Agent> agents;
    protected List<Agent> terminateAgents;

    protected AgentManager(int numAgents, VoxelWorld v)
    {
        this.v = v;
        this.numAgents = numAgents;
        

        agents = new List<Agent>();
        terminateAgents = new List<Agent>();

    }

    public void Step()
    {
        for (int i = 0; i < agents.Count; i++)
        {
            agents[i].Step();
        }

        for (int i = 0; i < terminateAgents.Count; i++)
        {
            agents.Remove(terminateAgents[i]);

        }
        terminateAgents.Clear();
    }


    public void Terminate(Agent a)
    {

        terminateAgents.Add(a);
    }

    public abstract Vector3Int GetValidStartPos();
}
