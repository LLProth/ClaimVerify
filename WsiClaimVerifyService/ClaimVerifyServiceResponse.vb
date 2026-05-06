Option Explicit On
Option Strict On

Imports System.Runtime.Serialization

<DataContract(NameSpace:="http://svcs.workforcesafety.com/claims/verify")>
Public Class ClaimVerifyServiceResponse

    Public Sub New()
        _ClaimList = New ClaimListType()
    End Sub

    <DataMember(Order:=2)>
    Public Property Status As String

    <DataMember(Order:=3)>
    Public Property Exception As String

    <DataMember(Order:=0)>
    Public Property RequestId As String

    <DataMember(Order:=1)>
    Public Property ClaimList As ClaimListType

End Class

<CollectionDataContractAttribute([Namespace]:="http://svcs.workforcesafety.com/claims/verify", ItemName:="Claim")>
Public Class ClaimListType
    Inherits System.Collections.Generic.List(Of Claim)
End Class

<DataContract(NameSpace:="http://svcs.workforcesafety.com/claims/verify")>
Public Class Claim

    <DataMember()>
    Public Property FirstName As String

    <DataMember()>
    Public Property MiddleName As String

    <DataMember()>
    Public Property LastName As String

    <DataMember()>
    Public Property DateOfBirth As Date

    <DataMember()>
    Public Property TypeOfInjury As String

    <DataMember()>
    Public Property InjuryDate As DateTime

    <DataMember()>
    Public Property EmployerName As String

    <DataMember()>
    Public Property ClaimStatus As String

    <DataMember()>
    Public Property PayStatus As String


End Class
