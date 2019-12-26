
 using System;
 using System.CodeDom.Compiler;
 using System.Collections.Generic;
 using System.ComponentModel;
 using System.Diagnostics;
 using System.Windows;
 using System.Windows.Controls;
 using System.Windows.Controls.Primitives;
 using System.Windows.Input;
 using System.Windows.Markup;
 using System.Windows.Media;
 
public class CategoryComboBox : UserControl, IComponentConnector, IStyleConnector
 {
     private Popup LastOpenPopup;
 
    internal ComboBox catComboBox;
 
    private bool _contentLoaded;
 
    public int SelectedIndex
     {
         get
         {
             return catComboBox.SelectedIndex;
         }
         set
         {
             catComboBox.SelectedIndex = value;
         }
     }
 
    public object SelectedItem
     {
         get
         {
             ContentPresenter cp = FindVisualChildByName<ContentPresenter>(catComboBox, "ContentSite");
             if (cp == null)
             {
                 return catComboBox.SelectedItem;
             }
             return cp.Content;
         }
         set
         {
             catComboBox.SelectedItem = value;
         }
     }
 
    public new Visibility Visibility
     {
         get
         {
             return catComboBox.Visibility;
         }
         set
         {
             catComboBox.Visibility = value;
         }
     }
 
    public event CategoryChangedEvent CategoryChanged;
 
    public void SetCategoryComboBoxItemSource(List<Category> categories)
     {
         catComboBox.SelectionChanged += catComboBox_SelectionChanged;
         catComboBox.ItemsSource = categories;
     }
 
    public CategoryComboBox()
     {
         InitializeComponent();
     }
 
    public void ClearItems()
     {
         ContentPresenter cp = FindVisualChildByName<ContentPresenter>(catComboBox, "ContentSite");
         if (cp != null)
         {
             cp.Content = null;
         }
     }
 
    private void catComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
     {
         if (catComboBox.SelectedIndex < 0)
         {
             e.Handled = true;
             return;
         }
         Category selectedCat = (Category)catComboBox.SelectedItem;
         ContentPresenter cp = FindVisualChildByName<ContentPresenter>(catComboBox, "ContentSite");
         if (cp != null)
         {
             cp.Content = selectedCat;
         }
         if (selectedCat != null)
         {
             this.CategoryChanged(selectedCat);
         }
         catComboBox.SelectionChanged -= catComboBox_SelectionChanged;
         e.Handled = true;
     }
 
    public static T FindVisualChildByName<T>(DependencyObject parent, string name) where T : DependencyObject
     {
         if (parent != null)
         {
             for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
             {
                 DependencyObject child = VisualTreeHelper.GetChild(parent, i);
                 if (child.GetValue(FrameworkElement.NameProperty) as string== name)
                 {
                     return child as T;
                 }
                 T result = FindVisualChildByName<T>(child, name);
                 if (result != null)
                 {
                     return result;
                 }
             }
         }
         return null;
     }
 
    private void nextLayerButton_Click(object sender, RoutedEventArgs e)
     {
         Popup subCategoryPopup = (Popup)((Button)sender).Tag;
         if (LastOpenPopup != null && LastOpenPopup != subCategoryPopup && LastOpenPopup.IsOpen)
         {
             LastOpenPopup.IsOpen = false;
         }
         LastOpenPopup = subCategoryPopup;
         LastOpenPopup.IsOpen = !LastOpenPopup.IsOpen;
         e.Handled = true;
     }
 
    private void StackPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
     {
         Category selectedCat = (Category)((StackPanel)sender).Tag;
         ContentPresenter cp = FindVisualChildByName<ContentPresenter>(catComboBox, "ContentSite");
         if (cp != null)
         {
             cp.Content = selectedCat;
         }
         if (selectedCat != null)
         {
             this.CategoryChanged(selectedCat);
         }
     } 
