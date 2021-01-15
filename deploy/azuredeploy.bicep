param appName string = 'ctb${uniqueString(resourceGroup().id)}'
param location string = resourceGroup().location
param sku string = 'S2'

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
      linuxFxVersion: 'DOCKER|jannemattila/catch-the-banana:latest'
    }
    serverFarmId: appServicePlan.id
    httpsOnly: true
    clientAffinityEnabled: false
  }
}