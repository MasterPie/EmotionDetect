// -----------------------------------------------------------------------
// <copyright file="ViewModelLoader.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace PostureRecognitionEngine
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

        public ViewModelLoader()
        {
            var prop = DesignerProperties.IsInDesignModeProperty;
            var isInDesignMode = (bool)DependencyPropertyDescriptor
                .FromProperty(prop, typeof(FrameworkElement))
                .Metadata.DefaultValue;
        }


        public static ViewModel ViewModelStatic
        {
            get
            {
                if (viewModelStatic == null)
                {
                    viewModelStatic = new ViewModel();
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
