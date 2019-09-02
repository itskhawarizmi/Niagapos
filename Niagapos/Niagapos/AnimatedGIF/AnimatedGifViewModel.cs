
using Niagapos.Core;
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Niagapos
{
    public static class AnimateGifViewModel
    {

        public static FrameInfo GetFrameInfo(BitmapFrame frame)
        {
            var frameInfo = new FrameInfo
            {
                Delay = TimeSpan.FromMilliseconds(100),
                DisposalMethod = FrameDisposal.Replace,
                Width = frame.PixelWidth,
                Height = frame.PixelHeight,
                Left = 0,
                Top = 0,

            };

            BitmapMetadata metadata;

            try
            {
                metadata = frame.Metadata as BitmapMetadata;

                if (metadata != null)
                {
                    const string delayQuery = "/grctlext/Delay";
                    const string disposalQuery = "/grctlext/Disposal";
                    const string widthQuery = "/imgdesc/Width";
                    const string heightQuery = "/imgdesc/Height";
                    const string leftQuery = "/imgdesc/Left";
                    const string topQuery = "/imgdesc/Top";

                    var delay = metadata.GetQueryOrNull<ushort>(delayQuery);
                    if (delay.HasValue)
                        frameInfo.Delay = TimeSpan.FromMilliseconds(10 * delay.Value);

                    var disposal = metadata.GetQueryOrNull<byte>(disposalQuery);
                    if (disposal.HasValue)
                        frameInfo.DisposalMethod = (FrameDisposal)disposal.Value;

                    var width = metadata.GetQueryOrNull<ushort>(widthQuery);
                    if (width.HasValue)
                        frameInfo.Width = width.Value;

                    var height = metadata.GetQueryOrNull<ushort>(heightQuery);
                    if (height.HasValue)
                        frameInfo.Height = height.Value;

                    var left = metadata.GetQueryOrNull<ushort>(leftQuery);
                    if (left.HasValue)
                        frameInfo.Left = left.Value;

                    var top = metadata.GetQueryOrNull<ushort>(topQuery);
                    if (top.HasValue)
                        frameInfo.Top = top.Value;

                }
            }
            catch (NotSupportedException Ex)
            {
                CoreDI.Logger.Log(Ex.Message);
            }

            return frameInfo;




        }

        public static BitmapDecoder GetDecoder(BitmapSource image)
        {
            BitmapDecoder decoder = null;

            try
            {

                if (image is BitmapFrame frame)
                    decoder = frame.Decoder;

                if (decoder == null)
                {
                    if (image is BitmapImage bimg)
                    {
                        if (bimg.StreamSource != null)
                        {
                            bimg.StreamSource.Position = 0;
                            decoder = BitmapDecoder.Create(bimg.StreamSource, bimg.CreateOptions, bimg.CacheOption);
                        }
                        else if (bimg.UriSource != null)
                        {
                            Uri uri = bimg.UriSource;
                            if (bimg.BaseUri != null && !uri.IsAbsoluteUri)
                                uri = new Uri(bimg.BaseUri, uri);

                            decoder = BitmapDecoder.Create(uri, bimg.CreateOptions, bimg.CacheOption);
                        }

                    }
                }

                return decoder;
            }
            catch (Exception Ex)
            {
                CoreDI.Logger.Log(Ex.Message);
            }

            return decoder;


        }




        public static BitmapSource MakeFrame(BitmapSource fullImage, BitmapSource rawFrame, FrameInfo frameInfo, BitmapSource previousFrame, FrameInfo previousFrameInfo)
        {
            DrawingVisual visual = new DrawingVisual();
            using (var context = visual.RenderOpen())
            {
                if (previousFrameInfo != null && previousFrame != null && previousFrameInfo.DisposalMethod == FrameDisposal.Combine)
                {
                    var fullRect = new Rect(0, 0, fullImage.PixelWidth, fullImage.PixelHeight);
                    context.DrawImage(previousFrame, fullRect);
                }

                context.DrawImage(rawFrame, frameInfo.Rectangle);
            }

            var bitmap = new RenderTargetBitmap(fullImage.PixelWidth, fullImage.PixelHeight, fullImage.DpiX, fullImage.DpiY, PixelFormats.Pbgra32);

            bitmap.Render(visual);


            return bitmap;

        }

        public static T? GetQueryOrNull<T>(this BitmapMetadata metadata, string query)
            where T : struct
        {
            if (metadata.ContainsQuery(query))
            {
                object value = metadata.GetQuery(query);
                if (value != null)
                    return (T)value;
            }

            return null;

        }


        #region Helpers

        public class FrameInfo
        {
            /// <summary>
            /// Represent a time interval to delay animated .gif
            /// </summary>
            public TimeSpan Delay { get; set; }

            public FrameDisposal DisposalMethod { get; set; }

            public double Width { get; set; }

            public double Height { get; set; }

            public double Left { get; set; }

            public double Top { get; set; }

            public Rect Rectangle => new Rect(Left, Top, Width, Height);
        }
        #endregion

    }
}
