using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace Niagapos
{
    /// <summary>
    /// Helpers to animate framework elements in specific ways
    /// </summary>
    public static class FrameworkElementAnimations
    {
        #region Slide In / Out

        /// <summary>
        /// Slides an element in
        /// </summary>
        /// <param name="element">The element to animate</param>
        /// <param name="direction">The direction of the slide</param>
        /// <param name="firstLoad">The time the animation will take</param>
        /// <param name="seconds">Whether to keep the element at the same width during animation</param>
        /// <param name="keepMargin">The animation width/height to animate to. If not specified the element size is used</param>
        /// <param name="size">Indicates if this is the first load</param>
        /// <returns></returns>
        public static async Task SlideAndFadeInAsync(this FrameworkElement element, AnimationSlideInDirection direction, bool firstLoad, float seconds = 0.3f, bool keepMargin = true, int size = 0)
        {
            var sb = new Storyboard();

            // Slide in the correct direction
            switch (direction)
            {
                // Add slide from left animation
                case AnimationSlideInDirection.Left:
                    sb.AddSlideFromLeft(seconds, size == 0 ? element.ActualWidth : size, keepMargin: keepMargin);
                    break;

                // Add slide from right animation
                case AnimationSlideInDirection.Right:
                    sb.AddSlideFromRight(seconds, size == 0 ? element.ActualWidth : size, keepMargin: keepMargin);
                    break;

                case AnimationSlideInDirection.Top:
                    sb.AddSlideFromTop(seconds, size == 0 ? element.ActualHeight : size, keepMargin: keepMargin);
                    break;

                case AnimationSlideInDirection.Bottom:
                    sb.AddSlideFromBottom(seconds, size == 0 ? element.ActualHeight : size, keepMargin: keepMargin);
                    break;


            }

            // Add fade in animation
            sb.AddFadeIn(seconds);

            // Start animating
            sb.Begin(element);

            // Make page visible only if we are animating or its first load
            if (seconds != 0 || firstLoad)
                element.Visibility = Visibility.Visible;

            // Wait for it to finish 
            await Task.Delay((int)(seconds * 1000));


        }

        /// <summary>
        /// Slides an element out
        /// </summary>
        /// <param name="element">The element to animate</param>
        /// <param name="direction">The direction of the slide (this's for the reverse slide out action, so left would slide out to left</param>
        /// <param name="seconds">The time the animate will take </param>
        /// <param name="keepMargin">Whether to keep the element at the same width during animation </param>
        /// <param name="size">The animation width/height to animate to. If not specified the elements size is used</param>
        /// <returns></returns>
        public static async Task SlideAndFadeOutAsync(this FrameworkElement element, AnimationSlideInDirection direction, float seconds = 0.3f, bool keepMargin = true, int size = 0)
        {
            var sb = new Storyboard();

            switch (direction)
            {
                case AnimationSlideInDirection.Left:
                    sb.AddSlideToLeft(seconds, size == 0 ? element.ActualWidth : size, keepMargin: keepMargin);
                    break;

                case AnimationSlideInDirection.Right:
                    sb.AddSlideToRight(seconds, size == 0 ? element.ActualWidth : size, keepMargin: keepMargin);
                    break;

                case AnimationSlideInDirection.Top:
                    sb.AddSlideToTop(seconds, size == 0 ? element.ActualHeight : size, keepMargin: keepMargin);
                    break;

                case AnimationSlideInDirection.Bottom:
                    sb.AddSlideToBottom(seconds, size == 0 ? element.ActualHeight : size, keepMargin: keepMargin);
                    break;
            }

            // Add fade in animation
            sb.AddFadeOut(seconds);

            // Start animating
            sb.Begin(element); ;

            // Make page visible only we are animating or its first load
            if (seconds != 0)
                element.Visibility = Visibility.Visible;

            // Wait for it to finish
            await Task.Delay((int)(seconds * 1000));

            // Make element invisible
            element.Visibility = Visibility.Hidden;
        }

        #endregion


        #region Add Fade In/Out

        /// <summary>
        /// Fades an element in
        /// </summary>
        /// <param name="element">The element to animate</param>
        /// <param name="firstLoad">Indicates if this is the first load</param>
        /// <param name="seconds">The time the animation will take</param>
        /// <returns></returns>
        public static async Task FadeInAsync(this FrameworkElement element, bool firstload, float seconds = 0.3f)
        {
            // Create the storyboard
            var sb = new Storyboard();

            // Add fade in animation
            sb.AddFadeIn(seconds);

            // Start animating
            sb.Begin(element);

            // Make page visibile only we are animating or its first load
            if (seconds != 0 || firstload)
                element.Visibility = Visibility.Visible;

            // Wait for it to finish
            await Task.Delay((int)(seconds * 1000));

        }


        /// <summary>
        /// Fades out an element
        /// </summary>
        /// <param name="element">The element to animate</param>
        /// <param name="seconds">The time the animation will take</param>
        /// <returns></returns>
        public static async Task FadeOutAsync(this FrameworkElement element, float seconds = 0.3f)
        {
            var sb = new Storyboard();

            sb.AddFadeOut(seconds);

            sb.Begin(element);

            // Make page visible only if we are animating or its the first load
            if (seconds != 0)
                element.Visibility = Visibility.Visible;

            await Task.Delay((int)(seconds * 1000));

            element.Visibility = Visibility.Collapsed;
        }

        #endregion


        // NB for Marquee Style Element:
        // The structure should be:
        // [Border ClipToBounds = "True"]
        //   [Border local:AnimateMarqueeProperty.Value = "True"]
        //      [Content HorizontalAlignment ="Left"]
        //   [/Border]
        //[/Border]

        /// <summary>
        /// Animate a marquee style element
        /// </summary>
        /// <param name="element">The element to animate</param>
        /// <param name="seconds">The time the animation will take</param>
        public static void MarqueeAsync(this FrameworkElement element, float seconds = 0.3f)
        {
            // Create storyboard
            var sb = new Storyboard();

            // Run until element is unloaded
            var unloaded = false;

            // Monitor for element unloading
            element.Unloaded += (dd, ee) => unloaded = true;

            // Run a loop off the caller thread
            Task.Run(async () =>
            {
                // While the element is still available, recheck the size
                // after every loop in case the container was resized
                while (element != null && !unloaded)
                {
                    // Create width variables
                    var width = 0d;
                    var innerWidth = 0d;

                    try
                    {
                        // Check if element is still loaded
                        if (element == null || unloaded)
                            break;

                        // Try and get current width
                        width = element.ActualWidth;
                        innerWidth = ((element as Border).Child as FrameworkElement).ActualWidth;
                    }

                    catch
                    {
                        // Any issues the stop animating (presume element destroyed)
                        break;
                    }

                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        // Start animating
                        sb.Begin(element);

                        // Make page visible
                        element.Visibility = Visibility.Visible;



                    });

                    // Wait for it to finish animating
                    await Task.Delay((int)seconds * 1000);

                    // If this is from first load or zero seconds of animation, do not repeat
                    if (seconds == 0)
                        break;
                }


            });
        }
    }
}
