export interface Species {
  id: number;
  name: string;
  powerLevel: number;
  specialAbility: string;
  createdAt: string;
}

export interface CreateSpecies {
  name: string;
  powerLevel: number;
  specialAbility: string;
}

export interface UpdateSpecies {
  name: string;
  powerLevel: number;
  specialAbility: string;
}