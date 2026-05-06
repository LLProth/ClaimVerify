Public Class ClaimSearch

    Public Shared Function SearchBySsn(ByVal request As ClaimVerifyServiceRequest, ByVal connectionString As String) As ClaimVerifyServiceResponse

        logUtility.LogMessage(" SearchBySsn() start", TraceLevel.Verbose, logUtility.LogSwitchType.generalMessage)
        Dim resp As New ClaimVerifyServiceResponse()
        resp.RequestId = request.RequestId

        Try
            '-- do lookup
            Dim verifier As New ClaimSearchDALNew(connectionString)
            Dim tableOfClaims As DataTable = verifier.LookupClaimBySsn(request.SSN)

            logUtility.LogMessage(String.Format(" SearchBySsn() {0} claim rows found", tableOfClaims.Rows.Count), TraceLevel.Verbose, logUtility.LogSwitchType.generalMessage)
            If tableOfClaims.Rows.Count > 0 Then
                Dim claimDataItem As Claim
                For Each claimRow As DataRow In tableOfClaims.Rows
                    claimDataItem = New Claim
                    claimDataItem.MiddleName = claimRow("mname").ToString()
                    claimDataItem.FirstName = claimRow("fname").ToString()
                    claimDataItem.LastName = claimRow("lname").ToString()
                    claimDataItem.DateOfBirth = CDate(claimRow("birthdate"))
                    claimDataItem.TypeOfInjury = claimRow("injury").ToString()
                    claimDataItem.InjuryDate = CDate(claimRow("injr_dtm"))
                    claimDataItem.EmployerName = claimRow("emplr_nm").ToString()
                    claimDataItem.ClaimStatus = decodeClaimStatus(claimRow("claim_sts").ToString())
                    claimDataItem.PayStatus = claimRow("Pay_status").ToString()
                    resp.ClaimList.Add(claimDataItem)
                Next
            End If
            resp.Status = "success"

        Catch ex As Exception
            resp.Status = "exception"
            resp.Exception = ex.Message
            logUtility.logException(ex, TraceLevel.Error, logUtility.LogSwitchType.generalMessage)
        End Try
        Return resp

    End Function

    Public Shared Function SearchByNameDob(ByVal request As ClaimVerifyServiceRequest, ByVal connectionString As String) As ClaimVerifyServiceResponse

        logUtility.LogMessage(" SearchByNameDob() start", TraceLevel.Verbose, logUtility.LogSwitchType.generalMessage)
        Dim resp As New ClaimVerifyServiceResponse()
        resp.RequestId = request.RequestId

        Try
            '-- do lookup
            Dim verifier As New ClaimSearchDALNew(connectionString)
            Dim tableOfClaims As DataTable = verifier.LookupClaimByName(request.FirstName, request.LastName, request.DateOfBirth)

            logUtility.LogMessage(String.Format(" SearchByNameDob() {0} claim rows found", tableOfClaims.Rows.Count), TraceLevel.Verbose, logUtility.LogSwitchType.generalMessage)
            If tableOfClaims.Rows.Count > 0 Then
                Dim claimDataItem As Claim
                For Each claimRow As DataRow In tableOfClaims.Rows
                    claimDataItem = New Claim
                    claimDataItem.MiddleName = claimRow("mname").ToString()
                    claimDataItem.FirstName = claimRow("fname").ToString()
                    claimDataItem.LastName = claimRow("lname").ToString()
                    claimDataItem.DateOfBirth = CDate(claimRow("birthdate"))
                    claimDataItem.TypeOfInjury = claimRow("injury").ToString()
                    claimDataItem.InjuryDate = CDate(claimRow("injr_dtm"))
                    claimDataItem.EmployerName = claimRow("emplr_nm").ToString()
                    claimDataItem.ClaimStatus = decodeClaimStatus(claimRow("claim_sts").ToString())
                    claimDataItem.PayStatus = claimRow("Pay_status").ToString()
                    resp.ClaimList.Add(claimDataItem)
                Next
            End If
            resp.Status = "success"

        Catch ex As Exception
            resp.Status = "exception"
            resp.Exception = ex.Message
            logUtility.logException(ex, TraceLevel.Error, logUtility.LogSwitchType.generalMessage)
        End Try
        Return resp

    End Function


    Private Shared Function decodeClaimStatus(ByVal statusName As String) As String


    Dim status As String = ""

    If statusName.ToUpper.Contains("ACCEPTED") Then
      status = "Accepted"
    ElseIf statusName.ToUpper.Contains("DENIED") Then
      status = "Denied"
    ElseIf statusName.ToUpper.Contains("PENDING") Then
      status = "Pending"
    Else
      status = ""
    End If
    logUtility.LogMessage(String.Format("    decodeClaimStatus {0} coded to {1}", statusName, status), TraceLevel.Verbose, logUtility.LogSwitchType.generalMessage)

    Return status

  End Function





End Class
