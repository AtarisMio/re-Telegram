using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace Telegram.Helper
{
    public class UIHelper
    {
        /// <summary>
        /// 查找控件中符合对应Type的子元素集
        /// </summary>
        /// <typeparam name="T">输出目标类型</typeparam>
        /// <param name="obj">在何处查找子元素</param>
        /// <param name="typename">目标控件类型名</param>
        /// <returns></returns>
        public static List<T> GetChildObjects<T>(DependencyObject obj, Type typename) where T : FrameworkElement
        {
            DependencyObject child = null;
            List<T> childList = new List<T>();

            for (int i = 0; i <= VisualTreeHelper.GetChildrenCount(obj) - 1; i++)
            {
                child = VisualTreeHelper.GetChild(obj, i);

                if (child is T && (((T)child).GetType() == typename))
                {
                    childList.Add((T)child);
                }
                childList.AddRange(GetChildObjects<T>(child, typename));
            }
            return childList;
        }

        /// <summary>
        /// 查找控件中符合对应Name的子元素集
        /// </summary>
        /// <typeparam name="T">输出目标类型</typeparam>
        /// <param name="obj">在何处查找子元素</param>
        /// <param name="name">目标控件名，值为null或为空字符串返回所有子元素</param>
        /// <returns></returns>
        public static List<T> GetChildObjects<T>(DependencyObject obj, string name) where T : FrameworkElement
        {
            DependencyObject child = null;
            List<T> childList = new List<T>();

            for (int i = 0; i <= VisualTreeHelper.GetChildrenCount(obj) - 1; i++)
            {
                child = VisualTreeHelper.GetChild(obj, i);

                if (child is T && (((T)child).Name == name | string.IsNullOrEmpty(name)))
                {
                    childList.Add((T)child);
                }
                childList.AddRange(GetChildObjects<T>(child, name));
            }
            return childList;
        }

        /// <summary>
        /// 查找控件中符合对应Name的子元素
        /// </summary>
        /// <typeparam name="T">输出目标类型</typeparam>
        /// <param name="obj">在何处查找子元素</param>
        /// <param name="name">目标控件名，值为null或为空字符串返回第一个子元素</param>
        /// <returns></returns>
        public static T GetChildObject<T>(DependencyObject obj, string name) where T : FrameworkElement
        {
            DependencyObject child = null;
            T grandChild = null;

            for (int i = 0; i <= VisualTreeHelper.GetChildrenCount(obj) - 1; i++)
            {
                child = VisualTreeHelper.GetChild(obj, i);

                if (child is T && (((T)child).Name == name | string.IsNullOrEmpty(name)))
                {
                    return (T)child;
                }
                else
                {
                    grandChild = GetChildObject<T>(child, name);
                    if (grandChild != null)
                        return grandChild;
                }
            }
            return null;
        }

        /// <summary>
        /// 查找控件中符合对应Name的父元素
        /// </summary>
        /// <typeparam name="T">输出目标类型</typeparam>
        /// <param name="obj">在何处查找父元素</param>
        /// <param name="name">目标控件名，值为null或为空字符串返回第一个父元素</param>
        /// <returns></returns>
        public static T GetParentObject<T>(DependencyObject obj, string name) where T : FrameworkElement
        {
            DependencyObject parent = VisualTreeHelper.GetParent(obj);

            while (parent != null)
            {
                if (parent is T && (((T)parent).Name == name | string.IsNullOrEmpty(name)))
                {
                    return (T)parent;
                }

                parent = VisualTreeHelper.GetParent(parent);
            }

            return null;
        }
    }
}
