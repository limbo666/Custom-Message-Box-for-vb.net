
Imports System.Windows.Forms
Imports System.Drawing


Public Class ClassMessageBox

    '' <summary>
    ''' Displays a customizable message box with specified title, text, buttons, and optional features.
    ''' </summary>
    ''' <param name="title">The title of the message box.</param>
    ''' <param name="text">The main message text.</param>
    ''' <param name="footer">Optional footer text.</param>
    ''' <param name="buttons">Array of button labels. Defaults to {"OK"}.</param>
    ''' <param name="autoCloseTime">Time in seconds before auto-closing. Defaults to 0 (disabled).</param>
    ''' <param name="textColor">Color of the main text. Defaults to Black.</param>
    ''' <param name="textStyle">Font style of the main text. Defaults to Regular.</param>
    ''' <param name="footerColor">Color of the footer text. Defaults to Gray.</param>
    ''' <param name="footerStyle">Font style of the footer text. Defaults to Italic.</param>
    ''' <param name="imageType">Type of icon ("None", "Info", "Warning", "Error", "Critical", "Custom"). Defaults to "None".</param>
    ''' <param name="customImagePath">Path to a custom image for the icon. Required if imageType is "Custom".</param>
    ''' <param name="soundType">Type of system sound ("None", "Asterisk", "Beep", "Exclamation", "Hand", "Question"). Defaults to "None".</param>
    ''' <returns>The text of the clicked button or "AutoClose" if closed by timer.</returns>


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
        Optional ByVal soundType As String = "None") _
        As String

        ' Validation checks
        'If String.IsNullOrEmpty(title) OrElse String.IsNullOrEmpty(text) Then
        '    Throw New ArgumentException("Title and Text are required parameters.")
        'End If

        ' Validate soundType
        Dim validSoundTypes As String() = {"None", "Asterisk", "Beep", "Exclamation", "Hand", "Question"}
        If Not validSoundTypes.Contains(soundType, StringComparer.OrdinalIgnoreCase) Then
            Debug.WriteLine($"Warning: Invalid soundType '{soundType}', no sound will be played.")
            soundType = "None"
        Else
            soundType = soundType.ToLower()
        End If

        If String.IsNullOrEmpty(title) Then
            title = "Message" 'Set default value to avoid crash
            '  Throw New ArgumentException("Title and Text are required parameters.")
        End If
        If String.IsNullOrEmpty(text) Then
            text = "Text" 'Set default value to avoid crash
            '  Throw New ArgumentException("Title and Text are required parameters.")
        End If

        If autoCloseTime < 0 Then
            autoCloseTime = 0
            Debug.WriteLine("Warning: AutoCloseTime was negative, set to 0.")
        ElseIf autoCloseTime > 3600 Then ' 1 hour max
            autoCloseTime = 3600
            Debug.WriteLine("Warning: AutoCloseTime capped at 3600 seconds.")
        End If

        If buttons Is Nothing OrElse buttons.Length = 0 Then
            buttons = {"OK"}
        Else
            buttons = buttons.Select(Function(b) b?.Trim()).Where(Function(b) Not String.IsNullOrEmpty(b)).ToArray()
            If buttons.Length = 0 Then
                buttons = {"OK"}
            ElseIf buttons.Length > 5 Then
                buttons = buttons.Take(5).ToArray()
                Debug.WriteLine("Warning: Button count capped at 5.")
            End If
        End If

        ' Assign default colors and styles if not provided
        Dim finalTextColor As Color = textColor.GetValueOrDefault(Color.Black)
        Dim finalTextStyle As FontStyle = textStyle.GetValueOrDefault(FontStyle.Regular)
        Dim finalFooterColor As Color = footerColor.GetValueOrDefault(Color.Gray)
        Dim finalFooterStyle As FontStyle = footerStyle.GetValueOrDefault(FontStyle.Italic)

        ' Create the form
        Dim messageBoxForm As New Form() With {
            .Text = title,
            .FormBorderStyle = FormBorderStyle.FixedDialog,
            .StartPosition = FormStartPosition.CenterScreen,
            .TopMost = True,
            .MaximizeBox = False,
            .MinimizeBox = False,
            .Width = Screen.PrimaryScreen.WorkingArea.Width \ 5,
            .Height = CInt(Screen.PrimaryScreen.WorkingArea.Height * 0.2), ' Adjusted height
            .MinimumSize = New Size(300, 100) ' Adjusted minimum size
        }



        ' Add icon if provided
        Dim pictureBox As New PictureBox() With {
            .SizeMode = PictureBoxSizeMode.Zoom, ' Maintain aspect ratio
            .Padding = New Padding(10),
            .Dock = DockStyle.Left,
            .Width = 50,
            .Height = 50
        }
        Dim validImageTypes As String() = {"None", "Info", "Warning", "Error", "Critical", "Custom"}
        If Not validImageTypes.Contains(imageType, StringComparer.OrdinalIgnoreCase) Then
            pictureBox.Visible = False
            Debug.WriteLine($"Warning: Invalid imageType '{imageType}', hiding image.")
        Else

            Select Case imageType
                Case "Info"
                    pictureBox.Image = SystemIcons.Information.ToBitmap()
                Case "Warning"
                    pictureBox.Image = SystemIcons.Warning.ToBitmap()
                Case "Error"
                    pictureBox.Image = SystemIcons.Error.ToBitmap()
                Case "Critical"
                    pictureBox.Image = SystemIcons.Hand.ToBitmap()
                Case "Custom"
                    If String.IsNullOrEmpty(customImagePath) Then
                        pictureBox.Visible = False
                        Debug.WriteLine("Warning: Custom image path is empty, hiding image.")
                    ElseIf Not IO.File.Exists(customImagePath) Then
                        pictureBox.Visible = False
                        Debug.WriteLine($"Warning: Custom image file '{customImagePath}' does not exist, hiding image.")
                    Else
                        Dim validExtensions = {".png", ".jpg", ".jpeg", ".bmp", ".gif"}
                        Dim extension = IO.Path.GetExtension(customImagePath).ToLower()
                        If Not validExtensions.Contains(extension) Then
                            pictureBox.Visible = False
                            Debug.WriteLine($"Warning: Unsupported image format '{extension}' for '{customImagePath}', hiding image.")
                        Else
                            Try
                                Dim fileInfo As New IO.FileInfo(customImagePath)
                                If fileInfo.Length > 5 * 1024 * 1024 Then
                                    pictureBox.Visible = False
                                    Debug.WriteLine($"Warning: Custom image file '{customImagePath}' size ({fileInfo.Length} bytes) exceeds 5MB, hiding image.")
                                Else
                                    Using img As Image = Image.FromFile(customImagePath)
                                        If img.Width > 1024 OrElse img.Height > 1024 Then
                                            pictureBox.Visible = False
                                            Debug.WriteLine($"Warning: Custom image '{customImagePath}' dimensions ({img.Width}x{img.Height}) exceed 1024x1024, hiding image.")
                                        Else
                                            pictureBox.Image = New Bitmap(img) ' Create a copy to avoid file locking
                                        End If
                                    End Using
                                End If
                            Catch ex As Exception
                                pictureBox.Visible = False
                                Debug.WriteLine($"Warning: Failed to load custom image '{customImagePath}': {ex.Message}")
                            End Try
                        End If
                    End If
                Case Else
                    pictureBox.Visible = False
            End Select
        End If

        ' Create text label
        Dim textLabel As New Label() With {
            .Text = text,
            .Font = New Font("Segoe UI", 8, finalTextStyle), ' Matching default Windows message box font
            .ForeColor = finalTextColor,
            .AutoSize = True,
            .Padding = New Padding(8),
            .MaximumSize = New Size(messageBoxForm.Width - 100, 0) ' Wrap text if too long
        }

        ' Footer label
        Dim footerLabel As New Label() With {
            .Text = footer,
            .Font = New Font("Segoe UI", 8, finalFooterStyle), ' Matching Windows font
            .ForeColor = finalFooterColor,
            .AutoSize = True,
            .Padding = New Padding(8),
            .Dock = DockStyle.Bottom
        }

        ' Button panel aligned to the right
        Dim buttonPanel As New FlowLayoutPanel() With {
            .FlowDirection = FlowDirection.RightToLeft,
            .Dock = DockStyle.Bottom,
            .AutoSize = True,
            .Padding = New Padding(10, 0, 10, 10)
        }

        Dim userResponse As String = "AutoClose"
        Dim defaultButton As Button = Nothing
        ' Add keyboard support

        messageBoxForm.KeyPreview = True
        AddHandler messageBoxForm.KeyDown, Sub(sender, e)
                                               If e.KeyCode = Keys.Enter AndAlso defaultButton IsNot Nothing Then
                                                   defaultButton.PerformClick()
                                               ElseIf e.KeyCode = Keys.Escape Then
                                                   userResponse = ""
                                                   messageBoxForm.Close()
                                               End If
                                           End Sub


        For Each buttonText As String In buttons
            Dim button As New Button() With {
                .Text = buttonText,
                .AutoSize = True,
                .Padding = New Padding(10),
                .Height = 20 ' Adjusted button height
            }

            If defaultButton Is Nothing Then
                defaultButton = button
            End If

            AddHandler button.Click, Sub(sender, e)
                                         userResponse = buttonText
                                         messageBoxForm.Close()
                                     End Sub

            buttonPanel.Controls.Add(button)
        Next

        ' Add autoclose timer if applicable
        If autoCloseTime > 0 Then
            Dim countdown As Integer = autoCloseTime
            Dim timer As New Timer() With {.Interval = 1000}

            AddHandler timer.Tick, Sub()
                                       countdown -= 1
                                       defaultButton.Text = $"{defaultButton.Text.Split("("c)(0).Trim()} ({countdown})"
                                       If countdown = 0 Then
                                           timer.Stop()
                                           defaultButton.PerformClick()
                                       End If
                                   End Sub

            timer.Start()
        End If

        ' Layout components with vertical alignment logic
        Dim contentPanel As New TableLayoutPanel() With {
            .Dock = DockStyle.Fill,
            .ColumnCount = 2,
            .RowCount = 1,
            .AutoSize = True,
            .Padding = New Padding(10)
        }

        contentPanel.ColumnStyles.Add(New ColumnStyle(SizeType.AutoSize))
        contentPanel.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100))
        contentPanel.Controls.Add(pictureBox, 0, 0)
        contentPanel.Controls.Add(textLabel, 1, 0)

        ' Center the text label vertically
        textLabel.Anchor = AnchorStyles.None

        ' Add controls to the form
        messageBoxForm.Controls.Add(contentPanel)
        messageBoxForm.Controls.Add(buttonPanel)
        If Not String.IsNullOrEmpty(footer) Then
            messageBoxForm.Controls.Add(footerLabel)
        Else
            ' Ensure space for footer even if not visible
            Dim footerPlaceholder As New Panel() With {
                .Height = footerLabel.Height,
                .Dock = DockStyle.Bottom
            }
            messageBoxForm.Controls.Add(footerPlaceholder)
        End If

        ' Adjust form size based on text length
        messageBoxForm.AutoSize = True
        messageBoxForm.AutoSizeMode = AutoSizeMode.GrowAndShrink
        AddHandler messageBoxForm.FormClosed, Sub(sender, e)
                                                  If pictureBox.Image IsNot Nothing Then pictureBox.Image.Dispose()
                                                  If textLabel.Font IsNot Nothing Then textLabel.Font.Dispose()
                                                  If footerLabel.Font IsNot Nothing Then footerLabel.Font.Dispose()
                                                  messageBoxForm.Dispose()
                                              End Sub
        ' Show the form and capture response

        ' Play system sound before showing the form
        Select Case soundType
            Case "asterisk"
                System.Media.SystemSounds.Asterisk.Play()
            Case "beep"
                System.Media.SystemSounds.Beep.Play()
            Case "exclamation"
                System.Media.SystemSounds.Exclamation.Play()
            Case "hand"
                System.Media.SystemSounds.Hand.Play()
            Case "question"
                System.Media.SystemSounds.Question.Play()
        End Select


        messageBoxForm.ShowDialog()
        Return userResponse
    End Function
End Class
'
' Examples:
' Example 1: Basic Message Box with Default Settings
' - Shows a simple message box with only title and text, using default "OK" button, no image, no sound.
'Dim result = ClassMessageBox.ShowMessageBox(
'    title:="Simple Message",
'    text:="This is a basic message box with default settings."
')
'MessageBox.Show("User Response: " & result)
'
' Example 2: Information Message with Sound and Icon
' - Displays an informational message with a system "Info" icon, blue text, and a "Question" sound.
'Dim result = ClassMessageBox.ShowMessageBox(
'    title:="Information",
'    text:="Your operation completed successfully!",
'    footer:="Click OK to continue.",
'    buttons:={"OK"},
'    textColor:=Color.DarkBlue,
'    textStyle:=FontStyle.Bold,
'    imageType:="Info",
'    soundType:="Question"
')
'MessageBox.Show("User Response: " & result)
'
' Example 3: Warning Message with Auto-Close Timer
' - Shows a warning message with a "Warning" icon, multiple buttons, and a 10-second auto-close timer.
'Dim result = ClassMessageBox.ShowMessageBox(
'    title:="Warning",
'    text:="Proceed with caution! This action may overwrite data.",
'    footer:="Auto-closes in 10 seconds.",
'    buttons:={"Proceed", "Cancel"},
'    autoCloseTime:=10,
'    imageType:="Warning",
'    soundType:="Exclamation"
')
'MessageBox.Show("User Response: " & result)
'
' Example 4: Error Message with Custom Buttons and Footer
' - Displays an error message with a red "Error" icon, custom buttons, and a styled footer.
'Dim result = ClassMessageBox.ShowMessageBox(
'    title:="Error",
'    text:="An unexpected error occurred while processing your request.",
'    footer:="Contact support for assistance.",
'    buttons:={"Retry", "Ignore", "Abort"},
'    footerColor:=Color.Red,
'    footerStyle:=FontStyle.Italic,
'    imageType:="Error",
'    soundType:="Hand"
')
'MessageBox.Show("User Response: " & result)
