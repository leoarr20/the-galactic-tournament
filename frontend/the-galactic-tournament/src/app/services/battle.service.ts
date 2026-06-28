import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '../../environments/environment';
import { BattleResult, StartBattle } from '../models/battle';

@Injectable({
  providedIn: 'root'
})
export class BattleService {

  private readonly apiUrl = environment.apiUrl + '/battles';

  constructor(private http: HttpClient) { }

  // Returns battle history. This gives the user visibility of stored tournament results.
  getAll(): Observable<BattleResult[]> {
    return this.http.get<BattleResult[]>(this.apiUrl);
  }

  // Starts a selected battle. The winner is calculated in the backend, not in Angular,
  // so the business rule has a single source of truth.
  startBattle(request: StartBattle): Observable<BattleResult> {
    return this.http.post<BattleResult>(this.apiUrl, request);
  }

  // Bonus feature from the exercise: the backend selects two random species and runs a battle.
  startRandomBattle(): Observable<BattleResult> {
    return this.http.post<BattleResult>(this.apiUrl + '/random', {});
  }
}
