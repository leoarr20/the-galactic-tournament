# Technical Decisions

This document explains why each main piece of the frontend was selected and what purpose it serves.

## Angular 10.2.x

The project intentionally uses Angular 10.2.x. Because of that, the code avoids newer Angular features such as standalone components, `inject()`, signals, `@if` and `@for`. This makes the solution compatible with the selected version.

## Component-based UI

The UI is split into focused components:

- `HomeComponent`: introduces the tournament and rules.
- `SpeciesFormComponent`: captures data for a new species.
- `SpeciesListComponent`: displays registered species and refreshes after creation.
- `BattleFormComponent`: starts selected or random battles and shows battle history.
- `RankingComponent`: displays the current tournament ranking.

This separation keeps each file easier to understand and defend during the technical interview.

## Services as API layer

Services isolate HTTP communication from the visual components. This is useful because if an endpoint changes, only the service needs to be adjusted.

## Models as contracts

Interfaces under `models` document the shape of data received from the backend. This helps ensure the frontend uses the same field names as the .NET DTOs.

## Backend as source of truth

The frontend does not calculate the battle winner. The frontend only sends the selected species IDs. This is intentional because:

1. The battle rule is business logic.
2. The result must be persisted.
3. The ranking depends on persisted battles.
4. Keeping the rule in the backend avoids duplicated logic.

## SCSS and visual design

The selected visual style uses:

- dark galactic header for identity,
- purple gradients for action areas,
- cards for readability,
- hover effects for feedback,
- responsive layouts for smaller screens.

The goal is to make the system feel like a tournament interface, not just a basic CRUD screen.

## Error handling

Components read API error messages when the backend returns `{ message: '...' }`. If the backend response has no message, the UI shows a generic fallback. This keeps the interface helpful without exposing technical stack traces.

## Environment configuration

The API URL is placed in `environment.ts` so it can be changed without editing each service. Production uses `/api` as a neutral default when frontend and backend are hosted under the same domain.
