using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace MaxFlow
{
    enum NetworkType
    {
        normal = 1,
        layered = 2,
        residual = 3
    }

    class Network
    {
        public class Node
        {
            #region attributes

            private string _name;
            private List<Arc> _arcs;
            private int _flowFromNodes;               //sucet tokov prichadzajucich do uzla
            private int _flowToNodes;                 //sucet tokov odchadzajucich z uzla

            #endregion attributes

            #region constructors

            //vytvori nepomenovany uzol bez susedov
            public Node()       
            {
                _name = "";
                _arcs = new List<Arc>();
                _flowFromNodes = 0;
                _flowToNodes = 0;
            }

            //vytvori pomenovany uzol bez susedov
            public Node(string name)
            {
                _name = name;
                _arcs = new List<Arc>();
                _flowFromNodes = 0;
                _flowToNodes = 0;
            }

            #endregion constructors

            #region getters&setters
            public void SetName(string name)
            {
                _name = name;
            }

            public string GetName()
            {
                return _name;
            }

            public void SetArcs(List<Arc> Arcs)
            {
                _arcs = Arcs;
            }
            public List<Arc> GetArcs()
            {
                return _arcs;
            }

            public void SetFlowFromNodes(int flowFromNodes)
            {
                _flowFromNodes = flowFromNodes;
            }

            public int GetFlowFromNodes()
            {
                return _flowFromNodes;
            }
            public void SetFlowToNodes(int flowToNodes)
            {
                _flowToNodes = flowToNodes;
            }

            public int GetFlowToNodes()
            {
                return _flowToNodes;
            }
            #endregion getters&setters

            #region methods
            //prida hranu medzi aktualnym uzlom a endNode s kapacitou capacity
            public void AddArc(Node endNode, int capacity)
            {
                _arcs.Add(new Arc(this, endNode, capacity));   
            }

            //prida hranu medzi aktualnym uzlom a endNode s kapacitou capacity a velkostou toku flow
            public void AddArc(Node endNode, int capacity, int flow)
            {
                _arcs.Add(new Arc(this, endNode, capacity, flow));

                //potrebujeme upravit toky kvoli kirchoffovemu zakonu
                UpdateFlows(this, endNode, flow);
            }

            //aktualizacia tokov - kirchoffov zakon
            public void UpdateFlows(Node startNode, Node endNode, int flow)
            {
                //aktualizovat velkost pritoku do koncoveho uzla
                endNode.SetFlowFromNodes(GetFlowFromNodes() + flow);
                //aktualizovat velkost odtoku zo startovacieho uzla
                startNode.SetFlowToNodes(GetFlowToNodes() + flow);
            }

            #endregion methods

        }

        public class Arc
        {
            #region attributes

            private Node _startNode;
            private Node _endNode;
            private int _capacity;
            private int _flow;
            private int _residualCapacity;

            #endregion attributes

            #region constructors
            //constructor

            //vytvori hranu medzi vrcholmi startNode a endNode s kapacitou capacity
            public Arc(Node startNode, Node endNode, int capacity)
            {
                SetStartNode(startNode);
                SetEndNode(endNode);
                SetCapacity(capacity);
            }

            //vytvori hranu medzi vrcholmi startNode a endNode s kapacitou, tokom
            public Arc(Node startNode, Node endNode, int capacity, int flow)
            {
                SetStartNode(startNode);
                SetEndNode(endNode);
                SetCapacity(capacity);
                SetFlow(flow);
            }

            #endregion constructors

            #region getters&setters

            public void SetStartNode(Node startNode)
            {
                _startNode = startNode;
            }

            public Node GetStartNode()
            {
                return _startNode;
            }

            public void SetEndNode(Node endNode)
            {
                _endNode = endNode;
            }
            public Node GetEndNode()
            {
                return _endNode;
            }
            public void SetCapacity(int capacity)
            {
                _capacity = capacity;
            }

            public int GetCapacity()
            {
                return _capacity;
            }

            public void SetFlow(int flow)
            {
                _flow = flow;
            }

            public int GetFlow()
            {
                return _flow;
            }

            public void SetResidualCapacity(int residualCapacity)
            {
                _residualCapacity = residualCapacity;
            }

            public int GetResidualCapacity()
            {
                return _residualCapacity;
            }

            #endregion getters&setters

            #region methods



            #endregion methods
        }

        #region network attributes

        private NetworkType _networkType;
        private List<Node> _nodes;
        private List<Arc> _arcs;
        private Node _source;
        private Node _target;

        #endregion network attributes

        #region network constructors

        //vytvori vybrany typ siete, siet je prazdna
        public Network(NetworkType networkType)
        {
            SetNetworkType(networkType);
            _nodes = new List<Node>();
            _arcs = new List<Arc>();
        }

        #endregion network constructors

        #region network getters&setters

        public void SetNetworkType(NetworkType networkType)
        {
            _networkType = networkType;
        }

        public NetworkType GetNetworkType()
        {
            return _networkType;
        }

        public void SetNodes(List<Node> nodes)
        {
            _nodes = nodes;
        }

        public List<Node> GetNodes()
        {
            return _nodes;
        }

        public void SetArcs(List<Arc> arcs)
        {
            _arcs = arcs;
        }

        public List<Arc> GetArcs()
        {
            return _arcs;
        }

        public void SetSource(Node source)
        {
            _source = source;
        }

        public Node GetSource()
        {
            return _source;
        }

        public void SetTarget(Node target)
        {
            _target = target;
        }

        public Node GetTarget()
        {
            return _target;
        }
        #endregion network getters&setters

        #region network methods

        public void AddNode(Node n)
        {
            _nodes.Add(n);
        }

        public void AddArc(Arc a)
        {
            _arcs.Add(a);
        }

        public int NodeCount()
        {
            return _nodes.Count();
        }

        //overi platnost kirchoffovho zakona (z kazdeho uzla priteka a odteka rovnake mnozstvo)
        public bool CheckKirchoffsLaw()
        {
            foreach(var n in _nodes)
            {
                Tuple<int, int> flows = CountFlowInNode(n);
                if(flows.Item1 != flows.Item2)
                {
                    return false;
                }
            }
            return true;
        }

        //scita vsetky prichadzajuce, odchadzajuce toky vo vrchole
        public Tuple<int, int> CountFlowInNode(Node node)
        {
            int sumIn = 0, sumOut = 0;
            foreach(var n in node.GetArcs())
            {
                if (node.Equals(n.GetEndNode()))
                {
                    sumIn += n.GetFlow();
                }
                else
                {
                    sumOut += n.GetFlow();
                }
            }
            return Tuple.Create(sumIn, sumOut);
        }

        //scita odtok v zdroji
        public int FlowOutOfSource(Node source)
        {
            int sum = 0;
            foreach(var n in source.GetArcs())
            {
                if (source.Equals(n.GetStartNode()))
                {
                    sum += n.GetFlow();
                }
            }
            return sum;
        }

        //scita pritok v stoku
        public int FlowToTarget(Node target)
        {
            int sum = 0;
            foreach(var n in target.GetArcs())
            {
                if (target.Equals(n.GetEndNode()))
                {
                    sum += n.GetFlow();
                }
            }
            return sum;
        }
        
        //vytvori maticu susednosti
        public int[,] AdjacencyMatrix()
        {
            int[,] matrix = new int[NodeCount(), NodeCount()];

            //inicializacia matice na 0
            for (int i = 0; i < NodeCount(); i++)
            {
                for (int j = 0; j < NodeCount(); j++)
                {
                    matrix[i, j] = 0;
                }
            }

            //ak medzi vrcholmi i,j existuje cesta s tokom na hrane, tak matrix[i,j] bude obsahovat velkost toku
            for(int i = 0; i < NodeCount(); i++)
            {
                Node actualNode = _nodes[i];
                for(int j = 0; j < NodeCount(); j++)
                {
                    Node neighbourNode = _nodes[j];
                    foreach (var n in actualNode.GetArcs())
                    {
                        if (n.GetStartNode().Equals(actualNode) && n.GetEndNode().Equals(neighbourNode))
                        {
                            matrix[i, j] = n.GetCapacity();
                        }
                    }
                }
            }

            return matrix;
        }

        public void BuildNetworkFromPictureBox(LinkedList<PictureBoxComponents.Node> pictureBoxNodes, LinkedList<PictureBoxComponents.Arc> pictureBoxArcs)
        {
            Dictionary<PictureBoxComponents.Node, Node> pairsOfNodes = new Dictionary<PictureBoxComponents.Node, Node>();
            //rozparsujeme uzly do siete
            foreach(var node in pictureBoxNodes)
            {
                Node newNode = new Node();
                _nodes.Add(newNode);

                //tento uzol je source
                if (node.GetColor().Equals(Color.Teal))
                {
                    _source = newNode;
                }

                //tento uzol je target
                if (node.GetColor().Equals(Color.Maroon))
                {
                    _target = newNode;
                }

                //vytvorim usporiadanu dvojicu <pictureboxnode, networknode>
                pairsOfNodes[node] = newNode;
            }

            //rozparsujeme hrany do siete
            foreach(var arc in pictureBoxArcs)
            {
                Node startNode = new Node();
                Node endNode = new Node();

                //zistime ktore uzly su pociatocny a koncovy
                foreach(var node in pictureBoxNodes)
                {
                    if (arc.GetStartNode().Equals(node))
                    {
                        startNode = pairsOfNodes[node];
                    }
                    if (arc.GetEndNode().Equals(node))
                    {
                        endNode = pairsOfNodes[node];
                    }
                }

                _arcs.Add(new Arc(startNode, endNode, arc.GetCapacity()));
            }

            //zadame hrany uzlom v sieti
            foreach(var arc in _arcs)
            {
                foreach(var node in _nodes)
                {
                    //ak sme nasli medzi uzlami rovnaky aky ma pociatocny, tak pridame uzlu hranu vychadzajucu z neho
                    if (arc.GetStartNode().Equals(node))
                    {
                        node.AddArc(arc.GetEndNode(), arc.GetCapacity());
                    }
                }
            }
        }
        
        #endregion network methods
    }
}
