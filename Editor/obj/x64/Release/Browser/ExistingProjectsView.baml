   M S B A M L   `   `   `  ���� A  =Editor, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:Editor.BrowserEditor.Browser  &     Editor.Browser.Existing_Projects   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     .localclr-namespace:Editor.Browser       � DataContext  %   Editor.Browser.ExistingProjects  .��� $��10q� � RowDefinitions �� $
��auto ��� $
��auto �.+��� $��32(�
Projects��  �� Orientation$ 
Horizontal=�$��Right=�.+��� -    �� 	IsDefault$
 True��$
��auto��$��32(�$��0,0,20,0q�.��Open�� -   $
��auto��$��32(�$��0,0,20,0q�.��Add�� -    ��Name$ ExistingProjectsView�� �� SelectedIndex$ 0��$��1��$
��auto��$��10q����� *
Projects+ �� View �� $��Disabled=�.P��� $
��Name�� �� Width$	 100�� �� DisplayMemberBinding �� 	 �� Path$	 ProjectName��� $
��Path��$	 345�� �� $	 FullPath��� $��Last Modified��$	 135�� �� $	 LastModified�