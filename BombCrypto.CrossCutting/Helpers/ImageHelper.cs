using AForge.Imaging;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace BombCrypto.CrossCutting.Helpers
{
    public static class ImageHelper
    {

        public static List<Rectangle> FindImage(Bitmap imgToSearch, Bitmap imageToFind)
        {
            // create template matching algorithm's instance
            // (set similarity threshold to 92.1%)

            //ExhaustiveTemplateMatching tm = new ExhaustiveTemplateMatching(0.921f);
            ExhaustiveTemplateMatching tm = new ExhaustiveTemplateMatching(0.8f);
            // find all matchings with specified above similarity

            var imageToSearchFormated = imgToSearch.ConvertToFormat(PixelFormat.Format24bppRgb);
            var imageToFindFormated = imageToFind.ConvertToFormat(PixelFormat.Format24bppRgb);
            TemplateMatch[] matchings = tm.ProcessImage(imageToSearchFormated, imageToFindFormated);

            var response = new List<Rectangle>();
            foreach (TemplateMatch m in matchings)
            {
                response.Add(m.Rectangle);
            }

            return response;
        }

        public static Bitmap ConvertToFormat(this Bitmap image, PixelFormat format)
        {
            Bitmap copy = new Bitmap(image.Width, image.Height, format);
            using (Graphics gr = Graphics.FromImage(copy))
            {
                gr.DrawImage(image, new Rectangle(0, 0, copy.Width, copy.Height));
            }
            return copy;
        }
    }
}
