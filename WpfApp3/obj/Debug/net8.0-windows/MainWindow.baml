   M S B A M L   `   `   `  ����B  >WpfApp3, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:WpfApp3WpfApp3  5   	   7 clr-namespace:WpfApp3.ViewModelsWpfApp3.ViewModels  5   	       WpfApp3.MainWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   'localclr-namespace:WpfApp3     5   	   7
ViewModels clr-namespace:WpfApp3.ViewModels     5   	     � Title$  
MainWindow��5	   	   $	��510��6   $	��800��6)    � 	Resources 5
       �� DataType*   $WpfApp3.ViewModels.FileInfoViewModel/   )WpfApp3.ViewModels.DirectoryInfoViewModel �� ItemsSource��%�   (��       5      *, +)(�� L     5   #   *, +)�� 5   
   "  6   .R�5      �� ��6   �� *	Caption+5      �� 5   
   "  6#    6Y   �� *Items+.R�5      �� $
��Bold"�61   ��6   �� *	Caption+61   5      5      .��5      �  � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6    � ColumnDefinitions 5   
   �� 5      $��2* �6   �� 5      $��2* �6   .+�5    
   c� $��0��6   $��0��6   $��2��6/   .@�5!      o� $	��Top=�6   .@�5"      m� $��_File��6   .@�5#      m� -   5        $��_Open��5#      6.   m� 5$      -   5        $��_Exit��5$      6.   5%      5&      5'      R� 5)   
   -   5         R�Name$ 
FolderTree��5*      $��1��5,      $��0��5-      $��Stretch=�5.      $��0,0,0,23q�5/      ��5+      �� *
Root.Items+ 51         TextFileContextMenu  FolderContextMenu��%)  &	        &	 �     �� 52      .@�53      m� -   5        $��Otwórz��53      6/   m� 54      -   5        $��Usuń��54      6-   55      �� 56      .@�57      m� -   5        $��Utwórz plik��57      64   m� 58      -   5        $��Usuń��58      6-   59      5:      5A      �� 5C   
   $��23��6   $��Bottom=�6    ���	#FFE6E6E6$��1��6R   .@�5D      �� .��5E      �� -   5         ��Name$ AccessibilityNote��5E      $�� ��67   5F      5G      �� 5I   
   $��2��6   $��2��6$   .��5J      �� -	   5        $ FileTextContentEl��5J      $
��Wrap=�63   5K      5L      5M      5N      