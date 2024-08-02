# NotificationService
This repository is a template or personal project to demonstrate notification service which implements a rate limiter on send email.

### Develop a REST API with the following requirements:

1. Rate-Limited Notification Service

1.2 We have a Notification system that sends out email notifications of various types (status update, daily news, project invitations, etc). We need to protect recipients from getting too many emails, either due to system errors or due to abuse, so let’s limit the number of emails sent to them by implementing a rate-limited version of NotificationService.
1.3 The system must reject requests that are over the limit.

2 Some sample notification types and rate limit rules, e.g.:

2.1 Status: not more than 2 per minute for each recipient
2.2 News: not more than 1 per day for each recipient
2.3 Marketing: not more than 3 per hour for each recipient
2.4 Etc. these are just samples, the system might have several rate limit rules!

## Technologies Used

- **.NET Core 8**: The latest version of .NET Core, offering improved performance and cross-platform support.
- **C#**: Main programming language for back-end development.
- **xUnit**: Unit testing framework used for automated testing.
- **MOQ/AutoMock**: Used to mock classes, interfaces in the test project.
- **Docker**: Used to package and deploy the API as a container.

## Project Structure

The project is divided into four internal projects:

1. **API**: Contains the API controllers.
2. **Core**: Includes use cases, entities and domain logic.
3. **Tests**: Houses unit and integration tests to ensure code quality.
4. **Infrastructure**: Provides the database access layer and dependency configurations.

## Suggested Initial Architecture

 ![image](https://github.com/user-attachments/assets/702bb97a-e335-41f7-86ea-26702a9d1e67)


 ## Use cases

Use cases define the business logic and are implemented in the Core project. Here are some examples:

- **NotificationUseCase **: This use case deals with:
  -  Creating type of notification based on plataform type (email, telegram, push notificatiom and etc) and for that I use the design pattern Factory, which given a plataform type return a instance class to each operation
  -  Using cache to working with limits for each type of email (news, status, marketing)
  -  Send email if passed some credentials of smtp

## Entities

Entities represent domain objects and are defined in the Core project. Here are some examples:

- **Notification**: Includes properties such as from, to, subject, body, phone number. Also I applied some concepts off ddd for this entity just like, receive parameters by construtors and validate them. The state of entity cannot changed by outside class and there some methods inside the entity related a expiration time cache based on type email (news, status, marketing)


## Tests

The initial tests are implemented in the Tests project and cover use cases, entities.

![image](https://github.com/user-attachments/assets/a452cf2f-62f6-4524-bbcd-de591e908cf8)

## Infrastructure

The infrastructure project manages database access and dependency configurations. Here are some key points:

- **Dependency Injection**: The project uses dependency injection to manage dependencies between classes and modules.
- **Settings**: Environment-specific settings are managed using configuration files.

## Evolution points:

- CI/CD flow in github action.
- Dockfile configured and being validated in the CI flow of github actions.
- Performance tests (newman + postmans).
- Given the applicability of the factory code standard, if a component is needed to send messages via telegram or other channels, this API is already prepared for it.
- If the expiration time rules for each type of email increase in the future, simply add the rules inside the entity, which followed some DDD (rich domain modeling) principles.

## Architecture Decision Records

Key architectural decisions can be queried in the /Documentation/Architecture Decision Record folder

- **Title**: Use of Clean Architecture for Notification API.

## Installation instruction
To install and run the project, follow these steps:

1. Clone the repository:
```sh
git clone https://github.com/Mellogab/iTransfer.git
```
2. Navigate to the project folder:
```sh
cd iTransfer
```
3. Restore dependencies:
```sh
dotnet restore
```
```
4. Run the project:
```sh
dotnet run --project iTransferencia
```

## Running the Tests
To run the unit and integration tests, use the following command:

```sh
dotnet test
```

## API endpoints

The API exposes the following endpoints:

Transfers

 POST /api/notification-async: Send a notification transfer.
