   M S B A M L   `   `   `  ����A  =Editor, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:Editor.EditorsEditor.Editors  5      clr-namespace:EditorEditor  5          Editor.Editors.LoggerView   5      W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      .localclr-namespace:Editor.Editors     5      'editorclr-namespace:Editor     5      .��5
      ]� .+�5   
   �� $	��Top=�6   $
��auto��6&   ._�5      ��   �� Orientation$  
Horizontal=�6   $��Right=�62   .+�5      �� -   5        .��5   3   Clear6:   d� 5      -   5         d� 	IsChecked$ FalseY�5      $��5,0,0,0q�61   .��6b   Trace6i   d� 5      -   5        $
 TrueY�5      $��5,0,0,0q�60   .��6`   Info6f   d� 5      -   5        $
 TrueY�5      $��5,0,0,0q�60   .��6c   	Warning6l   d� 5      -   5        $
 TrueY�5      $��5,0,0,0q�60   .��6a   Error6h   5      5      �� 5   
   .��5      �� ��6   ��    Editor.Logger   FilteredMessages �� Source# Z ��5      �� 5      .R�5      ]�  � ContextMenu 5      �� 5   "   .@�5   &   m� ��6/   �� *
Metadata+5   #   ��5      �� 5   "    �� 
TargetType" ]�6(   .�5    &   ��  �� Property � �� �� Value
 ���� 6C    �� Triggers 5!   &   P� 5"   *   	 P� Property	 �@J�IsMouseOver
 P� Value$

 True��6I   . �5#   .   ��  � ��
 �����6K   5$   +   5&   #   .+�5(      �� ��6(   �� *Time+ �� StringFormat$ hh:mm:ss����5)   "   �� 5*   &   " ��6,   .�5+   *   ��  � ��
 �iii�6G    5,   *   �� 5-   .      Editor.MessageType   Info �� Value# Z 6S    �� Binding 6:   �� *Type+.a�5.   2   ��  � ��
 � � �6O   5/   /   �� 50   .      Warning# Z 6S    6:   �� *Type+.a�51   2   ��  � ��
 � ���6O   52   /   �� 53   .      Error# Z 6S    6:   �� *Type+.a�54   2   ��  � ��
 �<��6O   55   /   57   '   59      �� 5:      $��: ��6(   �� 5;      $
��Wrap=�6A   ��6(   �� *	Message+6A   5<      5=      5?      5@      5A      5B      