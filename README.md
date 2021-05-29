# Introduction
GUIFE is an alternative front end for Flora Europaea on CD-ROM. Flora Europaea on CD-ROM is edited by The Flora 
Europaea Editorial Committee, prepared for publication by Siebe Jorna, published June 2001 (CD-ROM, ISBN-13: 9780521778114 | ISBN-10: 0521778115, Cambridge University Press).

The original software is implemented in VB4 and uses a window of 800x600 pixels on the screen. It doesn’t run on some systems 
with more than 500MB RAM (as on my system). I decided therefore to write another GUI based on new technology (NET 4.6, VB.NET). GUIFE only works with the original 
data and software distributed by Cambridge University Press.

Feature Overview of GUIFE:
•	Text browser (with taxon index)
•	Full text search (only simple search for a single word)
•	Search within page
•	Key browser
•	Glossary browser

The following features of the original software are not implemented:
•	Boolean search
•	Possible outcome of key

For context-sensitive help, drag ‘?’ from the top right corner of the current windows over the GUI element you need help for and click on it.

To show the definition of a term in a text page double click on it. If there is an entry in the glossary, the definition appears in the small text box on the left side of the window. You can also select a term in the text window and click on the “Glossary” button to get the definition of it.

If you double click on the name of a taxon for which there is a determination key in the database, the key browser window opens. The same happens if you select a taxons name in the text box and click the “Key” button.

If you click on the “Key” button and there is no active selection, the program looks for the key of the selected taxon in the index panel.

On the lower left part, you can see messages showing you what happens behind the scene.

# Menu Reference
## File
- Page Setup
   - Define page orientation, page format and printer
- Print preview
   - Shows a preview of print on screen
- Print
   - Print the text page shown in the main text box

## Edit
- Copy
   - Copies the select text to the clipboard
- Paste
   - Paste the content of the clipboard

- Select all
   - Select whole content

- Configuration
   - Starts configuration dialog

## Data
- Introduction
   - Introduction, abbreviations, maps and general information about the Flora Europaea project
- Browse Key
   - Key browser
- Browse Glossary
   - Glossary browser
- Help
   - Help Information

