using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM_P4
{
    public static class Thresholding
    {
        private const double iris_factor = 1.1;
        private const double pupil_factor = 4.8;
        public static double GetThreshold_SimpleGray(this DirectBitmap bmp, Mode mode)
        {
            double sum = 0;
            int w = bmp.Width, h = bmp.Height, wh = w * h;
            Color c;
            for (int i = 0; i < h; ++i)
                for (int j = 0; j < w; ++j)
                {
                    c = bmp.GetPixel(j, i); 
                    sum += c.R + c.G + c.B;
                }
            return mode == Mode.Iris ? 
                sum / (iris_factor * 3.0 * wh) : 
                sum / (pupil_factor * 3.0 * wh);
        }
        public enum Mode
        {
            Pupil,
            Iris
        }
    }
    
    
}
