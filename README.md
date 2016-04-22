# RepeatApp - Short Description

This is a note taking application. When the app is first istalled a "First Notebook" will be created, in which notes can be saved. Each note has to belong to a notebook. In order to distinguish different notes content they can be placed in other notebooks.
A Notebooks has a name and a list of Notes.
A Note has a name and a content.
The next feature that I am working on is data Syncronization accross all devices.

I built a NotebookAPI, which will be the central database and a Syncronization Service, which handles the sync of the data.

For the moment the Xamarin android application offers CRUD operations on the notebook and notes.
The android app persists it's data in an SQLite database in android. The NotebookAPI uses SQL-Server.

For the moment I have 2 separate microservices:
* **NotebookAPI** - which offers synchronous communication over **REST**
* **Syncronization Service** - which offers asynchronous communication via **Event Messaging using Rabbit - MessageQueue**

The next step would be to add another module to handle User Management.