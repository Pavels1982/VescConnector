   M S B A M L   `   `   `  ����H  DVescConnector, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullK*clr-namespace:VescConnector.CustomControlsVescConnector.CustomControls  5      8    2VescConnector.CustomControls.MaterialNumericUpDown   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      <local*clr-namespace:VescConnector.CustomControls     5      b ^MaterialDesignThemes.Wpf, Version=3.2.0.1979, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5        � 	Resources  5
       �� 
TargetType   RepeatButtonFlatStyle �� Property  FocusVisual �� Value  PrimaryHueMidBrush �� 
TargetType ��Name �� CornerRadius%  MaterialDesignThemes.Wpf.Ripple J� SnapsToDevicePixels �� Triggers	 P� Property
 P� Value �� 
TargetName ! MaterialDesignFlatButtonClick��%�  &	        7  �� 5   
   " ��6   .�5      ��  �@�FocusVisualStyle8   61   �� 5       � ��
 ���� 6+   �� 5       � ��
 ���� 6,   �� 5       � ��# �  6+   �� 5       � ��$ 0q�60   �� 5       � ��$ Center=�6;   �� 5       � ��$ Center=�69   �� 5       � ��$ 	16 4 16 4q�6(   �� 5       � ��$ 32��6'   �� 5       � �� 5      �� 5      " ��6&   .R�5      � .+�5      �� $ border��5   !   #��z��5   !   $ 2��5   !   #��z��5   !   #��z��5   !   ._�5    "     #��z��5!   %   #��z��5"   %   |�.  #��z��5$   %   #��z��5%   %   #��z��5&   %   # z 5'   %   5(      5)       5*      P� 5+      	 �@J�IsMouseOver
 . . �5.   "   �� $ border��50   %   # �  51   %    � ���� 52   "   $ border��54   %   # �  55   %    � ��56      P� 57      	 � {�
 .  . �58   "   ��  �@J�Opacity$
 0.23Q�6<   59      5;      5=      5?      5@      .��5A      � $��0 8 0 0 q�5B   	   $	��150��5C   	    � RowDefinitions 5D   
   �� 5E      $��32 �6   �� 5F      $��25 �6    � ColumnDefinitions 5H   
   �� 5I      $
��Auto �6   �� 5J      $
��Auto �6   �� 5K      $
��Auto �6   .+�5O   
   �� 
�������$��0��5Q      $��0��5R      $��3��5S      $��Bottom=�5T      $��Center=�5U      $��9(�5V      ��5W      ��  �� RelativeSource �� *FindAncestor+ �� AncestorType8   2VescConnector.CustomControls.MaterialNumericUpDown"   �� Path$ Title� �� Mode$ OneWay=��� 5]   
   -   5        $��
DownButton��5c      $��2 0 2 2q�5^      $��16��5_      $��16��5`      $��1��5a      $��0��5b      #��[  5f      +  %MaterialDesignThemes.Wpf.RippleAssist   Feedback �	#DD000000$��0q�5i      .��5j      '  !MaterialDesignThemes.Wpf.PackIcon     Kind$ Minus=�5k      
���iii� J� RenderTransformOrigin$ .3,.3&�5m      5n      �� 5p   
   -   5        $��TextBox��5r      $��2 0 2 2q�5q      $��1��5s      $��1��5t      {�.  $	��100��5v       �� TextAlignment$ Center=�5z      $��25��5{      ��5w      ��  �� *FindAncestor+"  $ Value�$ TwoWay=�.��5|      5~      �� 5�   
   -   5        $��UpButton��5�      $��2 0 2 2q�5�      $��16��5�      $��16��5�      $��1��5�      $��2��5�      #��[  5�       �	#DD000000$��0q�5�      .��5�        $
 Plus=�5�      
���iii�$ .3,.3&�5�      5�      5�      5�      