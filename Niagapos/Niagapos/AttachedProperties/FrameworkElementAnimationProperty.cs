/************************************
 * Author Name : Muhammad Hari      *
 * Author URI  : www.icodewizard.com  *
 ************************************/


using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;

namespace Niagapos
{
    /// <summary>
    /// A base class to run any animation method when a boolean is set to true
    /// and a reverse animation when set to false
    /// </summary>
    /// <typeparam name="Parent">An Animation framework</typeparam>
    public abstract class AnimateBaseProperty<Parent> : BaseAttachedProperty<Parent, bool>
        where Parent : BaseAttachedProperty<Parent, bool>, new()
    {
        #region Protected Properties

        /// <summary>
        /// True if this is the very first time the value has been updated
        /// Used to make sure we ran the logic at the least once during first load
        /// </summary>
        protected Dictionary<DependencyObject, bool> mAlreadyLoaded = new Dictionary<DependencyObject, bool>();

        /// <summary>
        /// The most recent value used if we get a value changed before we do the first load
        /// </summary>
        protected Dictionary<DependencyObject, bool> mFirstLoadValue = new Dictionary<DependencyObject, bool>();

        #endregion
        /// <summary>
        /// The method that is called when any attached property of this type is changed, even if the value is the same
        /// </summary>
        /// <param name="d">The UI element that this property was changed</param>
        /// <param name="value">The argument for this event</param>
        public override void OnValueUpdated(DependencyObject d, object value)
        {
            // Get the framework element
            if (!(d is FrameworkElement element))
                return;

            // Don't fire if the value doesn't change
            if ((bool)d.GetValue(ValueProperty) == (bool)value && mAlreadyLoaded.ContainsKey(d))
                return;

            // On first load
            if (!mAlreadyLoaded.ContainsKey(d))
            {
                // Flag that we are in first load but have not finished it
                mAlreadyLoaded[d] = false;

                // Start off hidden before we decide how to animate
                element.Visibility = Visibility.Hidden;

                // Create a single self-unhookable event 
                // for the elements Loaded event
                async void onLoaded(object dd, RoutedEventArgs ee)
                {
                    // Unhook ourselves
                    element.Loaded -= onLoaded;

                    // Slight delay after load is needed for some element to get laid out
                    // and their width/heights correctly calculated
                    await Task.Delay(5);

                    // Do desired animation
                    DoAnimation(element, mFirstLoadValue.ContainsKey(d) ? mFirstLoadValue[d] : (bool)value, true);

                    // Flag that we have finished first load
                    mAlreadyLoaded[d] = true;

                };

                // Hook into the Loaded event of the element
                element.Loaded += onLoaded;
            }

            // If we have started a first load but not fired the animation yet, update the property
            else if (mAlreadyLoaded[d] == false)
                mFirstLoadValue[d] = (bool)value;
            else
                // Do desired animation
                DoAnimation(element, (bool)value, false);

        }


        /// <summary>
        /// The animation method that is fired when the value changes
        /// </summary>
        /// <param name="element">The element/param>
        /// <param name="value">The new value</param>
        /// <param name="firstLoad">Indicating whether the value is first Loaded or not</param>
        protected virtual void DoAnimation(FrameworkElement element, bool value, bool firstLoad) { }

      
    }

    /// <summary>
    /// Animates a framework element sliding it in from the left on show
    /// and sliding out to the left on hide
    /// </summary>
    public class AnimateSlideInFromLeftProperty : AnimateBaseProperty<AnimateSlideInFromLeftProperty>
    {
        protected override async void DoAnimation(FrameworkElement element, bool value, bool firstLoad)
        {
            if (value)
                await element.SlideAndFadeInAsync(AnimationSlideInDirection.Left, firstLoad, firstLoad ? 0 : 0.3f, keepMargin: false);
            else
                await element.SlideAndFadeOutAsync(AnimationSlideInDirection.Left, firstLoad ? 0 : 0.3f, keepMargin: false);
        }
    }

    /// <summary>
    /// Animate a framework element sliding it in from the right on show
    /// and sliding out to the left on hide
    /// </summary>
    public class AnimateSlideInFromRightProperty : AnimateBaseProperty<AnimateSlideInFromRightProperty>
    {
        protected override async void DoAnimation(FrameworkElement element, bool value, bool firstLoad)
        {
            if (value)
                await element.SlideAndFadeInAsync(AnimationSlideInDirection.Right, firstLoad, firstLoad ? 0 : 0.3f, keepMargin: false);
            else
                await element.SlideAndFadeOutAsync(AnimationSlideInDirection.Right, firstLoad ? 0 : 0.3f, keepMargin: false);
        }
    }

    /// <summary>
    /// Animate a framework element sliding up from the bottom on show
    /// and sliding out to the bottom on hide
    /// </summary>
    public class AnimateSlideInFromBottomProperty : AnimateBaseProperty<AnimateSlideInFromBottomProperty>
    {
        protected override async void DoAnimation(FrameworkElement element, bool value, bool firstLoad)
        {
            if (value)
                await element.SlideAndFadeInAsync(AnimationSlideInDirection.Bottom, firstLoad, firstLoad ? 0 : 0.3f, keepMargin: false);
            else
                await element.SlideAndFadeOutAsync(AnimationSlideInDirection.Bottom, firstLoad ? 0 : 0.3f, keepMargin: false);
        }
    }

    /// <summary>
    /// Animates a framework element sliding up from the bottom on load
    /// if the value is true
    /// </summary>
    public class AnimateSlideInFromBottomOnLoadProperty : AnimateBaseProperty<AnimateSlideInFromBottomOnLoadProperty>
    {
        protected override async void DoAnimation(FrameworkElement element, bool value, bool firstLoad)
        {
            // Animate in
            await element.SlideAndFadeInAsync(AnimationSlideInDirection.Bottom, !value, !value ? 0 : 0.3f, keepMargin: false);

        }
    }

    /// <summary>
    /// Animates a framework element sliding up from the bottom on show
    /// and sliding out to the bottom on hide
    /// NOTE : Keeps the margin
    /// </summary>
    public class AnimateSlideInFromBottomMarginProperty : AnimateBaseProperty<AnimateSlideInFromBottomMarginProperty>
    {
        protected override async void DoAnimation(FrameworkElement element, bool value, bool firstLoad)
        {
            if (value)
                await element.SlideAndFadeInAsync(AnimationSlideInDirection.Bottom, firstLoad, firstLoad ? 0 : 0.3f, keepMargin: true);
            else
                await element.SlideAndFadeOutAsync(AnimationSlideInDirection.Bottom, firstLoad ? 0 : 0.3f, keepMargin: true);
        }
    }

    /// <summary>
    /// Animates a framework element fading in on show
    /// and fading out on hide
    /// </summary>
    public class AnimateFadeInProperty : AnimateBaseProperty<AnimateFadeInProperty>
    {
        protected override async void DoAnimation(FrameworkElement element, bool value, bool firstLoad)
        {
            if (value)
                await element.FadeInAsync(firstLoad, firstLoad ? 0 : 0.3f);
            else
                await element.FadeOutAsync(firstLoad ? 0 : 0.3f);
        }
    }

    /// <summary>
    /// Animate a framework element sliding it from right to left and repeating forever
    /// </summary>
    public class AnimateMarqueeProperty : AnimateBaseProperty<AnimateMarqueeProperty>
    {
        protected override void DoAnimation(FrameworkElement element, bool value, bool firstLoad)
        {
            // Animate in
            element.MarqueeAsync(firstLoad ? 0 : 3f);
        }
    }





}

