Option Explicit On
Option Strict On

Imports WsiClaimVerifyModel


' NOTE: You can use the "Rename" command on the context menu to change the interface name "IService1" in both code and config file together.
<ServiceContract(NameSpace:="http://svcs.workforcesafety.com/claims/verify")>
Public Interface IClaimVerifyService

    <OperationContract()>
    Function Verify(ByVal requestData As ClaimVerifyServiceRequest) As ClaimVerifyServiceResponse

End Interface



