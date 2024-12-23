Imports System.Drawing
Imports System.Windows.Forms

Public Class ClassMessageBox
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
        Optional ByVal customImagePath As String = ""
    ) As String

        ' Validation checks
        'If String.IsNullOrEmpty(title) OrElse String.IsNullOrEmpty(text) Then
        '    Throw New ArgumentException("Title and Text are required parameters.")
        'End If
        If String.IsNullOrEmpty(title) Then
            title = "Message" 'Set default value to avoid crash
            '  Throw New ArgumentException("Title and Text are required parameters.")
        End If
        If String.IsNullOrEmpty(text) Then
            text = "Text" 'Set default value to avoid crash
            '  Throw New ArgumentException("Title and Text are required parameters.")
        End If

        If autoCloseTime < 0 Then
            autoCloseTime = 0 'Set default value to zero in order to avoid crash by disapbing autoclose
            'Throw New ArgumentException("AutoCloseTime cannot be negative.")
        End If

        If buttons Is Nothing OrElse buttons.Length = 0 Then
            buttons = {"OK"} ' Default to a single "OK" button
        End If

        ' Assign default colors and styles if not provided
        Dim finalTextColor As Color = If(textColor, Color.Black)
        Dim finalTextStyle As FontStyle = If(textStyle, FontStyle.Regular)
        Dim finalFooterColor As Color = If(footerColor, Color.Gray)
        Dim finalFooterStyle As FontStyle = If(footerStyle, FontStyle.Italic)

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
                If IO.File.Exists(customImagePath) Then
                    pictureBox.Image = Image.FromFile(customImagePath)
                Else
                    Throw New ArgumentException("Custom image path is invalid or file does not exist.")
                End If
            Case Else
                pictureBox.Visible = False
        End Select

        ' Create text label
        Dim textLabel As New Label() With {
            .Text = text,
            .Font = New Font("Tahoma", 8, finalTextStyle), ' Matching default Windows message box font
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

        For Each buttonText As String In buttons
            Dim button As New Button() With {
                .Text = buttonText,
                .AutoSize = True,
                .Padding = New Padding(10),
                .Height = 40 ' Adjusted button height
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

        ' Show the form and capture response
        messageBoxForm.ShowDialog()
        Return userResponse
    End Function
End Class
'
' Examples:
'1.
'      Dim result = ClassMessageBox.ShowMessageBox("Custom Information Message Box",
'                                                    "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.",
'                                                   "This is a demo message with long bold colored text custom button options and autoclose enabled", {"Me", "You", "Someone Else", "No One"}, 30, Color.DarkBlue, FontStyle.Bold, Color.Fuchsia, FontStyle.Italic, "Info")
'      MessageBox.Show("User Response: " & result)

'2.
'    Dim result = ClassMessageBox.ShowMessageBox(
'            "Welcome",
'            "This message box demonstrates centered text alignment with appropriate sizing adjustments.", "This is a customizable messagebox", {"OK"}, 0, Color.Blue,, Color.Purple
'    )
'   MessageBox.Show("User Response: " & result)

'3.
'      ClassMessageBox.ShowMessageBox("Error",
'       "An unexpected error occurred.",
'       imageType:="Error"