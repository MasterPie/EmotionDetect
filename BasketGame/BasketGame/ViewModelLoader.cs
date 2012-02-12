// -----------------------------------------------------------------------
// <copyright file="ViewModelLoader.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace BasketGame
{
    using System;
    using System.ComponentModel;
    using System.Collections.Generic;
    using System.Windows;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ViewModelLoader
    {
        static ViewModel viewModelStatic;

        static DetectClient.Client emotionClassifer = null;
        static AbstractThemeBuilder themeBuilder = null;
        static IGameEngine gameEngine = null;
        static ILogger logger = null;

        public ViewModelLoader()
        {
            var prop = DesignerProperties.IsInDesignModeProperty;
            var isInDesignMode = (bool)DependencyPropertyDescriptor
                .FromProperty(prop, typeof(FrameworkElement))
                .Metadata.DefaultValue;

            emotionClassifer = new DetectClient.Client();
            themeBuilder = new FruitThemeBuilder();
            gameEngine = new AffectMediatedGameEngine();
            gameEngine.ItemFactory = themeBuilder.GetItemFactory();
            gameEngine.EmotionClassifier = emotionClassifer;


            ILevel levelOne = new BasicLevel() { ID = 1, LocationRandomness = 2, VarietyRandomness = 2, Speed = 5 };
            ILevel levelTwo = new BasicLevel() { ID = 2, LocationRandomness = 10, VarietyRandomness = 2, Speed = 5};
            ILevel levelThree = new BasicLevel() { ID = 3, LocationRandomness = 10, VarietyRandomness = 5, Speed = 5 };

            ILevelManager levelManager = new OrderedLevelManager();
            levelManager.LoadLevels(new List<ILevel>(){levelOne, levelTwo, levelThree});

            gameEngine.LevelManager = levelManager;

        }


        public static ViewModel ViewModelStatic
        {
            get
            {
                if (viewModelStatic == null)
                {
                    viewModelStatic = new ViewModel(gameEngine, logger);
                }
                return viewModelStatic;
            }
        }

        public ViewModel ViewModel
        {
            get { return ViewModelStatic; }
        }

        public static void Cleanup()
        {
            if (viewModelStatic != null)
            {
                viewModelStatic.Cleanup();
            }
        }
    }
}
