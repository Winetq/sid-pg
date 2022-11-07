#!/usr/bin/env bash
sed -i "s/db_name/$DB_NAME/" /app/database.json
sed -i "s/db_url/$DB_URL/" /app/database.json
sed -i "s/db_password/$DB_PASSWORD/" /app/database.json
sed -i "s/db_username/$DB_USERNAME/" /app/database.json
dotnet SmartPartyApi.dll