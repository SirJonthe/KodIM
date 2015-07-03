KodIM
=======

Author: Jonathan Karlsson (KodIM only)

About
-----
Send messages from your Windows/Linux/OSX machine to your Kodi/XBMC machines via IP or hostname. Uses the Notification interface accessed through the kodi-send part of the Kodi project.

Interdependence
---------------
Relies on kodi-send.py and its associated Python scripts (xbmcclient.py and xbmcclient.pyc). In order to run the application these scripts need to be present in the same folder as the executable. The scripts are located in project_folder/Scripts.

Compiling
---------
* You need Gtk (for mono C#), and mono. MonoDevelop is optional, but the rest of the instructions assume you use it to compile the project.
* When these requirements are met you simply need to open the project (project_folder/KodIM.sln) in MonoDevelop and press compile.
* I don't know if it is possible to compile under Visual Studio (probably not).
* Only tested on Linux for now, although if you have met the requirements I see no reason why other platforms should not work.

Running
-------
* You need to install Python (scripts work under version 2.7 but should hopefully work on both older and newer versions of Python).
* The "python" executable needs to be included in your environment variables so it is available globally. The Python installation process should do this for you. To test is Python is available simply open a terminal and type "python --version" and see if you encounter an error or if Python gives you a version number (Python is installed if a version number is printed).
* Don't forget to copy the contents of project_folder/Scripts and place them in the same folder as your executable.
* If the requirements are met, run the executable.
* If history is not recorded or loaded, make sure the working directory is set to the location of the KodIM executable when you run the application.

KNOWN BUGS
----------
* The scripts seems to return success even if the host or IP the message is sent to does not exist or is not an XBMC/Kodi machine, meaning the status label is useless at this point.

Legal notice
------------
Copyright Jonathan Karlsson 2015 (KodIM only)

* Licensed under GPL since kodi-send is as well (see project_folder/LICENSE.GPL for info).
* I take no responsibility for any damages arising directly or indirectly from the use of this program, its source code, or third-party components if the application.
