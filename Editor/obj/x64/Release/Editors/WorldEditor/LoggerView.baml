   M S B A M L   `   `   `  ���� A  =Editor, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:Editor.EditorsEditor.Editors  clr-namespace:EditorEditor      Editor.Editors.LoggerView   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     .localclr-namespace:Editor.Editors     'editorclr-namespace:Editor     .��� .+��� .���� ����   Editor.Logger    FilteredMessages �� Source# Z  ���� .R�]� ����  �� 
TargetType" ]�.���  �� Property � �� �� Value
 ����  �� Triggers P�  P� Property �@J�IsMouseOver P� Value$
 True��. ���  � ��
 �����.+��� ���� *Time+ �� StringFormat$ hh:mm:ss��	 � ContextMenu	 �� .@�m� ���� *
Metadata+���� " ��.���  � ��
 �iii� ��   Editor.MessageType
   Info �� Value# Z
  �� Binding �� *Type+.a���  � ��
 � � ���    Warning# Z  �� *Type+.a���  � ��
 � �����    Error# Z  �� *Type+.a���  � ��
 �<���� $
��Wrap=����� *	Message+