using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using Niagapos.Core;
using static Niagapos.Core.AnimateGifViewModel;

namespace Niagapos
{
    public class AnimatedGifAttachedProperty : BaseAttachedProperty<AnimatedGifAttachedProperty, ImageSource>
    {
        public override void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // If we don't have image control, just return it
            if (!(d is Image imageControl))
                return;


            if (e.OldValue is ImageSource oldValue)
                imageControl.BeginAnimation(Image.SourceProperty, null);

            if (e.NewValue is ImageSource newValue)
                imageControl.DoWhenLoad(InitAnimationOrImage);
        }


        private static async void InitAnimationOrImage(Image imageControl)
        {
            BitmapSource source = GetValue(imageControl) as BitmapSource;

            try
            {

                if (source != null)
                {
                    if (GetDecoder(source) is GifBitmapDecoder decoder && decoder.Frames.Count > 1)
                    {
                        var animation = new ObjectAnimationUsingKeyFrames();

                        var totalDuration = TimeSpan.Zero;
                        BitmapSource prevFrame = null;
                        FrameInfo prevInfo = null;

                        foreach (var rawFrame in decoder.Frames)
                        {
                            var info = GetFrameInfo(rawFrame);
                            var frame = MakeFrame(source, rawFrame, info, prevFrame, prevInfo);

                            var keyFrame = new DiscreteObjectKeyFrame(frame, totalDuration);

                            animation.KeyFrames.Add(keyFrame);

                            totalDuration += info.Delay;
                            prevFrame = frame;
                            prevInfo = info;

                        }

                        animation.Duration = totalDuration;

                        animation.RepeatBehavior = RepeatBehavior.Forever;

                        if (animation.KeyFrames.Count > 0)
                            imageControl.Source = (ImageSource)animation.KeyFrames[0].Value;

                        else
                            imageControl.Source = decoder.Frames[0];

                        imageControl.BeginAnimation(Image.SourceProperty, animation);

                        return;

                    }
                }

                imageControl.Source = source;

                return;
            }
            catch (NotSupportedException)
            {
                await DI.UI.ShowMessage(new MessageBoxDialogViewModel
                {
                    Title = "Informasi Kesalahan",
                    Message = "Gambar tidak ditemukan, pastikan Anda telah menyisipkan sebuah gambar"

                });

                return;
            }


        }




    }
}
