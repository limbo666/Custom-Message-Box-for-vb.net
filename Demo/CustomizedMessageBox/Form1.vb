Public Class Form1
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        ' Example 1: Basic usage with title and text
        ClassMessageBox.ShowMessageBox("Welcome", "This is a simple message box.")


    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        ' Example 2: Message box with a footer and multiple buttons
        ClassMessageBox.ShowMessageBox(
        "Confirmation",
        "Are you sure you want to proceed?",
        buttons:=New String() {"Yes", "No", "Cancel"}
    )
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click

        ' Example 3: Message box with an autoclose timer
        ClassMessageBox.ShowMessageBox(
    "Timeout Warning",
    "This message box will close in 5 seconds.",
    autoCloseTime:=5
)
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        ' Example 4: Message box with custom image and styles
        ClassMessageBox.ShowMessageBox("Error",
    "An unexpected error occurred.",
    imageType:="Error"
)
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Dim result = ClassMessageBox.ShowMessageBox(
    "Warning",
    "An issue was detected during the operation.",
    footer:="Please contact support if this persists.",
    imageType:="Warning",
    buttons:=New String() {"Retry", "Cancel"}
)
        MessageBox.Show("User Response: " & result)
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        Dim result = ClassMessageBox.ShowMessageBox(
    "Choose an Option",
    "Select one of the following options:",
    buttons:=New String() {"Yes", "No", "Cancel"}
)
        MessageBox.Show("User Response: " & result)
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        Dim result = ClassMessageBox.ShowMessageBox(
    "Important Information",
    "This is a long message to test how the message box dynamically resizes itself to accommodate all the text provided. Ensure it wraps properly."
)
        MessageBox.Show("User Response: " & result)

    End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        Dim result = ClassMessageBox.ShowMessageBox(
            "Welcome",
            "This message box demonstrates centered text alignment with appropriate sizing adjustments.", "This is a customizable messagebox", {"OK"}, 0, Color.Blue,, Color.Purple
        )
        MessageBox.Show("User Response: " & result)

    End Sub

    Private Sub Button9_Click(sender As Object, e As EventArgs) Handles Button9.Click
        Dim result =
        ClassMessageBox.ShowMessageBox("Title", "This is a lorem ipsum text that goes along long long way. The text should be dispalyed in the message box. Please check the message box for the text. Thank you.", "Created by Limbo", {"Yes", "No", "Maybe"}, 15, Color.Purple)
        MessageBox.Show("User Response: " & result)
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub Button10_Click(sender As Object, e As EventArgs) Handles Button10.Click
        Dim result = ClassMessageBox.ShowMessageBox("Custom Information Message Box",
                                                    "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.",
                                                    "This is a demo message with long bold colored text custom button options and autoclose enabled", {"Me", "You", "Someone Else", "No One"}, 30, Color.DarkBlue, FontStyle.Bold, Color.Fuchsia, FontStyle.Italic, "Info")
        MessageBox.Show("User Response: " & result)

    End Sub

    Private Sub Button11_Click(sender As Object, e As EventArgs)
        Dim result = ClassMessageBox.ShowMessageBox("Warning", "An issue was detected during the operation.", footer:="Please contact support if this persists.", buttons:=New String() {"Retry", "Cancel"}, imageType:="Warning")
    End Sub
End Class
