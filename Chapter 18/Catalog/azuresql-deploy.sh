#!/bin/bash

# Set an admin login and password for your database
export user_admin=samueler
export user_pass=P@ssW0rd
# The logical server name has to be unique in the system
export server_name=vinylstoresql
export database_name=catalog
# The resource group name
export resourceGroup=vinylstoreDataResourceGroup
# The ip address range that you want to allow to access your DB
export startip=0.0.0.0
export endip=0.0.0.0

# Create a resource group
az group create \
	--name ${resourceGroup} \
	--location westeurope

# Create a logical server in the resource group
az sql server create \
	--name ${server_name} \
	--resource-group ${resourceGroup} \
	--location westeurope  \
	--admin-user ${user_admin} \
	--admin-password ${user_pass}

# Configure a firewall rule and open to local Azure services
az sql server firewall-rule create \
	--resource-group ${resourceGroup} \
	--server ${server_name} \
	-n AllowYourIp \
	--start-ip-address ${startip} \
	--end-ip-address ${endip}

# Create a database in the server 
az sql db create \
	--resource-group ${resourceGroup} \
	--server ${server_name} \
	--name ${database_name} \
	--service-objective S0 \
    --zone-redundant false
