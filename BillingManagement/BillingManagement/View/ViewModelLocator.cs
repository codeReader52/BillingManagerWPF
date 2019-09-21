using System;
using System.Windows;
using BillingManagement.Utils;

namespace BillingManagement.View
{
    public static class ViewModelLocator
    {
        public static readonly DependencyProperty LoadDataContext = 
            DependencyProperty.RegisterAttached("LoadDataContext", typeof(bool), typeof(ViewModelLocator), 
                new PropertyMetadata(false, LoadDataContextChanged));

        public static bool GetLoadDataContext(DependencyObject obj)
        {
            return (bool)obj.GetValue(LoadDataContext);
        }

        public static void SetLoadDataContext(DependencyObject obj, bool value)
        {
            obj.SetValue(LoadDataContext, value);
        }

        private static void LoadDataContextChanged(DependencyObject obj, DependencyPropertyChangedEventArgs eventArgs)
        {
            string viewName = obj.DependencyObjectType.Name;
            if (ViewModelViewManager.MapViewNameToViewModelFactory.Keys.Contains(viewName))
            {
                Func<object> factoryFunction = ViewModelViewManager.MapViewNameToViewModelFactory[viewName];
                ((FrameworkElement)obj).DataContext = factoryFunction();
            }
        }

        
    }
}
