// Ranking row returned by the API.
// Position is calculated in the backend so the frontend only focuses on presentation.
export interface RankingItem {
  position: number;
  speciesId: number;
  name: string;
  powerLevel: number;
  specialAbility: string;
  victories: number;
}
