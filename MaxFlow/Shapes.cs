using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace MaxFlow
{
    public class Shapes
    {
        public class Dot : IShape
        {
            public void Draw(int x, int y, Color color, ref Graphics g)
            {
                g.FillEllipse(new SolidBrush(color), x - 10, y - 10, 25, 25);
            }
        }

        public class BiggerDot : IShape
        {
            public void Draw(int x, int y, Color color, ref Graphics g)
            {
                g.FillEllipse(new SolidBrush(color), x - 15, y - 15, 35, 35);
            }
        }
    }
}
