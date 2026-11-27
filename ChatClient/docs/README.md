# ChatApp

A simple terminal chat application built with .NET 10 (C# Fordypning, Case 3).

The solution is **centralized**: one server, and multiple clients that connect to it over TCP (IPv4, port 5000).

![Architecture](docs/arkitektur.png)

## Projects

| Project | Type | Responsibility |
|---|---|---|
| `ChatCore` | classlib | The logic: `User`, `ChatMessage`, `IClientConnection`, `ChatRoom` |
| `ChatServer` | console | Listens on port 5000 and passes messages on to `ChatRoom` |
| `ChatClient` | console | Connects to the server, sends and displays messages |
| `ChatTests` | xUnit | Tests `ChatRoom` using a `FakeClientConnection` |

`ChatCore` knows nothing about networking. The server is a thin layer around it, so the logic can be tested without real connections.

## How it works

1. The client connects and sends its name as the first line
2. The server calls `ChatRoom.Join`, and everyone gets `*** Name joined ***`
3. Each line the client sends goes to `ChatRoom.SendMessage`
4. `ChatRoom` creates a `ChatMessage` and broadcasts it to all users
5. When a client types `/quit` or disconnects, `ChatRoom.Leave` notifies the others

## Running the app

Start the server first:

```powershell
dotnet run --project ChatServer
```

Open a new terminal for each user and start a client:

```powershell
dotnet run --project ChatClient
```

Enter a name and start chatting. Type `/quit` to exit.

## Running the tests

```powershell
dotnet test
```

## Diagram

The architecture diagram is in `docs/`:

- `arkitektur.excalidraw`: editable in [Excalidraw](https://excalidraw.com)
- `arkitektur.png`: exported image