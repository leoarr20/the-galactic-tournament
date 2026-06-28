import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { HomeComponent } from './components/home/home.component';
import { SpeciesListComponent } from './components/species-list/species-list.component';
import { BattleFormComponent } from './components/battle-form/battle-form.component';
import { RankingComponent } from './components/ranking/ranking.component';

const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'species', component: SpeciesListComponent },
  { path: 'battles', component: BattleFormComponent },
  { path: 'ranking', component: RankingComponent },
  { path: '**', redirectTo: '' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }