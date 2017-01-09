using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab3
{
    class Resizer
    {
        int w;
        int h;
        public Resizer(int w, int h)
        {
            this.w = w;
            this.h = h;
        }
        public List<Picture> resize(List<Picture> pictureList)
        {
            foreach (Picture i in pictureList)
            {
                i.width = w;
                i.height = h;
            }
            return pictureList;
        }
    }
}
