using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace MaxFlow
{
   public interface IShape
    {
        void Draw(int x, int y, Color color, ref Graphics g);
    }
}
