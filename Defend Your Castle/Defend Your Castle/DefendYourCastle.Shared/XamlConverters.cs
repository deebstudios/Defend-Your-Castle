using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Defend_Your_Castle
{
    // XAML Converters not nested in clsConverters because nested classes cannot be used in XAML:
    // http://msdn.microsoft.com/en-us/library/ms753379.aspx#Requirements_for_a_Custom_Class_as_a_XAML_Element
    public sealed class XamlConverters
    {

    }

    // Class used for converting the data-binded PriceString value to a value in the Visibility enumeration
    // Basic implementation found at:
    // http://msdn.microsoft.com/en-us/library/windows/apps/windows.ui.xaml.data.ivalueconverter.aspx
    public class PriceToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            // Get the value of the String that was specified
            String PriceString = (String)value;

            // Translate everything except "Maxed Out" to true
            return ((PriceString != "Maxed Out") ? Visibility.Visible : Visibility.Collapsed);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}