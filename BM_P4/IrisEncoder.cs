using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Numerics;
using static BM_P4.Thresholding;

namespace BM_P4
{
    public partial class IrisEncoder : Form
    {
        DirectBitmap source = new DirectBitmap(Properties.Resources.Img_078_L_5_1);
        DirectBitmap binarized_iris_source;
        DirectBitmap binarized_pupil_source;
        DirectBitmap binarized_normalized_source;


        DirectBitmap target = new DirectBitmap(Properties.Resources.Img_078_L_5_1);
        DirectBitmap binarized_iris_target;
        DirectBitmap binarized_pupil_target;
        DirectBitmap binarized_normalized_target;

        DirectBitmap tmp;


        int x_center_source, y_center_source,
            x_center_target, y_center_target;
        int pupil_radius_source, iris_radius_source,
            pupil_radius_target, iris_radius_target;

        List<bool[]> code_target;
        List<bool[]> code_source;
        const int points_from_circle = 256;

        public IrisEncoder()
        {
            InitializeComponent();
            SourcePBox.Image?.Dispose();
            MainPBox.Image?.Dispose();

            SourcePBox.Image = new Bitmap(source.Bitmap);
            MainPBox.Image = new Bitmap(target.Bitmap);
        }
        #region Source
        public void LoadSourceImageButton_Click(object sender, EventArgs e)
        {
            LoadImage(Image.Source);
            code_source?.Clear();
            code_source = null;
            SourceProgressBar.Value = 0;
        }
        private void ComputeSourceButton_Click(object sender, EventArgs e)
        {
            AutoMode(Image.Source);
        }
        private void DrawRingsSourceButton_Click(object sender, EventArgs e)
        {

            x_center_source = (int)x_cen_source.Value;
            y_center_source = (int)y_cen_source.Value;
            var circle_image = new Bitmap(source.Bitmap);
            pupil_radius_source = (int)r_pupil_source.Value;
            iris_radius_source = (int)r_iris_source.Value;
            using (var drawer = Graphics.FromImage(circle_image))
            {
                drawer.DrawEllipse(new Pen(Color.Yellow, 1f),
                    x_center_source - pupil_radius_source,
                    y_center_source - pupil_radius_source,
                    2 * pupil_radius_source, 2 * pupil_radius_source);
                drawer.DrawEllipse(new Pen(Color.Yellow, 1f),
                    x_center_source - iris_radius_source,
                    y_center_source - iris_radius_source,
                    2 * iris_radius_source, 2 * iris_radius_source);
            }
            SourcePBox.Image = circle_image;
        }
        private void ComputeManuallySourceButton_Click(object sender, EventArgs e)
        {
            SourceProgressBar.Value = 0;
            x_center_source = (int)x_cen_source.Value;
            y_center_source = (int)y_cen_source.Value;
            pupil_radius_source = (int)r_pupil_source.Value;
            iris_radius_source = (int)r_iris_source.Value;
            if (pupil_radius_source >= iris_radius_source)
                return;
            try
            {
                ComputeCode(Image.Source);
            }
            catch (IndexOutOfRangeException)
            {
                MessageBox.Show("Image boundaries exceeded, try manual approach with another data set");
                code_source?.Clear();
                code_source = null;
                SourceProgressBar.Value = 0;
                return;
            }
            SourceProgressBar.Value = 100;
            DrawRingsSourceButton_Click(null, null);
            
        }
        #endregion Source

        #region Target
        private void CompareButton_Click(object sender, EventArgs e)
        {
            ComputeManuallyTargetButton_Click(null, null);
            ComputeManuallySourceButton_Click(null, null);
            CompareCodes();
        }
        private void LoadImageTargetButton_Click(object sender, EventArgs e)
        {
            LoadImage(Image.Target);
            code_target?.Clear();
            code_target = null;
            TargetProgressBar.Value = 0;
        }
        private void ComputeTargetButton_Click(object sender, EventArgs e)
        {
            AutoMode(Image.Target);
        }
        private void DrawRingsTargetButton_Click(object sender, EventArgs e)
        {
            x_center_target = (int)x_cen_target.Value;
            y_center_target = (int)y_cen_target.Value;
            var circle_image = new Bitmap(target.Bitmap);
            pupil_radius_target = (int)r_pupil_target.Value;
            iris_radius_target = (int)r_iris_target.Value;

            using (var drawer = Graphics.FromImage(circle_image))
            {
                drawer.DrawEllipse(new Pen(Color.Red, 1f),
                    x_center_target - pupil_radius_target,
                    y_center_target - pupil_radius_target,
                    2 * pupil_radius_target, 2 * pupil_radius_target);
                drawer.DrawEllipse(new Pen(Color.Red, 1f),
                    x_center_target - iris_radius_target,
                    y_center_target - iris_radius_target,
                    2 * iris_radius_target, 2 * iris_radius_target);
            }
            MainPBox.Image = circle_image;
        }
        private void ComputeManuallyTargetButton_Click(object sender, EventArgs e)
        {
            TargetProgressBar.Value = 0;
            x_center_target = (int)x_cen_target.Value;
            y_center_target = (int)y_cen_target.Value;
            pupil_radius_target = (int)r_pupil_target.Value;
            iris_radius_target = (int)r_iris_target.Value;
            if (pupil_radius_target >= iris_radius_target)
                return;
            try
            {
                ComputeCode(Image.Target);
            }
            catch (IndexOutOfRangeException)
            {
                MessageBox.Show("Image boundaries exceeded!\nTry manual approach with another data set");
                code_target?.Clear();
                code_target = null;
                TargetProgressBar.Value = 0;
                return;
            }
            TargetProgressBar.Value = 100;
            DrawRingsTargetButton_Click(null, null);
        }
        #endregion Target
        /// <summary>
        /// Creates image representation of iris code and shows similarity(%) to user
        /// </summary>
        public void CompareCodes()
        {
            if (code_source == null || code_target == null)
                return;
            
            var source_codeImage = new DirectBitmap(code_source[0].Length, 8);
            for (int j = 0; j < 8; ++j)
                for (int i = 0; i < code_source[0].Length; ++i)
                    if (code_source[j][i])
                        source_codeImage.SetPixel(i, j, Color.White);
                    else
                        source_codeImage.SetPixel(i, j, Color.Black);
            SourcePBox.Image?.Dispose();
            SourcePBox.Image = new Bitmap(source_codeImage.Bitmap);
            

            var target_codeImage = new DirectBitmap(code_target[0].Length, 8);
            for (int j = 0; j < 8; ++j)
                for (int i = 0; i < code_target[0].Length; ++i)
                    if (code_source[j][i])
                        target_codeImage.SetPixel(i, j, Color.White);
                    else
                        target_codeImage.SetPixel(i, j, Color.Black);

            MainPBox.Image?.Dispose();
            

            float same_count = 0;
            float count = 0;
            for (int i = 0; i < 8; ++i)
            {
                for (int j = 0; j < code_source[0].Length; ++j)
                {
                    ++count;
                    if (code_source[i][j] == code_target[i][j])
                        ++same_count;
                    else
                        target_codeImage.SetPixel(j, i, Color.Red);
                }
            }
            MainPBox.Image = new Bitmap(target_codeImage.Bitmap);
            source_codeImage?.Dispose();
            target_codeImage?.Dispose();

            MessageBox.Show("Similarity factor = " + (100 * same_count / count).ToString() + '%');
        }
        /// <summary>
        /// Works within the ring limited by points <paramref name="start"/> and <paramref name="end"/> of image <paramref name="b"/>. Computes average light/pixel
        /// </summary>
        /// <param name="line_index">Which ring this computation represents - for 0-3 we count 360 degrees, for 4-7 180 degrees but</param>
        /// <param name="start">Inner radius of ring</param>
        /// <param name="end">Outer radius of ring</param>
        /// <param name="b">Image of eye</param>
        /// <param name="img">Mode of computing, either source or target</param>
        /// <returns></returns>
        private double[] AverageCircleIntensity(int line_index, int start, int end, DirectBitmap b, Image img) 
        {
            int x_center=0, y_center=0;
            switch (img)
            {
                case Image.Source:{
                        x_center = x_center_source;
                        y_center = y_center_source;
                        break;
                    }
                case Image.Target:{
                        x_center = x_center_target;
                        y_center = y_center_target;
                        break;
                    }

            }
            double angular_delta = (360.0/points_from_circle)*(Math.PI/180);
            double[] intensity_array = new double[points_from_circle];

            if(line_index < 4)
            {
                int i = 0;
                for(double angle = 0; i<points_from_circle; angle += angular_delta)
                {
                    int tmp_count = 0;
                    double intensity = 0;
                    for(int radius = start; radius < end; ++radius)
                    {
                        
                        Color c = b.GetPixel((int)(x_center + radius * Math.Cos(angle)), (int)(y_center - radius * Math.Sin(angle)));
                        if (c != Color.White)
                        {
                            intensity = intensity + c.R + c.G + c.B;
                            ++tmp_count;

                        }
                    }
                    intensity_array[i++] = intensity / (3 * tmp_count);

                }
            }
            else
            {
                int i = 0;
                for (double angle = 0; i<points_from_circle; angle += angular_delta/2)
                {
                    if ((angle > Math.PI / 4 && angle < 3 * Math.PI / 4) || (angle > 5 * Math.PI / 4 && angle < 7 * Math.PI / 4))
                        continue;
                    int tmp_count = 0;
                    double intensity = 0;
                    for (int radius = start; radius < end; ++radius)
                    {
                        Color c = b.GetPixel((int)(x_center + radius * Math.Cos(angle)), (int)(y_center - radius * Math.Sin(angle)));
                        if (c != Color.White)
                        {
                            intensity = intensity + c.R + c.G + c.B;
                            
                            ++tmp_count;
                        }
                    }
                    intensity_array[i] = intensity / (3 * tmp_count);
                    ++i;
                }
            }
            return intensity_array;
        }
        /// <summary>
        /// Returns array of true/false, which depends on real and imaginary part of Gabor's wavelets.
        /// </summary>
        /// <param name="intensity_array"></param>
        /// <param name="frequency"></param>
        /// <param name="lambda"></param>
        /// <returns></returns>
        private bool[] Encode(double[] intensity_array, double frequency, double lambda)
        {
            var sector_1 = 0;
            var sector_2 = 0;
            var sector_3 = 0;
            var sector_4 = 0;

            bool[] code_array = new bool[intensity_array.Length * 2];
            double sigma = 0.5 * Math.PI * frequency;
            for(int i = 0; i < intensity_array.Length; ++i)
            {
                Complex c = new Complex(0, 0);
                for(int j = 0; j < intensity_array.Length; ++j)
                {
                    double delta_x = lambda * (i - j) / (intensity_array.Length);
                    c += intensity_array[j] * Math.Exp(-Math.Pow(delta_x / sigma, 2)) * Complex.Exp(new Complex(0, -2 * Math.PI * frequency * delta_x));

                }
                code_array[2 * i] = c.Real > 0.0;
                code_array[2 * i + 1] = c.Imaginary > 0.0;

                if (c.Imaginary >= 0 && c.Real >= 0)
                    ++sector_1;
                if (c.Imaginary >= 0 && c.Real < 0)
                    ++sector_2;
                if (c.Imaginary < 0 && c.Real < 0)
                    ++sector_3;
                if (c.Imaginary < 0 && c.Real >= 0)
                    ++sector_4;
            }
            return code_array;
        }
        private void Dilation(Mode mode, Image img)
        {
            if (img == Image.Source)
            {
                int w = source.Bitmap.Width,
                    h = source.Bitmap.Height;
                if (mode == Mode.Iris)
                {
                    tmp = new DirectBitmap(binarized_iris_source.Bitmap);
                    Parallel.For(2, w - 2, i =>
                    {
                        for (int j = 2; j < h - 2; ++j)
                            if (tmp.GetPixel(i, j).R > 0)
                            {
                                bool anyBlack = false;
                                for (int k = -2; k <= 2; ++k)
                                {
                                    if (anyBlack)
                                        break;
                                    for (int l = -2; l <= 2; ++l)
                                        if (tmp.GetPixel(i + k, j + l).R == 0)
                                        {
                                            anyBlack = true;
                                            break;
                                        }
                                }
                                if (anyBlack)
                                    binarized_iris_source.SetPixel(i, j, Color.Black);
                            }
                    });
                }
                else
                {
                    tmp = new DirectBitmap(binarized_pupil_source.Bitmap);
                    Parallel.For(2, w - 2, i => 
                    {
                        for (int j = 2; j < h - 2; ++j)
                            if (tmp.GetPixel(i, j).R > 0)
                            {
                                bool anyBlack = false;
                                for (int k = -2; k <= 2; ++k)
                                {
                                    if (anyBlack)
                                        break;
                                    for (int l = -2; l <= 2; ++l)
                                        if (tmp.GetPixel(i + k, j + l).R == 0)
                                        {
                                            anyBlack = true;
                                            break;
                                        }
                                }
                                if (anyBlack)
                                    binarized_pupil_source.SetPixel(i, j, Color.Black);
                            }
                    });
                }
            }
            else //if img==Image.Target
            {
                int w = target.Bitmap.Width,
                    h = target.Bitmap.Height;
                if (mode == Mode.Iris)
                {
                    tmp = new DirectBitmap(binarized_iris_target.Bitmap);
                    Parallel.For(2, w - 2, i =>
                    {
                        for (int j = 2; j < h - 2; ++j)
                            if (tmp.GetPixel(i, j).R > 0)
                            {
                                bool anyBlack = false;
                                for (int k = -2; k <= 2; ++k)
                                {
                                    if (anyBlack)
                                        break;
                                    for (int l = -2; l <= 2; ++l)
                                        if (tmp.GetPixel(i + k, j + l).R == 0)
                                        {
                                            anyBlack = true;
                                            break;
                                        }
                                }
                                if (anyBlack)
                                    binarized_iris_target.SetPixel(i, j, Color.Black);
                            }
                    });
                }
                else
                {
                    tmp = new DirectBitmap(binarized_pupil_target.Bitmap);
                    Parallel.For(2, w - 2, i =>
                    {
                        for (int j = 2; j < h - 2; ++j)
                            if (tmp.GetPixel(i, j).R > 0)
                            {
                                bool anyBlack = false;
                                for (int k = -2; k <= 2; ++k)
                                {
                                    if (anyBlack)
                                        break;
                                    for (int l = -2; l <= 2; ++l)
                                        if (tmp.GetPixel(i + k, j + l).R == 0)
                                        {
                                            anyBlack = true;
                                            break;
                                        }
                                }
                                if (anyBlack)
                                    binarized_pupil_target.SetPixel(i, j, Color.Black);
                            }
                    });
                }
            }
            tmp?.Dispose();
        }
        private void Erosion(Mode mode, Image img)
        {
            if (img == Image.Source)
            {
                int w = source.Bitmap.Width,
                    h = source.Bitmap.Height;
                if (mode == Mode.Iris)
                {
                    if (binarized_iris_source == null)
                        return;
                    tmp = new DirectBitmap(binarized_iris_source.Bitmap);

                    for (int j = 0; j < 2; ++j)
                        for (int i = 0; i < w; ++i)
                            binarized_iris_source.SetPixel(i, j, Color.White);
                    for (int j = h - 2; j < h; ++j)
                        for (int i = 0; i < w; ++i)
                            binarized_iris_source.SetPixel(i, j, Color.White);
                    for (int i = 0; i < 2; ++i)
                        for (int j = 0; j < h; ++j)
                            binarized_iris_source.SetPixel(i, j, Color.White);
                    for (int i = w - 2; i < w; ++i)
                        for (int j = 0; j < h; ++j)
                            binarized_iris_source.SetPixel(i, j, Color.White);

                    Parallel.For(2, w - 2, i =>
                    {
                        for (int j = 2; j < h - 2; ++j)
                            if (tmp.GetPixel(i, j).R == 0)
                            {
                                bool notAllBlack = false;
                                for (int k = -2; k <= 2; ++k)
                                {
                                    if (notAllBlack)
                                        break;
                                    for (int l = -2; l <= 2; ++l)
                                        if (tmp.GetPixel(i + k, j + l).R > 0)
                                        {
                                            notAllBlack = true;
                                            break;
                                        }
                                }
                                if (notAllBlack)
                                    binarized_iris_source.SetPixel(i, j, Color.White);
                            }
                    });
                }
                else
                {
                    if (binarized_pupil_source == null)
                        return;
                    tmp = new DirectBitmap(binarized_pupil_source.Bitmap);

                    for (int j = 0; j < 2; ++j)
                        for (int i = 0; i < w; ++i)
                            binarized_pupil_source.SetPixel(i, j, Color.White);
                    for (int j = h - 2; j < h; ++j)
                        for (int i = 0; i < w; ++i)
                            binarized_pupil_source.SetPixel(i, j, Color.White);
                    for (int i = 0; i < 2; ++i)
                        for (int j = 0; j < h; ++j)
                            binarized_pupil_source.SetPixel(i, j, Color.White);
                    for (int i = w - 2; i < w; ++i)
                        for (int j = 0; j < h; ++j)
                            binarized_pupil_source.SetPixel(i, j, Color.White);


                    Parallel.For(2, w - 2, i =>
                    {
                        for (int j = 2; j < h - 2; ++j)
                            if (tmp.GetPixel(i, j).R == 0)
                            {
                                bool notAllBlack = false;
                                for (int k = -2; k <= 2; ++k)
                                {
                                    if (notAllBlack)
                                        break;
                                    for (int l = -2; l <= 2; ++l)
                                        if (tmp.GetPixel(i + k, j + l).R > 0)
                                        {
                                            notAllBlack = true;
                                            break;
                                        }
                                }
                                if (notAllBlack)
                                    binarized_pupil_source.SetPixel(i, j, Color.White);

                            }
                    });
                }
            }
            else
            {
                int w = target.Bitmap.Width,
                   h = target.Bitmap.Height;
                if (mode == Mode.Iris)
                {
                    if (binarized_iris_target == null)
                        return;
                    tmp = new DirectBitmap(binarized_iris_target.Bitmap);

                    for (int j = 0; j < 2; ++j)
                        for (int i = 0; i < w; ++i)
                            binarized_iris_target.SetPixel(i, j, Color.White);
                    for (int j = h - 2; j < h; ++j)
                        for (int i = 0; i < w; ++i)
                            binarized_iris_target.SetPixel(i, j, Color.White);
                    for (int i = 0; i < 2; ++i)
                        for (int j = 0; j < h; ++j)
                            binarized_iris_target.SetPixel(i, j, Color.White);
                    for (int i = w - 2; i < w; ++i)
                        for (int j = 0; j < h; ++j)
                            binarized_iris_target.SetPixel(i, j, Color.White);

                    Parallel.For(2, w - 2, i =>
                    {
                        for (int j = 2; j < h - 2; ++j)
                            if (tmp.GetPixel(i, j).R == 0)
                            {
                                bool notAllBlack = false;
                                for (int k = -2; k <= 2; ++k)
                                {
                                    if (notAllBlack)
                                        break;
                                    for (int l = -2; l <= 2; ++l)
                                        if (tmp.GetPixel(i + k, j + l).R > 0)
                                        {
                                            notAllBlack = true;
                                            break;
                                        }
                                }
                                if (notAllBlack)
                                    binarized_iris_target.SetPixel(i, j, Color.White);
                            }
                    });
                }
                else
                {
                    if (binarized_pupil_target == null)
                        return;
                    tmp = new DirectBitmap(binarized_pupil_target.Bitmap);

                    for (int j = 0; j < 2; ++j)
                        for (int i = 0; i < w; ++i)
                            binarized_pupil_target.SetPixel(i, j, Color.White);
                    for (int j = h - 2; j < h; ++j)
                        for (int i = 0; i < w; ++i)
                            binarized_pupil_target.SetPixel(i, j, Color.White);
                    for (int i = 0; i < 2; ++i)
                        for (int j = 0; j < h; ++j)
                            binarized_pupil_target.SetPixel(i, j, Color.White);
                    for (int i = w - 2; i < w; ++i)
                        for (int j = 0; j < h; ++j)
                            binarized_pupil_target.SetPixel(i, j, Color.White);


                    Parallel.For(2, w - 2, i =>
                    {
                        for (int j = 2; j < h - 2; ++j)
                            if (tmp.GetPixel(i, j).R == 0)
                            {
                                bool notAllBlack = false;
                                for (int k = -2; k <= 2; ++k)
                                {
                                    if (notAllBlack)
                                        break;
                                    for (int l = -2; l <= 2; ++l)
                                        if (tmp.GetPixel(i + k, j + l).R > 0)
                                        {
                                            notAllBlack = true;
                                            break;
                                        }
                                }
                                if (notAllBlack)
                                    binarized_pupil_target.SetPixel(i, j, Color.White);

                            }
                    });
                }
            }
            tmp?.Dispose();
        }
        private void Normalize(Image img)
        {
            if (img == Image.Source)
            {
                binarized_normalized_source?.Dispose();
                binarized_normalized_source = new DirectBitmap(source.Bitmap);
                int w = source.Bitmap.Width;
                int h = source.Bitmap.Height;

                int[] freqTab = new int[256];
                for (int i = 0; i < h; ++i)
                    for (int j = 0; j < w; ++j)
                    {
                        var color = source.GetPixel(j, i);
                        ++freqTab[(color.R + color.G + color.B) / 3];
                    }

                var cdf = new int[256];
                int total = 0;
                int minCDF = -1;
                for (int i = 0; i < 256; ++i)
                {
                    total += freqTab[i];
                    cdf[i] = total;
                }
                for (int i = 0; i < 256; ++i)
                {
                    if (cdf[i] > 0)
                    {
                        minCDF = cdf[i];
                        break;
                    }
                }
                Parallel.For(0, h, i =>
                {
                    for (int j = 0; j < w; ++j)
                    {
                        var oldColor = source.GetPixel(j, i);
                        int newColor = (int)Math.Round((cdf[(oldColor.R + oldColor.G + oldColor.B) / 3] - minCDF) / ((w * h - minCDF) / 255.0));
                        binarized_normalized_source.SetPixel(j, i, Color.FromArgb(newColor, newColor, newColor));

                    }
                });
            }
            else //img==Image.Target
            {
                binarized_normalized_target?.Dispose();
                binarized_normalized_target = new DirectBitmap(target.Bitmap);
                int w = target.Bitmap.Width;
                int h = target.Bitmap.Height;

                int[] freqTab = new int[256];
                for (int i = 0; i < h; ++i)
                    for (int j = 0; j < w; ++j)
                    {
                        var color = target.GetPixel(j, i);
                        ++freqTab[(color.R + color.G + color.B) / 3];
                    }

                var cdf = new int[256];
                int total = 0;
                int minCDF = -1;
                for (int i = 0; i < 256; ++i)
                {
                    total += freqTab[i];
                    cdf[i] = total;
                }
                for (int i = 0; i < 256; ++i)
                {
                    if (cdf[i] > 0)
                    {
                        minCDF = cdf[i];
                        break;
                    }
                }
                Parallel.For(0, h, i =>
                {
                    for (int j = 0; j < w; ++j)
                    {
                        var oldColor = target.GetPixel(j, i);
                        int newColor = (int)Math.Round((cdf[(oldColor.R + oldColor.G + oldColor.B) / 3] - minCDF) / ((w * h - minCDF) / 255.0));
                        binarized_normalized_target.SetPixel(j, i, Color.FromArgb(newColor, newColor, newColor));

                    }
                });
            }
            
        }
        private void BinarizePupil(Image img)
        {
            if(img == Image.Source)
            {
                int w = source.Bitmap.Width,
                    h = source.Bitmap.Height;
                double threshold = Thresholding.GetThreshold_SimpleGray(binarized_normalized_source, Mode.Pupil);
                binarized_pupil_source = new DirectBitmap(source.Bitmap);
                for (int i = 0; i < h; ++i)
                    for (int j = 0; j < w; ++j)
                    {
                        Color c = binarized_normalized_source.GetPixel(j, i);
                        if ((c.R + c.G + c.B) / 3.0 > threshold)
                            binarized_pupil_source.SetPixel(j, i, Color.FromArgb(255, 255, 255));
                        else
                            binarized_pupil_source.SetPixel(j, i, Color.FromArgb(0, 0, 0));
                    }
            }
            else //TARGET
            {
                int w = target.Bitmap.Width,
                    h = target.Bitmap.Height;
                double threshold = Thresholding.GetThreshold_SimpleGray(binarized_normalized_target, Mode.Pupil);
                binarized_pupil_target = new DirectBitmap(target.Bitmap);
                for (int i = 0; i < h; ++i)
                    for (int j = 0; j < w; ++j)
                    {
                        Color c = binarized_normalized_target.GetPixel(j, i);
                        if ((c.R + c.G + c.B) / 3.0 > threshold)
                            binarized_pupil_target.SetPixel(j, i, Color.FromArgb(255, 255, 255));
                        else
                            binarized_pupil_target.SetPixel(j, i, Color.FromArgb(0, 0, 0));
                    }
            }
        }
        private void BinarizeIris(Image img)
        {
            if(img == Image.Source)
            {
                int w = source.Bitmap.Width,
                    h = source.Bitmap.Height;
                double threshold = Thresholding.GetThreshold_SimpleGray(binarized_normalized_source, Mode.Iris);
                binarized_iris_source = new DirectBitmap(source.Bitmap);
                for (int i = 0; i < h; ++i)
                    for (int j = 0; j < w; ++j)
                    {
                        Color c = binarized_normalized_source.GetPixel(j, i);
                        if ((c.R + c.G + c.B) / 3.0 >= threshold)
                            binarized_iris_source.SetPixel(j, i, Color.FromArgb(255, 255, 255));
                        else
                            binarized_iris_source.SetPixel(j, i, Color.FromArgb(0, 0, 0));
                    }
            }
            else //TARGET
            {
                int w = target.Bitmap.Width,
                    h = target.Bitmap.Height;
                double threshold = Thresholding.GetThreshold_SimpleGray(binarized_normalized_target, Mode.Iris);
                binarized_iris_target = new DirectBitmap(target.Bitmap);
                for (int i = 0; i < h; ++i)
                    for (int j = 0; j < w; ++j)
                    {
                        Color c = binarized_normalized_target.GetPixel(j, i);
                        if ((c.R + c.G + c.B) / 3.0 >= threshold)
                            binarized_iris_target.SetPixel(j, i, Color.FromArgb(255, 255, 255));
                        else
                            binarized_iris_target.SetPixel(j, i, Color.FromArgb(0, 0, 0));
                    }
            }
        }
        private void ComputeCode(Image img)
        {
            switch (img) {
                case Image.Source:
                    {
                        code_source = new List<bool[]>();
                        for (int line_index = 0, delta = (iris_radius_source - pupil_radius_source) / 8; line_index < 8; ++line_index)
                        {
                            code_source.Add(
                               Encode(
                                   AverageCircleIntensity(
                                       line_index,
                                       pupil_radius_source + delta * line_index,
                                       pupil_radius_source + delta * (line_index + 1),
                                       source,
                                       Image.Source),
                                   (double)FrequencyUpDown.Value,
                                   (double)LambdaUpDown.Value
                                   ));
                        }
                        break;
                    }
                case Image.Target:
                    {
                        code_target = new List<bool[]>();
                        for (int line_index = 0, delta = (iris_radius_target - pupil_radius_target) / 8; line_index < 8; ++line_index)
                        {
                            code_target.Add(
                               Encode(
                                   AverageCircleIntensity(
                                       line_index,
                                       pupil_radius_target + delta * line_index,
                                       pupil_radius_target + delta * (line_index + 1),
                                       target,
                                       Image.Target),
                                   (double)FrequencyUpDown.Value,
                                   (double)LambdaUpDown.Value
                                   ));
                        }
                        break;
                    }
            }

        }
        private void FindCenterOfPupil(Image img)
        {
            if (img == Image.Source)
            {
                int w = source.Width;
                int h = source.Height;
                var horizontal_projection = new int[w];
                var vertical_projection = new int[h];
                for(int i = 2; i < w - 2; ++i)
                {
                    for(int j = 2; j < h - 2; ++j)
                    {
                        Color c = binarized_pupil_source.GetPixel(i, j);
                        horizontal_projection[i] += c.R > 0 ? 0 : 1;
                        vertical_projection[j] += c.R > 0 ? 0 : 1;
                    }
                }
                int x1 = Array.FindIndex(horizontal_projection, i => (horizontal_projection.Max() - i) < 5);
                int x2 = Array.FindLastIndex(horizontal_projection, i => (horizontal_projection.Max() - i) < 5);
                int y1 = Array.FindIndex(vertical_projection, i => (vertical_projection.Max() - i) < 5);
                int y2 = Array.FindLastIndex(vertical_projection, i => (vertical_projection.Max() - i) < 5);
                x_center_source = (Array.FindIndex(horizontal_projection, i => (horizontal_projection.Max() - i) < 5) +
                                Array.FindLastIndex(horizontal_projection,i=>(horizontal_projection.Max()-i) < 5))/2;
                y_center_source = (Array.FindIndex(vertical_projection, i => (vertical_projection.Max() - i) < 5) +
                                Array.FindLastIndex(vertical_projection, i => (vertical_projection.Max() - i) < 5)) / 2;
                //NumericUpDown
                x_cen_source.Value = x_center_source;
                y_cen_source.Value = y_center_source;
            }
            else //TARGET
            {
                int w = target.Width;
                int h = target.Height;
                var horizontal_projection = new int[w];
                var vertical_projection = new int[h];
                for (int i = 2; i < w - 2; ++i)
                {
                    for (int j = 2; j < h - 2; ++j)
                    {
                        Color c = binarized_pupil_target.GetPixel(i, j);
                        horizontal_projection[i] += c.R > 0 ? 0 : 1;
                        vertical_projection[j] += c.R > 0 ? 0 : 1;
                    }
                }
                int x1 = Array.FindIndex(horizontal_projection, i => (horizontal_projection.Max() - i) < 5);
                int x2 = Array.FindLastIndex(horizontal_projection, i => (horizontal_projection.Max() - i) < 5);
                int y1 = Array.FindIndex(vertical_projection, i => (vertical_projection.Max() - i) < 5);
                int y2 = Array.FindLastIndex(vertical_projection, i => (vertical_projection.Max() - i) < 5);
                x_center_target = (Array.FindIndex(horizontal_projection, i => (horizontal_projection.Max() - i) < 5) +
                                Array.FindLastIndex(horizontal_projection, i => (horizontal_projection.Max() - i) < 5)) / 2;
                y_center_target = (Array.FindIndex(vertical_projection, i => (vertical_projection.Max() - i) < 5) +
                                Array.FindLastIndex(vertical_projection, i => (vertical_projection.Max() - i) < 5)) / 2;
                x_cen_target.Value = x_center_target;
                y_cen_target.Value = y_center_target;
            }
            BinarizePupil(img);
            Dilation(Mode.Pupil, img);
        }
        private void FindRadius( Image img)
        {
            if (img == Image.Source)
            {
                pupil_radius_source = 0;
                for (int r = 0; r + x_center_source < source.Width; ++r)
                    if (binarized_pupil_source.GetPixel(r + x_center_source, y_center_source).R > 0)
                    {
                        pupil_radius_source += r;
                        break;
                    }
                            
                for (int r = 0; x_center_source-r > 0; ++r)
                    if (binarized_pupil_source.GetPixel(x_center_source - r, y_center_source).R > 0)
                    {
                        pupil_radius_source += r;
                        break;
                    }

                for (int r = 0; y_center_source + r < source.Height; ++r)
                    if (binarized_pupil_source.GetPixel(x_center_source, y_center_source + r).R > 0)
                    {
                        pupil_radius_source += r;
                        break;
                    }
                for (int r = 0; y_center_source + r < source.Height && x_center_source + r < source.Width; ++r)
                    if (binarized_pupil_source.GetPixel(x_center_source + r, y_center_source + r).R > 0)
                    {
                        pupil_radius_source += r;
                        break;
                    }
                for (int r = 0; y_center_source + r < source.Height && x_center_source - r > 0; ++r)
                    if (binarized_pupil_source.GetPixel(x_center_source - r, y_center_source + r).R > 0)
                    {
                        pupil_radius_source += r;
                        break;
                    }
                pupil_radius_source /= 5;

                int r_right, r_left;
                for (r_right = 0; r_right + x_center_source < source.Width; ++r_right)
                    if (binarized_iris_source.GetPixel(r_right + x_center_source, y_center_source).R > 0)
                        break;
                for (r_left = 0; x_center_source-r_left > 0; ++r_left)
                    if (binarized_iris_source.GetPixel(x_center_source - r_left, y_center_source).R > 0)
                        break;
                iris_radius_source = (r_left + r_right)/2;
                
                r_pupil_source.Value = pupil_radius_source;
                r_iris_source.Value = iris_radius_source;

            }
            else //Target
            {
                pupil_radius_target = 0;
                for (int r = 0; r + x_center_target < target.Width; ++r)
                    if (binarized_pupil_target.GetPixel(r + x_center_target, y_center_target).R > 0)
                    {
                        pupil_radius_target += r;
                        break;
                    }

                for (int r = 0; x_center_target - r > 0; ++r)
                    if (binarized_pupil_target.GetPixel(x_center_target - r, y_center_target).R > 0)
                    {
                        pupil_radius_target += r;
                        break;
                    }

                for (int r = 0; y_center_target + r < target.Height; ++r)
                    if (binarized_pupil_target.GetPixel(x_center_target, y_center_target + r).R > 0)
                    {
                        pupil_radius_target += r;
                        break;
                    }


                for (int r = 0; y_center_target + r < target.Height && x_center_target + r < target.Width; ++r)
                    if (binarized_pupil_target.GetPixel(x_center_target + r, y_center_target + r).R > 0)
                    {
                        pupil_radius_target += r;
                        break;
                    }
                for (int r = 0; y_center_target + r < target.Height && x_center_target - r > 0; ++r)
                    if (binarized_pupil_target.GetPixel(x_center_target - r, y_center_target + r).R > 0)
                    {
                        pupil_radius_target += r;
                        break;
                    }
                pupil_radius_target /= 5;

                int r_right, r_left;
                for (r_right = 0; r_right + x_center_target < target.Width; ++r_right)
                    if (binarized_iris_target.GetPixel(r_right + x_center_target, y_center_target).R > 0)
                        break;
                for (r_left = 0; x_center_target - r_left > 0; ++r_left)
                    if (binarized_iris_target.GetPixel(x_center_target - r_left, y_center_target).R > 0)
                        break;
                iris_radius_target = (r_left + r_right) / 2;

                
                r_pupil_target.Value = pupil_radius_target;
                r_iris_target.Value = iris_radius_target;
            }
        }
        private void LoadImage(Image img)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Title = "Załaduj obraz";
                dialog.Filter = "Image files (*.jpg, *.jpeg, *.png, *.bmp) | *.jpg; *.jpeg; *.png; *.bmp";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    tmp?.Dispose();
                    if(img == Image.Source)
                    {
                        SourcePBox.Image?.Dispose();
                        binarized_iris_source?.Dispose();
                        binarized_normalized_source?.Dispose();
                        binarized_pupil_source?.Dispose();
                        source?.Dispose();

                        source = new DirectBitmap(new Bitmap(dialog.FileName));
                        Normalize(Image.Source);
                        BinarizePupil(Image.Source);
                        BinarizeIris(Image.Source);
                        SourcePBox.Image = source.Bitmap;
                    }
                    else if (img == Image.Target)
                    {
                    
                        MainPBox.Image?.Dispose();
                        binarized_iris_target?.Dispose();
                        binarized_normalized_target?.Dispose();
                        binarized_pupil_target?.Dispose();
                        target?.Dispose();

                        target = new DirectBitmap(new Bitmap(dialog.FileName));
                        Normalize(Image.Target);
                        BinarizePupil(Image.Target);
                        BinarizeIris(Image.Target);
                        MainPBox.Image = target.Bitmap;
                    }
                }
            }
        }
        private void AutoMode(Image img)
        {
            int progress = 0;
            switch (img)
            {
                case Image.Source:
                    {
                        Normalize(Image.Source);
                        BinarizePupil(Image.Source);
                        BinarizeIris(Image.Source);
                        progress += 10;
                        SourceProgressBar.Value = progress;
                        for (int i = 0; i < 10; ++i)
                        {
                            Dilation(Mode.Pupil, Image.Source);
                            if (i < 7)
                                Dilation(Mode.Iris, Image.Source);
                            progress += 2;
                            SourceProgressBar.Value = progress;
                        }
                        for (int i = 0; i < 22; ++i)
                        {
                            Erosion(Mode.Pupil, Image.Source);
                            if (i < 5)
                                Erosion(Mode.Iris, Image.Source);
                            progress += 2;
                            SourceProgressBar.Value = progress;
                        }
                        FindCenterOfPupil(Image.Source);
                        FindRadius(Image.Source);
                        try
                        { 
                            ComputeCode(Image.Source);
                        }
                        catch (IndexOutOfRangeException ex)
                        {
                            MessageBox.Show("Image boundaries exceeded, try manual approach with another data set");
                            code_source?.Clear();
                            code_source = null;
                            SourceProgressBar.Value = 0;
                            return;
                        }
                        SourceProgressBar.Value = 100;
                        DrawRingsSourceButton_Click(null, null);
                        break;
                    }
                case Image.Target:
                    {
                        Normalize(Image.Target);
                        BinarizePupil(Image.Target);
                        BinarizeIris(Image.Target);
                        progress += 10;
                        TargetProgressBar.Value = progress;
                        for (int i = 0; i < 10; ++i)
                        {
                            
                            Dilation(Mode.Pupil, Image.Target);
                            if (i < 7)
                                Dilation(Mode.Iris, Image.Target);
                            progress += 2;
                            TargetProgressBar.Value = progress;
                        }
                        for (int i = 0; i < 22; ++i)
                        {
                            Erosion(Mode.Pupil, Image.Target);
                            if (i < 5)
                                Erosion(Mode.Iris, Image.Target);
                            progress += 2;
                            TargetProgressBar.Value = progress;
                        }

                        FindCenterOfPupil(Image.Target);
                        FindRadius(Image.Target);
                        try
                        {
                            ComputeCode(Image.Target);
                        }
                        catch (IndexOutOfRangeException ex)
                        {
                            MessageBox.Show("Image boundaries exceeded, try manual approach with another data set");
                            code_target?.Clear();
                            code_target = null;
                            TargetProgressBar.Value = 0;
                            return;
                        }
                        
                        TargetProgressBar.Value = 100;
                        DrawRingsTargetButton_Click(null, null);
                        break;
                    }
            }
        }
        private enum Image
        {
            Source,
            Target
        }
    }
}