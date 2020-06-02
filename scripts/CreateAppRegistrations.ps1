[Cmdletbinding()]
Param(
    [Parameter(Mandatory = $true)][string]$environment,
    [Parameter(Mandatory = $true)][string]$identAppSecret,
    [Parameter(Mandatory = $true)][string]$proxyAppSecret,
    [Parameter(Mandatory = $true)][string]$graphAccessAppSecret
)



$identAppName = "IdentityExperienceFramework"
$identAppReplyUrl = "https://$($environment)authncs.b2clogin.com/devauthncs.onmicrosoft.com"
$proxyAppName = "ProxyIdentityExperienceFramework"
$proxyAppReplyUrl = "https://$($environment)authncs.b2clogin.com/oauth2/nativeclient"
$graphAccessAppName = "GraphAccess"


write-host getting graphId
$graphId = az ad sp list --query "[?appDisplayName=='Microsoft Graph'].appId | [0]" --all 
Write-Host found $graphId


write-host getting aadGraphId
$aadGraphId = az ad sp list --query "[?appDisplayName=='Windows Azure Active Directory'].appId | [0]" --all 
Write-Host found $aadGraphId


Write-Host Getting openIdPermission
$openid = az ad sp show --id $graphId --query "oauth2Permissions[?value=='openid'].id | [0]"
write-host found $openid

Write-Host getting offlineAccessPermission
$offlineAccess = az ad sp show --id $graphId --query "oauth2Permissions[?value=='offline_access'].id | [0]"
write-host found $offlineAccess


Write-Host Getting openIdPermission
$openid = az ad sp show --id $graphId --query "oauth2Permissions[?value=='openid'].id | [0]"
write-host found $openid


$identAppResources = @"
[{ "resourceAppId": $graphId, "resourceAccess": [{"id": $openid,"type": "Scope"},{"id": $offlineAccess,"type": "Scope"}]}]
"@ | ConvertTo-Json

Write-Host creating $identAppName
$identApp = az ad app create --display-name $identAppName --password $identAppSecret --reply-urls $identAppReplyUrl --required-resource-accesses $identAppResources --available-to-other-tenants false | ConvertFrom-Json
az ad app permission admin-consent --id $identApp.objectId 
write-host created $identApp.objectId 

write-host getting $identAppName service principal
$identSp = az ad sp list --query "[?appDisplayName=='$($identAppName)'].appId | [0]" --all
write-host found $identSp
write-host getting user_impersonation
$impersonationPermission = az ad sp show --id $identSp --query "oauth2Permissions[?value=='user_impersonation'].id | [0]"
write-host found $impersonationPermission

$proxyAppResources = @"
[{ "resourceAppId": $identSp, "resourceAccess": [{"id": $impersonationPermission, "type": "Scope"}] },{ "resourceAppId": $graphId, "resourceAccess": [{"id": $openid,"type": "Scope"},{"id": $offlineAccess,"type": "Scope"}]}]
"@ | ConvertTo-Json

Write-Host creating $proxyAppName
$proxyApp = az ad app create --display-name $proxyAppName --password $proxyAppSecret --reply-urls $proxyAppReplyUrl, "myapp://auth" --native-app true --required-resource-accesses $proxyAppResources --available-to-other-tenants false | ConvertFrom-Json
az ad app permission admin-consent --id $proxyApp.objectId 
write-host created $proxyApp.objectId 

Write-Host Getting auditLogReadAll
$auditLogReadAll = az ad sp show --id $graphId --query "appRoles[?value=='AuditLog.Read.All'].id | [0]"
write-host found $auditLogReadAll

Write-Host Getting directoryReadWriteAll
$directoryReadWriteAll = az ad sp show --id $graphId --query "appRoles[?value=='Directory.ReadWrite.All'].id | [0]"
write-host found $directoryReadWriteAll

Write-Host Getting readWriteTrustFramework
$readWriteTrustFramework = az ad sp show --id $graphId --query "appRoles[?value=='Policy.ReadWrite.TrustFramework'].id | [0]"
write-host found $readWriteTrustFramework


Write-Host creating $graphAccessAppName
$graphAccessApp = az ad app create --display-name $graphAccessAppName --password $graphAccessAppSecret --available-to-other-tenants false | ConvertFrom-Json
az ad app permission admin-consent --id $graphAccessApp.objectId 

write-host adding application permissions
az ad app permission add --id $graphAccessApp.objectId  --api $graphId --api-permissions "$($auditLogReadAll)=Role"
az ad app permission add --id $graphAccessApp.objectId  --api $graphId --api-permissions "$($directoryReadWriteAll)=Role"
az ad app permission add --id $graphAccessApp.objectId  --api $aadGraphId --api-permissions "$($directoryReadWriteAll)=Role"
az ad app permission add --id $graphAccessApp.objectId  --api $graphId --api-permissions "$($readWriteTrustFramework)=Role"
az ad app permission admin-consent --id $graphAccessApp.objectId 


