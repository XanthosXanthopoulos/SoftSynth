using NAudio.Wave;
using Ninject;
using System;
using System.Runtime.ConstrainedExecution;

namespace Synth
{

    /// <summary>
    /// The IoC for the application
    /// </summary>
    public static class IoCContainer
    {
        #region Public Properties

        /// <summary>
        /// The kernel for the IoC container
        /// </summary>
        public static IKernel Kernel { get; private set; } = new StandardKernel();

        #endregion

        #region Initialize

        /// <summary>
        /// Sets up the IoC container, binds all information required and its ready for use
        /// NOTE: Must be called as soon as the application starts up
        /// </summary>
        public static void Setup()
        {
            //Bind all required audio engines
            BindEngines();

            //Bind all required view models
            BindViewModels();
        }

        /// <summary>
        /// Binds all singleton view models
        /// </summary>
        private static void BindViewModels()
        {
            //Bind to a single instance of view models
            Kernel.Bind<OscillatorPageViewModel>().ToConstant(new OscillatorPageViewModel());
        }

        /// <summary>
        /// Binds all singleton audio engines
        /// </summary>
        private static void BindEngines()
        {
            Kernel.Bind<AudioEngine>().ToConstant(new AudioEngine());
        }

        #endregion

        /// <summary>
        /// Gets a service from the IoC container of the specified type
        /// </summary>
        /// <typeparam name="T">The type of service to get</typeparam>
        /// <returns></returns>
        public static T Get<T>()
        {
            return Kernel.Get<T>();
        }
    }
}
