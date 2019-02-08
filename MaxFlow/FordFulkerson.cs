using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxFlow
{
    class FordFulkerson
    {
        //creates pairs of nodes
        public static Dictionary<Network.Node, int> IndexNodes(Network network)
        {
            Dictionary<Network.Node, int> indexesOfNodes = new Dictionary<Network.Node, int>();

            for(int i = 0; i < network.NodeCount(); i++)
            {
                indexesOfNodes[network.GetNodes()[i]] = i;
            }

            return indexesOfNodes;
        }

        public int AssignSourceIndex(Network network)
        {
            Dictionary<Network.Node, int> indexes = IndexNodes(network);
            return indexes[network.GetSource()];
        }

        public int AssignTargetIndex(Network network)
        {
            Dictionary<Network.Node, int> indexes = IndexNodes(network);
            return indexes[network.GetTarget()];
        }

        //finds if there exists source-target path
        public bool BFS(Network network, int[,] residualGraph, int[] parent)
        {
            bool[] visited = new bool[network.NodeCount()];

            for(int i = 0; i < visited.Length; i++)
            {
                visited[i] = false;
            }
    
            //find source and target
            int indexSource = AssignSourceIndex(network);
            int indexTarget = AssignTargetIndex(network);
            
            Queue<int> q = new Queue<int>();
            q.Enqueue(indexSource);
            parent[indexSource] = -1;
            visited[indexSource] = true;

            //bfs algorhitm
            while(q.Count != 0)
            {
                int u = q.Dequeue();

                for(int v = 0; v < visited.Length; v++)
                {
                    if(visited[v] == false && residualGraph[u,v] > 0)
                    {
                        q.Enqueue(v);
                        parent[v] = u;
                        visited[v] = true;
                    }
                }
            }

            //ak sme narazili do targetu vratime true, inak false
            return visited[indexTarget] == true;
        }

        //finds the min in minHeap structure O(1)
        public static int ExtractMin(int[] parent, int[,] residualGraph, int sourceIndex, int targetIndex)
        {
            MinHeap minHeap = new MinHeap(parent.Length);
            int u, v;
            for (v = targetIndex; v != sourceIndex; v = parent[v])
            {
                u = parent[v];
                minHeap.Add(residualGraph[u,v]);
            }

            return minHeap.Pop();
        }

        //if there exists an augmenting path, we raise flow by epsilon
        public int FordFulkersonAlgo(Network network, int[,] graph)
        {
            int u, v;

            //we make residual graph
            //r(e) = c(e) - f(e), if the flow is 0, then the residual graph equals the original graph
            int[,] residualGraph = new int[graph.Length, graph.Length];
            for(u = 0; u < network.NodeCount(); u++)
            {
                for(v = 0; v < network.NodeCount(); v++)
                {
                    residualGraph[u,v] = graph[u,v];
                }
            }

            //bfs fill the array
            int[] parent = new int[network.NodeCount()];

            int maxFlow = 0;
            
            int sourceIndex = AssignSourceIndex(network);
            int targetIndex = AssignTargetIndex(network);
            
            //if there exists an augmenting path
            while(BFS(network, residualGraph, parent))
            {
                //we find the smallest residual capacity on the source-target path, which we have obtained from the BFS
                int pathFlow = ExtractMin(parent, residualGraph, sourceIndex, targetIndex);

                //update the flow and the backflow
                for (v = targetIndex; v != sourceIndex; v = parent[v])
                {
                    u = parent[v];
                    residualGraph[u, v] -= pathFlow;
                    residualGraph[v, u] += pathFlow;
                }
                
                //we add the flow to maxFlow
                maxFlow += pathFlow;
            }

            return maxFlow;
        }
    }
}
