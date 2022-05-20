# Twitter Streaming Application

This application is using  TweetinviAPI 5.0.4 nuget package.
Entity Framework flow is setup however not enabled for the database creation logic at this time. TODO
Unit tests projects are setup however there are no more unit tests due to time constrains. TODO
.env file has the twitter credential values. To run this application create Twitter Developer account. Register account and generate Consumer key, Consumer secrect and Bearer Token and replace these values in .env file located in root project location. If file is not downloaded from git clone please create .env file with the help of notepad
The env files are loaded using DotNetEnv nuget package.
The application also has logic to consume the twitter SampleStream V2 api with HttpClient inside the Twitter.server=> services class.
