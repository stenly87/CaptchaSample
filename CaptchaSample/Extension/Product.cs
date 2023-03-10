using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Cache;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CaptchaSample
{
    public partial class Product
    {
        BitmapImage image = null;
        public BitmapImage ImageSource 
        { 
            get {
                if (image != null)
                    return image;
                if (ProductPhoto == null)
                    return null;

                string path = $"{ProductArticleNumber}.png";
                File.WriteAllBytes(path, ProductPhoto);
                image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                
                image.UriSource = new Uri(path, UriKind.Relative);
                image.EndInit();
                return image;
            }
        }
    }
}
