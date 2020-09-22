[Cmdletbinding()]
Param(
    [Parameter(Mandatory = $true)][string]$apimApiName,
    [Parameter(Mandatory = $true)][string]$apimRg,
    [Parameter(Mandatory = $true)][string]$apimServiceName,
    [Parameter(Mandatory = $true)][string]$mockPolicyPathRoot
)

try {
    $ctx = New-AzApiManagementContext -ResourceGroupName $apimRg -ServiceName $apimServiceName
    $api = Get-AzApiManagementApi -Context $ctx -Name $apimApiName 
    Get-AzApiManagementOperation -Context $ctx -ApiId $api.ApiId | ForEach-Object {
        $operationId = $_.OperationId
        $mockPath = "$($mockPolicyPathRoot)\$($_.Name).xml"
        
        if (Test-Path $mockPath) {
            write-host $operationId policy found - setting mock policy
            Set-AzApiManagementPolicy -Context $ctx -ApiId $api.ApiId -OperationId $operationId -PolicyFilePath $mockPath
            write-host mock policy set
        } else {
            write-host $operationId not found - no work done
        }
    }
}
catch {

    exit 1
}

exit 0