Imports System.Text
Imports System.Runtime.Serialization
Imports System.Xml
Imports System.Net
Imports System.Net.Security

Class MainWindow

    Private x As CVS.ClaimVerifyServiceClient
    Public Shared Function ValidateCert() As Boolean

        Return True

    End Function


    Private Sub submitButton_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles submitButton.Click

        Dim req As New CVS.ClaimVerifyServiceRequest()

        ServicePointManager.ServerCertificateValidationCallback = New RemoteCertificateValidationCallback(AddressOf ValidateCert)
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12




        requestIdTextbox.Text = Guid.NewGuid().ToString()

        Try
            req.UserName = loginNameTextbox.Text
            req.UserPw = pwTextbox.Text
            If DateOfBirthTextbox.Text.Length > 0 Then
                req.DateOfBirth = DateOfBirthTextbox.Text
            
            End If

            req.FirstName = FirstNameTextbox.Text
            req.LastName = LastNameTextbox.Text
            req.SSN = ssnTextbox.Text
            req.RequestId = requestIdTextbox.Text
            requestXmlTextbox.Text = Serialize(req)

            Dim resp As CVS.ClaimVerifyServiceResponse = x.Verify(req)

            responseXmlTextbox.Text = Serialize(resp)
            statusTextbox.Text = resp.Status
            exceptionTextbox.Text = resp.Exception
            resultsGrid.DataContext = resp.ClaimList

        Catch ex As Exception
            MessageBox.Show(ex.Message)
            MessageBox.Show(ex.StackTrace)

        End Try

    End Sub

    Public Shared Function Serialize(Of T)(ByVal obj As T) As String

        Dim ser As New DataContractSerializer(GetType(T))

        Using backing As New System.IO.StringWriter()
            Using write As New System.Xml.XmlTextWriter(backing)
                ser.WriteObject(write, obj)
                Return backing.ToString()
            End Using

        End Using


    End Function

    Private Sub MainWindow_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        ' "dhsuser1" And pw = "DhsVerify1@" 
        x = New CVS.ClaimVerifyServiceClient()
        loginNameTextbox.Text = "dhsuser1"
        pwTextbox.Text = "DhsVerify1@"
        server.Content = x.Endpoint.Address
    End Sub
End Class
