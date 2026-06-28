// Payload used to request a battle between two already registered species.
export interface StartBattle {
  speciesAId: number;
  speciesBId: number;
}

// Result returned by the .NET API after a battle is executed.
// It includes the names and power levels to let the UI explain the decision clearly.
export interface BattleResult {
  id: number;
  speciesAId: number;
  speciesAName: string;
  speciesAPowerLevel: number;
  speciesBId: number;
  speciesBName: string;
  speciesBPowerLevel: number;
  winnerSpeciesId: number;
  winnerName: string;
  resultReason: string;
  battleDate: string;
}
