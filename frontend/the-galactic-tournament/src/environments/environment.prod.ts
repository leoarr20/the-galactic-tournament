// Production environment configuration.
// If the API is published under the same domain as the Angular app, '/api' is a clean
// default because it avoids exposing server names in the compiled frontend bundle.
// If the API is hosted elsewhere, replace this value with that HTTPS domain.
export const environment = {
  production: true,
  apiUrl: '/api'
};
