# Federation Gateway

Hive Gateway for federating GraphQL services in the Customer Account Setup system.

## Services

This gateway federates the following GraphQL services:

- **Identity Service** - User authentication and authorization
- **Config Store Service** - Configuration storage with versioning
- **Email Marketing Service** - Email marketing configuration management
- **Video Conferencing Service** - Video conferencing configuration management

## Development

### Prerequisites

- Node.js 18+
- npm

### Installation

```bash
npm install
```

### Running in Development

```bash
npm run dev
```

### Building for Production

```bash
npm run build
npm start
```

## Docker

### Build Image

```bash
docker build -t federation-gateway .
```

### Run Container

```bash
docker run -p 5000:5000 federation-gateway
```

## Environment Variables

- `PORT` - Server port (default: 5000)
- `IDENTITY_SERVICE_URL` - Identity service GraphQL endpoint
- `CONFIG_STORE_SERVICE_URL` - Config store service GraphQL endpoint
- `EMAIL_MARKETING_SERVICE_URL` - Email marketing service GraphQL endpoint
- `VIDEO_CONFERENCING_SERVICE_URL` - Video conferencing service GraphQL endpoint

## Endpoints

- `GET /graphql` - GraphQL endpoint (with Playground in development)
- `POST /graphql` - GraphQL endpoint
- `GET /health` - Health check endpoint

## Features

- **GraphQL Federation** - Combines multiple GraphQL services into a single endpoint
- **Schema Polling** - Automatically detects schema changes in subgraphs
- **CORS Support** - Configured for cross-origin requests
- **Health Checks** - Monitor gateway and subgraph status
- **Development Tools** - GraphQL Playground for development