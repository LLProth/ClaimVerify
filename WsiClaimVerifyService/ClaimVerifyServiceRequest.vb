Option Explicit On
Option Strict On

Imports System.Runtime.Serialization

<DataContract(NameSpace:="http://svcs.workforcesafety.com/claims/verify")>
Public Class ClaimVerifyServiceRequest

    <DataMember(Order:=0)>
    Public Property RequestId As String

    <DataMember(Order:=1)>
    Public Property UserName As String

    <DataMember(Order:=2)>
    Public Property UserPw As String

    <DataMember(Order:=3)>
    Public Property SSN As String

    <DataMember(Order:=6)>
    Public Property DateOfBirth As DateTime

    <DataMember(Order:=4)>
    Public Property FirstName As String

    <DataMember(Order:=5)>
    Public Property LastName As String


End Class
