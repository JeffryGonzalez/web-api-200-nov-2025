#!/bin/bash

# Check if name argument is provided
if [ -z "$1" ]; then
    echo "Usage: $0 <name> [role1] [role2] ..."
    echo "Example: $0 boss@company.com SoftwareCenter Admin"
    exit 1
fi

NAME=$1
shift  # Remove the first argument (name) so we can process the rest as roles

echo "Generating token for $NAME"

# Detect OS and set clipboard command
if [[ "$OSTYPE" == "darwin"* ]]; then
    CLIP_CMD="pbcopy"
elif [[ "$OSTYPE" == "msys" || "$OSTYPE" == "win32" ]]; then
    CLIP_CMD="clip"
else
    CLIP_CMD="xclip -selection clipboard"  # Linux fallback
fi

# Build the dotnet command with roles
CMD="dotnet user-jwts create -n $NAME"

# Add each role as a --role argument
for role in "$@"; do
    CMD="$CMD --role $role"
done

# Execute the command and copy token to clipboard
eval $CMD | grep 'Token:' | awk '{print $2}' | $CLIP_CMD

echo "Token for $NAME is in your clipboard."