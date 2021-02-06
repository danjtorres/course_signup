# course signup
Hi! Welcome to "Course Sign-up System". It uses CQRS pattern, Microservices, DDD and a service bus.

About scalability:

The application is in C# net core 3.1. What tha means? It's possible to run in Linux, using docker, create a stack with rules for horizontal and vertical scalability! with less human interference and without wasting resources.

About Microservices and DDD:

Each API represents a microservice as usual, but in this case they share the same Domain Layer and Infraestructure Layer (I was conscious that), notice that each API has It's own Repository and Context (inside Infra), easier for the next step "migrate each API for It's own Domain and/or Infra".

The APIs work with multiContext. Also they share the same Database, a nextstep could be "relacional Database for write and a read database (not normalized, NoSql)". This way reading content will not concur with write operations. The read process will be less costly. Doing that is necessary a component to update the read database.

The repository pattern make easier to change the tecnology, I developed to use SQL because It was handy.

About CQRS:

Controllers are publicly exposed and has low responsability, using Mediator pattern the interaction between classes was decoupled. 

Some commands don't need to return data besides the ValidationResult, but in my case I thought It's interesting to return the guid of the item.

All the Ids uses Guids, in case It's necessary to use NoSql databases for queries.

As I said, the code uses the same database, but the Queries are implemented using Dapper and especific selects that return dinamic data and fill up DTO classes.

Another interesting thing, in CoursesStatistics.API was implemented a basic cache layer for keeping the heavy query under control.

About Message Bus:
Message bus was implemented in a class library with the two main methods to access RabbitMq.

Choose rabbitMq because I'ts free and simple to run his docker localy for tests.

About cache layer:

I guess I mentioned the cache layer in the CoursesStatisticsController, It's up for 60 seconds.

About logs:

Nothing especial, just covered all the services, data access and message bus.

Well, because of the lack of time, I decided keep it simple and save each api log in his own file, but could be saved in a database or use the elastic search concept as a next step.

A next step could be and Event Sourcing development and event store data!

About the consumer:

Hey, maybe you ask "Why your consumer is an API?" Well, There's a good point for that. The handler that cosumes information uses BackgroundService as heritage! That proves a concept that APIs could exchange important data using BackgroundService even without a backbone or an event sourcing implementation. And the BackgroundService is very safe.

About Tests:

I did few tests, the most interesting for me is SignUpThousandCoursesInFiveSecondsTest where i setted 5000 milisseconds timeout for performance validate.

There are some lacks, that's not suposed to be perfect, only show all the possibilities and good stuff.

I hope you enjoy reading my code.

Thanks!



