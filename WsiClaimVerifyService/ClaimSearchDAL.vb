Option Explicit On
Option Strict On

'Imports WSI.Utility.DbAccess
Imports WSI.Utility.Database
Imports System.Configuration
Imports System.Text
Imports WsiMvvmTools

'Public Class ClaimSearchDAL
'    Inherits DALBase

'    Public Sub New()
'        MyBase.New("claimLookupData", True)

'        logUtility.LogMessage("Created ClaimSearchDAL", TraceLevel.Info, logUtility.LogSwitchType.dataMessage)
'        logUtility.LogMessage(String.Format(" connection: {0}", MyBase.ConnectionString), TraceLevel.Verbose, logUtility.LogSwitchType.dataMessage)

'    End Sub

'    Public Function LookupClaimBySsn(ByVal ssn As String) As DataTable

'        Try
'            Dim selectSql As String = claimLookupQuery()
'            Dim parmNames As String() = {":ssn", "lname", "fname", "dob"}
'            Dim parmValues As Object() = {ssn, "", "", DBNull.Value}
'            Dim returnData As DataSet = MyBase.ExecuteSelect(selectSql, parmNames, parmValues)
'            Return returnData.Tables(0)

'        Catch ex As Exception
'            Throw New ApplicationException("There was an error accessing the WSI Claims database. ", ex)

'        End Try

'    End Function

'    Public Function LookupClaimByName(ByVal fname As String, ByVal lname As String, ByVal dob As Date) As DataTable

'        Try

'            Dim selectSql As String = claimLookupQuery()

'            Dim parmNames As String() = {":ssn", "lname", "fname", "dob"}
'            Dim parmValues As Object() = {DBNull.Value, lname, fname, dob.ToString("yyyy-MM-dd")}
'            Dim returnData As DataSet = MyBase.ExecuteSelect(selectSql, parmNames, parmValues)

'            Return returnData.Tables(0)

'        Catch ex As Exception
'            Throw New ApplicationException("There was an error accessing the WSI Claims database. ", ex)

'        End Try


'    End Function

'    Private Function claimLookupQuery() As String

'        Dim sql As New StringBuilder("Select DISTINCT ")
'        sql.AppendLine(" TRIM(ph.prsn_hist_nm_fst) fname, ")
'        sql.AppendLine(" TRIM(ph.prsn_hist_nm_mid) mname, ")
'        sql.AppendLine(" TRIM(ph.prsn_hist_nm_lst) lname, ")
'        sql.AppendLine(" ph.prsn_hist_brth_dt birthdate, ")
'        sql.AppendLine(" pob.pob_nm injury, ")
'        sql.AppendLine(" i.injr_dtm, ")
'        sql.AppendLine(" le.lgl_enty_drv_nm emplr_nm, ")
'        sql.AppendLine(" med.f_get_cur_clm_sts(i.injr_id) claim_sts, ")
'        sql.AppendLine(" med.f_clm_sts_actv(i.injr_id) pay_status, ")
'        sql.AppendLine(" ph.prsn_hist_ssn ssn ")
'        sql.AppendLine("FROM person_history ph, injury i, injury_part_of_body ipob, part_of_body pob, legal_entity le ")
'        sql.AppendLine("WHERE prsn_hist_ssn = :ssn  ")
'        sql.AppendLine("    AND ph.lgl_enty_id_prsn = i.lgl_enty_id_clmt ")
'        sql.AppendLine("    AND i.injr_id = ipob.injr_id ")
'        sql.AppendLine("    AND ipob.pob_cd = pob.pob_cd ")
'        sql.AppendLine("    AND i.lgl_enty_id_emplr = le.lgl_enty_id ")
'        sql.AppendLine("    AND ph.prsn_hist_end_dt is null ")
'        sql.AppendLine("    AND ipob.injr_pob_pri_ind = 'y' ")
'        sql.AppendLine("UNION ")
'        sql.AppendLine("Select DISTINCT ")
'        '     sql.AppendLine("         TRIM(TRIM(ph.prsn_hist_nm_fst) || ' ' || TRIM(ph.prsn_hist_nm_mid)) || ' ' || TRIM(ph.prsn_hist_nm_lst) clmt_nm, ")
'        sql.AppendLine(" TRIM(ph.prsn_hist_nm_fst) fname, ")
'        sql.AppendLine(" TRIM(ph.prsn_hist_nm_mid) mname, ")
'        sql.AppendLine(" TRIM(ph.prsn_hist_nm_lst) lname, ")
'        sql.AppendLine("         ph.prsn_hist_brth_dt birthdate, ")
'        sql.AppendLine("         pob.pob_nm injury, ")
'        sql.AppendLine("         i.injr_dtm, ")
'        sql.AppendLine("         le.lgl_enty_drv_nm emplr_nm, ")
'        sql.AppendLine("         med.f_get_cur_clm_sts(i.injr_id) claim_sts, ")
'        sql.AppendLine("         med.f_clm_sts_actv(i.injr_id) pay_status, ")
'        sql.AppendLine("         NULL ssn ")
'        sql.AppendLine("  FROM person_history ph, injury i, injury_part_of_body ipob, part_of_body pob, legal_entity le ")
'        sql.AppendLine("WHERE UPPER(prsn_hist_nm_lst) = UPPER(:lname) ")
'        sql.AppendLine("    AND UPPER(prsn_hist_nm_fst) = UPPER(:fname) ")
'        sql.AppendLine("    AND TRUNC(prsn_hist_brth_dt) = :dob ")
'        sql.AppendLine("    AND ph.lgl_enty_id_prsn = i.lgl_enty_id_clmt ")
'        sql.AppendLine("    AND i.injr_id = ipob.injr_id ")
'        sql.AppendLine("    AND ipob.pob_cd = pob.pob_cd ")
'        sql.AppendLine("    AND i.lgl_enty_id_emplr = le.lgl_enty_id ")
'        sql.AppendLine("    AND ph.prsn_hist_end_dt is null ")
'        sql.AppendLine("    AND ipob.injr_pob_pri_ind = 'y' ")

'        Return sql.ToString()

'    End Function

'End Class

Public Class ClaimSearchDALNew
    Private _dbService As DatabaseService

    Public Sub New(ByVal connectionString As String)
        _dbService = New DatabaseService
        _dbService.ConnectionString = connectionString

        logUtility.LogMessage("Created ClaimSearchDAL", TraceLevel.Info, logUtility.LogSwitchType.dataMessage)
        logUtility.LogMessage(String.Format(" connection: {0}", _dbService.ConnectionString), TraceLevel.Verbose, logUtility.LogSwitchType.dataMessage)

    End Sub

    Public Function LookupClaimBySsn(ByVal ssn As String) As DataTable

        Try
            Dim selectSql As String = claimLookupQuery()
            Dim parmNames As String() = {":ssn", "lname", "fname", "dob"}
            Dim parmValues As Object() = {ssn, "", "", DBNull.Value}
            Dim returnData As DataTable = _dbService.ExecuteSelect(selectSql, parmNames, parmValues)
            Return returnData

        Catch ex As Exception
            Throw New ApplicationException("There was an error accessing the WSI Claims database. ", ex)

        End Try

    End Function

    Public Function LookupClaimByName(ByVal fname As String, ByVal lname As String, ByVal dob As Date) As DataTable

        Try

            Dim selectSql As String = claimLookupQuery()

            Dim parmNames As String() = {":ssn", "lname", "fname", "dob"}
            Dim parmValues As Object() = {DBNull.Value, lname, fname, dob.ToString("dd-MMM-yyyy")}
            Dim returnData As DataTable = _dbService.ExecuteSelect(selectSql, parmNames, parmValues)

            Return returnData

        Catch ex As Exception
            Throw New ApplicationException("There was an error accessing the WSI Claims database. ", ex)

        End Try


    End Function

    Private Function claimLookupQuery() As String

        Dim sql As New StringBuilder("Select DISTINCT ")
        sql.AppendLine(" TRIM(ph.prsn_hist_nm_fst) fname, ")
        sql.AppendLine(" TRIM(ph.prsn_hist_nm_mid) mname, ")
        sql.AppendLine(" TRIM(ph.prsn_hist_nm_lst) lname, ")
        sql.AppendLine(" ph.prsn_hist_brth_dt birthdate, ")
        sql.AppendLine(" pob.pob_nm injury, ")
        sql.AppendLine(" i.injr_dtm, ")
        sql.AppendLine(" le.lgl_enty_drv_nm emplr_nm, ")
        sql.AppendLine(" med.f_get_cur_clm_sts(i.injr_id) claim_sts, ")
        sql.AppendLine(" med.f_clm_sts_actv(i.injr_id) pay_status, ")
        sql.AppendLine(" ph.prsn_hist_ssn ssn ")
        sql.AppendLine("FROM person_history ph, injury i, injury_part_of_body ipob, part_of_body pob, legal_entity le ")
        sql.AppendLine("WHERE prsn_hist_ssn = :ssn  ")
        sql.AppendLine("    AND ph.lgl_enty_id_prsn = i.lgl_enty_id_clmt ")
        sql.AppendLine("    AND i.injr_id = ipob.injr_id ")
        sql.AppendLine("    AND ipob.pob_cd = pob.pob_cd ")
        sql.AppendLine("    AND i.lgl_enty_id_emplr = le.lgl_enty_id ")
        sql.AppendLine("    AND ph.prsn_hist_end_dt is null ")
        sql.AppendLine("    AND ipob.injr_pob_pri_ind = 'y' ")
        sql.AppendLine("UNION ")
        sql.AppendLine("Select DISTINCT ")
        '     sql.AppendLine("         TRIM(TRIM(ph.prsn_hist_nm_fst) || ' ' || TRIM(ph.prsn_hist_nm_mid)) || ' ' || TRIM(ph.prsn_hist_nm_lst) clmt_nm, ")
        sql.AppendLine(" TRIM(ph.prsn_hist_nm_fst) fname, ")
        sql.AppendLine(" TRIM(ph.prsn_hist_nm_mid) mname, ")
        sql.AppendLine(" TRIM(ph.prsn_hist_nm_lst) lname, ")
        sql.AppendLine("         ph.prsn_hist_brth_dt birthdate, ")
        sql.AppendLine("         pob.pob_nm injury, ")
        sql.AppendLine("         i.injr_dtm, ")
        sql.AppendLine("         le.lgl_enty_drv_nm emplr_nm, ")
        sql.AppendLine("         med.f_get_cur_clm_sts(i.injr_id) claim_sts, ")
        sql.AppendLine("         med.f_clm_sts_actv(i.injr_id) pay_status, ")
        sql.AppendLine("         NULL ssn ")
        sql.AppendLine("  FROM person_history ph, injury i, injury_part_of_body ipob, part_of_body pob, legal_entity le ")
        sql.AppendLine("WHERE UPPER(prsn_hist_nm_lst) = UPPER(:lname) ")
        sql.AppendLine("    AND UPPER(prsn_hist_nm_fst) = UPPER(:fname) ")
        sql.AppendLine("    AND TRUNC(prsn_hist_brth_dt) = :dob ")
        sql.AppendLine("    AND ph.lgl_enty_id_prsn = i.lgl_enty_id_clmt ")
        sql.AppendLine("    AND i.injr_id = ipob.injr_id ")
        sql.AppendLine("    AND ipob.pob_cd = pob.pob_cd ")
        sql.AppendLine("    AND i.lgl_enty_id_emplr = le.lgl_enty_id ")
        sql.AppendLine("    AND ph.prsn_hist_end_dt is null ")
        sql.AppendLine("    AND ipob.injr_pob_pri_ind = 'y' ")

        Return sql.ToString()

    End Function
End Class