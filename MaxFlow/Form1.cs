using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MaxFlow
{
    public partial class Form1 : Form
    {
        #region Form1 attributes
        //zdroj a stok v sieti pictureBox
        private PictureBoxComponents.Node _source;
        private PictureBoxComponents.Node _target;
        //node nad ktorym sa vznasame myskou a ktory tahame
        private PictureBoxComponents.Node _hoverNode;
        private PictureBoxComponents.Node _dragNode;
        //zoznamy uzlov a hran na grafickej ploche
        private List<PictureBoxComponents.Node> _pictureBoxNodes;
        private List<PictureBoxComponents.Arc> _pictureBoxArcs;
        //graficke atributy
        private Graphics _graphics;
        private Bitmap _bitmap;
        //tvary uzlov
        private Shapes.Dot _dot;
        private Shapes.BiggerDot _biggerDot;
        //vytvaranie hran
        private List<PictureBoxComponents.Node> _clickedNodes;
        private List<PictureBoxComponents.Node> _doubleClickedNodes;
        private List<PictureBoxComponents.Node> _edgeCapacityClicked;

        #endregion Form1 attributes

        #region Form1 getters&setters

        public void SetSource(PictureBoxComponents.Node source)
        {
            _source = source;
        }

        public PictureBoxComponents.Node GetSource()
        {
            return _source;
        }

        public void SetTarget(PictureBoxComponents.Node target)
        {
            _target = target;
        }

        public PictureBoxComponents.Node GetTarget()
        {
            return _target;
        }

        public void SetHoverNode(PictureBoxComponents.Node hoverNode)
        {
            _hoverNode = hoverNode;
        }

        public PictureBoxComponents.Node GetHoverNode()
        {
            return _hoverNode;
        }

        public void SetDragNode(PictureBoxComponents.Node dragNode)
        {
            _dragNode = dragNode;
        }

        public PictureBoxComponents.Node GetDragNode()
        {
            return _dragNode;
        }

        public void SetPictureBoxArcs(List<PictureBoxComponents.Arc> pictureBoxArcs)
        {
            _pictureBoxArcs = pictureBoxArcs;
        }

        public List<PictureBoxComponents.Arc> GetPictureBoxArcs()
        {
            return _pictureBoxArcs;
        }

        public void SetPictureBoxNodes(List<PictureBoxComponents.Node> pictureBoxNodes)
        {
            _pictureBoxNodes = pictureBoxNodes;
        }

        public List<PictureBoxComponents.Node> GetPictureBoxNodes()
        {
            return _pictureBoxNodes;
        }

        public void SetGraphics(Graphics graphics)
        {
            _graphics = graphics;
        }

        public Graphics GetGraphics()
        {
            return _graphics;
        }

        public void SetBitmap(Bitmap bitmap)
        {
            _bitmap = bitmap;
        }

        public Bitmap GetBitmap()
        {
            return _bitmap;
        }

        public void SetDot(Shapes.Dot dot)
        {
            _dot = dot;
        }

        public Shapes.Dot GetDot()
        {
            return _dot;
        }

        public void SetBiggerDot(Shapes.BiggerDot biggerDot)
        {
            _biggerDot = biggerDot;
        }

        public Shapes.BiggerDot GetBiggerDot()
        {
            return _biggerDot;
        }

        public void SetDoubleClickedNodes(List<PictureBoxComponents.Node> doubleClickedNodes)
        {
            _doubleClickedNodes = doubleClickedNodes;
        }

        public List<PictureBoxComponents.Node> GetDoubleClickedNodes()
        {
            return _doubleClickedNodes;
        }

        public void SetEdgeCapacityClicked(List<PictureBoxComponents.Node> edgeCapacityClicked)
        {
            _edgeCapacityClicked = edgeCapacityClicked;
        }

        public List<PictureBoxComponents.Node> GetEdgeCapacityClicked()
        {
            return _edgeCapacityClicked;
        }

        #endregion Form1 getters&setters

        public Form1()
        {
            InitializeComponent();
                        
            #region example network

            //vytvorime siet N
            Network N = new Network(NetworkType.normal);
            Network.Node s = new Network.Node("s");
            Network.Node t = new Network.Node("t");
            N.SetSource(s);
            N.SetTarget(t);

            //vytvorime uzly, ktore neskor vlozime do siete N
            Network.Node a = new Network.Node("a");
            Network.Node b = new Network.Node("b");
            Network.Node c = new Network.Node("c");
            Network.Node d = new Network.Node("d");
            Network.Node e = new Network.Node("e");
            Network.Node f = new Network.Node("f");
            Network.Node g = new Network.Node("g");

            //pridame uzly do siete N
            N.AddNode(s);
            N.AddNode(a);
            N.AddNode(b);
            N.AddNode(c);
            N.AddNode(d);
            N.AddNode(e);
            N.AddNode(f);
            N.AddNode(g);
            N.AddNode(t);

            //pridame hrany uzlom a medzi uzlami
            s.AddArc(a, 20);
            s.AddArc(b, 2);
            s.AddArc(c, 2);
            a.AddArc(e, 9);
            a.AddArc(b, 4);
            b.AddArc(d, 1);
            b.AddArc(c, 4);
            c.AddArc(d, 5);
            d.AddArc(g, 8);
            e.AddArc(f, 4);
            e.AddArc(t, 4);
            f.AddArc(b, 3);
            f.AddArc(t, 9);
            g.AddArc(f, 10);
            g.AddArc(t, 2);

            FordFulkerson fordFulkAlgo = new FordFulkerson();
            label2.Text = fordFulkAlgo.FordFulkersonAlgo(N, N.AdjacencyMatrix()).ToString();

            #endregion example network
        }

        #region Form1 methods

        private void Form1_Load(object sender, EventArgs e)
        {
            //Inicializacia
            _bitmap = new Bitmap(pictureBox1.Size.Width, pictureBox1.Size.Height);
            _graphics = (Graphics.FromImage(_bitmap));
            pictureBox1.Image = _bitmap;

            _pictureBoxNodes = new List<PictureBoxComponents.Node>();
            _pictureBoxArcs = new List<PictureBoxComponents.Arc>();
            _clickedNodes = new List<PictureBoxComponents.Node>();
            _doubleClickedNodes = new List<PictureBoxComponents.Node>();
            _edgeCapacityClicked = new List<PictureBoxComponents.Node>();

            _dot = new Shapes.Dot();
            _biggerDot = new Shapes.BiggerDot();

            //prida prvy uzol
            _pictureBoxNodes.Add(new PictureBoxComponents.Node(150, 150, Color.Red));
            textBox2.Text = "working..";
        }

        //overi ci nehoverujeme nad nejakym uzlom
        public PictureBoxComponents.Node HoverCheck(List<PictureBoxComponents.Node> nodes, MouseEventArgs e)
        {
            PictureBoxComponents.Node node = null;
            foreach(var n in nodes)
            {
                if(Math.Sqrt(Math.Pow(e.X - n.GetX(), 2) + Math.Pow(e.Y - n.GetY(), 2)) < n.GetRadius())
                {
                    node = n;
                    break;
                }
            }

            return node;
        }

        //vykresli nodes
        public void DrawNodes(List<PictureBoxComponents.Node> nodes, ref Graphics g)
        {
            foreach (var n in nodes)
            {
                n.Draw(ref g);
            }
        }

        //zafarbi uzol jeho spravnou farbou
        public void ColorNode(PictureBoxComponents.Node node)
        {
            if (node.Equals(GetSource()))
            {
                node.SetColor(Color.Teal);
            }
            else if (node.Equals(GetTarget()))
            {
                node.SetColor(Color.Maroon);
            }
            else
            {
                node.SetColor(Color.Red);
            }
        }

        //vykresli hrany
        public void DrawArcs(List<PictureBoxComponents.Arc> arcs, ref Graphics g)
        {
            foreach(var a in arcs)
            {
                a.Draw(a.GetStartNode(), a.GetEndNode(), ref g);
                ColorNode(a.GetStartNode());
                ColorNode(a.GetEndNode());
            }
            
        }

        //pri pohybe mysky
        private void PictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            PictureBoxComponents.Node n = HoverCheck(_pictureBoxNodes, e);
            if (n != null)
            {
                n.SetShape(new Shapes.BiggerDot());
            }
            else
            {
                if (_hoverNode != null)
                {
                    _hoverNode.SetShape(new Shapes.Dot());
                }
            }

            _hoverNode = n;

            if (_dragNode != null)
            {
                _dragNode.SetX(e.X);
                _dragNode.SetY(e.Y);
            }

            _graphics.Clear(Color.White);

            //vykreslim hrany
            DrawArcs(_pictureBoxArcs, ref _graphics);

            //vykreslim uzly
            DrawNodes(_pictureBoxNodes, ref _graphics);

            pictureBox1.Image = _bitmap;
        }

        //pri stlaceni tlacidla na mysi
        private void PictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            _dragNode = _hoverNode;
        }

        //pri zdvihnuti tlacidla na mysi
        private void PictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            _dragNode = null;
        }

        //tlacidlo "Draw Node" nakresli novy uzol
        private void DrawNode_MouseClick(object sender, MouseEventArgs e)
        {
            _pictureBoxNodes.Add(new PictureBoxComponents.Node(150, 150, Color.Red));
        }

        //vytvaranie hran medzi kliknutymi uzlami
        private void PictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            PictureBoxComponents.Node possibleClickedNode = null;
            if (e.Button == MouseButtons.Right)
            {
                possibleClickedNode = HoverCheck(_pictureBoxNodes, e);
                if (possibleClickedNode != null)
                {
                    //pridame kliknuty uzol do zoznamu
                    _clickedNodes.Add(possibleClickedNode);

                    //ak sme klikli na jedno, tak zafarbime
                    if (_clickedNodes.Count == 1)
                    {
                        _clickedNodes[0].SetColor(Color.Aqua);
                    }

                    //ak sme klikli na dve, rozoberieme pripady
                    if(_clickedNodes.Count == 2)
                    {
                        //ak sme klikli na rovnake, chceme zrusit volbu
                        if (_clickedNodes[0].Equals(_clickedNodes[1]))
                        {
                            //prefarbime uzol na povodnu farbu
                            ColorNode(_clickedNodes[0]);
                        }
                        else
                        {
                            PictureBoxComponents.Arc newArc = new PictureBoxComponents.Arc(_clickedNodes[0], _clickedNodes[1], Color.LightSalmon, 0);
                            _pictureBoxArcs.Add(newArc);
                            _clickedNodes[0].AddArc(newArc);
                            _clickedNodes[1].AddArc(newArc);
                        }
                        //vymazeme zoznam po kazdych 2 kliknutiach
                        _clickedNodes.Clear();
                    }
                }
            }
        }
        
        //oznaci uzol ak sme stlacili na klavesnici a hoverali nad node (source = ctrl + s; target = ctrl + t; vymazanie node-u = delete; pridane kapacity hrany = c;)
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            //farebne oznacime zdroj
            if (e.Control && e.KeyCode == Keys.S)
            {
                if (_hoverNode != null)
                {
                    _source = _hoverNode;
                    _source.SetColor(Color.Teal);
                }
            }
            //farebne oznacime stok
            if (e.Control && e.KeyCode == Keys.T)
            {
                if (_hoverNode != null)
                {
                    _target = _hoverNode;
                    _target.SetColor(Color.Maroon);
                }
            }
            //vymazanie vrcholu (spolu s hranami)
            if (e.KeyCode == Keys.Delete)
            {
                if (_hoverNode != null)
                {
                    //vymazeme hrany smerujuce z/do uzla
                    foreach (var arc in _hoverNode.GetArcs())
                    {
                        _pictureBoxArcs.Remove(arc);
                    }
                    //vymazeme uzol
                    _pictureBoxNodes.Remove(_hoverNode);
                    _hoverNode = null;
                }
            }

            if (e.KeyCode == Keys.C)
            {
                if (_hoverNode != null && _pictureBoxArcs.Count >= 1)
                {
                    _edgeCapacityClicked.Add(_hoverNode);
                    _edgeCapacityClicked[0].SetColor(Color.Lavender);

                    if(_edgeCapacityClicked.Count > 1 && !_edgeCapacityClicked[0].Equals(_edgeCapacityClicked[1]))
                    {
                        //najdi hranu
                        PictureBoxComponents.Arc selectedArc = null;
                        for(int i = 0; i < _pictureBoxArcs.Count; i++)
                        {
                            if((_pictureBoxArcs[i].GetStartNode().Equals(_edgeCapacityClicked[0]) && _pictureBoxArcs[i].GetEndNode().Equals(_edgeCapacityClicked[1])) 
                                || ((_pictureBoxArcs[i].GetStartNode().Equals(_edgeCapacityClicked[1]) && _pictureBoxArcs[i].GetEndNode().Equals(_edgeCapacityClicked[0]))))
                            {
                                selectedArc = _pictureBoxArcs[i];
                                selectedArc.SetColor(Color.Black);
                                break;
                            }
                        }
                        //nastav hrane kapacitu z textboxu 1
                        int capacity;
                        int.TryParse(textBox1.Text, out capacity);
                        selectedArc.SetCapacity(capacity);
                        //vykresli kapacitu nad hranu
                            //!
                        //vrat farbu
                        ColorNode(_edgeCapacityClicked[0]);
                        ColorNode(_edgeCapacityClicked[1]);
                        //! vratit farbu aj hrane po dokonceni nastavenia kapacity
                        //vycisti list
                        _edgeCapacityClicked.Clear();
                    }
                    else if(_edgeCapacityClicked.Count == 2 && _edgeCapacityClicked[0].Equals(_edgeCapacityClicked[1]))
                    {
                        //klikli sme na rovnak - vrat farbu
                        ColorNode(_hoverNode);
                        //vycisti list
                        _edgeCapacityClicked.Clear();
                    }
                }

            }
        }
        #endregion Form1 methods
    }
}
