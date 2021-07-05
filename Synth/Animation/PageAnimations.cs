using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace Synth
{
    /// <summary>
    /// Helpers to animate pages in specific ways
    /// </summary>
    public static class PageAnimations
    {
        /// <summary>
        /// Expands and fades in a page
        /// </summary>
        /// <param name="page">The page to animate</param>
        /// <param name="seconds">The time the animation will take</param>
        /// <param name="initialScale">The initial scale of the page</param>
        /// <param name="finalScale">The final scale of the page</param>
        /// <returns></returns>
        public static async Task ExpandAndFadeIn(this Page page, double seconds, double initialScale, double finalScale = 1)
        {
            //Create the storyboard
            Storyboard storyboard = new Storyboard();

            //Add expand animation 
            storyboard.AddExpand(seconds, initialScale, finalScale);

            //Add fade in animation
            storyboard.AddFadeIn(seconds);

            //Start animating the page
            storyboard.Begin(page);

            //Make page visible
            page.Visibility = Visibility.Visible;

            //Wait until the animation is finished
            await Task.Delay((int)(seconds * 1000));
        }

        /// <summary>
        /// Shrinks and fades out a page
        /// </summary>
        /// <param name="page">The page to animate</param>
        /// <param name="seconds">The time the animation will take</param>
        /// <param name="initialScale">The initial scale of the page</param>
        /// <param name="finalScale">The final scale of the page</param>
        /// <returns></returns>
        public static async Task ShrinkAndFadeOut(this Page page, double seconds, double initialScale, double finalScale)
        {
            //Create the storyboard
            Storyboard storyboard = new Storyboard();

            //Add shrink animation 
            storyboard.AddShrink(seconds, initialScale, finalScale);

            //Add fade out animation
            storyboard.AddFadeOut(seconds);

            //Start animating the page
            storyboard.Begin(page);

            //Make page visible
            page.Visibility = Visibility.Visible;

            //Wait until the animation is finished
            await Task.Delay((int)(seconds * 1000));
        }
    }
}
