# Custom Message Box for vb.net
A reusable class to display custom message box on vb.net forms apps.
## Features
1.	Customizable Appearance:
<br>	Modify text and footer styles (font, color).
<br>	Add optional icons (predefined or custom).
2.	Dynamic Resizing:
<br>	Automatically adjusts the size based on the text length.
3.	Auto-Close Functionality:
<br>	Close the message box after a specified time.
4.	User Interaction:
<br>	Return the button clicked by the user (even the custom ones).
5.	Error Handling:
<br>	Validates input parameters to prevent errors

## Examples
![image](https://github.com/user-attachments/assets/3a1dad19-6f78-4174-90c9-d5d9044379ba)\
*A message box with colored text footer, custom buttons and autoclose timer*\
<br>
![image](https://github.com/user-attachments/assets/3f58a5dc-5b83-4da2-92c8-a18318185f0e)\
*A highly customized message box with colored text footer, custom buttons and autoclose timer*\
<br>
![image](https://github.com/user-attachments/assets/0ad7b0ec-8a4c-4430-878e-e65a4aa78574)\
*A simple custom message box*
<br>
## Documentation for ClassMessageBox
The ClassMessageBox class provides a reusable, customizable message box for .NET applications. It allows you to create message boxes with advanced features, such as custom icons, text styles, auto-close timers, and dynamic resizing.
________________________________________________________________________________________________________________________
## Class Overview
### Namespace
Ensure this class is included in your project and imported as necessary.
### Method
Public Shared Function ShowMessageBox(
    ByVal title As String,
    ByVal text As String,
    Optional ByVal footer As String = "",
    Optional ByVal buttons As String() = Nothing,
    Optional ByVal autoCloseTime As Integer = 0,
    Optional ByVal textColor As Nullable(Of Color) = Nothing,
    Optional ByVal textStyle As Nullable(Of FontStyle) = Nothing,
    Optional ByVal footerColor As Nullable(Of Color) = Nothing,
    Optional ByVal footerStyle As Nullable(Of FontStyle) = Nothing,
    Optional ByVal imageType As String = "None",
    Optional ByVal customImagePath As String = "",
    Optional ByVal soudType As String = ""
) As String

---

________________________________________________________________________________________________________________________
## Parameters
### Required Parameters
1.	title (String)
o	Specifies the title of the message box.
o	*Example:* `"Warning"` or `"Info"`
2.	text (String)
o	The main message content to display.
o	*Example:* `"An error occurred during the operation."`

### Optional Parameters
1.	footer (String)
<br>	A footer message, often used for additional instructions or disclaimers.
<br>	*Default:* Empty string (`""`).
<br>	*Example:* `"Please contact support if the issue persists."`
2.	buttons (String[])
<br>	An array of strings representing the buttons to display. Each button is clickable and returns its respective text when clicked.
<br>	*Default:* `{ "OK" }`
<br>	*Example:* `{ "Retry", "Cancel" }`
3.	autoCloseTime (Integer)
<br>	Specifies the duration (in seconds) before the message box auto-closes. If 0, auto-close is disabled.
<br>	*Default:* `0`
<br>	*Example:* `10` (message box auto-closes in 10 seconds).
4.	textColor (Nullable(Of Color))
<br>	The color of the main text.
<br>	*Default:* `Black`
<br>	*Example:* `Color.Red`
5.	textStyle (Nullable(Of FontStyle))
<br>	The font style of the main text.
<br>	*Default:* `FontStyle.Regular`
<br>	*Example:* `FontStyle.Bold`
6.	footerColor (Nullable(Of Color))
<br>	The color of the footer text.
<br>	*Default:* `Gray`
<br>	*Example:* `Color.Blue`
7.	footerStyle (Nullable(Of FontStyle))
<br>	The font style of the footer text.
<br>	*Default:* `FontStyle.Italic`
<br>	*Example:* `FontStyle.Bold`
8.	imageType (String)
<br>	Specifies a predefined icon to display on the left side of the message box.
<br>	Supported values: 
<br>	`"None"` (No image)
<br>	`"Info"`
<br>	`"Warning"`
<br>	`"Error"`
<br>	`"Critical"`
<br>	`"Custom"` (Use a custom image path)
<br>	*Default:* `"None"`.
9.	customImagePath (String)
<br>	Specifies the path to a custom image to display. Only valid if imageType is set to "Custom".
<br>	*Default:* Empty string (`""`).
<br>	*Example:* `"C:\Images\CustomIcon.png"`
10.	 soundType (String)
<br>	Type of system sound ("None", "Asterisk", "Beep", "Exclamation", "Hand", "Question").
<br>	*Default:* Empty string (`""`).
<br>	*Example:* `"Beep"`
<br>	Supported values: 
<br>	`"None"` (No image)
<br>	`"Asterisk"`
<br>	`"Beep"`
<br>	`"Exclamation"`
<br>	`"Hand"`
<br>	`"Question"`
________________________________________________________________________________________________________________________
### Return Value
The method returns a String representing the button clicked by the user or "AutoClose" if no option is selected by the user.
________________________________________________________________________________________________________________________
### How to use
Download the class file from [Class folder](https://github.com/limbo666/Custom-Message-Box-for-vb.net/tree/main/Class).<br>
Add the class to your vb.net project\
Call the message box in your code by stating:  `ClassMessageBox.ShowMessageBox`\
<br>

*Example 0:* 
```
Dim result = ClassMessageBox.ShowMessageBox(
    title:="Simple Message",
    text:="This is a basic message box with default settings."
)
```



*Example 1:* 
```
Dim result = ClassMessageBox.ShowMessageBox(
    title:="Information",
    text:="Your operation completed successfully!",
    footer:="Click OK to continue.",
    buttons:={"OK"},
    textColor:=Color.DarkBlue,
    textStyle:=FontStyle.Bold,
    imageType:="Info",
    soundType:="Question"
)
```

*Example 2:* 
```
Dim result = ClassMessageBox.ShowMessageBox(
    title:="Warning",
    text:="Proceed with caution! This action may overwrite data.",
    footer:="Auto-closes in 10 seconds.",
    buttons:={"Proceed", "Cancel"},
    autoCloseTime:=10,
    imageType:="Warning",
    soundType:="Exclamation"
)
MessageBox.Show("User Response: " & result)
```

*Example 3:* 
```
Dim result = ClassMessageBox.ShowMessageBox(
    title:="Error",
    text:="An unexpected error occurred while processing your request.",
    footer:="Contact support for assistance.",
    buttons:={"Retry", "Ignore", "Abort"},
    footerColor:=Color.Red,
    footerStyle:=FontStyle.Italic,
    imageType:="Error",
    soundType:="Hand"
)
MessageBox.Show("User Response: " & result)
```

________________________________________________________________________________________________________________________

