#!/bin/bash

# Wait for SQL Server to be ready
until /opt/mssql-tools/bin/sqlcmd -S sqlserver -U sa -Q "SELECT 1"; do
  sleep 1
done

# Run migrations
dotnet ef database update --project Infrastructure --startup-project WebApi

# Start the application
exec dotnet WebApi.dll