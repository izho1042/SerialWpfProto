   M S B A M L   `   `   `  ����D  @SerialWpf, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:SerialWpfProtoSerialWpfProto  5   	       SerialWpfProto.MainWindow   5   	   \ XPresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   8x,http://schemas.microsoft.com/winfx/2006/xaml    5   	   >d2http://schemas.microsoft.com/expression/blend/2008    5   	   Hmc;http://schemas.openxmlformats.org/markup-compatibility/2006    5   	   ,localclr-namespace:SerialWpfProto    5   	     � Title$$  Serial Communication Prototype��5   	   $	��350��60   $	��720��6=    � 
ResizeMode$ CanMinimize=�6I   .��5	      � -   5         �Name$ mainGrid��5	      
���p�.+�5
   
   �� -   5         ��Name$ 	portCombo��5
      $
��Left=�6&   $��	86,14,0,0q�6A   $	��Top=�6T   $	��120��6l   �� 5   
   $��Port:��6   $
��Left=�6    $��	44,10,0,0q�6;   $	��Top=�6N   
�������6f   �� 5   
   -   5        $ 	baudCombo��5      $
��Left=�6&   $��	86,45,0,0q�6A   $	��Top=�6T   $	��120��6l   .@�5      �� .��6   24006!   �� 5      .��6   48006!   �� 5       �� 
IsSelected . .��6-   960063   �� 5      .��6   192006"   �� 5      .��6   384006"   �� 5      .��6   1152006#   5      �� 5   
   $��
Baud Rate:��6   $
��Left=�6%   $��	10,41,0,0q�6@   $	��Top=�6S   
�������6k   �� 5   
   -   5         ��Name$ 	sendInput��5      $
��Left=�6%   $��23��6@   $��117,264.958,0,0q�6L    �� TextWrapping$
 Wrap=�6e   $��Send Message��6y   $	��Top=�6�   $	��378��6�   �� 5   
   $�� ��6   $
��Left=�6   $��133,264.958,0,0q�66   $	��Top=�6O   �� 5   
   -   5         ��Name$ 
sendButton��5      $
��Send��6%   $
��Left=�6M   $��500,265.958,0,0q�6h   $	��Top=�6�   $��75��6�   {�.  6�   �� 5   
   -   5        $ openPortButton��5      $��	Open Port��6)   $
��Left=�6[   $��86,155.129,0,0q�6v   $	��Top=�6�   $��75��6�   �� 5   
   -   5        $ closePortButton��5      $��
Close Port��6*   $
��Left=�6^   $��86,182.129,0,0q�6y   $	��Top=�6�   $��75��6�   {�.  6�   �� 5   
   -   5         ��Name$ resultScroll��5      $
��Auto=�6-   $"��232.749,9.305,29.691,117.871q�6P   .��5      �� -	   5        	 ��Name$	 resultTextBlock��5      
�������$
��Wrap=�6D   $��Result��6X   $	��452��6f   5      �� 5   
   -
   5        $ clearButton��5      $��
Clear Data��6&   $
��Left=�6O   $��420,209.129,0,0q�6j   $	��Top=�6�   $��75��6�   	� 5   
   -   5        
 	�Name$
 responseRadio��5      $��Response��6-   $
��Left=�6@   $��86,106.381,0,0q�6[   $	��Top=�6s   
������� d� 	IsChecked$
 TrueY�6�   6�   	� 5   
   -   5        $
 continuousRadio��5      $��
Continuous��6/   $
��Left=�6D   $��86,122.381,0,0q�6_   $	��Top=�6w   
�������6�   �� 5    
   $��Receive Mode��6   $
��Left=�6'   $��86,80.381,0,0q�6B   $	��Top=�6Y   
�������6q   5!      5"      