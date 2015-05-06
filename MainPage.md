# Overview #

The Laan Game Library (LGL) is an attempt to build a distributed object architecture for gaming.  It employs 2 way blocking socket communication between client and server using Indy (Internet Direct) component library with all server processing done by a threaded message queue.


# Framework Components #

The Library consists of several components that together provide the gaming network architecture:

**[Socket Architecture](SocketArchitecture.md)**

> Employing a two way blocking socket model, messages can be sent between the client and server from either direction, without the use of polling. This structure employs the excellent [Indy](http://www.indyproject.org/index.en.aspx) networking component library.

**[Message Processing](MessageProcessing.md)**

> All server directed messages are processed within a classic message pump style architecture. As clients send messages to the server, they are queued and prcesses in turn, and as results are cascaded through the entity model, these results are accumulated and dispatched in chunks to the clients.  Only clients that are eligible for updates will receive them.  Thus, [Fog Of War](FogOfWar.md) can easily be implemented.

**[Entity Framework](EntityFramework.md)**

> All entities that can be distributed across the network must use this framework, consisting of either a contained server or client communication class.

> Using the [CodeSmith](http://www.codesmithtools.com) code generation technique, new entities and the entire 'plumbing' infrastructure are built automatically.

**[Game Arena / Lobby](GameArena.md)**

> Although still a work in progress, the Lobby allows finding, joining and commencing games using the predefined classes, that uses sub-classing for any necessary custom additions.