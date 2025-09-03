import { ApplicationConfig } from '@angular/core';
import { APOLLO_OPTIONS } from 'apollo-angular';
import { ApolloClientOptions, InMemoryCache } from '@apollo/client/core';
import { HttpLink } from 'apollo-angular/http';
import { inject } from '@angular/core';

const uri = 'http://localhost:5005/graphql';

export function apolloOptionsFactory(): ApolloClientOptions<any> {
  const httpLink = inject(HttpLink);
  return {
    link: httpLink.create({ uri }),
    cache: new InMemoryCache(),
    defaultOptions: {
      watchQuery: {
        errorPolicy: 'all',
      },
    },
  };
}

export const apolloConfig: ApplicationConfig = {
  providers: [
    {
      provide: APOLLO_OPTIONS,
      useFactory: apolloOptionsFactory,
    },
  ],
};