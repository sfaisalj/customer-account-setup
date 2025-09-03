"use strict";
var __createBinding = (this && this.__createBinding) || (Object.create ? (function(o, m, k, k2) {
    if (k2 === undefined) k2 = k;
    var desc = Object.getOwnPropertyDescriptor(m, k);
    if (!desc || ("get" in desc ? !m.__esModule : desc.writable || desc.configurable)) {
      desc = { enumerable: true, get: function() { return m[k]; } };
    }
    Object.defineProperty(o, k2, desc);
}) : (function(o, m, k, k2) {
    if (k2 === undefined) k2 = k;
    o[k2] = m[k];
}));
var __setModuleDefault = (this && this.__setModuleDefault) || (Object.create ? (function(o, v) {
    Object.defineProperty(o, "default", { enumerable: true, value: v });
}) : function(o, v) {
    o["default"] = v;
});
var __importStar = (this && this.__importStar) || (function () {
    var ownKeys = function(o) {
        ownKeys = Object.getOwnPropertyNames || function (o) {
            var ar = [];
            for (var k in o) if (Object.prototype.hasOwnProperty.call(o, k)) ar[ar.length] = k;
            return ar;
        };
        return ownKeys(o);
    };
    return function (mod) {
        if (mod && mod.__esModule) return mod;
        var result = {};
        if (mod != null) for (var k = ownKeys(mod), i = 0; i < k.length; i++) if (k[i] !== "default") __createBinding(result, mod, k[i]);
        __setModuleDefault(result, mod);
        return result;
    };
})();
var __importDefault = (this && this.__importDefault) || function (mod) {
    return (mod && mod.__esModule) ? mod : { "default": mod };
};
Object.defineProperty(exports, "__esModule", { value: true });
const express_1 = __importDefault(require("express"));
const gateway_1 = require("@graphql-hive/gateway");
const fs = __importStar(require("node:fs"));
const supergraph = JSON.parse(fs.readFileSync('./supergraph.json', 'utf8'));
const app = (0, express_1.default)();
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
        const gateway = (0, gateway_1.createGatewayRuntime)({
            supergraph,
            pollingInterval: 30000, // Poll subgraphs every 30 seconds for schema changes
            // introspection: true,
            // playground: true,
        });
        // Add CORS middleware
        app.use((req, res, next) => {
            res.header('Access-Control-Allow-Origin', '*');
            res.header('Access-Control-Allow-Methods', 'GET, POST, OPTIONS');
            res.header('Access-Control-Allow-Headers', 'Origin, X-Requested-With, Content-Type, Accept, Authorization');
            if (req.method === 'OPTIONS') {
                res.sendStatus(200);
            }
            else {
                next();
            }
        });
        // Mount the gateway
        app.use(gateway.graphqlEndpoint, gateway);
        // Health check endpoint
        app.get('/health', (req, res) => {
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
    }
    catch (error) {
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
