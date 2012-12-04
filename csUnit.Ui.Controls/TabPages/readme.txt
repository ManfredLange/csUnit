Yes, this folder is called TabPage although none of the controls contained in
here is a TabPage.

The reason is that the tab page control is dynamically filled during startup
and/or during runtime, e.g. through configuration.

At the same time it is desirable to be able to view and edit the tab pages in 
design mode. This cannot be done for tab pages themselves.

Therefore the content of each tab page is put in a UserControl, which can be
edited in design mode. In order to add the content to the csUnitControl, the
controls in this folder are put in a tab page each, and the tab page is then
added to the tab control. This process can also be performed and repeated
during runtime, e.g. to allow the user choosing which tab pages are visible.

[26Mar2006, ml]
