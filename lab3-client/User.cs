using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab3_client
{
    class User
    {
        public int countPictures = 0;
        public List<Picture> picturesList = new List<Picture>();
        public void generatePicture(int width, int height)
        {
            Random rand = new Random();
            Picture newPicture = new Picture();
            newPicture.width = width;
            newPicture.height = height;
            picturesList.Add(newPicture);
        }
    }
}
