FROM node:20-slim AS base

# Install dependencies only when needed
FROM base AS deps
WORKDIR /app

# Install dependencies based on the preferred package manager
COPY package.json package-lock.json* ./
RUN npm ci --only=production && npm cache clean --force

# Rebuild the source code only when needed
FROM base AS builder
WORKDIR /app
COPY package.json package-lock.json* ./
COPY elv2_license.toml ./
COPY elv2.toml ./

RUN npm ci

COPY . .
RUN npm run build
RUN curl -sSL https://rover.apollo.dev/nix/latest | sh
# Production image, copy all the files and run the app
FROM base AS runner
WORKDIR /app

ENV NODE_ENV production

RUN addgroup --system --gid 1001 nodejs
RUN adduser --system --uid 1001 hivegateway

COPY --from=deps /app/node_modules ./node_modules
COPY --from=builder /app/dist ./dist
COPY --from=builder /app/package.json ./package.json
COPY --from=builder /app/elv2_license.toml ./elv2_license.toml
COPY --from=builder /app/elv2.toml ./elv2.toml


USER hivegateway

EXPOSE 5005

ENV PORT 5005
RUN ls -al /app
RUN export APOLLO_CONFIG_HOME=/app/ && npm run compose
CMD ["npm", "run gateway"]