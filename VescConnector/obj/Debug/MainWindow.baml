   M S B A M L   `   `   `  ����H  DVescConnector, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:VescConnectorVescConnector  5   	   K*clr-namespace:VescConnector.CustomControlsVescConnector.CustomControls  5
   	       VescConnector.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   -localclr-namespace:VescConnector     5   	   b ^MaterialDesignThemes.Wpf, Version=3.2.0.1979, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5   	   c _System.Windows.Interactivity, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35Bi:http://schemas.microsoft.com/expression/2010/interactivity  5   	   R NOxyPlot.Wpf, Version=2.0.0.0, Culture=neutral, PublicKeyToken=75e952ba404cdbb0 oxyhttp://oxyplot.org/wpf  5	   	   ?customControls*clr-namespace:VescConnector.CustomControls  5
   	   -   5           MaterialDesignPaper#���   5   	     MaterialDesignBody#���  5   	   $��Medium"�5   	   $��14(�5   	   $X��Rpack://application:,,,/MaterialDesignThemes.Wpf;component/Resources/Roboto/#Roboto)�5   	     � Title$  
MainWindow��5   	   $��576.53��6   $��824.545��6,   .��5      �  � RowDefinitions 5   
   �� 5      $
��0.3* �6   �� 5      $
��0.7* �6   .+�5   
     OxyPlot.Wpf.PlotView  $��10q�5      $��0��5      $��0��5         Model 5      �� *Chart.Model+� 5       $��1��6    5!      �� 5"      $
��auto �6$   �� 5#      $��* �6$   .+�5%      �� $��0��5&      $��+ Add Device��5'      $
��Left=�5)      $��5q�5*      ��5(      �� *	AddVesc+5*      �� 5+       �� Orientation$ 
Horizontal=�6   $��Right=�66   .+�5,      �� $��2q�5-        MaterialDesignFlatButton#��[ 5.      ��5/      �� *RefreshPorts+.��51      '  !MaterialDesignThemes.Wpf.PackIcon     Kind$ Refresh=�62   $��25��6B   $��25��6M   52      �� 54      $��2q�55      #��[ 56      ��57      �� *	StopVescs+.��59        $
 Stop=�62   $��25��6?   $��25��6J   5:      5;      �� 5>      
�������$
��Auto=�5@      $��1��5A      .��5B      �� ��5C      �� *
VescList+ �� Mode$ TwoWay=���5E      �� 5F      .R�5G   "   9� $��3q�6+   ��5H   &   A� 5I   *    A� Columns$ 2��66   .+�5J   .   �� $��0��69   $ Vertical=�6I   .+�5M   2   �� $ 
Horizontal=�6=   .+�5N   2   �� $��20��6<   $��20��6G   ��6S   ��  �� Path$ 
ChartColor��� 5P   2   $��0��5Q   )   $��2q�5R   )   
���   ���5T   )   �� $ ID� �� StringFormat$ VESC ID: {0}���� 5V   6   $��0��5W   )   $��2q�5X   )   
���   ���5Z   )   �� $ Info.Hw�$ 	  HW: {0}��5\   3   �� 5]   2   ��68   �� $ 
StatusText�$ TwoWay=�5`   /   �� 5b   .   $��Right=�5c   )   $��1��5d   )   $ 
Horizontal=�5e   )   .+�5f   2   �� $��2 2 40 2q�5h   -   $	��150��5i   -   	 �� SelectedItem	 5g   -   �� $ SelectedPort���5j   -   �� $ PortList��� 5l   2   $��2q�5m   .   $��Connect��5n   .   ��5o   .   �� *DataContext.ConnectPort+
 �� RelativeSource
 ��  �� AncestorType$ ItemsControlL���5p   .   �� �� 5r   2   $��2q�5s   .   $��
Disconnect��5t   .   ��5u   .   �� *DataContext.DisconnectPort+
 �� $ ItemsControlL���5v   .   �� �� 5x   2   $��2q�5y   .   ��5z   .   �� *DataContext.RemoveVesc+
 �� $ ItemsControlL���5{   .   �� .��5|   6     $ Delete=�6N   $��25��6]   $��25��6h   5}   3   5�   /   5�   +   .��5�   &   �  5�   *   �� 5�   .   �� 5�   .    � ColumnDefinitions 5�   *   �� 5�   .   �� 5�   .   .+�5�   *   �� $��2��5�   %   $��0��5�   %   $��0��5�   %   $ 
Horizontal=�5�   %   $��10q�6>   $��Center=�5�   %   .+�5�   .   �� 
��� � �$��2q�5�   )   .  (System.Windows.Interactivity.Interaction   Triggers 5�   2   /  )System.Windows.Interactivity.EventTrigger  5�   6      	EventName$  PreviewMouseLeftButtonDown��6E   .  (System.Windows.Interactivity.TriggerBase   Actions. 5�   :   6  0System.Windows.Interactivity.InvokeCommandAction     Command 6P   �� $" DataContext.StartReverseDuty�
 �� $ ItemsControlL�   CommandParameter 6�   �� 5�   7     5�   6   $ PreviewMouseLeftButtonUp��6E   . 5�   :      6P   �� $ DataContext.StopDuty�
 �� $ ItemsControlL� 6�   �� 5�   7   .��5�   2     $ 	ArrowLeft=�6J   $��25��6\   $��25��6g   5�   /   �� 5�   .   $��5q�5�   )   $��50��5�   )   {�.  $��0��5�   )   )  #MaterialDesignThemes.Wpf.HintAssist   Hint$
 Duty��5�   )     MaterialDesignTextBox#��[ 5�   )   ��5�   )   �� $
 Duty�$ TwoWay=��� 5�   .   
��� � �$��2q�5�   )    5�   2     5�   6   $  PreviewMouseLeftButtonDown��6E   . 5�   :      6P   �� $" DataContext.StartForwardDuty�
 �� $ ItemsControlL� 6�   �� 5�   7     5�   6   $ PreviewMouseLeftButtonUp��6E   . 5�   :      6P   �� $ DataContext.StopDuty�
 �� $ ItemsControlL� 6�   �� 5�   7   .��5�   2     $ 
ArrowRight=�6J   $��25��6]   $��25��6h   5�   /   �� 5�   .   $��5q�5�   )   
���  ����5  )   �� *DataContext.BrakeVesc+
 �� $ ItemsControlL���5  )   �� .��5  2     $
 Stop=�6J   $��25��6W   $��25��6b   5  /   �� 5  .    �� DisplayMemberPath$
 Name��5
  -   $��	2 12 10 2q�5  -   $	��150��5  -   	 5	  -   �� $ 	SynchVesc�$ TwoWay=� �� SelectedValue 5  -   �� $ 	SynchVesc���5  -   �� *DataContext.VescList+
 �� $ ItemsControlL��� 5  *   $��2q�5  )   ��5  )   �� *DataContext.RemoveSyncVesc+
 �� $ ItemsControlL���5  )   �� .��5  .     $ Close=�6F   $��25��6T   $��25��6_   5  +   �� 5  *   $��2q�5  )   ��5  )   �� *DataContext.ConnectingCommand+
 �� $ ItemsControlL���5  )   �� .��5   .     $ 
Connection=�6F   $��25��6Y   $��25��6d   5!  +   5#  '   #  MaterialDesignThemes.Wpf.Card  5%  *   $��1��5&  %   $��2��5'  %   $��Stretch=�5(  %   .��5*  .   �� $��5q�5+  )   $ Vertical=�5,  )   .+�5.  2   �� $��Real time data��5/  -    d� 	IsChecked 50  -   �� $ IsRealTimeData�� 52  2   .+�59  6   �� $��5q�6@   $	��150��6K   
���������6o   �� $ RealTimeData.Duty_now�$ Duty_now : {0}���� 5:  6   $��5q�6@   $	��150��6K   
���������6o   �� $ RealTimeData.Rpm�$ 	Rpm : {0}���� 5;  6   $��5q�6@   $	��150��6K   
���������6o   �� $ RealTimeData.V_in�$ 
V_in : {0}���� 5<  6   $��5q�6@   $	��150��6K   
���������6o   �� $ Delta�$ Delta rpm : {0}���� 5=  2   $��5q�6<   $	��150��6G   
���������6k   �� $ RealTimeData.Position�$ Position : {0}���� 5>  2   $��5q�6<   $	��150��6G   
���������6k   �� $ 
isIncrease�$ isIncrease : {0}��5N  3   5O  /   5Q  +   5]  '   5_  #   5`     5b     5e     5f     5g     5i     