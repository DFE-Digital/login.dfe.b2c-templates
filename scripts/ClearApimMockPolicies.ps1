[Cmdletbinding()]
Param(
    [Parameter(Mandatory = $true)][string]$apimRg,
    [Parameter(Mandatory = $true)][string]$apimServiceName,
    [Parameter(Mandatory = $true)][string]$apimApiName
)

try {
    $ctx = New-AzApiManagementContext -ResourceGroupName $apimRg -ServiceName $apimServiceName
    $api = Get-AzApiManagementApi -Context $ctx -Name $apimApiName
    Get-AzApiManagementOperation -Context $ctx -ApiId $api.ApiId | ForEach-Object {
        $operationId = $_.OperationId
        Remove-AzApiManagementPolicy -Context $ctx -ApiId $api.ApiId  -OperationId $operationId
        write-host $operationId policy cleared
    }
}
catch {
    exit 1
}
exit 0