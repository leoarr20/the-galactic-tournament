import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '../../environments/environment';
import { RankingItem } from '../models/ranking-item';

@Injectable({
  providedIn: 'root'
})
export class RankingService {

  private readonly apiUrl = environment.apiUrl + '/ranking';

  constructor(private http: HttpClient) { }

  // Ranking is requested from the API because victories are based on persisted battles.
  getRanking(): Observable<RankingItem[]> {
    return this.http.get<RankingItem[]>(this.apiUrl);
  }

  resetRanking(): Observable<any> {
    return this.http.post<any>(this.apiUrl + '/reset', {});
  }
}
