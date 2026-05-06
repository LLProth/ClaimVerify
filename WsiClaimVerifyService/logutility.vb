Imports System.Diagnostics
Imports System.Text



Public Class logUtility

    Private Shared DataMessagesSwitch As TraceSwitch = New TraceSwitch("DataMessagesSwitch", "General  App Trace Switch")
    Private Shared ServiceMessageSwitch As TraceSwitch = New TraceSwitch("ServiceMessageSwitch", "Data method logging Trace Switch")

    Public Enum LogSwitchType As Integer
        dataMessage
        generalMessage

    End Enum

    Public Shared Sub LogMessage(ByVal message As String, ByVal level As TraceLevel, ByVal lst As LogSwitchType)

        Try
            Dim switchToUse As TraceSwitch
            If lst = LogSwitchType.generalMessage Then
                switchToUse = ServiceMessageSwitch
            ElseIf lst = LogSwitchType.dataMessage Then
                switchToUse = DataMessagesSwitch
            Else
                Throw New ApplicationException("switch value not valid")
            End If

            Select Case level
                Case TraceLevel.Error
                    If switchToUse.TraceError Then
                        writeTraceMsg(message, level)
                    End If
                Case TraceLevel.Info
                    If switchToUse.TraceInfo Then
                        writeTraceMsg(message, level)
                    End If
                Case TraceLevel.Verbose
                    If switchToUse.TraceVerbose Then
                        writeTraceMsg(message, level)
                    End If
                Case TraceLevel.Warning
                    If switchToUse.TraceWarning Then
                        writeTraceMsg(message, level)
                    End If

            End Select

        Catch 
        End Try

    End Sub

    Private Shared Sub writeTraceMsg(ByVal message As String, ByVal level As TraceLevel)

        Try

            Dim formattedMsg As String = String.Empty
            Select Case level
                Case TraceLevel.Error
                    formattedMsg = "Error:"
                Case TraceLevel.Info
                    formattedMsg = "Info:"
                Case TraceLevel.Off
                    formattedMsg = String.Empty
                Case TraceLevel.Verbose
                    formattedMsg = "Verbose:"
                Case TraceLevel.Warning
                    formattedMsg = "Warning"
            End Select
            formattedMsg = String.Format("{0}:{1} {2} {3}", formattedMsg, DateTime.Now.ToShortDateString(), DateTime.Now.ToShortTimeString(), message)

            ' Trace.WriteLine("-----")
            Trace.WriteLine(formattedMsg)
            Trace.Flush()


        Catch

        End Try


    End Sub

    Public Shared Sub logException(ByVal ex As Exception, ByVal level As TraceLevel, ByVal lst As LogSwitchType)

        Try

            LogMessage(exceptionText(ex), level, lst)

        Catch

        End Try


    End Sub

    Private Shared Function exceptionText(ByVal ex As Exception) As String

        Dim s As String = ""
        If ex.InnerException IsNot Nothing Then
            s = exceptionText(ex.InnerException)
        End If

        s = String.Format("{1}{0}{2}{0}{3}", Environment.NewLine, s, ex.Message, ex.StackTrace)
        Return s

    End Function


    Public Shared Function XmlFromObject(Of T)(ByVal obj As T) As String

        Dim resultXml As String = ""
        Try

            Dim ser As New DataContractSerializer(GetType(T))
            Using backing As New System.IO.StringWriter()
                Using write As New System.Xml.XmlTextWriter(backing)
                    ser.WriteObject(write, obj)
                    resultXml = backing.ToString()
                End Using

            End Using
        Catch
        End Try
        Return resultXml

    End Function
End Class
