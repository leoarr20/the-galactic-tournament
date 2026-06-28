// Development environment configuration.
// The Angular app is intentionally separated from the .NET API, so the base API URL
// lives here instead of being hard-coded inside every service. This makes local
// changes safer and keeps the application easier to deploy later.
export const environment = {
  production: false,
  apiUrl: 'http://localhost:5154/api'
};

/*
 * For easier debugging in development mode, you can import the following file
 * to ignore zone related error stack frames such as `zone.run`, `zoneDelegate.invokeTask`.
 * This import should be commented out in production mode because it will have a negative impact
 * on performance if an error is thrown.
 */
// import 'zone.js/dist/zone-error';  // Included with Angular CLI.
