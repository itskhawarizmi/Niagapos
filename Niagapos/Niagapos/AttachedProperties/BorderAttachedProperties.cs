﻿/************************************
 * Author Name : Muhammad Hari      *
 * Author URI  : www.xcodeplus.net  *
 ************************************/

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Niagapos
{
    /// <summary>
    /// Creates a clipping region from the parent <see cref="Border"/> <see cref="CornerRadius"/>
    /// </summary>
    public class ClipFromBorderProperty : BaseAttachedProperty<ClipFromBorderProperty, bool>
    {
        #region Private Members

        /// <summary>
        /// Called when the parent border first loads
        /// </summary>
        private RoutedEventHandler mBorder_Loaded;

        /// <summary>
        /// Called when the border size changes
        /// </summary>
        private SizeChangedEventHandler mBorder_SizeChanged;

        #endregion

        public override void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // Get self
            var self = (d as FrameworkElement);

            // Check we have a parent border
            if (!(self.Parent is Border border))
            {
                Debugger.Break();
                return;

            }

            // Setup loaded event
            mBorder_Loaded = (dd, ee) => Border_OnChanged(dd, ee, self);

            // Setup size changed event
            mBorder_SizeChanged = (dd, ee) => Border_OnChanged(dd, ee, self);

            // If true, hook into events
            if ((bool)e.NewValue)
            {
                border.Loaded += mBorder_Loaded;
                border.SizeChanged += mBorder_SizeChanged;
            }
            // Otherwise, unhook
            else
            {
                border.Loaded -= mBorder_Loaded;
                border.SizeChanged -= mBorder_SizeChanged;
            }
        }

        /// <summary>
        /// Called when the border is loaded and changed size
        /// </summary>
        /// <param name="dd">The border</param>
        /// <param name="ee"></param>
        /// <param name="child">The child element (our selves)</param>
        private void Border_OnChanged(object dd, RoutedEventArgs ee, FrameworkElement child)
        {
            // Get border
            var border = (Border)dd;

            // Check we have an actual size
            if (border.ActualWidth == 0 && border.ActualHeight == 0)
                return;

            // Setup the new child clipping area
            var rect = new RectangleGeometry();

            // Match the corner radius with the borders corner radius
            rect.RadiusX = rect.RadiusY = Math.Max(0, border.CornerRadius.TopLeft - (border.BorderThickness.Left * 0.5));

            // Set rectangle size to match child's actual size
            rect.Rect = new Rect(child.RenderSize);

            // Assign clipping area to the child
            child.Clip = rect;
            
            

            

           

        }
    }
}

