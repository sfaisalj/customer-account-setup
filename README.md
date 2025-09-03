# customer-account-setup
Exploring orchestration of a distributed entity.

Modelling a SaaS account setup where a customer can choose which services to enable. 

Microservices:
- Authentication and Authorization Service
- Email Marketing Service
- Video Conferencing Service
- ConfigStore


Authentication Service.
- There are two persona's. One is a customer, and the second is the support.
- Uses simple JWT Auth, in which there are Authorization claims.
Customer claims:
    EmailMarketing.Update
    TextNotification.Update
    VideoConferencing.Update

Support claims:
    EmailMarketing.Update
    TextNotification.Update
    VideoConferencing.Update
    EmailMarketing.Delete
    TextNotification.Delete
    VideoConferencing.Delete

CustomerAccount Entity:
    Id: String
    FirstName: String
    LastName: String
    EmailAddress: String

Email Marketing Service:
The Email Marketing domain has configurations for how to communicate with prospects.

EmailMarketing Entity:
    EmailDeliveryInfra: string # SES, Mailchimp
    EmailTemplate: string
    Analytics: string
    ABTesting: string

Video Conferencing Service:
The Video Conferencing domain has configurations settings to define what the customer wants.

VideoConferencing Entity:
    CalendarIntegration: string 
    VideoConferencingProvider: string
    BandwidthOptimization: string
    ScreenSharing: boolean 
    TranscriptionService: boolean

ConfigStore:
- Stores configuration for each account with versioning, using documentDB.
- Versions each save with the user context.

AccountSetupUI:
- Client Side Single Page Application using Angular
- Connects the the backend services using apollo angular.
- Has a codegen setup
- Has a Login Page
- Has an Account Setup Page that starts a workflow.
- First Screen would be the Email Marketing Setup. Save and Next should save the config to the configstore and load next screen.
- Second Screen would be the Video Conferencing Setup Save and Next should save the config to the configstore and load next screen.
- Summary Screen where the user can verify their setup and confirm "Commit" to make the configs live on each service.

Architecture:
- Each Service will use the Ports and Adaptor Pattern, with incoming and outgoing ports.
- Each Service will have a GraphQL API using Chillicream's HotChocolate
- There will be a Client UI in Angular which will have configuration screens
- The GraphQL API's will be federated using the HiveGateway, and custom resolvers to create a transactional write.
- The custom resolver will store partial saves to the Config Store
- When the UI passes the commit variable on save, validators will verify the config according to business rules and update each service.
- Authorization will be each each service's Business Layer. 
- Each service should be able to run in Docker
- Docker compose at the root creates our infrastructure for local development.

Todo List:

- [] Create a simple IdentityService
- [] Create the EmailMarketing Service
- [] Create the VideoConferencing Service
- [] Create the ConfigStore Service
- [] Create the AccountSetupUI 
- [] Create the GatewayService with HiveGateway and setup federation.

Folder Structure:

    client/
        account-setup
    gateway/
        federation-gateway
    services/
        Email.Marketing/
        Video.Conferencing/
    docker-compose.yaml





