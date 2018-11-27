using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxFlow
{
    class FordFulkerson
    {
        //priradi uzlom indexy, obsahuje usporiadane dvojice (x,y), x patri do uzlov, y je indexova mnozina
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

        //zisti ci existuje s-t cesta v rezidualnom grafe
        public bool BFS(Network network, int[,] residualGraph, int[] parent)
        {
            //zoznam navstivenych vrcholov
            bool[] visited = new bool[network.NodeCount()];

            for(int i = 0; i < visited.Length; i++)
            {
                visited[i] = false;
            }

            //najdenie indexov source a target
            int indexSource = AssignSourceIndex(network);
            int indexTarget = AssignTargetIndex(network);
            
            //fronta pre BFS
            Queue<int> q = new Queue<int>();
            q.Enqueue(indexSource);
            parent[indexSource] = -1;
            visited[indexSource] = true;

            //klasicke BFS
            while(q.Count != 0)
            {
                //prvy prvok vo fronte -> u
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

        //vyberie min hodnotu v O(1) z binarnej min haldy (najdenie najmensej rezidualnej kapacity na zlepsujucej s-t ceste)
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

        //pokial existuje zlepsujuca cesta, najde ju a zlepsi ju o epsilon, nasledne prirata ku maximalnemu toku
        public int FordFulkersonAlgo(Network network, int[,] graph)
        {
            int u, v;

            //vytvorime rezidualny graf
            //r(e) = c(e) - f(e), kedze je tok 0, tak je to povodny graf
            int[,] residualGraph = new int[graph.Length, graph.Length];
            for(u = 0; u < network.NodeCount(); u++)
            {
                for(v = 0; v < network.NodeCount(); v++)
                {
                    residualGraph[u,v] = graph[u,v];
                }
            }

            //pole sa naplni BFSkom a bude ukladat cestu
            int[] parent = new int[network.NodeCount()];

            //velkost maximalneho toku
            int maxFlow = 0;
            
            int sourceIndex = AssignSourceIndex(network);
            int targetIndex = AssignTargetIndex(network);

            //pokial budu existovat zlepsovacie cesty
            while(BFS(network, residualGraph, parent))
            {
                //najdenie najmensej rezidualnej kapacity na ceste, na s-t ceste, ktoru sme dostali z BFS
                //binarna min halda
                int pathFlow = ExtractMin(parent, residualGraph, sourceIndex, targetIndex);

                //update toku na ceste a reverse hran
                for (v = targetIndex; v != sourceIndex; v = parent[v])
                {
                    u = parent[v];
                    residualGraph[u, v] -= pathFlow;
                    residualGraph[v, u] += pathFlow;
                }
                
                //pridanie toku do maximalneho toku
                maxFlow += pathFlow;
            }

            return maxFlow;
        }
    }
}
