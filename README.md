# Twitter Streaming Application

- This application is using  DotNet 6.X.
- Entity Framework flow is setup for storing Tweet and Author  and Entities<HashTag> records.
- Unit tests projects are setup for API integration tests[WebApplicationFactory] and the Controller test [Moq]. Test are just sample and do not have much time to cover all scenarios.
- .env file has the twitter credential values and sql express db connection string. To run this application create Twitter Developer account. Register account and generate Consumer key, Consumer secrect and Bearer Token and replace these values in .env file located in root project location. If file is not downloaded from git clone please create .env file with the help of notepad
- The env files are loaded using DotNetEnv nuget package.
- The tweet streaming logic runs in the background. Tweet count and Nth recent hash tags are fetched from Web API methods.
- The application also has logic to consume the twitter SampleStream V2 api with HttpClient inside the Twitter.server=> services class.

# Setup Database
 - Create your first migration <dotnet ef migrations add InitialCreate --project Twitter.StreammingApi>
 - Create your database and schema <dotnet ef database update --project Twitter.StreammingApi>
 - Run migrtion and update databse while model is evolved with the agile business changes.
