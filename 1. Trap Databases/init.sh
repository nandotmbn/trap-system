#!/bin/bash

# Function to create a database
create_database() {
  local dbname=$1
  local ctname=$2
  echo "Creating database $dbname..."
  docker exec $ctname psql -U postgres -p $PORT -c "CREATE DATABASE $dbname"
}

# Function to grant privileges on a database
grant_privileges() {
  local dbname=$1
  local ctname=$2
  echo "Granting privileges on database $dbname..."
  docker exec $ctname psql -U postgres -p $PORT -c "GRANT ALL PRIVILEGES ON DATABASE $dbname TO plntrapuser;"
}

# Function to grant privileges on the public schema
grant_schema_privileges() {
  local dbname=$1
  local ctname=$2
  echo "Granting schema privileges on database $dbname..."
  docker exec $ctname psql -U postgres -d $dbname -p $PORT -c "GRANT ALL ON SCHEMA public TO plntrapuser;"
}

# Main function to set up a database
setup_database() {
  local dbname=$1
  local ctname=$2
  create_database $dbname $ctname
  grant_privileges $dbname $ctname
  grant_schema_privileges $dbname $ctname
}

# Set up the databases

PORT=5432
setup_database database 1trapdatabases-psql-1