[Cmdletbinding()]
Param(
    [Parameter(Mandatory = $true)][string]$environment,
    [Parameter(Mandatory = $true)][string]$identAppSecret,
    [Parameter(Mandatory = $true)][string]$proxyAppSecret,
    [Parameter(Mandatory = $true)][string]$graphAccessAppSecret,
    [Parameter(Mandatory = $true)][string]$relyAppSecret
)

$identAppName = "IdentityExperienceFramework"
$identAppReplyUrl = "https://$($environment)authncs.b2clogin.com/devauthncs.onmicrosoft.com"
$proxyAppName = "ProxyIdentityExperienceFramework"
$proxyAppReplyUrl = "https://$($environment)authncs.b2clogin.com/oauth2/nativeclient"
$graphAccessAppName = "GraphAccess"
$replyAppName = "RelyAuthApp"

write-host GETTING PRINCIALS..

$graphId = az ad sp list --query "[?appDisplayName=='Microsoft Graph'].appId | [0]" --all 
Write-Host found graphId : $graphId

$aadGraphId = az ad sp list --query "[?appDisplayName=='Windows Azure Active Directory'].appId | [0]" --all 
Write-Host found aadGraphId : $aadGraphId

write-host GETTING PERMISSIONS..

$openid = az ad sp show --id $graphId --query "oauth2Permissions[?value=='openid'].id | [0]"
write-host found openIdPermission : $openid

$offlineAccess = az ad sp show --id $graphId --query "oauth2Permissions[?value=='offline_access'].id | [0]"
write-host found offlineAccessPermission :  $offlineAccess

$auditLogReadAll = az ad sp show --id $graphId --query "appRoles[?value=='AuditLog.Read.All'].id | [0]"
write-host found auditLogReadAll : $auditLogReadAll

$directoryReadWriteAll = az ad sp show --id $graphId --query "appRoles[?value=='Directory.ReadWrite.All'].id | [0]"
write-host found directoryReadWriteAll : $directoryReadWriteAll

$readWriteTrustFramework = az ad sp show --id $graphId --query "appRoles[?value=='Policy.ReadWrite.TrustFramework'].id | [0]"
write-host found readWriteTrustFramework : $readWriteTrustFramework

$identAppResources = @"
[{ "resourceAppId": $graphId, "resourceAccess": [{"id": $openid,"type": "Scope"},{"id": $offlineAccess,"type": "Scope"}]}]
"@ | ConvertTo-Json


$relyAppResources = @"
[{ "resourceAppId": $graphId, "resourceAccess": [{"id": $openid,"type": "Scope"},{"id": $offlineAccess,"type": "Scope"}]}]
"@ | ConvertTo-Json

write-host CREATING REGISTRATIONS..

Write-Host creating app : $identAppName
$identApp = az ad app create --display-name $identAppName --password $identAppSecret --reply-urls $identAppReplyUrl --required-resource-accesses $identAppResources --available-to-other-tenants false | ConvertFrom-Json
write-host created $identApp.objectId 

write-host GETTING $identAppName IMPERSONATION PERMISSIONS..

$identSp = az ad sp list --query "[?appDisplayName=='$($identAppName)'].appId | [0]" --all
write-host found $identAppName service principal : $identSp

$impersonationPermission = az ad sp show --id $identSp --query "oauth2Permissions[?value=='user_impersonation'].id | [0]"
write-host found user_impersonation : $impersonationPermission

$proxyAppResources = @"
[{ "resourceAppId": $identSp, "resourceAccess": [{"id": $impersonationPermission, "type": "Scope"}] },{ "resourceAppId": $graphId, "resourceAccess": [{"id": $openid,"type": "Scope"},{"id": $offlineAccess,"type": "Scope"}]}]
"@ | ConvertTo-Json

Write-Host creating app : $proxyAppName
$proxyApp = az ad app create --display-name $proxyAppName --password $proxyAppSecret --reply-urls $proxyAppReplyUrl "myapp://auth" --native-app true --required-resource-accesses $proxyAppResources --available-to-other-tenants false | ConvertFrom-Json
write-host created $proxyApp.objectId 

Write-Host creating app : $replyAppName
$relyApp = az ad app create --display-name $replyAppName --password $relyAppSecret --native-app true --reply-urls "https://$($environment)authncs.b2clogin.com/oauth2/nativeclient" "https://login.microsoftonline.com/tfp/oauth" "https://$($environment)authncs.b2clogin.com/tfp/oauth2/nativeclient" --native-app true --required-resource-accesses $relyAppResources --available-to-other-tenants false | ConvertFrom-Json
write-host created $relyApp.objectId 

Write-Host creating app : $graphAccessAppName
$graphAccessApp = az ad app create --display-name $graphAccessAppName --password $graphAccessAppSecret --available-to-other-tenants false | ConvertFrom-Json

write-host EXTENDING GRAPH ACCESS PERMISSIONS..

write-host adding application permissions
az ad app permission add --id $graphAccessApp.objectId  --api $graphId --api-permissions "$($auditLogReadAll)=Role"
az ad app permission add --id $graphAccessApp.objectId  --api $graphId --api-permissions "$($directoryReadWriteAll)=Role"
az ad app permission add --id $graphAccessApp.objectId  --api $aadGraphId --api-permissions "$($directoryReadWriteAll)=Role"
az ad app permission add --id $graphAccessApp.objectId  --api $graphId --api-permissions "$($readWriteTrustFramework)=Role"



