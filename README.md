# Napier Bank Messaging

Napier bank messaging application is used to parse messages input into the system into the type of message (SMS, Email, Tweet) and parse the message to a readable format for the user.

## Demo

https://user-images.githubusercontent.com/47451932/173249410-212fcb27-1ae5-47d6-9bdc-b3ba0c9e66a1.mp4

## Input Page

![image](https://user-images.githubusercontent.com/47451932/173249358-65f168dd-91a6-4a3b-96c9-fe517780e0a3.png)

The input page is the main page of the prototype. When the user starts the app, this will be the first page displayed to the user. From this page the user is able to:

•	Manually input a message header and message body into the respective textboxes then submit the input message by selecting the “Submit Message” button at the bottom of the window.

•	Select the browse button next to the select file textbox which will allow the user to select a file on their computer to input. Then they can submit the file by selecting the “Submit File” button to the right.

•	Select the “Stop Input” button which is coloured in red to change views to the list page where it will display any relevant information for all messages input into the system.

## List Page

![image](https://user-images.githubusercontent.com/47451932/173249475-b3f53beb-66cc-49e6-900b-a1c5df80d2ec.png)

The list page will display the contents of the trending list, mentions list, and SIR (significant incident report) in their respective box. The trending list will show the hashtag that was in a message on the left and how many times it has been seen. The mentions list will show the twitter IDs of users that have been mentioned in the message and the number of times it has been mentioned. The SIR list will show the sort codes and nature of incident pairing for each significant incident report that was input.

From this page the user is able to:

•	Select the “Back” button at the top left of the window this will take the user back to the input page where they can continue to input messages.

•	Select the “Save/Exit” button at the top right of the window-coloured red. This will open up the save file dialog where the user can enter in the name of the file, they wish to save the serialized message too.
