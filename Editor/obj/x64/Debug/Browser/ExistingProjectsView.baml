   M S B A M L   `   `   `  ����A  =Editor, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:Editor.BrowserEditor.Browser  5      &     Editor.Browser.Existing_Projects   5      W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      .localclr-namespace:Editor.Browser     5        � DataContext  5
      %   Editor.Browser.ExistingProjects  5   
   .��5      � $��10q�6    � RowDefinitions 5   
   �� 5      $
��auto �6   �� 5      $
��auto �6   .+�5   
   �� $��32(�6   
Projects5      �� 5   
    �� Orientation$ 
Horizontal=�6   $��Right=�6.   .+�5      �� -   5         �� 	IsDefault$
 True��5      $
��auto��6A   $��32(�6N   $��0,0,20,0q�6\   .��6n   Open5      �� 5      -   5        $
��auto��5   -   $��32(�6:   $��0,0,20,0q�6H   .��6[   Add5      5      �� 5   
   -   5         ��Name$ ExistingProjectsView��5       �� SelectedIndex$ 0��5   4   $��1��5   1   $
��auto��5      $��10q�6!   ��5      �� *
Projects+ �� View 5       �� 5!      $��Disabled=�6   .P�5#      �� $
��Name��6%    �� Width$	 100��63    �� DisplayMemberBinding 5$   %   �� 	 �� Path$	 ProjectName��� 5%      $
��Path��6%   $	 345��63    5&   %   �� $	 FullPath��� 5'      $��Last Modified��6%   $	 135��6<    5(   %   �� $	 LastModified�5)      5+      5,      5-      