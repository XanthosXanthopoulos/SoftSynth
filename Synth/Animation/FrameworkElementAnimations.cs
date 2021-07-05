using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;

namespace Synth
{
    /// <summary>
    /// Helpers to animate framework elements in specific ways
    /// </summary>
    public static class FrameworkElementAnimations
    {
        /// <summary>
        /// Slides and fades in an element from the right
        /// </summary>
        /// <param name="element">The element to animate</param>
        /// <param name="seconds">The time the animation will take</param>
        /// <returns></returns>
        public static async Task SlideAndFadeInFromRight(this FrameworkElement element, double seconds)
        {
            //Create the storyboard
            Storyboard storyboard = new Storyboard();

            //Add expand animation 
            storyboard.AddSlideFromRight(seconds, element.ActualWidth);

            //Add fade in animation
            storyboard.AddFadeIn(seconds);

            //Start animating the element
            storyboard.Begin(element);

            //Make element visible
            element.Visibility = Visibility.Visible;

            //Wait until the animation is finished
            await Task.Delay((int)(seconds * 1000));
        }

        /// <summary>
        /// Slides and fades in an element from the left
        /// </summary>
        /// <param name="element">The element to animate</param>
        /// <param name="seconds">The time the animation will take</param>
        /// <returns></returns>
        public static async Task SlideAndFadeInFromLeft(this FrameworkElement element, double seconds)
        {
            //Create the storyboard
            Storyboard storyboard = new Storyboard();

            //Add expand animation 
            storyboard.AddSlideFromLeft(seconds, element.ActualWidth);

            //Add fade in animation
            storyboard.AddFadeIn(seconds);

            //Start animating the element
            storyboard.Begin(element);

            //Make element visible
            element.Visibility = Visibility.Visible;

            //Wait until the animation is finished
            await Task.Delay((int)(seconds * 1000));
        }

        /// <summary>
        /// Slides and fades out an element to the left
        /// </summary>
        /// <param name="element">The element to animate</param>
        /// <param name="seconds">The time the animation will take</param>
        /// <returns></returns>
        public static async Task SlideAndFadeOutToLeft(this FrameworkElement element, double seconds)
        {
            //Create the storyboard
            Storyboard storyboard = new Storyboard();

            //Add expand animation 
            storyboard.AddSlideToLeft(seconds, element.ActualWidth);

            //Add fade in animation
            storyboard.AddFadeOut(seconds);

            //Start animating the element
            storyboard.Begin(element);

            //Make element visible
            element.Visibility = Visibility.Visible;

            //Wait until the animation is finished
            await Task.Delay((int)(seconds * 1000));
        }

        /// <summary>
        /// Slides and fades out an element to the right
        /// </summary>
        /// <param name="element">The element to animate</param>
        /// <param name="seconds">The time the animation will take</param>
        /// <returns></returns>
        public static async Task SlideAndFadeOutToRight(this FrameworkElement element, double seconds)
        {
            //Create the storyboard
            Storyboard storyboard = new Storyboard();

            //Add expand animation 
            storyboard.AddSlideToRight(seconds, element.ActualWidth);

            //Add fade in animation
            storyboard.AddFadeOut(seconds);

            //Start animating the element
            storyboard.Begin(element);

            //Make element visible
            element.Visibility = Visibility.Visible;

            //Wait until the animation is finished
            await Task.Delay((int)(seconds * 1000));
        }

        /// <summary>
        /// Fades an element in
        /// </summary>
        /// <param name="element">The element to animate</param>
        /// <param name="seconds">The time the animation will take</param>
        /// <param name="firstLoad">Indicates if this is the first load</param>
        /// <returns></returns>
        public static async Task FadeInAsync(this FrameworkElement element, bool firstLoad, float seconds = 0.3f)
        {
            // Create the storyboard
            var sb = new Storyboard();

            // Add fade in animation
            sb.AddFadeIn(seconds);

            // Start animating
            sb.Begin(element);

            // Make page visible only if we are animating or its the first load
            if (seconds != 0 || firstLoad)
                element.Visibility = Visibility.Visible;

            // Wait for it to finish
            await Task.Delay((int)(seconds * 1000));
        }

        /// <summary>
        /// Fades out an element
        /// </summary>
        /// <param name="element">The element to animate</param>
        /// <param name="seconds">The time the animation will take</param>
        /// <param name="firstLoad">Indicates if this is the first load</param>
        /// <returns></returns>
        public static async Task FadeOutAsync(this FrameworkElement element, float seconds = 0.3f)
        {
            // Create the storyboard
            var sb = new Storyboard();

            // Add fade in animation
            sb.AddFadeOut(seconds);

            // Start animating
            sb.Begin(element);

            // Make page visible only if we are animating or its the first load
            if (seconds != 0)
                element.Visibility = Visibility.Visible;

            // Wait for it to finish
            await Task.Delay((int)(seconds * 1000));

            // Fully hide the element
            element.Visibility = Visibility.Collapsed;
        }
    }
}
