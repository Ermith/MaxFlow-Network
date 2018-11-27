using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace MaxFlow
{
    public class PictureBoxComponents
    {
        public class Node
        {
            #region Node attributes

            private int _x;
            private int _y;
            private IShape _shape;
            private int _radius = 20;
            private bool _drag;
            private Color _color;
            private List<Arc> _arcs;

            #endregion Node attributes

            #region Node constructors

            public Node(int x, int y, Color color)
            {
                _x = x;
                _y = y;
                _shape = new Shapes.Dot();
                _drag = false;
                _color = color;
                _arcs = new List<Arc>();
            }

            #endregion Node constructors

            #region Node getters&setters

            public void SetX(int x)
            {
                _x = x;
            }

            public int GetX()
            {
                return _x;
            }

            public void SetY(int y)
            {
                _y = y;
            }

            public int GetY()
            {
                return _y;
            }

            public void SetShape(IShape shape)
            {
                _shape = shape;
            }

            public IShape GetShape()
            {
                return _shape;
            }

            public void SetRadius(int radius)
            {
                _radius = radius;
            }

            public int GetRadius()
            {
                return _radius;
            }

            public void SetDrag(bool drag)
            {
                _drag = drag;
            }

            public bool GetDrag()
            {
                return _drag;
            }

            public void SetColor(Color color)
            {
                _color = color;
            }
            public Color GetColor()
            {
                return _color;
            }

            public void SetArcs(List<Arc> arcs)
            {
                _arcs = arcs;
            }

            public List<Arc> GetArcs()
            {
                return _arcs;
            }

            #endregion Node getters&setters

            #region Node methods

            public void Draw(ref Graphics g)
            {
                _shape.Draw(_x, _y, _color, ref g);
            }

            public void AddArc(Arc arc)
            {
                _arcs.Add(arc);
            }
            #endregion Node methods
        }

        public class Arc
        {
            #region Arc attributes

            private Node _startNode;
            private Node _endNode;
            private Color _color;
            private int _capacity;

            #endregion Arc attributes

            #region Arc constructor

            public Arc(Node startNode, Node endNode, Color color, int capacity)
            {
                _startNode = startNode;
                _endNode = endNode;
                _color = color;
                _capacity = capacity;
            }

            #endregion Arc constructor

            #region Arc getters&setters

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

            public void SetColor(Color color)
            {
                _color = color;
            }

            public Color GetColor()
            {
                return _color;
            }

            public void SetCapacity(int capacity)
            {
                _capacity = capacity;
            }

            public int GetCapacity()
            {
                return _capacity;
            }

            #endregion Arc getters&setters

            #region Arc methods

            public void Draw(Node startNode, Node endNode, ref Graphics g)
            {
                Pen pen = new Pen(_color);
                pen.Width = 6;
                g.DrawLine(pen, startNode.GetX(), startNode.GetY(), endNode.GetX(), endNode.GetY());
            }

            #endregion Arc methods
        }
    }
}
