﻿// -----------------------------------------------------------------------
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
        static IGameEngine gameEngine = null;
        static ILogger logger = null;

        public ViewModelLoader()
        {
            var prop = DesignerProperties.IsInDesignModeProperty;
            var isInDesignMode = (bool)DependencyPropertyDescriptor
                .FromProperty(prop, typeof(FrameworkElement))
                .Metadata.DefaultValue;

            emotionClassifer = new DetectClient.Client();


            if (Properties.Settings.Default.AffectMediated)
                gameEngine = new AffectMediatedGameEngine();
            else
                gameEngine = new SimpleGameEngine();
            
            logger = new TimeBasedLogger();
            gameEngine.ItemFactory = new BasicItemFactory();
            gameEngine.EmotionClassifier = emotionClassifer;


            //ILevel levelOne = new BasicLevel() { ID = 1, LocationRandomness = 2, VarietyRandomness = 2, Speed = 3 };
            //ILevel levelTwo = new BasicLevel() { ID = 2, LocationRandomness = 10, VarietyRandomness = 3, Speed = 5 };
            //ILevel levelThree = new BasicLevel() { ID = 3, LocationRandomness = 10, VarietyRandomness = 4, Speed = 5 };
            //ILevel levelFour = new BasicLevel() { ID = 4, LocationRandomness = 10, VarietyRandomness = 5, Speed = 5 };
            //ILevel levelFive = new BasicLevel() { ID = 5, LocationRandomness = 15, VarietyRandomness = 5, Speed = 5 };

            List<ILevel> demo = new List<ILevel>() { new BasicLevel() { ID = 1, LocationRandomness = 5, VarietyRandomness = 2, Speed = 4 }};

            ILevel levelOne = new BasicLevel() { ID = 1, LocationRandomness = 2, VarietyRandomness = 2, Speed = 3 };
            ILevel levelTwo = new BasicLevel() { ID = 2, LocationRandomness = 10, VarietyRandomness = 3, Speed = 3 };
            ILevel levelThree = new BasicLevel() { ID = 3, LocationRandomness = 10, VarietyRandomness = 4, Speed = 3 };
            ILevel levelFour = new BasicLevel() { ID = 4, LocationRandomness = 10, VarietyRandomness = 5, Speed = 4 };
            ILevel levelFive = new BasicLevel() { ID = 5, LocationRandomness = 15, VarietyRandomness = 5, Speed = 5 };

            ILevelManager levelManager = new OrderedLevelManager();
            //TODO: load demo level
            levelManager.LoadLevels(new List<ILevel>(){levelOne, levelTwo, levelThree, levelFour, levelFive});

            gameEngine.LevelManager = levelManager;
                
        }


        public static ViewModel ViewModelStatic
        {
            get
            {
                if (viewModelStatic == null)
                {
                    viewModelStatic = new ViewModel(gameEngine, logger, Properties.Settings.Default.Theme);
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
