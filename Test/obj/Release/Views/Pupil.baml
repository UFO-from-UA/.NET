   M S B A M L   `   `   `  ���� ?  ;Test, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:TestTest      
Test.Pupil   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     $localclr-namespace:Test     V RGu.Wpf.Adorners, Version=1.5.1.0, Culture=neutral, PublicKeyToken=6dfb64f9972bd31d$adornershttp://gu.se/Adorners  -     � WindowStartupLocation$  CenterScreen=� � Icon$ ../Icons/profile.png��$	��400��$	��400�� � Title$# Ваша информация��$	��400��$	��400�� � 	Resources  �� 
TargetType �� Property �� Value��%Z  '��       '�� n     �� " ��.���  � ��$ Center=���  � ��$ Right=���  � ��$ 14(���  � ��
 ������� " ��.���  � ��$ Center=���  � ��$ Stretch=���  � ��$ 14(���  � ��$ 0 0 0 4q���  � ��
 �  ����  � ��$ 50����  �@��	MaxLength$ 50����  � ��$	 5 0q�.��� ����  � SpreadMethod$ Reflect=� �� 
StartPoint$ 0,0.5&�	 �� EndPoint$	 0.2,0.5&�.��� 
 � Color$

 Blue�� � Offset$ 0Q�� $
 #8800ff��$ 1Q� � ColumnDefinitions �� ��  � RowDefinitions �� �� �� �� �� �� �� �� .+��� -   $��borderLastNаme��$��0��$��0��$��2��
�������$��2q��� Фамилия :�� -   $��	LastNаme��  Gu.Wpf.Adorners.Watermark   Text$ Фамилия��$��1��.��Какойто�� -   $��borderFirstName��$��1��$��0��$��2��
�������$��2q��� $��1��
Имя :�� -   $��	FirstName��$ Имя��$��1��$��1��.��Валерий�� -   $��borderSecondName��$��2��$��0��$��2��
�������$��2q��� $��2��Отчество :�� -   $��
SecondName��$ Отчество��$��2��$��1��.��Психопатович�� -   $��borderGroup��$��3��$��0��$��2��
�������$��2q��� $��3��Класс :�� -	   $��Group��$ 
Класс��$��3��$��1�� �� 	MaxLength$ 5��.��2-б�� -
   $��	borderAge��$��4��$��0��$��2��
�������$��2q��� $��4��Возраст :�� -   $	��Age��$. (Возраст (Только цыфры)��$��4��$��1��$ 2��.��8�� -   $��borderPhone��$��5��$��0��$��2��
�������$��2q��� $��5��Телефон :�� -   $��Phone��$1 +Телефон (Необезательно)��$��5��$��1��$ 13��.��
57438291�� -   $��
borderMail��$��6��$��0��$��2��
�������$��2q��� $��6��
E-mail :�� -   $
��Mail��$) #E-mail (Необезательно)��$��6��$��1��.��yuhim93gmail.com�� $��7��$��0��$��2��
�������$��2q��� -    � Cursor$
 Hand��$��7��$��Right=�$��5q�$	��100��$��20��.��OK�� $
 Hand�� �� IsCancel . $��7��$��1��$
��Left=�$��5q�$	��100��$��20��.��Отмена