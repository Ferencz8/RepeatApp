# RepeatApp - Short Description

This is a note taking application. When the app is first istalled a "First Notebook" will be created, in which notes can be saved. Each note has to belong to a notebook. In order to distinguish different notes content they can be placed in other notebooks.
A Notebooks has a name and a list of Notes.
A Note has a name and a content.
The next feature that I am working on is data Syncronization accross all devices.

I built a NotebookAPI, which will be the central database and a Syncronization Service, which handles the sync of the data.

The project consists of 2 servers and 2 clients.  
The clients are mobile (Xamarin - Android) and web (Node JS) and have the following functionalities:
•	 Authentication using an External Service
•	 Data stored on a local database (only for mobile client). - SQLite
•	 Synchronization of data with Web API. 
The servers are: 
•	 Web API, which serves as a central synchronized point for data to be stored.
•	 Synchronizing Service, which handles the data synchronization (C#)


The architecture:
![alt tag](https://github.com/Ferencz8/RepeatApp/blob/Authentication_Authorization/Images/L.png)