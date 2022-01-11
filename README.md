# KChatbox


<strong>1. Aim of this project:</strong>
- Practice using SignalR to crate real-time comunications.
- Follow Model - ViewModel - Controller pattern.
- Pratice using ASP .Net Core 5.
- This is the backend of KChatbox.

<strong>2. Features:</strong>
- Clients can chat directly to each other (one-to-one) in real-time manner.
- Using SignalR to provide real-time communications.
- Using FluentValidation to validate request before processing bussiness logic.
- Using AutoMapper to map objects to objects.
- Using MediaTr to crate pipeline process for each request.
- Ultilizing the strength of Dependency Injection to create abstractions for project.
- Databases: MongoDB and MySQL.
- Using Docker.
Note: All of the above features are implemented at a very basic level.

<strong>3. Current tasks:</strong>
- Improving authozation and authentication process. For example, using a distributed cache to store JWT token, later can use this cache to revoke alredy logging out users.
- Allowing users to make a group chat.
