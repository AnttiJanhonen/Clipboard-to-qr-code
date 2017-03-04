using QRCoder;
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;
using System.Drawing;

namespace ClipboardToQR
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            QRCodeGenerator qrGenerator = new QRCodeGenerator();


        }

        private void nappi_Click(object sender, RoutedEventArgs e)
        {
            renderQR();
        }

        private void renderQR()
        {
            if (Clipboard.GetText(TextDataFormat.UnicodeText).Length <= 1000)
            {
                try
                {
                    QRCodeGenerator qrGenerator = new QRCodeGenerator();
                    BitmapImage bi = new BitmapImage();
                    textBlock.Text = Clipboard.GetText(TextDataFormat.UnicodeText);
                    QRCodeData qrCodeData = qrGenerator.CreateQrCode(Clipboard.GetText(TextDataFormat.UnicodeText), QRCodeGenerator.ECCLevel.Q);
                    QRCode qrCode = new QRCode(qrCodeData);
                    Bitmap qrCodeImage = qrCode.GetGraphic(20);
                    ConvertBitmapToBitmapImage T = new ConvertBitmapToBitmapImage();
                    BitmapImage imageee;
                    imageee = T.Convert(qrCodeImage);
                    PngBitmapEncoder BtP = new PngBitmapEncoder();
                    BtP.Frames.Add(BitmapFrame.Create(imageee));
                    image.Stretch = Stretch.Uniform;
                    image.Source = imageee;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception: {0}", e);

                    throw;
                }
            }
        }

        // Demonstrates SetText, ContainsText, and GetText.
        public String SwapClipboardHtmlText(String replacementHtmlText)
        {
            String returnHtmlText = null;
            if (Clipboard.ContainsText(TextDataFormat.Html))
            {
                returnHtmlText = Clipboard.GetText(TextDataFormat.Html);
                Clipboard.SetText(replacementHtmlText, TextDataFormat.Html);
            }
            return returnHtmlText;
        }

        private void image_GotFocus(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Activated(object sender, EventArgs e)
        {
            if (autogen_checkBox.IsChecked == true)
            {
                renderQR();
            }
        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            if (alwaysontop_checkBox.IsChecked == true)
            {
                Window window = (Window)sender;
                window.Topmost = true;
            }
        }

        private void alwaysontop_checkBox_Checked(object sender, RoutedEventArgs e)
        {
            this.Topmost = true;
        }

        private void alwaysontop_checkBox_Unchecked(object sender, RoutedEventArgs e)
        {
            this.Topmost = false;
        }
    }
    public class ConvertBitmapToBitmapImage
    {
        /// <summary>
        /// Takes a bitmap and converts it to an image that can be handled by WPF ImageBrush
        /// </summary>
        /// <param name="src">A bitmap image</param>
        /// <returns>The image as a BitmapImage for WPF</returns>
        public BitmapImage Convert(Bitmap src)
        {
            MemoryStream ms = new MemoryStream();
            ((System.Drawing.Bitmap)src).Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            ms.Seek(0, SeekOrigin.Begin);
            image.StreamSource = ms;
            image.EndInit();
            return image;
        }
    }
}
