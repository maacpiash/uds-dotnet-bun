# POST
curl -w "\n" \
    --header "Content-Type: application/json" \
    --request POST \
    --data '{ "title": "Wash the dishes" }' \
    --unix-socket /tmp/uds-dotnet-bun.sock \
    http://localhost/todos

# GET
curl -w "\n" --unix-socket /tmp/uds-dotnet-bun.sock http://localhost/todos | jq
