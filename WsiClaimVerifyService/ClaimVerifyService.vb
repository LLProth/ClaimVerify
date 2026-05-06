Imports System.Configuration

' NOTE: You can use the "Rename" command on the context menu to change the class name "Service1" in both code and config file together.
Public Class ClaimVerifyService
    Implements IClaimVerifyService

    Private Shared loggingSource As New TraceSource("wsiClaimVerifyService")

    Public Function Verify(ByVal requestData As ClaimVerifyServiceRequest) As ClaimVerifyServiceResponse Implements IClaimVerifyService.Verify

        logUtility.LogMessage("------------------- Start Verify -------------------", TraceLevel.Info, logUtility.LogSwitchType.generalMessage)
        logUtility.LogMessage(logUtility.XmlFromObject(requestData), TraceLevel.Verbose, logUtility.LogSwitchType.generalMessage)

        Dim connectionString As New String(ConfigurationManager.ConnectionStrings("ConnectionString" & ConfigurationManager.AppSettings("CurrentEnvironment").ToString).ToString)
        Dim returnValue As New ClaimVerifyServiceResponse
        Try
            '-- validate input
            '-- first validate user/pw
            If requestData.UserName IsNot Nothing AndAlso requestData.UserPw IsNot Nothing AndAlso validateUser(requestData.UserName, requestData.UserPw) Then
                '-- search database
                If requestData.SSN.Trim.Length > 0 Then
                    returnValue = ClaimSearch.SearchBySsn(requestData, connectionString)
                ElseIf requestData.FirstName IsNot Nothing AndAlso requestData.LastName IsNot Nothing AndAlso requestData.DateOfBirth > Date.MinValue Then
                    returnValue = ClaimSearch.SearchByNameDob(requestData, connectionString)
                Else
                    Throw New ArgumentException("Either SSN, or Name and DOB is required")
                End If
            Else
                Throw New ArgumentException("Invalid User/PW")
            End If

        Catch ex As Exception
            returnValue.Status = "exception"
            returnValue.Exception = ex.Message()
            logUtility.logException(ex, TraceLevel.Error, logUtility.LogSwitchType.generalMessage)

        Finally
            logUtility.LogMessage(logUtility.XmlFromObject(returnValue), TraceLevel.Verbose, logUtility.LogSwitchType.generalMessage)

        End Try

        Return returnValue


    End Function


    ''' <summary>
    ''' Tests user name and password to verify they are authentic
    ''' </summary>
    ''' <param name="user"></param>
    ''' <param name="pw"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function validateUser(ByVal user As String, ByVal pw As String) As Boolean

        Dim result As Boolean = False

        If user.ToLower() = "wsitestuser" And pw = "wsitest123" Then
            result = True
        ElseIf user.ToLower = "dhsuser1" And pw = "DhsVerify1@" Then
            result = True
        End If

        Return result

    End Function



End Class
