using System;
using System.Windows.Automation;

namespace BombCrypto.ConsoleApplication.Helpers
{
    public class ScreenHelper
    {
        ///--------------------------------------------------------------------
        /// <summary>
        /// Obtains a TransformPattern control pattern from 
        /// an automation element.
        /// </summary>
        /// <param name="targetControl">
        /// The automation element of interest.
        /// </param>
        /// <returns>
        /// A TransformPattern object.
        /// </returns>
        ///--------------------------------------------------------------------
        public static TransformPattern GetTransformPattern(
            AutomationElement targetControl)
        {
            try
            {
                return
                    targetControl.GetCurrentPattern(TransformPattern.Pattern)
                    as TransformPattern;
            }
            catch (InvalidOperationException)
            {
                // object doesn't support the TransformPattern control pattern
                return null;
            }
        }

        ///--------------------------------------------------------------------
        /// <summary>
        /// Calls the TransformPattern.Resize() method for an associated 
        /// automation element.
        /// </summary>
        /// <param name="transformPattern">
        /// The TransformPattern control pattern obtained from
        /// an automation element.
        /// </param>
        /// <param name="width">
        /// The requested width of the automation element.
        /// </param>
        /// <param name="height">
        /// The requested height of the automation element.
        /// </param>
        ///--------------------------------------------------------------------
        public static void ResizeElement(
            TransformPattern transformPattern, double width, double height)
        {
            try
            {
                if (transformPattern.Current.CanResize)
                {
                    transformPattern.Resize(width, height);
                }
            }
            catch (InvalidOperationException)
            {
                // object is not able to perform the requested action
                return;
            }
        }

        public static void ResizeElement(AutomationElement targetControl, double width, double height)
        {
            try
            {
                var transformPattern = GetTransformPattern(targetControl);
                if (transformPattern != null)
                {
                    ResizeElement(transformPattern, width, height);
                }

            }
            catch (InvalidOperationException)
            {
                // object is not able to perform the requested action
                return;
            }
        }

    }
}
