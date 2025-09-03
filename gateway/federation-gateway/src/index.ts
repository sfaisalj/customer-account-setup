import express from 'express';
import { createGatewayRuntime } from '@graphql-hive/gateway';
import * as fs from 'node:fs';
const supergraph = JSON.parse(fs.readFileSync('./supergraph.json', 'utf8'))
const app = express();
const port = process.env.PORT || 5005;

// Define the subgraph endpoints
const subgraphs = [
  {
    name: 'identity',
    url: process.env.IDENTITY_SERVICE_URL || 'http://identity-service:80/graphql',
  },
  {
    name: 'config-store',
    url: process.env.CONFIG_STORE_SERVICE_URL || 'http://config-store-service:80/graphql',
  },
  {
    name: 'email-marketing',
    url: process.env.EMAIL_MARKETING_SERVICE_URL || 'http://email-marketing-service:80/graphql',
  },
  {
    name: 'video-conferencing',
    url: process.env.VIDEO_CONFERENCING_SERVICE_URL || 'http://video-conferencing-service:80/graphql',
  },
];

async function startGateway() {
  try {
    console.log('Starting Hive Gateway...');
    console.log('Subgraphs:', subgraphs);

    const gateway = createGatewayRuntime({
      supergraph,
      pollingInterval: 30000, // Poll subgraphs every 30 seconds for schema changes
      // introspection: true,
      // playground: true,
    });

    // Add CORS middleware
    app.use((req: any, res: any, next: any) => {
      res.header('Access-Control-Allow-Origin', '*');
      res.header('Access-Control-Allow-Methods', 'GET, POST, OPTIONS');
      res.header('Access-Control-Allow-Headers', 'Origin, X-Requested-With, Content-Type, Accept, Authorization');
      
      if (req.method === 'OPTIONS') {
        res.sendStatus(200);
      } else {
        next();
      }
    });

    // Mount the gateway
    app.use(gateway.graphqlEndpoint, gateway);

    // Health check endpoint
    app.get('/health', (req: any, res: any) => {
      res.json({ 
        status: 'healthy', 
        timestamp: new Date().toISOString(),
        subgraphs: subgraphs.map(s => ({ name: s.name, url: s.url }))
      });
    });

    // Start the server
    app.listen(port, () => {
      console.log(`🚀 Hive Gateway ready at http://localhost:${port}/graphql`);
      console.log(`📊 Health check at http://localhost:${port}/health`);
      console.log(`🎮 GraphQL Playground available in development mode`);
    });

  } catch (error) {
    console.error('Failed to start gateway:', error);
    process.exit(1);
  }
}

// Handle graceful shutdown
process.on('SIGINT', () => {
  console.log('Received SIGINT, shutting down gracefully...');
  process.exit(0);
});

process.on('SIGTERM', () => {
  console.log('Received SIGTERM, shutting down gracefully...');
  process.exit(0);
});

// Start the gateway
startGateway().catch((error) => {
  console.error('Unhandled error:', error);
  process.exit(1);
});