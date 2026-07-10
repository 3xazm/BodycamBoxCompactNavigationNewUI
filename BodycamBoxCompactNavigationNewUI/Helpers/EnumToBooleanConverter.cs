using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace BodycamBoxCompactNavigationNewUI.Helpers
{
    internal class EnumToBooleanConverter : IValueConverter
    {
        // 从 Enum 转换为 bool (用于绑定 RadioButton 的 IsChecked)
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null)
                return DependencyProperty.UnsetValue;

            string checkValue = parameter.ToString();

            // 智能适配：自动获取当前绑定的枚举类型，不再硬编码 ApplicationTheme
            Type enumType = value.GetType();
            if (!enumType.IsEnum)
                return false; // 如果不是枚举，安全返回 false，防止程序崩溃闪退

            // 检查参数名字是否合法
            if (!Enum.IsDefined(enumType, checkValue))
                return false;

            // 将参数字符串解析为对应的枚举项，并进行对比
            object paramValue = Enum.Parse(enumType, checkValue);
            return paramValue.Equals(value);
        }

        // 从 bool 转换回 Enum (当用户点击 RadioButton 时)
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isChecked && isChecked && parameter != null)
            {
                // targetType 就是系统预期你要返回的枚举类型（比如 ApplicationTheme）
                if (targetType.IsEnum)
                {
                    return Enum.Parse(targetType, parameter.ToString());
                }
            }

            return DependencyProperty.UnsetValue;
        }
    }
}