#!/bin/bash

echo "Taking down local development environment..."
docker-compose -f docker-compose.yml down

echo "Removing local development environment volumes..."
docker volume rm local-dev-environment_db_data

echo "Reset complete."

echo "Restarting local development environment..."
docker-compose -f docker-compose.yml up -d
