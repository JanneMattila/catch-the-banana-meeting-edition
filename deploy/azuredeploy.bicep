param location string = resourceGroup().location
param appName string = 'ctb${uniqueString(resourceGroup().id)}'
param image string = 'jannemattila/catch-the-banana:latest'
param sku string = 'B2'

resource appServicePlan 'Microsoft.Web/serverfarms@2020-06-01' = {
  name: 'asp-webapp'
  location: location
  kind: 'linux'
  sku: {
    name: sku
    capacity: 1
  }
  properties: {
    reserved: true
  }
}

resource appService 'Microsoft.Web/sites@2020-06-01' = {
  name: appName
  location: location
  kind: 'web'
  properties: {
    siteConfig: {
      alwaysOn: true
      webSocketsEnabled: true
      http20Enabled: true
      ftpsState: 'Disabled'
      linuxFxVersion: 'DOCKER|${image}'
    }
    serverFarmId: appServicePlan.id
    httpsOnly: true
    clientAffinityEnabled: false
  }
}