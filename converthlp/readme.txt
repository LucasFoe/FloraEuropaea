Decompiling Existing HLP Files
https://www.helpscribble.com/decompiler.html
If you have an existing WinHelp file without the HPJ and RTF source files, 
you can download the WinHelp decompiler to recreate the HPJ and RTF sources 
from the HLP file. Extract the files inside helpdc21.zip into a new folder. 
Then you can easily decompile any .hlp file from the command prompt, by 
typing: helpdeco helpfile.hlp where helpfile.hlp is the help file you 
want to decompile. The decompiler will generate the HPJ and RTF files, 
along with a series of bitmap files if the help file contains images. 
Use Project|Import Help Project in HelpScribble to import the help file.
Even if you do have the HPJ and RTF sources of the HLP file, you still 
may want to use the decompiler. The RTF format is very loosely defined. 
Each and every application has its own interpretation of the format. 
Microsoft Word is notorious for generating messy RTF files. 
The decompiler on the other hand, generates very clean RTF files, 
which HelpScribble will import just fine. If HelpScribble has troubles 
importing the HPJ and RTF files exported by your previous help authoring 
tool, use the decompiler instead.
Note: The WinHelp decompiler is a freeware application written by Manfred 
Winterhoff. It is not part of the HelpScribble product. We provide it here 
for download as a courtesy to our customers.