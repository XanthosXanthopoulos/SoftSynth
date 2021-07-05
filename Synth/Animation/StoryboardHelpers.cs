using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Synth
{
    /// <summary>
    /// Animation helpers for <see cref="Storyboard"/>
    /// </summary>
    public static class StoryboardHelpers
    {
        /// <summary>
        /// Adds an expand animation to the storyboard
        /// </summary>
        /// <param name="storyboard">The storyboard to add the animation to</param>
        /// <param name="seconds">The time the animation will take</param>
        /// <param name="initialScale">The initial sclae of the page</param>
        /// <param name="finalScale">The final scale of the page</param>
        public static void AddExpand(this Storyboard storyboard, double seconds, double initialScale, double finalScale)
        {
            //Create the expand animation
            DoubleAnimation xScaleAnimation = new DoubleAnimation(initialScale, finalScale, TimeSpan.FromSeconds(seconds));
            DoubleAnimation yScaleAnimation = new DoubleAnimation(initialScale, finalScale, TimeSpan.FromSeconds(seconds));

            //Set the target name
            Storyboard.SetTargetName(xScaleAnimation, "ScaleTr");
            Storyboard.SetTargetName(yScaleAnimation, "ScaleTr");

            //Set the target property name
            Storyboard.SetTargetProperty(xScaleAnimation, new PropertyPath(ScaleTransform.ScaleXProperty));
            Storyboard.SetTargetProperty(yScaleAnimation, new PropertyPath(ScaleTransform.ScaleYProperty));

            //Add the animation to the storyboard
            storyboard.Children.Add(xScaleAnimation);
            storyboard.Children.Add(yScaleAnimation);
        }

        /// <summary>
        /// Adds a shrink animation to the storyboard
        /// </summary>
        /// <param name="storyboard">The storyboard to add the animation to</param>
        /// <param name="seconds">The time the animation will take</param>
        /// <param name="initialScale">The initial sclae of the page</param>
        /// <param name="finalScale">The final scale of the page</param>
        public static void AddShrink(this Storyboard storyboard, double seconds, double initialScale, double finalScale)
        {
            //Create the shrink animation
            DoubleAnimation xScaleAnimation = new DoubleAnimation(initialScale, finalScale, TimeSpan.FromSeconds(seconds));
            DoubleAnimation yScaleAnimation = new DoubleAnimation(initialScale, finalScale, TimeSpan.FromSeconds(seconds));

            //Set the target name
            Storyboard.SetTargetName(xScaleAnimation, "ScaleTr");
            Storyboard.SetTargetName(yScaleAnimation, "ScaleTr");

            //Set the target property name
            Storyboard.SetTargetProperty(xScaleAnimation, new PropertyPath(ScaleTransform.ScaleXProperty));
            Storyboard.SetTargetProperty(yScaleAnimation, new PropertyPath(ScaleTransform.ScaleYProperty));

            //Add the animation to the storyboard
            storyboard.Children.Add(xScaleAnimation);
            storyboard.Children.Add(yScaleAnimation);
        }

        /// <summary>
        /// Adds a fade in animation to the storyboard
        /// </summary>
        /// <param name="storyboard">The storyboard to add the animation to</param>
        /// <param name="seconds">The time the animation will take</param>
        public static void AddFadeIn(this Storyboard storyboard, double seconds)
        {
            //Create the fade in animation
            DoubleAnimation opacityAnimation = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(seconds));

            //Set the target property name
            Storyboard.SetTargetProperty(opacityAnimation, new PropertyPath("Opacity"));

            //Add the animation to the storyboard
            storyboard.Children.Add(opacityAnimation);
        }

        /// <summary>
        /// Adds a fade out animation to the storyboard
        /// </summary>
        /// <param name="storyboard">The storyboard to add the animation to</param>
        /// <param name="seconds">The time the animation will take</param>
        public static void AddFadeOut(this Storyboard storyboard, double seconds)
        {
            //Create the fade out animation
            DoubleAnimation opacityAnimation = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(seconds));

            //Set the target property name
            Storyboard.SetTargetProperty(opacityAnimation, new PropertyPath("Opacity"));

            //Add the animation to the storyboard
            storyboard.Children.Add(opacityAnimation);
        }

        /// <summary>
        /// Adds a slide to left animation to the storyboard
        /// </summary>
        /// <param name="storyboard">The storyboard to add the animation to</param>
        /// <param name="seconds">The time the animation will take</param>
        /// <param name="offset">The distance to the left to end at</param>
        /// <param name="decelerationRatio">The rate of deceleration</param>
        public static void AddSlideToLeft(this Storyboard storyboard, double seconds, double offset, double decelerationRatio = 0.9)
        {
            //Create the slide to left animation
            ThicknessAnimation thicknessAnimation = new ThicknessAnimation(new Thickness(0), new Thickness(-offset, 0, 0, 0), TimeSpan.FromSeconds(seconds))
            {
                DecelerationRatio = decelerationRatio
            };

            //Set the target property name
            Storyboard.SetTargetProperty(thicknessAnimation, new PropertyPath("Margin"));

            //Add the animation to the storyboard
            storyboard.Children.Add(thicknessAnimation);
        }

        /// <summary>
        /// Adds a slide to right animation to the storyboard
        /// </summary>
        /// <param name="storyboard">The storyboard to add the animation to</param>
        /// <param name="seconds">The time the animation will take</param>
        /// <param name="offset">The distance to the right to end at</param>
        /// <param name="decelerationRatio">The rate of deceleration</param>
        public static void AddSlideToRight(this Storyboard storyboard, double seconds, double offset, double decelerationRatio = 0.9)
        {
            //Create the slide to right animation
            ThicknessAnimation thicknessAnimation = new ThicknessAnimation(new Thickness(0), new Thickness(0, 0, -offset, 0), TimeSpan.FromSeconds(seconds))
            {
                DecelerationRatio = decelerationRatio
            };

            //Set the target property name
            Storyboard.SetTargetProperty(thicknessAnimation, new PropertyPath("Margin"));

            //Add the animation to the storyboard
            storyboard.Children.Add(thicknessAnimation);
        }

        /// <summary>
        /// Adds a slide from right animation to the storyboard
        /// </summary>
        /// <param name="storyboard">The storyboard to add the animation to</param>
        /// <param name="seconds">The time the animation will take</param>
        /// <param name="offset">The distance to the right to start from</param>
        /// <param name="decelerationRatio">The rate of deceleration</param>
        public static void AddSlideFromRight(this Storyboard storyboard, double seconds, double offset, double decelerationRatio = 0.9)
        {
            //Create the slide from right animation
            ThicknessAnimation thicknessAnimation = new ThicknessAnimation(new Thickness(0, 0, -offset, 0), new Thickness(0), TimeSpan.FromSeconds(seconds))
            {
                DecelerationRatio = decelerationRatio
            };

            //Set the target property name
            Storyboard.SetTargetProperty(thicknessAnimation, new PropertyPath("Margin"));

            //Add the animation to the storyboard
            storyboard.Children.Add(thicknessAnimation);
        }

        /// <summary>
        /// Adds a slide from left animation to the storyboard
        /// </summary>
        /// <param name="storyboard">The storyboard to add the animation to</param>
        /// <param name="seconds">The time the animation will take</param>
        /// <param name="offset">The distance to the left to start from</param>
        /// <param name="decelerationRatio">The rate of deceleration</param>
        public static void AddSlideFromLeft(this Storyboard storyboard, double seconds, double offset, double decelerationRatio = 0.9)
        {
            //Create the slide from left animation
            ThicknessAnimation thicknessAnimation = new ThicknessAnimation(new Thickness(-offset, 0, 0, 0), new Thickness(0), TimeSpan.FromSeconds(seconds))
            {
                DecelerationRatio = decelerationRatio
            };

            //Set the target property name
            Storyboard.SetTargetProperty(thicknessAnimation, new PropertyPath("Margin"));

            //Add the animation to the storyboard
            storyboard.Children.Add(thicknessAnimation);
        }
    }
}
