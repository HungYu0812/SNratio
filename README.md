# SNratio
## Intoduction
This project is to calculate the signal-background ratio of microscope image. The programming language is C#, because it's speed is better than Python if the image is too large. 
We could download the directory bin/Debug/net5.0/. There are test file which is processed by ImageJ. One is for signal (ClearOutside.tif), and another is for backgorund. 
Click the SNratio.exe (You need to make sure that you have .net core in your computer) and enter the file name (test). It will output txt file. First column of txt file is signal and second is background.
